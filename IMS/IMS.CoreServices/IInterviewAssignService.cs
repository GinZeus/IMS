using IMS.Models;

namespace IMS.CoreServices
{
	public interface IInterviewAssignService
	{
		public Task CreateInterviewAssign(InterviewAssign interviewAssign);

		public Task<IEnumerable<int>> GetInterviewsByInterviewerId(string interviewerId);

        public Task<IEnumerable<string>> GetInterviewersByInterviewId (int interviewId);

		public Task RemoveInterviewAssign(int interviewId);

	}
}
