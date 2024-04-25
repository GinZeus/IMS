using IMS.Models;

namespace IMS.CoreServices
{
	public interface IJobBenefitsInterface
	{
		public Task CreateJobBenefit(JobBenefit jobBenefit);

		public IEnumerable<int> GetBenefitsByJobId(int jobId);

		public Task DeleteJobBenefit(int jobId);
	}
}
