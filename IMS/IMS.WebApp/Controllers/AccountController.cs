using IMS.CoreServices;
using IMS.Models;
using IMS.Utilities.Constants;
using IMS.WebApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text;


namespace IMS.WebApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUserService _userService;
		private readonly IDepartmentService _departmentService;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly ILogger<AccountController> _logger;
		private readonly IRecoveryToken _recoveryToken;
		private readonly IEmailService _emailService;

		public AccountController(IUserService userService, IDepartmentService departmentService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger, IRecoveryToken recoveryToken, IEmailService emailService)
		{
			_userService = userService;
			_departmentService = departmentService;
			_roleManager = roleManager;
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_recoveryToken = recoveryToken;
			_emailService = emailService;
		}

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public string? ReturnUrl { get; set; }

		[TempData]
		public string? ErrorMessage { get; set; }

		// GET: /Account/Login
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Login(string? returnUrl = null)
		{
			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				ModelState.AddModelError(string.Empty, ErrorMessage);
			}

			returnUrl ??= Url.Content("~/");

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			ReturnUrl = returnUrl;

			return View();
		}

		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginVM model, string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");

			var externalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
			ExternalLogins = externalLogins.ToList();

			if (ModelState.IsValid)
			{
				var user = await _signInManager.UserManager.FindByNameAsync(model.Username);
				if (user != null && !user.IsActive)
				{
					ModelState.AddModelError(string.Empty, "Your Account has been De-Active please contact Adminstrator for more information.");
					_logger.LogInformation("Account {username} attempted to login, but this account has been deactivated.", model.Username);
					return View();
				}

				var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					_logger.LogInformation("User {Username} logged in.", model.Username);
					return LocalRedirect(returnUrl);
				}
				if (result.RequiresTwoFactor)
				{
					// DID NOT IMPLEMENT YET WHEN CONVERTING TO MVC. We can clearly notice that RedirectToPage is not a method of MVC
					return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
				}
				if (result.IsLockedOut)
				{
					_logger.LogWarning("User account locked out.");
					// DID NOT IMPLEMENT YET WHEN CONVERTING TO MVC. We can clearly notice that RedirectToPage is not a method of MVC
					return RedirectToPage("./Lockout");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					_logger.LogInformation("Invalid login attempt for account {username}", model.Username);
					return View();
				}
			}
			return View(model);
		}
		[HttpPost]
		[Authorize] // Only logged in user may logout
		public async Task<IActionResult> Logout(string? returnUrl = null)
		{
			string? loggedUsername = User?.Identity?.Name;
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User {username} logged out.", loggedUsername);
			if (returnUrl != null)
			{
				return LocalRedirect(returnUrl);
			}
			else
			{
				// This needs to be a redirect so that the browser performs a new
				// request and the identity for the user gets updated.
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpGet]
		[Authorize]
		public IActionResult AccessDenied()
		{
			return View();
		}

		// GET: /Account/ForgotPassword
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		// POST: /Account/ForgotPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
				{
					// Don't reveal that the user does not exist or is not confirmed
					return RedirectToAction(nameof(ForgotPasswordConfirmation));
				}

				_logger.LogInformation("A request to reset password for {email} has been made", model.Email);

				// User already have recovery token then remove
				if (_recoveryToken.GetRecoveryTokenByUid(user.Id) != null)
				{
					_recoveryToken.RemoveRecoveryToken(_recoveryToken.GetRecoveryTokenByUid(user.Id));
				}

				// For more information on how to enable account confirmation and password reset please
				// visit https://go.microsoft.com/fwlink/?LinkID=532713
				var code = await _userManager.GeneratePasswordResetTokenAsync(user);
				// Token usually contains +, etc. special symbols which is not safe when transmit, therefore we Base64 encoded it
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

				//Expire time 24 hrs
				DateTime expireTime = DateTime.Now.AddHours(24);

				var recovery = new RecoveryToken
				{
					UserID = user.Id,
					User = user,
					ExpiredTime = expireTime,
					Code = code
				};

				_recoveryToken.AddRecoveryToken(recovery);

				// Create Url
				var callbackUrl = Url.Action(
					"ResetPassword",
					"Account",
					values: new { code },
					protocol: Request.Scheme);

				_emailService.SendForgotPasswordLink(HtmlEncoder.Default.Encode(callbackUrl), model.Email);
				return RedirectToAction(nameof(ForgotPasswordConfirmation));
			}

			return View(model);
		}

		// GET: /Account/ForgotPasswordConfirmation
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPassword(string? code = null)
		{
			RecoveryToken recoveryTokens = _recoveryToken.GetRecoveryTokenByCode(code);

			if (recoveryTokens == null || recoveryTokens.ExpiredTime < DateTime.UtcNow || code == null)
			{
				return RedirectToAction(nameof(Error));
			}

			ResetPasswordModel model = new ResetPasswordModel
			{
				UserID = recoveryTokens.UserID,
				Code = code
			};

			return View(model);
		}

		// POST: /Account/ResetPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByIdAsync(model.UserID);
			if (user == null)
			{
				// Don't reveal that the user does not exist
				_logger.LogInformation("Reset password failed, UserID {id} does not exist", model.UserID);

				return RedirectToAction(nameof(ResetPasswordConfirmation));
			}

			string decodedBase64Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
			var result = await _userManager.ResetPasswordAsync(user, decodedBase64Token, model.Password);
			if (result.Succeeded)
			{
				// remove token if password reset success
				_recoveryToken.RemoveRecoveryToken(_recoveryToken.GetRecoveryTokenByUid(user.Id));
				_logger.LogInformation("Password for UserId {id} has been reset successfully", model.UserID);

				return RedirectToAction(nameof(ResetPasswordConfirmation));
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}

		// GET: /Account/ResetPasswordConfirmation
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		// GET: /Account/Error
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Error()
		{
			return View();
		}

		[Authorize(Roles = "Admin")]
		public IActionResult Index()
		{
			UserViewModel userViewModel = new UserViewModel();
			userViewModel.Roles = _roleManager.Roles;
			userViewModel.User = _userService.GetAllUsers();

			return View(userViewModel);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> Detail(string id)
		{
			var user = await _userService.GetUser(id);
			var department = _departmentService.GetDepartmentById(user.DepartmentId);
			if (user == null)
			{
				return NotFound();
			}

			var userViewModel = new DetailUserViewModel
			{
				User = user,
				Department = department,
				Role = (await _userManager.GetRolesAsync(user))[0]
			};

			string currentAdmin = User.Identity.Name;
			_logger.LogInformation("Admin {adm} view detailed information of {username}", currentAdmin, user.UserName);

			return View(userViewModel);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public IActionResult Create()
		{
			CreateAccountVM createAccountVM = new CreateAccountVM();
			createAccountVM.Departments = _departmentService.GetDepartments();
			createAccountVM.Roles = _roleManager.Roles;
			createAccountVM.Genders = new List<Gender>() { Gender.Male, Gender.Female, Gender.Other };

			return View(createAccountVM);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			// Select list for Departments,Roles
			EditUserVM editUserVM = new EditUserVM();
			editUserVM.Departments = _departmentService.GetDepartments();
			editUserVM.Roles = _roleManager.Roles;

			// Get Existed User
			var existedUser = await _userService.GetUser(id);

			try
			{
				// Get User Department
				editUserVM.SelectedDepartment = _departmentService.GetDepartmentById(existedUser.DepartmentId).DepartmentId;
				editUserVM.SelectedGender = existedUser.Gender;
				editUserVM.SelectedRole = (await _userManager.GetRolesAsync(existedUser))[0];
				editUserVM.FullName = existedUser.FullName;
				editUserVM.Address = existedUser.Address;
				editUserVM.PhoneNumber = existedUser.PhoneNumber;
				editUserVM.DOB = existedUser.DOB;
				editUserVM.Note = existedUser.Note;
				editUserVM.Email = existedUser.Email;
				editUserVM.IsActive = existedUser.IsActive;
				return View(editUserVM);
			}

			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return RedirectToAction(nameof(AccountController.Index), "Index");
		}

		[AcceptVerbs("GET")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> IsEmailExist(string email)
		{
			if (await _userManager.FindByEmailAsync(email) != null)
			{
				return Json($"Email {email} is already in use");
			}
			return Json(true);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> Create(CreateAccountVM accountVM)
		{
			// Remove unnecessary properties from VM
			ModelState.Remove(nameof(CreateAccountVM.Departments));
			ModelState.Remove(nameof(CreateAccountVM.Roles));
			ModelState.Remove(nameof(CreateAccountVM.Genders));

			if (ModelState.IsValid)
			{
				User user = new User();
				// pollute data to user
				user.FullName = accountVM.FullName;
				user.Email = accountVM.Email;
				user.Gender = accountVM.SelectedGender.Value;
				user.Address = accountVM.Address;
				user.DOB = accountVM.DOB.Value;
				user.PhoneNumber = accountVM.PhoneNumber;
				user.DepartmentId = accountVM.SelectedDepartmentId.Value;
				user.Note = accountVM.Note;

				// Set confirmation status
				user.IsActive = true;
				user.EmailConfirmed = true; // NEED TO IMPLEMENT LATER

				try
				{
					await _userService.CreateUser(user, accountVM.SelectedRole);

					// Account added successfully
					string currentAdmin = User.Identity.Name;
					_logger.LogInformation("Account admin {username} has created a new account with email {email}", currentAdmin, user.Email);

					return RedirectToAction(nameof(Index), "Account");
				}
				catch (ArgumentException argEx)
				{
					ModelState.AddModelError("argumentError", argEx.Message);
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("generalExceptionError", ex.Message);
				}
			}

			// Load data for VM, then return Create page again to display error
			accountVM.Departments = _departmentService.GetDepartments();
			accountVM.Roles = _roleManager.Roles;
			accountVM.Genders = new List<Gender>() { Gender.Male, Gender.Female, Gender.Other };

			return View(accountVM);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> Edit(string id, EditUserVM editUserVM)
		{
			ModelState.Remove(nameof(EditUserVM.SelectedDepartment));
			ModelState.Remove(nameof(EditUserVM.SelectedRole));
			ModelState.Remove(nameof(EditUserVM.SelectedGender));
			ModelState.Remove(nameof(EditUserVM.Departments));
			ModelState.Remove(nameof(EditUserVM.Roles));
			ModelState.Remove(nameof(EditUserVM.Genders));

			if (ModelState.IsValid)
			{
				try
				{
					var user = await _userService.GetUser(id);

					if (user == null)
					{
						return NotFound(); // Or handle the case where the user doesn't exist
					}

					user.FullName = editUserVM.FullName;
					user.Address = editUserVM.Address;
					user.DOB = editUserVM.DOB.Value;
					user.Email = editUserVM.Email;
					user.PhoneNumber = editUserVM.PhoneNumber;
					user.Gender = editUserVM.SelectedGender;
					var selectedRole = editUserVM.SelectedRole; // assuming SelectedRole is a string
					if (!string.IsNullOrEmpty(selectedRole))
					{
						await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
						await _userManager.AddToRoleAsync(user, selectedRole);
					}
					user.DepartmentId = editUserVM.SelectedDepartment;
					user.Note = editUserVM.Note;

					// Save changes to the user
					await _userService.UpdateUser(user);

					// Set success message
					TempData["success"] = "Update user successfully";

					string currentAdmin = User.Identity.Name;
					_logger.LogInformation("Admin {adm} modified information of {username}", currentAdmin, user.UserName);

					return RedirectToAction(nameof(Index));
				}
				catch (ArgumentException)
				{
					// Set error message for argument error
					TempData["error"] = "Failed to update user. Please try again";
				}
				catch (Exception ex)
				{
					// Set error message for general exception
					TempData["ErrorMessage"] = "Failed to update: " + ex.Message;
				}
			}

			// If there are errors, repopulate select lists and return to the view
			editUserVM.Roles = await _roleManager.Roles.ToListAsync();
			editUserVM.Departments = _departmentService.GetDepartments();

			return View(editUserVM);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> UpdateStatus(string id)
		{
			var user = await _userService.GetUser(id);

			if (user == null)
			{
				return NotFound(); // Or handle the case where the user doesn't exist
			}

			if (user.UserName == User.Identity.Name)
			{
				TempData["error"] = "You cannot activate or deactivate your own account.";
				return RedirectToAction("Detail", new { id });
			}

			user.IsActive = !user.IsActive; // Toggle the IsActive status

			try
			{
				await _userService.UpdateUser(user);
				TempData["success"] = "Change has been successfully updated";
				string currentAdmin = User.Identity.Name;
				_logger.LogInformation("Admin {adm} has change status of {username} from {from} to {to}", currentAdmin, user.UserName, !user.IsActive, user.IsActive);

				return RedirectToAction("Detail", new { id }); // Redirect back to the detail view
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("generalExceptionError", ex.Message);
				return RedirectToAction("Detail", new { id }); // Redirect back to the detail view with error
			}
		}
	}
}