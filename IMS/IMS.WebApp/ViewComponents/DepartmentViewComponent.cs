using IMS.CoreServices;
using Microsoft.AspNetCore.Mvc;

namespace IMS.WebApp.ViewComponents
{
    public class DepartmentViewComponent : ViewComponent
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentViewComponent(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        
        public IViewComponentResult Invoke()
        {
            var departments = _departmentService.GetDepartments();
            return View(departments);
        }
    }
}
