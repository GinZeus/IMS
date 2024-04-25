using IMS.Models;

namespace IMS.WebApp.ViewModels
{
    public class ManagerHomeVM
    {
        public int TotalOffers { get; set; }
        public int TotalOfferWaitingForApproval { get; set; }
        public int TotalOfferWaitingForResponse { get; set; }
        public int TotalOfferApproved { get; set; }
        public int TotalOfferAccepted { get; set; }
        public int TotalOfferRemain { get; set; }
        public IEnumerable<Offer> OfferWaitingForApproval { get; set; }
    }
}
