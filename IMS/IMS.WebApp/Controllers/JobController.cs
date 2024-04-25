using IMS.CoreServices;
using IMS.CoreServices.Implementations;
using IMS.Models;
using IMS.Utilities.Constants;
using IMS.WebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IMS.WebApp.Controllers
{
	[Authorize]
	public class JobController : Controller
	{
		private readonly IJobInterface _jobService;
		private readonly IBenefitService _benefitService;
		private readonly ILevelService _levelService;
		private readonly ISkillService _skillService;
		private readonly IJobBenefitsInterface _jobBenefitsService;
		private readonly IJobLevelsInterface _jobLevelsService;
		private readonly IJobSkillsInterface _jobSkillsService;

		public JobController(IJobInterface jobService, IBenefitService benefitService, ILevelService levelService, ISkillService skillService, IJobBenefitsInterface jobBenefitService, IJobSkillsInterface jobSkillsService, IJobLevelsInterface jobLevelsService)
		{
			_jobService = jobService;
			_benefitService = benefitService;
			_levelService = levelService;
			_skillService = skillService;
			_jobBenefitsService = jobBenefitService;
			_jobLevelsService = jobLevelsService;
			_jobSkillsService = jobSkillsService;

		}
		public IActionResult Index()
		{
			var list = _jobService.GetAllJobs();
			var jobViewModels = new List<JobViewModel>();
			foreach (var job in list)
			{
				JobViewModel jobViewModel = new JobViewModel
				{
					Job = job,
					Skills = job.JobSkills.Select(jb => jb.Skill).ToList(),
					Benefits = job.JobBenefits.Select(jl => jl.Benefit).ToList(),
					Levels = job.JobLevels.Select(jl => jl.Level).ToList()
				};

				jobViewModels.Add(jobViewModel);
			}

			return View(jobViewModels);
		}

		[Authorize(Roles = "Manager,Admin,Recruiter")]
		[HttpGet]
		public IActionResult Create()
		{
			CreateJobVM createJobVM = new CreateJobVM();
			createJobVM.Benefits = _benefitService.GetBenefits();
			createJobVM.Levels = _levelService.GetLevels();
			createJobVM.Skills = _skillService.GetSkills();

			return View(createJobVM);
		}

		[Authorize(Roles = "Manager,Admin,Recruiter")]
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var job = _jobService.GetJobById(id);

			if (job == null || job.IsDeleted == true)
			{
				ErrorViewModel errorVm = new ErrorViewModel
				{
					ErrorMessage = "Job does not exist",
					RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
				};
				return View("~/Views/Shared/404.cshtml", errorVm);
			}

			var editJobVM = new EditJobVM
			{
				JobID = job.JobId,
				JobTitle = job.Title,
				StartDate = job.StartDate,
				EndDate = job.EndDate,
				SalaryFrom = job.SalaryFrom,
				SalaryTo = job.SalaryTo,
				WorkingAddress = job.WorkingAddress ?? string.Empty,
				Description = job.Description ?? string.Empty,
				Status = job.Status,
				Skills = _skillService.GetSkills(),
				Benefits = _benefitService.GetBenefits(),
				Levels = _levelService.GetLevels(),
			};
			editJobVM.SelectedBenefits = _jobBenefitsService.GetBenefitsByJobId(id);
			editJobVM.SelectedLevels = _jobLevelsService.GetLevelsByJobId(id);
			editJobVM.SelectedSkills = _jobSkillsService.GetSkillsByJobId(id);

			return View(editJobVM);
		}

		[Authorize(Roles = "Manager,Admin,Recruiter")]
		[HttpPost]
		public async Task<IActionResult> Create(CreateJobVM createJobVM)
		{
			// Remove unnecessary validation  
			ModelState.Remove(nameof(CreateJobVM.SalaryFrom));
			ModelState.Remove(nameof(CreateJobVM.SalaryTo));
			ModelState.Remove(nameof(CreateJobVM.WorkingAddress));
			ModelState.Remove(nameof(CreateJobVM.Description));
			ModelState.Remove(nameof(CreateJobVM.Levels));
			ModelState.Remove(nameof(CreateJobVM.Skills));
			ModelState.Remove(nameof(CreateJobVM.Benefits));
			if (createJobVM.EndDate < createJobVM.StartDate)
			{
				ModelState.AddModelError("EndDate", "End Date must be greater than Start Date.");
			}
			if (createJobVM.SalaryTo != null && createJobVM.SalaryFrom != null)
			{
				if (createJobVM.SalaryTo < createJobVM.SalaryFrom.Value)
					ModelState.AddModelError("SalaryTo", "Salary To must be greater than From.");
			}
			if (ModelState.IsValid)
			{
				try
				{
					// Job model
					Job job = new Job
					{
						Title = createJobVM.JobTitle,
						CreatedOn = DateTime.Now,
						LastUpdatedBy = User.Identity.Name,
						LastUpdatedOn = DateTime.Now,
						StartDate = createJobVM.StartDate,
						EndDate = createJobVM.EndDate,
						SalaryFrom = createJobVM.SalaryFrom,
						SalaryTo = createJobVM.SalaryTo,
						WorkingAddress = createJobVM.WorkingAddress,
						Description = createJobVM.Description,
						Status = JobStatus.Draft // Need change after got ultilities
					};
					int JobId = await _jobService.CreateJob(job);

					// Set relation with Level, Skill and Benefit
					foreach (var skillId in createJobVM.SelectedSkills)
					{
						JobSkill jobSkill = new JobSkill();
						jobSkill.SkillId = skillId;
						jobSkill.JobId = JobId;
						await _jobSkillsService.CreateJobSkill(jobSkill);
					}
					foreach (var levelId in createJobVM.SelectedLevels)
					{
						JobLevel jobLevel = new JobLevel();
						jobLevel.LevelId = levelId;
						jobLevel.JobId = JobId;
						await _jobLevelsService.CreateJobLevel(jobLevel);
					}
					foreach (var benefitId in createJobVM.SelectedBenefits)
					{
						JobBenefit jobBenefit = new JobBenefit();
						jobBenefit.BenefitId = benefitId;
						jobBenefit.JobId = JobId;
						await _jobBenefitsService.CreateJobBenefit(jobBenefit);
					}
					TempData["success"] = "Successfully created job";
					return RedirectToAction(nameof(Index));

				}
				catch (ArgumentException)
				{
					TempData["error"] = "Failed to updated change";
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("generalExceptionError", ex.Message);
				}
			}
			createJobVM.Benefits = _benefitService.GetBenefits();
			createJobVM.Levels = _levelService.GetLevels();
			createJobVM.Skills = _skillService.GetSkills();
			TempData["error"] = "Failed to created job";
			return View(createJobVM);
		}

		[Authorize(Roles = "Interviewer,Manager,Admin,Recruiter")]
		[HttpGet]
		public IActionResult Detail(int id)
		{
			//get job by job id
			var job = _jobService.GetJobById(id);
			if (job == null || job.IsDeleted == true)
			{
				ErrorViewModel errorVm = new ErrorViewModel
				{
					ErrorMessage = "Job does not exist",
					RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
				};
				return View("~/Views/Shared/404.cshtml", errorVm);
			}

			//mapping to Job view model
			JobViewModel jobViewModel = new JobViewModel
			{
				Job = job,
				Skills = job.JobSkills.Select(jb => jb.Skill).ToList(),
				Benefits = job.JobBenefits.Select(jl => jl.Benefit).ToList(),
				Levels = job.JobLevels.Select(jl => jl.Level).ToList()
			};
			return View(jobViewModel);
		}

		[Authorize(Roles = "Manager,Admin,Recruiter")]
		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditJobVM editJobVM)
		{
			ModelState.Remove(nameof(EditJobVM.SalaryFrom));
			ModelState.Remove(nameof(EditJobVM.SalaryTo));
			ModelState.Remove(nameof(EditJobVM.WorkingAddress));
			ModelState.Remove(nameof(EditJobVM.Description));
			ModelState.Remove(nameof(EditJobVM.Levels));
			ModelState.Remove(nameof(EditJobVM.Skills));
			ModelState.Remove(nameof(EditJobVM.Benefits));
			if (editJobVM.EndDate < editJobVM.StartDate)
				ModelState.AddModelError("EndDate", "End Date must be greater than Start Date.");
			if (editJobVM.SalaryTo < editJobVM.SalaryFrom)
				ModelState.AddModelError("SalaryTo", "Salary To must be greater than From.");
			if (ModelState.IsValid)
			{

				try
				{
					var jobToUpdate = _jobService.GetJobById(id);

					if (jobToUpdate == null)
					{
						return NotFound();
					}

					// Update core job details
					jobToUpdate.Title = editJobVM.JobTitle;
					jobToUpdate.StartDate = editJobVM.StartDate;
					jobToUpdate.EndDate = editJobVM.EndDate;
					jobToUpdate.SalaryFrom = editJobVM.SalaryFrom;
					jobToUpdate.SalaryTo = editJobVM.SalaryTo;
					jobToUpdate.WorkingAddress = editJobVM.WorkingAddress;
					jobToUpdate.Description = editJobVM.Description;
					jobToUpdate.Status = editJobVM.Status;


					// Delete existing related data (Skills, Levels, Benefits)
					await _jobSkillsService.DeleteJobSkill(id);
					await _jobLevelsService.DeleteJobLevel(id);
					await _jobBenefitsService.DeleteJobBenefit(id);

					// Add newly selected related data
					foreach (var skill in editJobVM.SelectedSkills)
					{
						JobSkill jobSkill = new JobSkill();
						jobSkill.SkillId = skill;
						jobSkill.JobId = id;
						await _jobSkillsService.CreateJobSkill(jobSkill);
					}
					foreach (var level in editJobVM.SelectedLevels)
					{
						JobLevel jobLevel = new JobLevel();
						jobLevel.LevelId = level;
						jobLevel.JobId = id;
						await _jobLevelsService.CreateJobLevel(jobLevel);
					}
					foreach (var benefit in editJobVM.SelectedBenefits)
					{
						JobBenefit jobBenefit = new JobBenefit();
						jobBenefit.BenefitId = benefit;
						jobBenefit.JobId = id;
						await _jobBenefitsService.CreateJobBenefit(jobBenefit);
					}

					await _jobService.UpdateJob(jobToUpdate);
					TempData["success"] = "Change has been successfully updated";
					return RedirectToAction(nameof(Index));

				}
				catch (Exception ex)
				{
					TempData["error"] = "Failed to update change";
				}
			}

			// Re-fetch lists in case of model errors
			editJobVM.Benefits = _benefitService.GetBenefits();
			editJobVM.Levels = _levelService.GetLevels();
			editJobVM.Skills = _skillService.GetSkills();

			return View(editJobVM);
		}

		[Authorize(Roles = "Interviewer,Manager,Admin,Recruiter")]
		[HttpPost]
		public async Task<IActionResult> Import(IFormFile jobList)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				await jobList.CopyToAsync(memoryStream);
				try
				{
					await _jobService.ImportJob(memoryStream, User.Identity.Name);
				}
				catch (Exception ex)
				{
					TempData["error"] = "Some error occured. Please ensure your file is in correct format";
				}
			}

			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = "Admin,Recruiter,Manager")]
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				Job job = _jobService.GetJobById(id);

				await _jobService.DeleteJob(job);
				TempData["success"] = "Job deleted successfully";
				return RedirectToAction(nameof(JobController.Index));
			}
			catch (Exception ex)
			{
				TempData["error"] = "Failed to delete job. Please try again";
				Console.WriteLine(ex.Message);
			}
			// Return to Index
			return RedirectToAction(nameof(JobController.Index));
		}
	}
}
