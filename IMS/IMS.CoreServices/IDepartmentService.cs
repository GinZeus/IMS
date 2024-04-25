using IMS.Models;

namespace IMS.CoreServices
{
	public interface IDepartmentService
	{
		IEnumerable<Department> GetDepartments();

		Department GetDepartmentById(int id);
	}
}
