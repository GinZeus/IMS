using IMS.Models;
using IMS.Data;
using Microsoft.EntityFrameworkCore;

namespace IMS.CoreServices.Implementations
{
	public class JobBenefitsService : IJobBenefitsInterface
	{
		private readonly ApplicationDbContext _dbContext;

		public JobBenefitsService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task CreateJobBenefit(JobBenefit jobBenefit)
		{
			ArgumentNullException.ThrowIfNull(jobBenefit);

			try
			{
				_dbContext.JobBenefits.Add(jobBenefit);
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		public IEnumerable<int> GetBenefitsByJobId(int jobId)
		{
			var benefits = _dbContext.JobBenefits
			.Where(jb => jb.JobId == jobId).Select(jb => jb.BenefitId)
			.ToList();
			return benefits;
		}
        public async Task DeleteJobBenefit(int jobId)
        {
            try
            {
				var jobBenefit = await _dbContext.JobBenefits.Where(jB => jB.JobId == jobId).ToListAsync();
                if (jobBenefit != null)
                {
                    _dbContext.JobBenefits.RemoveRange(jobBenefit);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Job Benefit not found for the given job ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        
    }
}
