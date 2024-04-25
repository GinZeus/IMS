using System.ComponentModel.DataAnnotations;

namespace IMS.Utilities.Constants
{
	public enum CandidateStatus
	{
		[Display(Name = "Open")]
		Open,
		[Display(Name = "Banned")]
		Banned,
		[Display(Name = "Waiting for interview")]
		WaitingForInterview,
		[Display(Name = "Cancelled interview")]
		CancelledInterview,
		[Display(Name = "Passed interview")]
		PassedInterview,
		[Display(Name = "Failed interview")]
		FailedInterview,
		[Display(Name = "Waiting for approval")]
		WaitingForApproval,
		[Display(Name = "Approved offer")]
		ApprovedOffer,
		[Display(Name = "Rejected offer")]
		RejectedOffer,
		[Display(Name = "Waiting for response")]
		WaitingForResponse,
		[Display(Name = "Accepted offer")]
		AcceptedOffer,
		[Display(Name = "Declined offer")]
		DeclinedOffer,
		[Display(Name = "Cancelled offer")]
		CancelledOffer
	}
}
