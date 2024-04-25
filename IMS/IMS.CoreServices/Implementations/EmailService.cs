using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MailKit.Security;
using IMS.Models;
using IMS.Utilities.Constants;
using IMS.HtmlEmail;
using IMS.HtmlEmail.Views.Emails.AccountInfo;
using IMS.HtmlEmail.Views.Emails.Forgotpassword;
using IMS.HtmlEmail.Views.Emails.InterviewReminder;
using IMS.HtmlEmail.Views.Emails.OfferReminder;
using Microsoft.IdentityModel.Tokens;

namespace IMS.CoreServices.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IRazorViewToStringRenderer _renderer;

        public EmailService(IConfiguration configuration, IRazorViewToStringRenderer renderer)
        {
            _configuration = configuration;
            _renderer = renderer;
        }

        public void Send(string from, string to, string senderName, string receiverName, string subject, string body)
        {
            Console.WriteLine("Dang gui");
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(senderName, from));
            message.To.Add(new MailboxAddress(receiverName, to));
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Note: only needed if the SMTP server requires authentication
                string account = _configuration["EmailSettings:Account"];
                string password = _configuration["EmailSettings:Password"];

                client.Authenticate(account, password);

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public void SendWithAttachment(string from, string to, string senderName, string receiverName, string subject, string body, byte[]? file, string? contentType, string? fileName)
        {
            Console.WriteLine("Sending mail with attachment");

            var message = new MimeMessage();


            message.From.Add(new MailboxAddress(senderName, from));
            message.To.Add(new MailboxAddress(receiverName, to));
            message.Subject = subject;

            // Build body with attachment using body builder
            var builder = new BodyBuilder();

            // HTML body 
            builder.HtmlBody = body;

            // Directly add
            if (!file.IsNullOrEmpty())
            {
                string extension = string.Empty;
                if (contentType == "application/pdf")
                    extension = "pdf";
                else
                    extension = "docx";
                builder.Attachments.Add($"CV_{fileName}.{extension}", file, new ContentType("application", extension));
            }
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Note: only needed if the SMTP server requires authentication
                string account = _configuration["EmailSettings:Account"];
                string password = _configuration["EmailSettings:Password"];

                client.Authenticate(account, password);

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public async void SendAccountInformation(User user, string password)
        {
            string from = _configuration["EmailSettings:Account"];
            string to = user.Email;
            string sender = _configuration["EmailSettings:Name"];
            string receiver = user.FullName;
            string subject = EmailConstants.ACCOUNT_INFO_SUBJECT;

            // create VM
            AccountInfoVM viewModel = new AccountInfoVM { Username = user.UserName, Password = password };
            string body = await _renderer.RenderViewToStringAsync("/Views/Emails/AccountInfo/AccountInfo.cshtml", viewModel);

            Send(from, to, sender, receiver, subject, body);
        }

        public async void SendForgotPasswordLink(string link, string email)
        {
            string from = _configuration["EmailSettings:Account"];
            string to = email;
            string sender = _configuration["EmailSettings:Name"];
            string receiver = email;
            string subject = EmailConstants.FORGOT_PASSWORD_SUBJECT;

            // create VM
            ForgotPasswordVM viewModel = new ForgotPasswordVM { Email = email, Link = link };
            string body = await _renderer.RenderViewToStringAsync("/Views/Emails/ForgotPassword/ForgotPassword.cshtml", viewModel);

            Send(from, to, sender, receiver, subject, body);
        }

        public async void SendInterviewReminder(Candidate candidate, InterviewSchedule interview, string position, string recruiter, IEnumerable<string> email)
        {
            string from = _configuration["EmailSettings:Account"];
            string sender = _configuration["EmailSettings:Name"];
            string subject = EmailConstants.ACCOUNT_INFO_SUBJECT + " " + interview.Title;
            try
            {
                string url = $"https://localhost:7056/InterviewSchedule/SubmitResult/{interview.InterviewScheduleId}";
                // create VM
                InterviewReminderVM viewModel = new InterviewReminderVM
                {
                    CandidateName = candidate.FullName,
                    StartTime = interview.StartTime,
                    EndTime = interview.EndTime,
                    InterviewDetailURL = url,
                    MeetingId = interview.MeetingId,
                    Position = position,
                    Recruiter = recruiter,
                };
                string body = await _renderer.RenderViewToStringAsync("/Views/Emails/InterviewReminder/InterviewReminder.cshtml", viewModel);
                // Set file name 
                string fileName = $"{interview.Title}_{candidate.FullName}";

                SendMultiple(from, email, sender, subject, body, candidate.CVAttachment, candidate.CVMimeType, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async void SendOfferReminder(OfferReminderVM offer)
        {
            string from = _configuration["EmailSettings:Account"];
            string to = offer.ManagerEmail;
            string sender = _configuration["EmailSettings:Name"];
            string receiver = offer.ManagerUsername;
            string subject = EmailConstants.OFFER_REMINDER_SUBJECT;

            string body = await _renderer.RenderViewToStringAsync("/Views/Emails/OfferReminder/OfferReminder.cshtml", offer);
            string cvName = $"{offer.CandidateName} - CV";

            SendWithAttachment(from, to, sender, receiver, subject, body, offer.CvAttachment, offer.CvMimeType, cvName);
        }

        public void SendMultiple(string from, IEnumerable<string> to, string senderName, string subject, string body, byte[] file, string contentType, string fileName)
        {
            Console.WriteLine("Sending mail with attachment");

            var message = new MimeMessage();


            message.From.Add(new MailboxAddress(senderName, from));
            foreach (var item in to)
            {
                message.To.Add(new MailboxAddress(item, item));
            }
            message.Subject = subject;

            // Build body with attachment using body builder
            var builder = new BodyBuilder();

            // HTML body 
            builder.HtmlBody = body;

            // Directly add
            if (!file.IsNullOrEmpty())
            {
                string extension = string.Empty;
                if (contentType == "application/pdf")
                    extension = "pdf";
                else
                    extension = "docx";
                builder.Attachments.Add($"CV_{fileName}.{extension}", file, new ContentType("application", extension));
            }
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Note: only needed if the SMTP server requires authentication
                string account = _configuration["EmailSettings:Account"];
                string password = _configuration["EmailSettings:Password"];

                client.Authenticate(account, password);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
