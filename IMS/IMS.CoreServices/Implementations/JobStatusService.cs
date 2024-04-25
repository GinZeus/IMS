using IMS.Models;
using IMS.Utilities.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace IMS.CoreServices.Implementations
{
	public class JobStatusService : BackgroundService
	{
		private readonly ILogger<JobStatusService> _logger;
		private readonly IServiceProvider _serviceProvider;
		private Timer _timer;

		public JobStatusService(ILogger<JobStatusService> logger, IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
		}

		protected async override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			// Calculate delay until next 0h 
			var delay = CalculateTimeToMidnight();
			_timer = new Timer(async state => await UpdateJobStatusesAsync(stoppingToken), null, delay, TimeSpan.FromDays(1));
		}

		private static TimeSpan CalculateTimeToMidnight()
		{
			var now = DateTime.Now;
			var midnight = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local);

			if (midnight < now)
			{
				midnight = midnight.AddDays(1); // Move to tomorrow's midnight
			}

			return midnight - now;
		}

		private async Task UpdateJobStatusesAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Background service!!");

			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}

			try
			{
				// Using service provider to inject Scope into Singleton (Use using to disposed when it not needed) 
				using var scope = _serviceProvider.CreateScope();

				// Resolve the Job service
				var _jobService = scope.ServiceProvider.GetRequiredService<IJobInterface>();

				// Get all Job that not Closed
				var jobs = await _jobService.GetUnclosedJobs();

				foreach (var job in jobs)
				{
					// If start date pass and JobStatus is not Open => change status
					if (job.StartDate <= DateTime.Now && job.Status != (int)JobStatus.Open)
					{
						job.Status = (int)JobStatus.Open;
						await _jobService.UpdateJob(job);
						_logger.LogInformation($"Job '{job.JobId}' status changed to Open.");
					}
					// If end date pass and JobStatus is not Closed => change status
					if (job.EndDate <= DateTime.UtcNow && job.Status != JobStatus.Closed)
					{
						job.Status = JobStatus.Closed;
						await _jobService.UpdateJob(job);
						_logger.LogInformation($"Job '{job.JobId}' status changed to Closed.");
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while updating job statuses.");
			}
		}
	}

}
