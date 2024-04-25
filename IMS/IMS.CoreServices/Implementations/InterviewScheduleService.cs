using IMS.Data;
using IMS.Models;
using IMS.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace IMS.CoreServices.Implementations
{
	public class InterviewScheduleService : IInterviewScheduleService
	{
		private readonly ApplicationDbContext _dbContext;

		public InterviewScheduleService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// Create interview schedule and return InterviewScheduleId of the interview schedule just created
		public async Task<int> CreateInterviewSchedule(InterviewSchedule interviewSchedule)
		{
			ArgumentNullException.ThrowIfNull(interviewSchedule);

			try
			{
				_dbContext.InterviewSchedules.Add(interviewSchedule);
				await _dbContext.SaveChangesAsync();
				return interviewSchedule.InterviewScheduleId;
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		public IEnumerable<InterviewSchedule> GetAllValidInterviewSchedules()
		{

			return _dbContext.InterviewSchedules
				.Include(i => i.Candidate)
				.Include(i => i.Job)
				.Include(i => i.InterviewAssigns)
					.ThenInclude(i => i.User)
				.ToList();

		}

		[HttpPost]
		public IEnumerable<InterviewSchedule> FilterInterview(InterviewScheduleStatus? status, string? interviewerId)
		{
			var interviews = GetAllValidInterviewSchedules();
			if (status != null)
			{
				interviews = interviews.Where(i => i.Status == status);
			}

			if (interviewerId != null)
			{
				interviews = interviews.Where(i => i.InterviewAssigns.Any(assign => assign.InterviewerId == interviewerId))
		.ToList();
			}
			return interviews;
		}

		public async Task CreateOffer(Offer offer)
		{
			ArgumentNullException.ThrowIfNull(offer);

			try
			{
				_dbContext.Offers.Add(offer);
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}


		// Get interview schedule by Id 
		public async Task<InterviewSchedule> GetInterviewScheduleById(int InterviewScheduleId)
		{
			ArgumentNullException.ThrowIfNull(InterviewScheduleId);

			try
			{
				return await _dbContext.InterviewSchedules.FirstOrDefaultAsync(i => i.InterviewScheduleId == InterviewScheduleId);
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public async Task UpdateInterviewSchedule(InterviewSchedule interviewSchedule)
		{
			if (interviewSchedule == null)
				throw new ArgumentNullException(nameof(interviewSchedule));

			try
			{
				_dbContext.Entry(interviewSchedule).State = EntityState.Modified;
				await _dbContext.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public IEnumerable<InterviewSchedule> GetInterviewSchedulesByCandidateId(int? candidateId)
		{
			return _dbContext.InterviewSchedules.Where(i => i.CandidateId == candidateId && i.Result == true).ToList();
		}

        public async Task<IEnumerable<InterviewSchedule>> GetInterviewsByStatus(InterviewScheduleStatus status)
        {
            if (status == null)
                throw new ArgumentNullException();
			try
			{
				return await _dbContext.InterviewSchedules.Where(i => i.Status == status).ToListAsync();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message, ConsoleColor.Red);
				throw;
			}
        }
        public InterviewSchedule getDetailInterviewById(int InterviewScheduleId)
        {
            var interview = _dbContext.InterviewSchedules
        .Include(i => i.Candidate)
        .Include(i => i.Job)
        .Include(i => i.InterviewAssigns)
        .ThenInclude(ia => ia.User)
        .Include(i => i.Recruiter)
        .FirstOrDefault(i => i.InterviewScheduleId == InterviewScheduleId);

            return interview;
        }

        public async Task<IEnumerable<InterviewSchedule>> GetTodayInterview()
        {
            return await _dbContext.InterviewSchedules
				.Where(i => i.DueDate.Date == DateTime.Now.Date && i.Status != InterviewScheduleStatus.Interviewed && i.Status != InterviewScheduleStatus.Cancelled)
				.ToListAsync();
        }

        public async Task<IEnumerable<InterviewSchedule>> GetUpcomingInterview()
        {
            return await _dbContext.InterviewSchedules
				.Where(i => i.DueDate.Date > DateTime.Now.Date && i.Status != InterviewScheduleStatus.Interviewed && i.Status != InterviewScheduleStatus.Cancelled)
				.OrderBy(i => i.DueDate)
				.Take(5)
				.ToListAsync();
        }

        public async Task<IEnumerable<InterviewSchedule>> UnhandleInterview()
        {
			return await _dbContext.InterviewSchedules
				.Where(i => i.DueDate.Date < DateTime.Now.Date && i.Status != InterviewScheduleStatus.Interviewed && i.Status != InterviewScheduleStatus.Cancelled)
				.ToListAsync();
        }

        public async Task<IEnumerable<InterviewSchedule>> TomorrowInterview()
        {
            return await _dbContext.InterviewSchedules
                .Where(i => i.DueDate.Date == DateTime.Now.AddDays(1).Date && i.Status != InterviewScheduleStatus.Interviewed && i.Status != InterviewScheduleStatus.Cancelled)
                .ToListAsync();
        }
    }
}




