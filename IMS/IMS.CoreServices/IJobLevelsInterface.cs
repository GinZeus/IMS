using IMS.Models;

namespace IMS.CoreServices
{
    public interface IJobLevelsInterface
    {
		public Task CreateJobLevel(JobLevel jobLevel);
		public IEnumerable<int> GetLevelsByJobId(int jobId);

        public Task DeleteJobLevel(int jobId);
    }
}
