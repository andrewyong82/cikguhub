using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CikguHub.Data;
using CikguHub.Helpers;
using Microsoft.AspNetCore.Identity;
using CikguHub.Document;
using Microsoft.Extensions.Logging;
using System.Net;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace CikguHub.Pages.Class
{
    public class SetupModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IDocumentService _documentService;
        private readonly IActivityLogger _activityLogger;
        private readonly ILogger<SetupModel> _logger;
        private readonly BlobServiceClient _blobServiceClient;

        public SetupModel(ApplicationDbContext context, IConfiguration configuration, IDocumentService documentService, IActivityLogger activityLogger, ILogger<SetupModel> logger, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _configuration = configuration;
            _documentService = documentService;
            _activityLogger = activityLogger;
            _logger = logger;
            _blobServiceClient = blobServiceClient;
        }

        [BindProperty]
        public Data.Class Class { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //if (!User.HasCase(id.Value))
            //{
            //    return NotFound();
            //}

            Class = await _context.Classes
                .Include(c => c.ImageResource)
                .Include(c => c.Course)
                .Include(c => c.Enrolments)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(m => m.ClassId == id);

            //Default values
            //if (String.IsNullOrWhiteSpace(Class.TenantEmail))
            //{
            //    Class.TenantEmail = User.GetUserEmail();
            //}

            if (Class == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetCompleteEnrolment(int enrolmentId)
        {
            Data.Enrolment c = await _context.Enrolments.Include(e => e.User)
            .FirstOrDefaultAsync(m => m.EnrolmentId == enrolmentId);

            try
            {
                c.Status = EnrolmentStatus.Completed;

                //TODO: put this in a service worker queue --->
                WebClient wc = new WebClient();
                byte[] pdf;
                using (MemoryStream stream = new MemoryStream(wc.DownloadData("https://cikguhub.blob.core.windows.net/templates/certificate2.docx")))
                {
                    pdf = this._documentService.GeneratePdfFromModel(c, stream, "https://cikguhub.blob.core.windows.net/templates/certificate.png");
                }

                string folderName = this.User.GetFolderName();
                string blobName = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + "certificate" + ".pdf";

                BlobContainerClient containerClient = this._blobServiceClient.GetBlobContainerClient(folderName);
                await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                var blobHttpHeader = new BlobHttpHeaders();
                blobHttpHeader.ContentType = "application/pdf";

                using (MemoryStream ms = new MemoryStream(pdf))
                {
                    ms.Position = 0;
                    BlobClient blobClient = containerClient.GetBlobClient(blobName);
                    await blobClient.UploadAsync(ms, blobHttpHeader);
                }

                string StoragePath = folderName + "/" + blobName;
                c.CertificateUrl = _configuration["Azure:BlobHost"] + StoragePath;

                await _context.SaveChangesAsync();
                await _activityLogger.LogActivityAsync(EntityType.Enrolment, c.EnrolmentId, ActivityType.Edited);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred trying to generate document");
                throw e;
            }

            return Partial("Partials/_Enrolment", c);
        }

    }
}
