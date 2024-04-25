using IMS.Data;
using IMS.Models;

namespace IMS.CoreServices.Implementations
{
    public class AcademicLevelService : IAcademicLevelService
    {
        private readonly ApplicationDbContext _context;

        public AcademicLevelService(ApplicationDbContext context)
        {
            _context = context;
        }

		public string? GetAcademicLevelName(int academicId)
		{
			return _context.AcademicLevel.Find(academicId)?.AcademicLevelName;
		}

		public IEnumerable<AcademicLevel> GetAcademicLevels()
        {
            return _context.AcademicLevel;
        }
    }
}
