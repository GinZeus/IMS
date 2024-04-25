using IMS.CoreServices;
using IMS.Models;
using IMS.WebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using IMS.CoreServices.Implementations;
using IMS.Utilities.Constants;

namespace IMS.WebApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IInterviewScheduleService _interviewScheduleService;
		private readonly IUserService _userService;
		private readonly ICandidateService _candidateService;
        private readonly IOfferService _offerService;

        public HomeController(ILogger<HomeController> logger, IInterviewScheduleService interviewScheduleService, IUserService userService, ICandidateService candidateService, IOptions<IdentityOptions> options, IOptions<CookieAuthenticationOptions> cookies, IOfferService offerService)
		{
			_logger = logger;
			_interviewScheduleService = interviewScheduleService;
			_userService = userService;
			_candidateService = candidateService;
			_offerService = offerService;
		}

		[Authorize]
		public async Task<IActionResult> Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

	}
}
