using IMS.Models;

namespace IMS.CoreServices
{
    public interface ISkillService
    {
        IEnumerable<Skill> GetSkills();

    }
}
