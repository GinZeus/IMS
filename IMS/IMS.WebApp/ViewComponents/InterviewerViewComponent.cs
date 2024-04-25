using IMS.CoreServices;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApp.ViewComponents
{
    public class InterviewerViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public InterviewerViewComponent(IUserService userService)
        {
            _userService = userService;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Interviewer = await _userService.GetAllInterviewer();
            return View(Interviewer);
        }
    }
}
