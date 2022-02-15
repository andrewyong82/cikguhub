using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CikguHub.Data;
using CikguHub.Helpers;
using Microsoft.Extensions.Azure;
using Azure.Storage.Blobs;
using System.IO;
using System.Reflection.Metadata;
using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using CikguHub.Api.Model;
using AutoMapper;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CikguHub.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseRenterDepositsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<CaseRenterDepositsController> _logger;
        private readonly IActivityLogger _activityLogger;

        public CaseRenterDepositsController(
            ApplicationDbContext context, 
            BlobServiceClient blobServiceClient, 
            IConfiguration configuration, 
            IMapper mapper,
            ILogger<CaseRenterDepositsController> logger,
            IActivityLogger activityLogger)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _activityLogger = activityLogger;
        }

        #region to remove

        // GET: api/CaseRenterDeposits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaseRenterDeposit>>> GetCaseRenterDeposits()
        {
            return await _context.CaseRenterDeposits.ToListAsync();
        }

        // GET: api/CaseRenterDeposits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CaseRenterDeposit>> GetCaseRenterDeposit(int id)
        {
            var caseRenterDeposit = await _context.CaseRenterDeposits.FindAsync(id);

            if (caseRenterDeposit == null)
            {
                return NotFound();
            }

            return caseRenterDeposit;
        }

        // PUT: api/CaseRenterDeposits/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCaseRenterDeposit(int id, CaseRenterDeposit caseRenterDeposit)
        {
            if (id != caseRenterDeposit.CaseRenterDepositId)
            {
                return BadRequest();
            }

            _context.Entry(caseRenterDeposit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaseRenterDepositExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CaseRenterDeposits
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CaseRenterDeposit>> PostCaseRenterDeposit(CaseRenterDeposit caseRenterDeposit)
        {
            _context.CaseRenterDeposits.Add(caseRenterDeposit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCaseRenterDeposit", new { id = caseRenterDeposit.CaseRenterDepositId }, caseRenterDeposit);
        }

        // DELETE: api/CaseRenterDeposits/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CaseRenterDeposit>> DeleteCaseRenterDeposit(int id)
        {
            var caseRenterDeposit = await _context.CaseRenterDeposits.FindAsync(id);
            if (caseRenterDeposit == null)
            {
                return NotFound();
            }

            _context.CaseRenterDeposits.Remove(caseRenterDeposit);
            await _context.SaveChangesAsync();

            return caseRenterDeposit;
        }

        private bool CaseRenterDepositExists(int id)
        {
            return _context.CaseRenterDeposits.Any(e => e.CaseRenterDepositId == id);
        }

        #endregion

        // Below actions for wizard --->
        [HttpPost("Step1")]
        public async Task<ActionResult<CaseRenterDeposit>> PostStep1(List<IFormFile> files, [FromForm]int caseId)
        {
            var caseRenterDeposit = await _context.CaseRenterDeposits.FirstAsync(c => c.CaseId == caseId);
            if (caseRenterDeposit == null)
            {
                return BadRequest();
            }

            if (files == null || files.Count <= 0)
            {
                return BadRequest();
            }

            var file = files.First();
            if (!(file != null && file.Length > 0))
                return BadRequest();


            var fileType = Path.GetExtension(file.FileName);
            if (!(fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg"))
              return BadRequest();

            var fileName = Path.GetFileName(file.FileName);

            //Create Resource
            Resource resource = new Resource();
            resource.Name = fileName;
            resource.FileType = fileType;
            resource.Size = file.Length;
            resource.ContentType = file.ContentType;
            resource.OwnerId = User.GetUserId();

            string folderName = User.GetFolderName();
            string blobName = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + fileName;
            resource.ContainerName = folderName;
            resource.BlobName = blobName;
            resource.StoragePath = folderName + "/" + blobName;

            caseRenterDeposit.TenancyContractResource = resource;
            caseRenterDeposit.TenancyContractUrl = _configuration["Azure:BlobHost"] + resource.StoragePath;

            //Save results later...

            //Upload contract to cloud storage
            BlobContainerClient containerClient = this._blobServiceClient.GetBlobContainerClient(folderName);
            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(file.OpenReadStream(), true);


            //Extract fields from uploaded contract...
            try
            {
                var credential = new AzureKeyCredential(_configuration["Azure:FormParserKey"]);
                var client = new FormRecognizerClient(new Uri(_configuration["Azure:FormParserEndpoint"]), credential);

                var agreementUri = caseRenterDeposit.TenancyContractUrl;
                FormPageCollection formPages = await client
                    .StartRecognizeContentFromUri(new Uri(agreementUri))
                    .WaitForCompletionAsync();

            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error occurred trying to parse uploaded contract");
            }

            //Save all results
            await _context.SaveChangesAsync();

            return Ok(caseRenterDeposit);
        }

        [HttpPost("Step2")]
        public async Task<ActionResult<CaseRenterDeposit>> PostStep2([FromForm] CaseRenterDepositModel model)
        {
            if (model.CaseRenterDepositId == 0)
            {
                return BadRequest();
            }


            var caseRenterDeposit = await _context.CaseRenterDeposits.FindAsync(model.CaseRenterDepositId);

            caseRenterDeposit = _mapper.Map<CaseRenterDepositModel, CaseRenterDeposit>(model, caseRenterDeposit);
            _context.Entry(caseRenterDeposit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                await _activityLogger.LogCaseActivityAsync(model.CaseId, ActivityType.Edited);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaseRenterDepositExists(caseRenterDeposit.CaseRenterDepositId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(caseRenterDeposit);
        }

        [HttpPost("Step3")]
        public async Task<ActionResult<CaseRenterDeposit>> PostStep3([FromForm] CaseRenterDepositModel model)
        {
            if (model.CaseRenterDepositId == 0)
            {
                return BadRequest();
            }

            var caseRenterDeposit = await _context.CaseRenterDeposits.Include(c => c.Case).FirstOrDefaultAsync(c => c.CaseRenterDepositId == model.CaseRenterDepositId);
            
            if (model.KeyReturned)
                caseRenterDeposit.KeyReturnedDate = model.KeyReturnedDate;
            else
                caseRenterDeposit.KeyReturnedDate = null;

            if (model.Inspection)
                caseRenterDeposit.JointInspectionDate = model.JointInspectionDate;
            else
                caseRenterDeposit.JointInspectionDate = null;

            caseRenterDeposit.NoComplaints = model.NoComplaints;
            caseRenterDeposit.LandlordTerminatedDate = model.LandlordTerminatedDate;
            caseRenterDeposit.ResolutionDisputed = model.ResolutionDisputed;

            caseRenterDeposit.LandlordAddress = model.LandlordAddress;
            caseRenterDeposit.LandlordContact = model.LandlordContact;
            caseRenterDeposit.TenantContact = model.TenantContact;
            caseRenterDeposit.TenantAddress = model.TenantAddress;
            caseRenterDeposit.TenantEmail = model.TenantEmail;
            caseRenterDeposit.TenantBankDetails = model.TenantBankDetails;


            return Ok(caseRenterDeposit);
        }

        [HttpPost("SignLetterOfDemand")]
        public async Task<ActionResult> PostSignLetterOfDemand([FromForm] int id, [FromForm] string signature)
        {
            var caseRenterDeposit = await _context.CaseRenterDeposits.FindAsync(id);

            if (caseRenterDeposit == null)
            {
                return BadRequest();
            }

            caseRenterDeposit.LetterOfDemandSignature = signature;


            return Ok(caseRenterDeposit.LetterOfDemandUrl);
        }
    }
}