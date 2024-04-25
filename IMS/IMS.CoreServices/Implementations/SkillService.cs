using IMS.Data;
using IMS.Models;

namespace IMS.CoreServices.Implementations
{
    public class SkillService : ISkillService
    {
        private readonly ApplicationDbContext _context;

        public SkillService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Skill> GetSkills()
        {
            return _context.Skill;
        }
    }
}
