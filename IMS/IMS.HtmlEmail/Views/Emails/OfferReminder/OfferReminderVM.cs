namespace IMS.HtmlEmail.Views.Emails.OfferReminder
{
    public class OfferReminderVM
    {
        // info displayed in the email
        public string CandidateName { get; set; }
        public string CandidatePostion { get; set; }
        public DateTime DueDate { get; set; }
        public string OfferDetailURL { get; set; }
        public string RecruiterUsername { get; set; }
        public byte[]? CvAttachment { get; set; }

        // info used in email's meta data
        public string ManagerEmail { get; set; }
        public string ManagerUsername { get; set; }
        public string? CvMimeType { get; set; }
    }
}
