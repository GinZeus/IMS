using IMS.Data;
using IMS.Models;
using Microsoft.EntityFrameworkCore;

namespace IMS.CoreServices.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Department GetDepartmentById(int id)
        {
            var department = _context.Department
                .FirstOrDefault(d => d.DepartmentId == id);

            return department;
        }

        public IEnumerable<Department> GetDepartments()
		{
            return _context.Department;
        }


    }
}
