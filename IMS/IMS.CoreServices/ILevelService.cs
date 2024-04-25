

using IMS.Models;

namespace IMS.CoreServices
{
    public interface ILevelService
    {
        IEnumerable<Level> GetLevels();
        Level GetLevelbyId(int levelId);

	}
}
