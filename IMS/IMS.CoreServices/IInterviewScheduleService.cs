using IMS.Models;
using IMS.Utilities.Constants;

namespace IMS.CoreServices
{
	public interface IInterviewScheduleService
	{
		public Task<int> CreateInterviewSchedule(InterviewSchedule interviewSchedule);
		public IEnumerable<InterviewSchedule> GetAllValidInterviewSchedules();
		public IEnumerable<InterviewSchedule> FilterInterview(InterviewScheduleStatus? status, string? InterviewerId);
		public Task<InterviewSchedule> GetInterviewScheduleById(int InterviewScheduleId);
		public Task UpdateInterviewSchedule(InterviewSchedule interviewSchedule);
		public IEnumerable<InterviewSchedule> GetInterviewSchedulesByCandidateId(int? candidateId);
		public Task<IEnumerable<InterviewSchedule>> GetInterviewsByStatus(InterviewScheduleStatus status);
		public InterviewSchedule getDetailInterviewById(int InterviewscheduleId);
        public Task<IEnumerable<InterviewSchedule>> GetTodayInterview();
        public Task<IEnumerable<InterviewSchedule>> GetUpcomingInterview();
		public Task<IEnumerable<InterviewSchedule>> UnhandleInterview();
        public Task<IEnumerable<InterviewSchedule>> TomorrowInterview();



    }
}
