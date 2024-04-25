using IMS.Data;
using IMS.Models;
using Microsoft.EntityFrameworkCore;

namespace IMS.CoreServices.Implementations
{
	public class JobLevelsService : IJobLevelsInterface
	{
		private readonly ApplicationDbContext _dbContext;

		public JobLevelsService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task CreateJobLevel(JobLevel jobLevel)
		{
			ArgumentNullException.ThrowIfNull(jobLevel);

			try
			{
				_dbContext.JobLevels.Add(jobLevel);
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public IEnumerable<int> GetLevelsByJobId(int jobId)
		{
            var Levels = _dbContext.JobLevels
            .Where(jl => jl.JobId == jobId).Select(jl => jl.LevelId)
            .ToList();
            return Levels;
        }

        public async Task DeleteJobLevel(int jobId)
        {
            try
            {
                var jobLevels = await _dbContext.JobLevels.Where(jL => jL.JobId == jobId).ToListAsync();

                if (jobLevels.Any())
                {
                    _dbContext.JobLevels.RemoveRange(jobLevels);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return;
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
