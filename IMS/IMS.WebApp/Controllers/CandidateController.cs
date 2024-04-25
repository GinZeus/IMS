using DocumentFormat.OpenXml.Spreadsheet;
using IMS.CoreServices;
using IMS.Models;
using IMS.Utilities.Constants;
using IMS.WebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace IMS.WebApp.Controllers
{
	[Authorize]
	public class CandidateController : Controller
	{
		private readonly ICandidateService _candidateService;
		private readonly ISkillService _skillService;
		private readonly IPositionService _positionService;
		private readonly IAcademicLevelService _academicLevelService;
		private readonly IUserService _userService;
		private readonly UserManager<User> _userManager;
		private readonly ICandidateSkillService _candidateSkillService;
		private readonly IInterviewAssignService _interviewAssignService;
		private readonly IInterviewScheduleService _interviewScheduleService;
		private readonly IOfferService _offerService;

		public CandidateController(ICandidateService candidateService, ISkillService skillService, IPositionService positionService,
				IAcademicLevelService academicLevelService, IUserService userService, UserManager<User> userManager,
				ICandidateSkillService candidateSkillService, IInterviewAssignService interviewAssignService,
				IInterviewScheduleService interviewScheduleService, IOfferService offerService)
		{
			_candidateService = candidateService;
			_skillService = skillService;
			_positionService = positionService;
			_academicLevelService = academicLevelService;
			_userService = userService;
			_userManager = userManager;
			_candidateSkillService = candidateSkillService;
			_interviewAssignService = interviewAssignService;
			_interviewScheduleService = interviewScheduleService;
			_offerService = offerService;
		}

		public async Task<IActionResult> Index()
		{
			if (!User.IsInRole("Interviewer"))
			{
				var list = _candidateService.GetAvailableCandidates();
				var candidateViewModels = new List<CandidateViewModel>();

				foreach (var candidate in list)
				{
					CandidateViewModel candidateViewModel = new CandidateViewModel
					{
						Candidate = candidate,
						Position = candidate.Position,
						User = candidate.Recruiter
					};

					candidateViewModels.Add(candidateViewModel);
				}
				return View(candidateViewModels);
			}
			else
			{
				var userId = _userManager.GetUserId(HttpContext.User);
				if (userId == null)
				{
					return NotFound();
				}
				else
				{
					var interviewSchedules = await _interviewAssignService.GetInterviewsByInterviewerId(userId);
					var candidateListId = new List<int>();
					var candidateViewModels = new List<CandidateViewModel>();
					foreach (int scheduleId in interviewSchedules)
					{
						var candidateId = _interviewScheduleService.getDetailInterviewById(scheduleId).CandidateId;
						candidateListId.Add(candidateId);
					}
					candidateListId = candidateListId.Distinct().ToList();
					foreach (int candidateId in candidateListId)
					{
						var candidate = _candidateService.GetCandidate(candidateId);
						if (candidate != null)
						{
                            CandidateViewModel candidateViewModel = new CandidateViewModel
                            {
                                Candidate = candidate,
                                Position = candidate.Position,
                                User = candidate.Recruiter
                            };
                            candidateViewModels.Add(candidateViewModel);
                        }
						
						
					}

					return View(candidateViewModels);
				}
			}
		}

		[Authorize(Roles = "Recruiter, Manager, Admin")]
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			CreateCandidateVM createCandidateVM = new CreateCandidateVM();

			createCandidateVM.Skills = _skillService.GetSkills();
			createCandidateVM.Positions = _positionService.GetPositions();
			createCandidateVM.AcademicLevels = _academicLevelService.GetAcademicLevels();
			createCandidateVM.RecruiterList = await _userService.GetAllRecruiter();
			createCandidateVM.CandidateStatuses = new List<CandidateStatus> { CandidateStatus.Open, CandidateStatus.Banned };
			createCandidateVM.Genders = new List<Gender> { Gender.Male, Gender.Female, Gender.Other };
			return View(createCandidateVM);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id">candidate's id</param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			Candidate? candidate = _candidateService.GetCandidate(id);
			if (candidate == null || candidate.IsDeleted == true)
			{
				ErrorViewModel errorVm = new ErrorViewModel
				{
					ErrorMessage = "Candidate does not exist",
					RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
				};
				return View("~/Views/Shared/404.cshtml", errorVm);
			}

			// pollute data
			CandidateDetailsVM viewModel = new CandidateDetailsVM
			{
				CandidateId = id,
				DOB = candidate.DOB,
				PhoneNumber = candidate.PhoneNumber,
				Email = candidate.Email,
				Address = candidate.Address,
				Gender = candidate.Gender,
				FullName = candidate.FullName,
				CVAttachmentName = $"{candidate.FullName}-CV",
				CvAvailable = candidate.CVAttachment != null,
				PositionName = _positionService.GetPositionName(candidate.PositionId),
				StatusName = candidate.Status,
				YearOfExp = candidate.YearOfExp,
				AcademicLevelName = _academicLevelService.GetAcademicLevelName(candidate.AcademicLevelId),
				Note = candidate.Note,
				LastUpdatedBy = candidate.LastUpdatedBy,
				LastUpdatedOn = candidate.LastUpdatedOn,
				CreatedOn = candidate.CreatedOn
			};

			viewModel.SkillsName = _candidateService.GetCandidateSkill(id).Select(s => s.SkillName);
			User recruiter = await _userService.GetUser(candidate.RecruiterId);
			viewModel.RecruiterName = $"{recruiter.FullName} ({recruiter.UserName})";
			return View(viewModel);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id">Candidate's ID</param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult DownloadCV(int id)
		{
			Candidate? candidate = _candidateService.GetCandidate(id);

			if (candidate == null || candidate.CVMimeType == null || candidate.CVAttachment == null)
			{
				ErrorViewModel errorVm = new ErrorViewModel
				{
					ErrorMessage = "CV not found for this candidate",
					RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
				};
				return View("~/Views/Shared/404.cshtml", errorVm);
			}

			byte[] cv = candidate.CVAttachment;
			string contentType = candidate.CVMimeType;
			string candidateName = candidate.FullName;
			string extension = string.Empty;
			if (contentType == "application/pdf")
				extension = "pdf";
			else
				extension = "docx";
			return File(cv, contentType, $"{candidateName}.{extension}");
		}

		[Authorize(Roles = "Recruiter, Manager, Admin")]
		[HttpPost]
		public async Task<IActionResult> Create(CreateCandidateVM viewModel)
		{
			// remove unnecessary properties
			ModelState.Remove(nameof(CreateCandidateVM.Genders));
			ModelState.Remove(nameof(CreateCandidateVM.Positions));
			ModelState.Remove(nameof(CreateCandidateVM.Skills));
			ModelState.Remove(nameof(CreateCandidateVM.RecruiterList));
			ModelState.Remove(nameof(CreateCandidateVM.CandidateStatuses));
			ModelState.Remove(nameof(CreateCandidateVM.AcademicLevels));
			ModelState.Remove(nameof(CreateCandidateVM.LastUpdatedBy));


			if (ModelState.IsValid)
			{
				byte[] Cv = null;
				string CvMime = null;
				// handling IFormFile
				using (MemoryStream memoryStream = new MemoryStream())
				{
					await viewModel.CVAttachment.CopyToAsync(memoryStream);

					// Upload the file if less than 5MB
					if (memoryStream.Length < 5 * 1024 * 1024)
					{
						Cv = memoryStream.ToArray();
						CvMime = viewModel.CVAttachment.ContentType;
					}
				}

				// pollute data from view model
				Candidate candidate = new Candidate
				{
					PositionId = viewModel.SelectedPositionId.Value,
					AcademicLevelId = viewModel.SelectedAcademicLevelId.Value,
					RecruiterId = viewModel.SelectedRecruiterId,
					FullName = viewModel.FullName,
					DOB = viewModel.DOB.Value,
					Gender = viewModel.SelectedGender.Value,
					Email = viewModel.Email,
					Address = viewModel.Address,
					PhoneNumber = viewModel.PhoneNumber,
					YearOfExp = viewModel.YearOfExp,
					CVAttachment = Cv,
					CVMimeType = CvMime,
					Note = viewModel.Note,
					Status = viewModel.SelectedStatus.Value,
					CreatedOn = DateTime.Now,
					LastUpdatedOn = DateTime.Now,
					LastUpdatedBy = _userManager.GetUserName(User)
				};

				try
				{
					// add candidate to Candidate table
					_candidateService.CreateCandidate(candidate);
					// add candidate's skill to CandidateSkills table
					_candidateSkillService.AddSkillsForCandidate(candidate.CandidateId, viewModel.SelectedSkillId);
					//Display notification
					TempData["success"] = "Create candidate successfully";
					// candidate added successfully
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					// Log exception details for debugging
					Console.WriteLine(ex.Message);

					//Display error message
					TempData["error"] = "Failed to create candidate";
				}
			}

			CreateCandidateVM createCandidateVM = new CreateCandidateVM();

			createCandidateVM.Skills = _skillService.GetSkills();
			createCandidateVM.Positions = _positionService.GetPositions();
			createCandidateVM.AcademicLevels = _academicLevelService.GetAcademicLevels();
			createCandidateVM.RecruiterList = await _userService.GetAllRecruiter();
			createCandidateVM.CandidateStatuses = new List<CandidateStatus> { CandidateStatus.Open, CandidateStatus.Banned };
			createCandidateVM.Genders = new List<Gender> { Gender.Male, Gender.Female, Gender.Other };

			return View(createCandidateVM);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var candidate = _candidateService.GetCandidate(id);

			if (candidate == null || candidate.IsDeleted == true)
			{
				ErrorViewModel errorVm = new ErrorViewModel
				{
					ErrorMessage = "Candidate does not exist",
					RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
				};
				return View("~/Views/Shared/404.cshtml", errorVm);
			}

			var editCandidateVM = new EditCandidateVM
			{
				FullName = candidate.FullName,
				Email = candidate.Email,
				DOB = candidate.DOB,
				Address = candidate.Address ?? string.Empty,
				PhoneNumber = candidate.PhoneNumber ?? string.Empty,
				Genders = Enum.GetValues(typeof(Gender)).Cast<Gender>(),
				SelectedGender = candidate.Gender,
				Positions = _positionService.GetPositions(),
				SelectedPositionId = candidate.PositionId,
				AcademicLevels = _academicLevelService.GetAcademicLevels(),
				SelectedAcademicLevelId = candidate.AcademicLevelId,
				Skills = _skillService.GetSkills(),
				SelectedSkillsId = _candidateSkillService.GetSkillsByCandidateId(id),
				RecruiterList = await _userService.GetAllRecruiter(),
				SelectedRecruiterId = candidate.RecruiterId,
				YearOfExp = candidate.YearOfExp,
				SelectedStatus = candidate.Status,
				Note = candidate.Note,
			};

			return View(editCandidateVM);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditCandidateVM editCandidateVM)
		{
			// remove unnecessary properties
			ModelState.Remove(nameof(EditCandidateVM.Genders));
			ModelState.Remove(nameof(EditCandidateVM.Positions));
			ModelState.Remove(nameof(EditCandidateVM.Skills));
			ModelState.Remove(nameof(EditCandidateVM.RecruiterList));
			ModelState.Remove(nameof(EditCandidateVM.CandidateStatuses));
			ModelState.Remove(nameof(EditCandidateVM.AcademicLevels));
			ModelState.Remove(nameof(EditCandidateVM.LastUpdatedBy));

			if (ModelState.IsValid)
			{
				try
				{
					var candidateToUpdate = _candidateService.GetCandidate(id);

					if (candidateToUpdate == null)
					{
						return NotFound();
					}

					// Update core candidate details
					candidateToUpdate.FullName = editCandidateVM.FullName;
					candidateToUpdate.Email = editCandidateVM.Email;
					candidateToUpdate.DOB = editCandidateVM.DOB.Value;
					candidateToUpdate.Address = editCandidateVM.Address;
					candidateToUpdate.PhoneNumber = editCandidateVM.PhoneNumber;
					candidateToUpdate.Gender = editCandidateVM.SelectedGender;
					candidateToUpdate.PositionId = editCandidateVM.SelectedPositionId;
					candidateToUpdate.AcademicLevelId = editCandidateVM.SelectedAcademicLevelId;
					candidateToUpdate.RecruiterId = editCandidateVM.SelectedRecruiterId;
					candidateToUpdate.YearOfExp = editCandidateVM.YearOfExp;
					candidateToUpdate.Note = editCandidateVM.Note;

					// handling IFormFile
					if (editCandidateVM.CVAttachment != null)
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							await editCandidateVM.CVAttachment.CopyToAsync(memoryStream);

							// Upload the file if less than 2MB
							if (memoryStream.Length < 2097152)
							{
								candidateToUpdate.CVAttachment = memoryStream.ToArray();
								candidateToUpdate.CVMimeType = editCandidateVM.CVAttachment.ContentType;
							}
						}
					}

					// Delete existing related data (Skills)
					await _candidateSkillService.DeleteCandidateSkill(id);

					// Add newly selected related data
					foreach (var skill in editCandidateVM.SelectedSkillsId)
					{
						CandidateSkill candidateSkill = new CandidateSkill();
						candidateSkill.SkillId = skill;
						candidateSkill.CandidateId = id;
						await _candidateSkillService.CreateCandidateSkill(candidateSkill);
					}

					await _candidateService.UpdateCandidate(candidateToUpdate);
					TempData["success"] = "Change has been successfully updated";

					return RedirectToAction("Index", "Candidate");

				}
				catch (Exception ex)
				{
					// Log exception details for debugging
					Console.WriteLine(ex.Message);

					// Display error message
					TempData["error"] = "Failed to update change";
				}
			}

			// Re-fetch lists in case of model errors
			editCandidateVM.Genders = Enum.GetValues(typeof(Gender)).Cast<Gender>();
			editCandidateVM.Positions = _positionService.GetPositions();
			editCandidateVM.AcademicLevels = _academicLevelService.GetAcademicLevels();
			editCandidateVM.Skills = _skillService.GetSkills();
			editCandidateVM.RecruiterList = await _userService.GetAllRecruiter();

			return View(editCandidateVM);
		}

		[Authorize(Roles = "Admin,Recruiter,Manager")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				Candidate? candidate = _candidateService.GetCandidate(id);

				if (candidate == null)
				{
					return NotFound();
				}

				await _candidateService.DeleteCandidate(candidate);
				TempData["success"] = "Delete candidate successfully";
				return RedirectToAction(nameof(CandidateController.Index));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				TempData["error"] = "Delete candidate failed";
			}
			// Return to Index
			return RedirectToAction(nameof(CandidateController.Index));

		}

		[Authorize(Roles = "Admin,Recruiter,Manager")]
		public async Task<IActionResult> Ban(int id)
		{
			try
			{
				// status to ban
				_candidateService.UpdateCandidateStatus(id, CandidateStatus.Banned);

				// All exist interivew of that candidate is canceled
				var interview = _interviewScheduleService.GetInterviewSchedulesByCandidateId(id);
				foreach (var interviewSchedule in interview)
				{
					interviewSchedule.Status = InterviewScheduleStatus.Cancelled;
					await _interviewScheduleService.UpdateInterviewSchedule(interviewSchedule);
				}

				// All offer of that candidate also cancel
				var offers = await _offerService.GetOffersByCandidateId(id);
				foreach (var offer in offers)
				{
					offer.Status = OfferStatus.Cancelled;
					await _offerService.UpdateOffer(offer);
				}

				TempData["success"] = "Candidate banned successfully";
				return RedirectToAction(nameof(CandidateController.Index));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			TempData["error"] = "Failed to ban candidate";
			// Return to Index
			return RedirectToAction(nameof(CandidateController.Index));

		}

		[Authorize(Roles = "Admin,Recruiter,Manager")]
		public async Task<IActionResult> Unban(int id)
		{
			try
			{
				_candidateService.UpdateCandidateStatus(id, CandidateStatus.Open);
				TempData["success"] = "Unban candidate successfully";
				return RedirectToAction(nameof(CandidateController.Index));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				TempData["error"] = "Failed to unban candidate";
			}
			// Return to Index
			return RedirectToAction(nameof(CandidateController.Index));

		}
	}
}
