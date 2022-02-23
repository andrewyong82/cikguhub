using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CikguHub.Data
{
    public static class CaseRenterDepositStatus
    {
        public const string LetterOfDemand = "LetterOfDemand";
        public const string CourtHearing = "CourtHearing";
        public const string CourtOrder = "CourtOrder";
        public const string GarnisheeApplication = "GarnisheeApplication";
    }

    public class CaseRenterDeposit
    {
        public int CaseRenterDepositId { get; set; }

        public int CaseId { get; set; }
        public Course Case { get; set; }

        public string Status { get; set; }

        [DisplayName("Tenant Name")]
        public string TenantName { get; set; }

        [DisplayName("Tenant IC")]
        public string TenantIC { get; set; }

        [DisplayName("Tenant Email")]
        public string TenantEmail { get; set; }

        [DisplayName("Tenant Contact")]
        public string TenantContact { get; set; }

        [DisplayName("Tenant Address")]
        public string TenantAddress { get; set; }

        [DisplayName("Tenant Bank Details")]
        public string TenantBankDetails { get; set; }

        [DisplayName("Tenancy Start Date")]
        public DateTime? TenancyStart { get; set; }

        [DisplayName("Tenancy End Date")]
        public DateTime? TenancyEnd { get; set; }

        [DisplayName("Property Address")]
        public string PropertyAddress { get; set; }

        [DisplayName("Landlord Name")]
        public string LandlordName { get; set; }

        [DisplayName("Landlord IC")]
        public string LandlordIC { get; set; }

        [DisplayName("Landlord Contact")]
        public string LandlordContact { get; set; }

        [DisplayName("Landlord Email")]
        public string LandlordEmail { get; set; }

        [DisplayName("Landlord Address")]
        public string LandlordAddress { get; set; }

        [DisplayName("Landlord Bank Details")]
        public string LandlordBankDetails { get; set; }

        [DisplayName("Security Deposit")]
        public decimal? SecurityDeposit { get; set; }

        [DisplayName("Utilities Deposit")]
        public decimal? UtilityDeposit { get; set; }

        [DisplayName("Other Amounts")]
        public decimal? OtherAmounts { get; set; }

        [DisplayName("Reason for Other Amounts")]
        public string OtherAmountsReason { get; set; }

        public decimal TotalOwed()
        {
            decimal amount = SecurityDeposit.GetValueOrDefault() + UtilityDeposit.GetValueOrDefault() + OtherAmounts.GetValueOrDefault();
            return amount;
        }

        //dispute cirsumstances -->
        public DateTime? KeyReturnedDate { get; set; }

        public DateTime? JointInspectionDate { get; set; }

        public bool? NoComplaints { get; set; }

        public DateTime? LandlordTerminatedDate { get; set; }

        public bool? ResolutionDisputed { get; set; }

        public string VariableText()
        {
            string vt = "";

            if (KeyReturnedDate.HasValue)
            {
                vt += "The Tenancy has ended, and I have returned / handed over the keys to the Landlord, on " + KeyReturnedDate.Value.ToString("dd MMMM yyyy") + ". ";
            }

            if (NoComplaints.HasValue && NoComplaints.Value)
            {
                vt += "The Landlord has no complaints on the state of the Demised Premises. ";
            }

            if (JointInspectionDate.HasValue)
            {
                vt += "There was a joint inspection on " + JointInspectionDate.Value.ToString("dd MMMM yyyy") + ". ";
            }

            if (LandlordTerminatedDate.HasValue)
            {
                vt += "The Tenancy was terminated by the Landlord on " + LandlordTerminatedDate.Value.ToString("dd MMMM yyyy") + ", before the end of term, however the Landlord had unreasonably refused to refund the Deposits and/or the Outstanding Sum. ";
            }

            if (ResolutionDisputed.HasValue && ResolutionDisputed.Value)
            {
                vt += "There was a joint inspection, the Landlord and I cannot agree on the state of the Demised Premises, and cannot agree on the contractor to restore the Demised Premises. ";
            }

            return vt;
        }

        //suggested values from document parser -->
        public string TenantName_Suggested { get; set; }
        public string TenantIC_Suggested { get; set; }
        public string TenancyStart_Suggested { get; set; }
        public string TenancyEnd_Suggested { get; set; }
        public string PropertyAddress_Suggested { get; set; }
        public string LandlordName_Suggested { get; set; }
        public string LandlordIC_Suggested { get; set; }
        public string LandlordContact_Suggested { get; set; }
        public string LandlordAddress_Suggested { get; set; }
        public string LandlordBankDetails_Suggested { get; set; }

        public string SecurityDeposit_Suggested { get; set; }
        public string UtilityDeposit_Suggested { get; set; }

        //case assets -->
        public string TenancyContractUrl { get; set; }
        public int? TenancyContractResourceId { get; set; }
        public Resource TenancyContractResource { get; set; }

        public string LetterOfDemandUrl { get; set; }
        public Payment LetterOfDemandPayment { get; set; }
        public string LetterOfDemandServiceLevel { get; set; }

        public string LetterOfDemandSignature { get; set; }

        public string LetterOfDemandTrackingUrl { get; set; }

        public string CourtHearingApplicationUrl { get; set; }
        public Payment CourtHearingApplicationPayment { get; set; }

        public string CourtOrderApplicationUrl { get; set; }
        public Payment CourtOrderApplicationPayment { get; set; }

        public string GarnisheeApplicationUrl { get; set; }
        public Payment GarnisheeApplicationPayment { get; set; }
    }
}
