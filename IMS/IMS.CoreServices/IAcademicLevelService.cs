using IMS.Models;

namespace IMS.CoreServices
{
    public interface IAcademicLevelService
    {
        IEnumerable<AcademicLevel> GetAcademicLevels();
        string? GetAcademicLevelName(int academicId);
    }
}
