
using IMS.Data;
using IMS.Models;
using Microsoft.EntityFrameworkCore;

namespace IMS.CoreServices.Implementations
{
    public class LevelService : ILevelService
    {
        private readonly ApplicationDbContext _context;
        
        public LevelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Level> GetLevels()
        {
            return _context.Levels;
        }
        public Level GetLevelbyId(int levelId)
        {
			Level level = _context.Levels.Where(l => l.LevelId == levelId).FirstOrDefault();
			return level;
		}
    }
}
