using IMS.Models;
using IMS.Utilities.Constants;

namespace IMS.CoreServices
{
	public interface IJobInterface
	{
		public IEnumerable<Job> GetAllJobs();
		public Task<int> CreateJob(Job job);
		Task CreateJobs(IEnumerable<Job> jobs);
		public IEnumerable<Job> GetJobsByStatus(JobStatus? status);
		public Job GetJobById(int id);
		public Task UpdateJob(Job job);
		public Task<IEnumerable<Job>> GetUnclosedJobs();
		/// <summary>
		/// Import jobs from byte stream (Excel file) and add to database
		/// </summary>
		/// <param name="stream">Uploaded excel file converted to stream</param>
		/// <param name="lastUpdateBy"></param>
		/// <returns></returns>
		Task ImportJob(Stream stream, string? lastUpdateBy = null);
		Task<int> DeleteJob(Job job);
	}
}
