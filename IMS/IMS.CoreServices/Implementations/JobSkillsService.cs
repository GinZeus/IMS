using IMS.Models;
using IMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace IMS.CoreServices.Implementations
{
    public class JobSkillsService : IJobSkillsInterface
    {
        private readonly ApplicationDbContext _dbContext;

        public JobSkillsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public async Task CreateJobSkill(JobSkill jobSkill)
		{
			ArgumentNullException.ThrowIfNull(jobSkill);

			try
			{
				_dbContext.JobSkill.Add(jobSkill);
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
        
        public IEnumerable<int> GetSkillsByJobId(int jobId)
        {
            var Skill = _dbContext.JobSkill
            .Where(js => js.JobId == jobId).Select(js => js.SkillId)
            .ToList();
            return Skill;
        }
        public async Task DeleteJobSkill(int jobId)
        {
            try
            {
                var jobSkills = await _dbContext.JobSkill.Where(jS => jS.JobId == jobId).ToListAsync();

                if (jobSkills.Any())
                {
                    _dbContext.JobSkill.RemoveRange(jobSkills);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return ;
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
