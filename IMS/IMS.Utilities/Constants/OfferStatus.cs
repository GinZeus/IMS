using System.ComponentModel.DataAnnotations;

namespace IMS.Utilities.Constants
{
    public enum OfferStatus
    {
        [Display(Name = "Waiting for Approval")]
        WaitingForApproval,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "Rejected")]
        Rejected,
        [Display(Name = "Waiting for Response")]
        WaitingForResponse,
        [Display(Name = "Accepted Offer")]
        AcceptedOffer,
        [Display(Name = "Declined Offer")]
        DeclinedOffer,
        [Display(Name = "Cancelled")]
        Cancelled
    }
}
