using IMS.Models;

namespace IMS.CoreServices
{
	public interface IUserService
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="user"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		Task CreateUser(User user, string role);

		//IEnumerable<User> GetUsersInRole(string roleName);

		/// <summary>
		/// Extract name from user's FullName. For example, Ha Quang Thang would be ThangHQ. 
		/// This name will be combined with incremental value to generate unique username, eg. ThangHQ8
		/// </summary>
		/// <param name="fullName">User's full name</param>
		/// <returns>PartsName</returns>
		string ExtractPartsName(string fullName);

		/// <summary>
		/// Count how many username has pattern regex in the DB
		/// </summary>
		/// <param name="regex"></param>
		/// <returns></returns>
		int CountUsernamePattern(string regex);

		/// <summary>
		/// Generate unique username for an account. This method works by extract PartsName from FullName, and then combine with
		/// an incemental value
		/// TODO: Need to handle the case when database's data inconsistence. That is, eg. there are 2 ThangHQ in database, but
		/// there is username is ThangHQ8 (value > count number of ThangHQ)
		/// </summary>
		/// <param name="fullName">Account's FullName</param>
		/// <returns></returns>
		string GenerateUsername(string fullName);

		IEnumerable<User> GetAllUsers();

        Task<IEnumerable<User>> GetAllRecruiter();

        Task<IEnumerable<User>> GetAllInterviewer();

		Task<User?> GetUser(string userId);

        Task<IEnumerable<User>> GetAllManager();


		Task UpdateUser(User user);

		int GetActiveUsersCount();

		int GetActiveUsersInactiveCount();

		Task<int> CountAllInterviewer();
    }
}
