using DocumentFormat.OpenXml.Bibliography;
using IMS.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace IMS.CoreServices.Implementations
{
    public class InterviewReminderService : BackgroundService
    {
        private readonly ILogger<InterviewReminderService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public InterviewReminderService(ILogger<InterviewReminderService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
			// Calculate delay until next 8AM (UTC)
			var delay = CalculateTimeToEightAM();
            _timer = new Timer(async state => await SendReminder(stoppingToken), null, delay, TimeSpan.FromDays(1));
        }

		private static TimeSpan CalculateTimeToEightAM()
		{
			var now = DateTime.Now;
			var eightAM = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0, DateTimeKind.Local);

			if (eightAM < now)
			{
				eightAM = eightAM.AddDays(1); 
			}

			return eightAM - now;
		}

		private async Task SendReminder(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Interview Reminder Service sending reminder !!");

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            try
            {
                // Using service provider to inject Scope into Singleton (Use using to disposed when it not needed) 
                using var scope = _serviceProvider.CreateScope();

                // Resolve required services
                var _interviewService = scope.ServiceProvider.GetRequiredService<IInterviewScheduleService>();
                var _interviewAssign = scope.ServiceProvider.GetRequiredService<IInterviewAssignService>();
                var _candidateService = scope.ServiceProvider.GetRequiredService<ICandidateService>();
                var _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var _positionService = scope.ServiceProvider.GetRequiredService<IPositionService>();
                // Get open interview only
                var interviews = await _interviewService.GetInterviewsByStatus(InterviewScheduleStatus.Open);

                // Send mail to invite
                foreach( var interview in interviews)
                {
                    // Check if interview has reach due date
                    if(interview.DueDate.Date == DateTime.UtcNow.Date)
                    {
                        // Get candidate
                        var candidate = _candidateService.GetCandidate(interview.CandidateId);
                        // Get interviewers 
                        var interviewers = await _interviewAssign.GetInterviewersByInterviewId(interview.InterviewScheduleId);
                        var allInterviewers = await _userService.GetAllInterviewer();
                        var interviewersInfo = allInterviewers.Where(user => interviewers.Contains(user.Id));

                        var position = _positionService.GetPositionName(candidate.PositionId);
                        var recruiter = await _userService.GetUser(interview.RecruiterId);

						// Change status to invited
						interview.Status = InterviewScheduleStatus.Invited;
						_interviewService.UpdateInterviewSchedule(interview);

                        // Get list email
                        var emailList = interviewersInfo.Select(i => i.Email).ToList();

                        // Send reminder
                        _emailService.SendInterviewReminder(candidate, interview, position, recruiter.UserName, emailList);

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending reminder.");
            }

        }
    }
}
