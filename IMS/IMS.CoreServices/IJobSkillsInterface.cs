using IMS.Models;

namespace IMS.CoreServices
{
    public interface IJobSkillsInterface
    {
        public IEnumerable<int> GetSkillsByJobId(int jobId);

        public Task CreateJobSkill(JobSkill jobSkill);

        public Task DeleteJobSkill(int jobId);

    }
}
