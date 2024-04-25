using IMS.Data;
using IMS.Models;
using Microsoft.EntityFrameworkCore;

namespace IMS.CoreServices.Implementations
{
	public class InterviewAssignService : IInterviewAssignService
	{
		private readonly ApplicationDbContext _dbContext;

		public InterviewAssignService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task CreateInterviewAssign(InterviewAssign interviewAssign)
		{
			ArgumentNullException.ThrowIfNull(interviewAssign);

			try
			{
				_dbContext.InterviewAssigns.Add(interviewAssign);
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		// Get all interview schedules 
		public async Task<IEnumerable<int>> GetInterviewsByInterviewerId (string interviewerId)
		{
            ArgumentNullException.ThrowIfNull(interviewerId);

            try
            {
                return await _dbContext.InterviewAssigns
                    .Where(ia => ia.InterviewerId == interviewerId)
                    .Select(ia => ia.InterviewScheduleId)
                    .ToListAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

		// Get all interviewers Id that has been assigned for interview 
		public async Task<IEnumerable<string>> GetInterviewersByInterviewId(int interviewId)
		{
			ArgumentNullException.ThrowIfNull(interviewId);

			try
			{
				return await _dbContext.InterviewAssigns
					.Where(ia => ia.InterviewScheduleId == interviewId)
					.Select(ia => ia.InterviewerId)
					.ToListAsync();
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		// Remove exist interview assign by interview Id
		public async Task RemoveInterviewAssign(int interviewId)
		{
			try
			{
				var interviewAssigns = await _dbContext.InterviewAssigns.Where(ia => ia.InterviewScheduleId == interviewId).ToListAsync();

				_dbContext.InterviewAssigns.RemoveRange(interviewAssigns);
				await _dbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
	}
}
