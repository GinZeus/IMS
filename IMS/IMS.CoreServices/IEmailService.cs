using IMS.HtmlEmail.Views.Emails.OfferReminder;
using IMS.Models;

namespace IMS.CoreServices
{
	public interface IEmailService
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="senderName"></param>
		/// <param name="receiverName"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		void Send(string from, string to, string senderName, string receiverName, string subject, string body);

		void SendWithAttachment(string from, string to, string senderName, string receiverName, string subject, string body, byte[] file, string contentType, string fileName);

        void SendAccountInformation(User user, string password);

		void SendForgotPasswordLink(string link, string email);

		void SendInterviewReminder (Candidate candidate, InterviewSchedule interview, string position, string recruiter, IEnumerable<string> email);
		
		void SendOfferReminder(OfferReminderVM offer);
        void SendMultiple(string from, IEnumerable<string> to, string senderName, string subject, string body, byte[] file, string contentType, string fileName);

    }
}
