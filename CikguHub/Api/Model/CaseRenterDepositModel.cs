using CikguHub.Data;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CikguHub.Api.Model
{
    public class CaseRenterDepositModel
    {
        public int CaseRenterDepositId { get; set; }
        public int CaseId { get; set; }
        public string TenantName { get; set; }
        public string TenantIC { get; set; }
        public string TenantContact { get; set; }
        public string TenantAddress { get; set; }
        public string TenantEmail { get; set; }
        public string TenantBankDetails { get; set; }
        public DateTime? TenancyStart { get; set; }
        public DateTime? TenancyEnd { get; set; }
        public string PropertyAddress { get; set; }
        public string LandlordName { get; set; }
        public string LandlordIC { get; set; }
        public string LandlordContact { get; set; }
        public string LandlordEmail { get; set; }
        public string LandlordAddress { get; set; }
        public string LandlordBankDetails { get; set; }
        public decimal? SecurityDeposit { get; set; }
        public decimal? UtilityDeposit { get; set; }
        public decimal? OtherAmounts { get; set; }
        public string OtherAmountsReason { get; set; }

        public bool KeyReturned { get; set; }
        public DateTime? KeyReturnedDate { get; set; }
        public bool Inspection { get; set; }
        public DateTime? JointInspectionDate { get; set; }

        public bool? NoComplaints { get; set; }
        public DateTime? LandlordTerminatedDate { get; set; }
        public bool? ResolutionDisputed { get; set; }

        public string LetterOfDemandServiceLevel { get; set; }

        public CaseRenterDepositModel() { }

    }
}
