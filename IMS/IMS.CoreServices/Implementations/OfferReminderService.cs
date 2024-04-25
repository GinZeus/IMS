using Microsoft.Extensions.Hosting;
using IMS.HtmlEmail.Views.Emails.OfferReminder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.CoreServices.Implementations
{
    public class OfferReminderService : IHostedService, IDisposable
    {
        private readonly ILogger<OfferReminderService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public OfferReminderService(ILogger<OfferReminderService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("OfferReminerService is running");
            // Calculate delay until next 8AM (UTC)
            var delay = CalculateTimeToEightAM();
            _timer = new Timer(SendOfferReminder, null, delay, TimeSpan.FromDays(1));
            //_timer = new Timer(SendOfferReminder, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void SendOfferReminder(object? state)
        {
            _logger.LogInformation("SendOfferReminder is running");
            // Using service provider to inject Scope into Singleton (Use using to disposed when it not needed) 
            using var scope = _serviceProvider.CreateScope();

            // Resolved required services
            var _offerService = scope.ServiceProvider.GetRequiredService<IOfferService>();
            var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            IEnumerable<OfferReminderVM> dueOffers = _offerService.GetDueDateOffers();

            // Send reminder email
            foreach (OfferReminderVM offer in dueOffers)
            {
                _emailService.SendOfferReminder(offer);
            }
        }

        private TimeSpan CalculateTimeToEightAM()
        {
            var now = DateTime.Now;
            var eightAM = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0, DateTimeKind.Local);

            if (eightAM < now)
            {
                eightAM = eightAM.AddDays(1);
            }

            return eightAM - now;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
