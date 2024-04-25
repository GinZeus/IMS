using Diacritics.Extensions;
using IMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace IMS.CoreServices.Implementations
{
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IOptions<IdentityOptions> _identityOptions;
		private readonly IEmailService _emailService;

		public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> identityOptions, IEmailService emailService)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_identityOptions = identityOptions;
			_emailService = emailService;
		}

		public async Task CreateUser(User user, string roleName)
		{
			ArgumentNullException.ThrowIfNull(user);

			if (!await _roleManager.RoleExistsAsync(roleName))
			{
				throw new ArgumentException("Role does not exist.", nameof(roleName));
			}

			// Get PasswordOptions configuration, and uses it to generate random password
			string password = Utilities.PasswordUtils.GenerateRandomPassword(_identityOptions.Value.Password);

			// generate random username
			user.UserName = GenerateUsername(user.FullName);

			IdentityResult result = await _userManager.CreateAsync(user, password);

			// send password to user's email
			_emailService.SendAccountInformation(user, password);

			if (result.Succeeded)
			{
				IdentityResult roleResult = await _userManager.AddToRoleAsync(user, roleName);
				if (!roleResult.Succeeded)
				{
					throw new Exception("Failed to add role to user");
				}
			}
			else
			{
				throw new Exception($"Failed to create new user, Result {result.ToString()}");
			}
		}


		public IEnumerable<User> GetAllUsers()
		{
			return _userManager.Users;
		}

		private string ConvertToTitleCase(string fullName)
		{
			TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
			string titleCaseName = textInfo.ToTitleCase(fullName);
			return titleCaseName;
		}

		public string ExtractPartsName(string fullName)
		{
			StringBuilder partsName = new StringBuilder();
			string[] parts = fullName.Split(' ');
			string firstName = parts[parts.Length - 1];
			partsName.Append(firstName);
			for (int i = 0; i < parts.Length - 1; i++)
			{
				partsName.Append(parts[i][0]);
			}

			return partsName.ToString();
		}

		public string GenerateUsername(string fullName)
		{
			// Replace all diacritic characters
			fullName = fullName.RemoveDiacritics();
			// Convert FullName to TitleCase (so that dts and Dts would result in same PartsName)
			fullName = ConvertToTitleCase(fullName);
			string partsName = ExtractPartsName(fullName);

			// regex pattern to query for
			string pattern = "^" + partsName + "\\d+$";
			// query for how many username patterns like this in the database
			int incrementalValue = CountUsernamePattern(pattern);
			// generate unique username
			string username = partsName + (++incrementalValue);

			return username;
		}

		public int CountUsernamePattern(string regex)
		{
			// SQL Server does not support regex, therefore we have to queries all users first
			IEnumerable<string> userList = _userManager.Users.Select(u => u.UserName);
			// and then perform regex matching from C# side
			int count = userList.Where(name => Regex.IsMatch(name, regex)).Count();
			return count;
		}

		public async Task<IEnumerable<User>> GetAllRecruiter()
		{
			try
			{
				return await _userManager.GetUsersInRoleAsync("Recruiter");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Enumerable.Empty<User>();
			}
		}

		public async Task<IEnumerable<User>> GetAllInterviewer()
		{
			try
			{
				return await _userManager.GetUsersInRoleAsync("Interviewer");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Enumerable.Empty<User>();
			}
		}

		public async Task<User?> GetUser(string userId)
		{
			User? user = await _userManager.FindByIdAsync(userId);
			return user;
		}

		public async Task<IEnumerable<User>> GetAllManager()
		{
			try
			{
				return await _userManager.GetUsersInRoleAsync("Manager");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Enumerable.Empty<User>();
			}
		}

		public async Task UpdateUser(User user)
		{
			if (user == null)
				throw new ArgumentNullException(nameof(user));

			try
			{
				await _userManager.UpdateAsync(user);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}
		public int GetActiveUsersCount()
		{
			return _userManager.Users.Where(u => u.IsActive).Count();
		}
		public int GetActiveUsersInactiveCount()
		{
			return _userManager.Users.Where(u => u.IsActive == false).Count();
		}
		public async Task<int> CountAllInterviewer()
		{
			var interviewers = await _userManager.GetUsersInRoleAsync("Interviewer");
			return interviewers.Count;
		}
	}
}
