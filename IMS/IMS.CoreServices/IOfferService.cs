using IMS.HtmlEmail.Views.Emails.OfferReminder;
using IMS.Models;
using IMS.Utilities.Constants;

namespace IMS.CoreServices
{
    public interface IOfferService
    {
        public IEnumerable<Offer> GetAllOffer();

        public Task CreateOffer(Offer offer);

		public Task<Offer>? GetOfferById(int offerId);

		public Task UpdateOffer(Offer offer);
		Task ApproveRejectOffer(int offerId, OfferStatus status);

		Task UpdateOfferStatus(int offerId, OfferStatus status);

		/// <summary>
		/// Get all due date offers with status WaitingForApproval
		/// OfferReminderVM objects have been eager loaded all fields
		/// </summary>
		/// <returns>List of due date offers</returns>
		IEnumerable<OfferReminderVM> GetDueDateOffers();
		IEnumerable<Offer>? GetOffersByDueDate(DateTime startDate, DateTime endDate);

        Task<byte[]> GenerateExcelOfferAsync(IEnumerable<Offer> offers);

        int GetTotalOffer();
        int GetTotalOfferByStatus(OfferStatus status);
		Task<IEnumerable<Offer>> GetOffersByCandidateId(int candidateId);
        IEnumerable<Offer> GetOfferWaitingForApproval();
    }
}
