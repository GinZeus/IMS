using IMS.CoreServices;
using IMS.CoreServices.Implementations;
using IMS.Models;
using IMS.Utilities.Constants;
using IMS.WebApp.Controllers;
using IMS.WebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Diagnostics;
namespace IMS.WebApp.Controllers
{
	[Authorize]
	public class InterviewScheduleController : Controller
	{
		private readonly IUserService _userService;
		private readonly IJobInterface _jobService;
		private readonly ICandidateService _candidateService;
		private readonly IInterviewScheduleService _interviewScheduleService;
		private readonly IInterviewAssignService _interviewAssignService;
		private readonly IEmailService _emailService;
		private readonly IPositionService _positionService;
		private readonly UserManager<User> _userManager;
	
		public InterviewScheduleController(IJobInterface jobService, IUserService userService, IInterviewScheduleService interviewScheduleService, IInterviewAssignService interviewAssignService, ICandidateService candidateService, IEmailService emailService, IPositionService positionService, UserManager<User> userManager)
		{
			_userService = userService;
			_jobService = jobService;
			_interviewScheduleService = interviewScheduleService;
			_interviewAssignService = interviewAssignService;
			_candidateService = candidateService;
			_emailService = emailService;
			_positionService = positionService;
			_userManager = userManager;
		}

		[HttpPost]	
        //Filter offer by status and interviewer id
        public IActionResult FilterInterview(InterviewScheduleStatus? status, string? InterviewerId)
        {
			Console.WriteLine(status.ToString());
            var filterInterview = _interviewScheduleService.FilterInterview(status, InterviewerId);

            var ListInterviewVM = new List<InterviewScheduleVM>();
            foreach (var interview in filterInterview)
            {
                InterviewScheduleVM InterviewVM = new InterviewScheduleVM
                {
                    InterviewSchedule = interview,
                    Candidate = interview.Candidate,
                    Job= interview.Job,
                    Interviewers = interview.InterviewAssigns.Select(ia => ia.User).ToList()
                };

                ListInterviewVM.Add(InterviewVM);
            }
            return View("Index", ListInterviewVM);
        }

		[Authorize(Roles = "Manager,Admin,Recruiter")]
		[HttpGet]	
		public async Task<IActionResult> Create()
		{
			// Select list for Interviewers, Recruiters, Candidate, Job 
			CreateInterviewScheduleVM createInterviewScheduleVM = new CreateInterviewScheduleVM();
			createInterviewScheduleVM.Candidates = _candidateService.GetCandidatesNotBannedAndDeleted();
			createInterviewScheduleVM.Interviewers = await _userService.GetAllInterviewer();
			createInterviewScheduleVM.RecruiterOwners = await _userService.GetAllRecruiter();
			createInterviewScheduleVM.Jobs = _jobService.GetJobsByStatus((int)JobStatus.Open);
			return View(createInterviewScheduleVM);
		}

		[HttpGet]
        public async Task<IActionResult> Index()
        {
            var InterS = _interviewScheduleService.GetAllValidInterviewSchedules();
            var InterviewSVM = new List<InterviewScheduleVM>();

            if (User.IsInRole("Interviewer"))
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                //var interview = _interviewAssignService.GetInterviewsByInterviewerId(user.Id);
				var listInterview = InterS.Where(i => i.InterviewAssigns.Any(ia => ia.InterviewerId == user.Id));
                foreach (var InterSchedu in listInterview)
                {
                    InterviewScheduleVM InterviewSheduleVM = new InterviewScheduleVM
                    {
                        InterviewSchedule = InterSchedu,
                        Candidate = InterSchedu.Candidate,
                        Job = InterSchedu.Job,
                        Interviewers = InterSchedu.InterviewAssigns.Select(ia => ia.User).ToList()
                    };

                    InterviewSVM.Add(InterviewSheduleVM);
                }

            }
            else
            {
                foreach (var InterSchedu in InterS)
                {
                    InterviewScheduleVM InterviewSheduleVM = new InterviewScheduleVM
                    {
                        InterviewSchedule = InterSchedu,
                        Candidate = InterSchedu.Candidate,
                        Job = InterSchedu.Job,
                        Interviewers = InterSchedu.InterviewAssigns.Select(ia => ia.User).ToList()
                    };

                    InterviewSVM.Add(InterviewSheduleVM);
                }
            }

            return View(InterviewSVM);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var interview = _interviewScheduleService.getDetailInterviewById(id);

			if ( interview == null)
			{
				ErrorViewModel errorVm = new ErrorViewModel
				{
					ErrorMessage = "Interview does not exist",
					RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
				};
				return View("~/Views/Shared/404.cshtml", errorVm);
			}
			if (User.IsInRole("Interviewer"))
			{
				var user = await _userManager.FindByNameAsync(User.Identity.Name);
				// Might be null
				var hasInterview = interview.InterviewAssigns.Where(ia => ia.InterviewerId == user.Id).IsNullOrEmpty();
				if (hasInterview)
				{
                    ErrorViewModel errorVm = new ErrorViewModel
                    {
                        ErrorMessage = "Interview does not exist",
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                    };
                    return View("~/Views/Shared/404.cshtml", errorVm);
                }
            }
			var interviewVM = new InterviewScheduleVM
            {
                InterviewSchedule = interview,
                Candidate = interview.Candidate,
                Job = interview.Job,
                Interviewers = interview.InterviewAssigns.Select(ia => ia.User).ToList(),
                Recruiters = new List<User> { interview.Recruiter }
            };

            return View(interviewVM);
        }

		[Authorize(Roles = "Manager,Admin,Recruiter,Interviewer")]
		[HttpPost]
		public async Task<IActionResult> Create(CreateInterviewScheduleVM createInterviewScheduleVM)
		{
			// Remove unnecessary validation  
			ModelState.Remove(nameof(createInterviewScheduleVM.Candidates)); // Candidate SelectList
			ModelState.Remove(nameof(createInterviewScheduleVM.Interviewers)); // Interviewer SelectList
			ModelState.Remove(nameof(createInterviewScheduleVM.RecruiterOwners)); // RecruiterOwner SelectList
			ModelState.Remove(nameof(createInterviewScheduleVM.Jobs)); // Job SelectList
			ModelState.Remove(nameof(createInterviewScheduleVM.Location)); // Location input is optional
			ModelState.Remove(nameof(createInterviewScheduleVM.MeetingId)); // MeetingId input is optional
			ModelState.Remove(nameof(createInterviewScheduleVM.Notes)); // Notes input is optional
			if(createInterviewScheduleVM.ScheduleFrom != null && createInterviewScheduleVM.ScheduleTo != null)
			{
				if (createInterviewScheduleVM.ScheduleFrom.Value.Hour > createInterviewScheduleVM.ScheduleTo.Value.Hour)
					ModelState.AddModelError("ScheduleFrom", "Invalid schedule time! Schedule's end time must be after schedule's start time");
			}
			if(createInterviewScheduleVM.ScheduleDate < DateTime.Now.Date)
			{
				ModelState.AddModelError("ScheduleDate", "Invalid schedule date! Schedule Date must be after current date");
			}
			
			
			if (ModelState.IsValid)
			{
				try
				{
					//Add value from VM to Interview Schedule model
					InterviewSchedule interviewSchedule = new InterviewSchedule 
					{
						Title = createInterviewScheduleVM.InterviewScheduleTitle,
						LastUpdatedBy = User.Identity.Name,
						CreatedOn = DateTime.Now,
						LastUpdatedOn = DateTime.Now,
						CandidateId = createInterviewScheduleVM.SelectedCandidate.Value,
						RecruiterId = createInterviewScheduleVM.SelectedRecruiterOwner,
						Location = createInterviewScheduleVM.Location,
						DueDate = createInterviewScheduleVM.ScheduleDate,
						StartTime = createInterviewScheduleVM.ScheduleFrom,
						EndTime = createInterviewScheduleVM.ScheduleTo,
						JobId = createInterviewScheduleVM.SelectedJob.Value,
						Notes = createInterviewScheduleVM.Notes,
						MeetingId = createInterviewScheduleVM.MeetingId,
						Status = InterviewScheduleStatus.Open,
					};
					// Set the Candidate's status to Waiting for interview
					_candidateService.UpdateCandidateStatus(createInterviewScheduleVM.SelectedCandidate.Value, CandidateStatus.WaitingForInterview);

					// Get id of the created interview schedule
					int interviewScheduleId = await _interviewScheduleService.CreateInterviewSchedule(interviewSchedule);

					// After add Schedule add relation between interview and interviewer
					foreach (var interviewerId in createInterviewScheduleVM.SelectedInterviewer)
					{
						InterviewAssign interviewAssign = new InterviewAssign();
						interviewAssign.InterviewScheduleId = interviewScheduleId;
						interviewAssign.InterviewerId = interviewerId;
						await _interviewAssignService.CreateInterviewAssign(interviewAssign);
					}
                    TempData["success"] = "Successfully created interview schedule";
                    return RedirectToAction(nameof(Index));

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
			// If Error add removed select list then return view 
			createInterviewScheduleVM.Interviewers = await _userService.GetAllInterviewer();
			createInterviewScheduleVM.Candidates = _candidateService.GetCandidatesNotBannedAndDeleted();
			createInterviewScheduleVM.RecruiterOwners = await _userService.GetAllRecruiter();
			createInterviewScheduleVM.Jobs = _jobService.GetJobsByStatus((int)JobStatus.Open);

            TempData["error"] = "Failed to created interview schedule";
            return View(createInterviewScheduleVM);
		}

		[Authorize(Roles = "Manager,Admin,Recruiter")]
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			// Select list for Interviewers, Recruiters, Candidate, Job 
			EditInterviewScheduleVM editInterviewScheduleVM = new EditInterviewScheduleVM();
			editInterviewScheduleVM.Candidates = _candidateService.GetCandidatesNotBannedAndDeleted();
			editInterviewScheduleVM.Interviewers = await _userService.GetAllInterviewer();
			editInterviewScheduleVM.RecruiterOwners = await _userService.GetAllRecruiter();
			editInterviewScheduleVM.Jobs = _jobService.GetJobsByStatus((int)JobStatus.Open);


			// Get existed Interview Schedule
			var existedInterviewSchedule = await _interviewScheduleService.GetInterviewScheduleById(id);
			if (existedInterviewSchedule == null)
			{
				ErrorViewModel errorVm = new ErrorViewModel
				{
					ErrorMessage = "Interview does not exist",
					RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
				};
				return View("~/Views/Shared/404.cshtml", errorVm);
			}

			try
			{
				if(existedInterviewSchedule.Status == InterviewScheduleStatus.Cancelled) 
				{
					TempData["error"] = "Interview cancelled! The interview schedule is unable to edit.";
					return RedirectToAction(nameof(Index));
				}
				else
				{
					// Get existed interviewers who has been assigned to the interview
					editInterviewScheduleVM.SelectedInterviewer = await _interviewAssignService.GetInterviewersByInterviewId(id);
					// Other data
					editInterviewScheduleVM.SelectedCandidate = existedInterviewSchedule.CandidateId;
					editInterviewScheduleVM.SelectedJob = existedInterviewSchedule.JobId;
					editInterviewScheduleVM.SelectedRecruiterOwner = existedInterviewSchedule.RecruiterId;
					editInterviewScheduleVM.InterviewScheduleTitle = existedInterviewSchedule.Title;
					editInterviewScheduleVM.Location = existedInterviewSchedule.Location;
					editInterviewScheduleVM.ScheduleDate = existedInterviewSchedule.DueDate;
					// DateTime is not nullable
					editInterviewScheduleVM.ScheduleFrom = existedInterviewSchedule.StartTime;
					editInterviewScheduleVM.ScheduleTo = existedInterviewSchedule.EndTime;
					editInterviewScheduleVM.MeetingId = existedInterviewSchedule.MeetingId;
					editInterviewScheduleVM.Notes = existedInterviewSchedule.Notes;
					editInterviewScheduleVM.Status = existedInterviewSchedule.Status;
					return View(editInterviewScheduleVM);
				}
				
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		[Authorize(Roles = "Manager,Admin,Recruiter")]
		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditInterviewScheduleVM editInterviewScheduleVM)
		{
			// Remove unnecessary validation  
			ModelState.Remove(nameof(editInterviewScheduleVM.Candidates)); // Candidate SelectList
			ModelState.Remove(nameof(editInterviewScheduleVM.Interviewers)); // Interviewer SelectList
			ModelState.Remove(nameof(editInterviewScheduleVM.RecruiterOwners)); // RecruiterOwner SelectList
			ModelState.Remove(nameof(editInterviewScheduleVM.Jobs)); // Job SelectList
			ModelState.Remove(nameof(editInterviewScheduleVM.Location)); // Location input is optional
			ModelState.Remove(nameof(editInterviewScheduleVM.MeetingId)); // MeetingId input is optional
			ModelState.Remove(nameof(editInterviewScheduleVM.Notes)); // Notes input is optional
			if (editInterviewScheduleVM.ScheduleFrom != null && editInterviewScheduleVM.ScheduleTo != null)
			{
				if (editInterviewScheduleVM.ScheduleFrom.Value.Hour > editInterviewScheduleVM.ScheduleTo.Value.Hour)
					ModelState.AddModelError("ScheduleFrom", "Invalid schedule time! Schedule's end time must be after schedule's start time");
			}

			if (ModelState.IsValid)
			{
				try
				{
					// Get interviewSchedule need update
					var interviewSchedule = await _interviewScheduleService.GetInterviewScheduleById(id);

					// Change value from VM to Interview Schedule model
					interviewSchedule.Title = editInterviewScheduleVM.InterviewScheduleTitle;
					interviewSchedule.LastUpdatedBy = User.Identity.Name;
					interviewSchedule.LastUpdatedOn = DateTime.Now;
					interviewSchedule.CandidateId = editInterviewScheduleVM.SelectedCandidate.Value;
					interviewSchedule.RecruiterId = editInterviewScheduleVM.SelectedRecruiterOwner;
					interviewSchedule.Location = editInterviewScheduleVM.Location;
					interviewSchedule.DueDate = editInterviewScheduleVM.ScheduleDate.Value;
					interviewSchedule.StartTime = editInterviewScheduleVM.ScheduleFrom;
					interviewSchedule.EndTime = editInterviewScheduleVM.ScheduleTo;
					interviewSchedule.JobId = editInterviewScheduleVM.SelectedJob.Value;
					interviewSchedule.Notes = editInterviewScheduleVM.Notes;
					interviewSchedule.MeetingId = editInterviewScheduleVM.MeetingId;

					//Update interview schedule by Id
					await _interviewScheduleService.UpdateInterviewSchedule(interviewSchedule);

					// Remove existed interviewers who have been assigned by Id
					await _interviewAssignService.RemoveInterviewAssign(id);

					// Add all selected interviewers into InterviewAssign table
					foreach (var interviewerId in editInterviewScheduleVM.SelectedInterviewer)
					{
						InterviewAssign interviewAssign = new InterviewAssign();
						interviewAssign.InterviewScheduleId = id;
						interviewAssign.InterviewerId = interviewerId;
						await _interviewAssignService.CreateInterviewAssign(interviewAssign);
					}

					// Return to list page 
					return RedirectToAction(nameof(Index));

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
			// If Error add removed select list then return view 
			// Select list for Interviewers, Recruiters, Candidate, Job 
			editInterviewScheduleVM.Candidates = _candidateService.GetCandidatesNotBannedAndDeleted();
			editInterviewScheduleVM.Interviewers = await _userService.GetAllInterviewer();
			editInterviewScheduleVM.RecruiterOwners = await _userService.GetAllRecruiter();
			editInterviewScheduleVM.Jobs = _jobService.GetJobsByStatus((int)JobStatus.Open);

			// Get existed Interview Schedule
			var existedInterviewSchedule = await _interviewScheduleService.GetInterviewScheduleById(id);

			try
			{
				// Get existed interviewers who has been assigned to the interview
				editInterviewScheduleVM.SelectedInterviewer = await _interviewAssignService.GetInterviewersByInterviewId(id);
				// Other data
				editInterviewScheduleVM.SelectedCandidate = existedInterviewSchedule.CandidateId;
				editInterviewScheduleVM.SelectedJob = existedInterviewSchedule.JobId;
				editInterviewScheduleVM.SelectedRecruiterOwner = existedInterviewSchedule.RecruiterId;
				editInterviewScheduleVM.InterviewScheduleTitle = existedInterviewSchedule.Title;
				editInterviewScheduleVM.Location = existedInterviewSchedule.Location;
				editInterviewScheduleVM.ScheduleDate = existedInterviewSchedule.DueDate;
				editInterviewScheduleVM.ScheduleFrom = existedInterviewSchedule.StartTime;
				editInterviewScheduleVM.ScheduleTo = existedInterviewSchedule.EndTime;
				editInterviewScheduleVM.MeetingId = existedInterviewSchedule.MeetingId;
				editInterviewScheduleVM.Notes = existedInterviewSchedule.Notes;
				editInterviewScheduleVM.Status = existedInterviewSchedule.Status;
				return View(editInterviewScheduleVM);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return RedirectToAction("Error");
		}

		[Authorize(Roles ="Admin,Recruiter,Manager")]
		public async Task<IActionResult> CancelInterivew(int id)
		{
			try
			{
				// Get interviewSchedule need update
				var interviewSchedule = await _interviewScheduleService.GetInterviewScheduleById(id);

				// Set interview's status to cancelled
				if(interviewSchedule.Status == InterviewScheduleStatus.Open)
				{
					interviewSchedule.Status = InterviewScheduleStatus.Cancelled;
					await _interviewScheduleService.UpdateInterviewSchedule(interviewSchedule);
				}
				// Set candidate's status to cancelled interview

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			// Return to Index
			return RedirectToAction(nameof(HomeController.Index), "Home");

		}

		// Submit interview schedule
		[Authorize(Roles ="Interviewer")]
		[HttpGet]
		public async Task<IActionResult> SubmitResult(int id)
		{
			// Select list for Interviewers, Recruiters, Candidate, Job 
			SubmitResultVM submitResultVM = new SubmitResultVM();
			submitResultVM.Candidates = _candidateService.GetCandidatesNotBannedAndDeleted();
			submitResultVM.Interviewers = await _userService.GetAllInterviewer();
			submitResultVM.RecruiterOwners = await _userService.GetAllRecruiter();
			submitResultVM.Jobs = _jobService.GetJobsByStatus((int)JobStatus.Open);

            // Get existed Interview Schedule
            var existedInterviewSchedule = await _interviewScheduleService.GetInterviewScheduleById(id);

			// Get user
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            // Might be null
            var hasInterview = _interviewAssignService.GetInterviewersByInterviewId(id).Result.Contains(user.Id);
            
            if (existedInterviewSchedule == null || !hasInterview)
			{
				ErrorViewModel errorVm = new ErrorViewModel
				{
					ErrorMessage = "Interview does not exist",
					RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
				};
				return View("~/Views/Shared/404.cshtml", errorVm);
			}

			try
			{
				if(existedInterviewSchedule.Status == InterviewScheduleStatus.Cancelled)
				{
					TempData["error"] = "Interview cancelled! The interview schedule is unable to submit result.";
					return RedirectToAction(nameof(Index));
				}
				else
				{
					// Get existed interviewers who has been assigned to the interview
					submitResultVM.SelectedInterviewer = await _interviewAssignService.GetInterviewersByInterviewId(id);
					// Other data
					submitResultVM.SelectedCandidate = existedInterviewSchedule.CandidateId;
					submitResultVM.SelectedJob = existedInterviewSchedule.JobId;
					submitResultVM.SelectedRecruiterOwner = existedInterviewSchedule.RecruiterId;
					submitResultVM.LastUpdatedBy = existedInterviewSchedule.LastUpdatedBy;
					submitResultVM.InterviewScheduleTitle = existedInterviewSchedule.Title;
					submitResultVM.Location = existedInterviewSchedule.Location;
					submitResultVM.ScheduleDate = existedInterviewSchedule.DueDate;
					// DateTime is not nullable
					submitResultVM.ScheduleFrom = existedInterviewSchedule.StartTime;
					submitResultVM.ScheduleTo = existedInterviewSchedule.EndTime;
					submitResultVM.MeetingId = existedInterviewSchedule.MeetingId;
					submitResultVM.Notes = existedInterviewSchedule.Notes;
					submitResultVM.Status = existedInterviewSchedule.Status;
					submitResultVM.LastUpdatedOn = existedInterviewSchedule.LastUpdatedOn;
					submitResultVM.CreatedOn = existedInterviewSchedule.CreatedOn;
					submitResultVM.Result = existedInterviewSchedule.Result;
					return View(submitResultVM);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}
		[Authorize(Roles = "Interviewer")]
		[HttpPost]
        public async Task<IActionResult> SubmitResult(int id, SubmitResultVM submitResultVM)
        {
            // Remove unnecessary validation  
            ModelState.Remove(nameof(submitResultVM.Candidates)); // Candidate SelectList
            ModelState.Remove(nameof(submitResultVM.Interviewers)); // Interviewer SelectList
            ModelState.Remove(nameof(submitResultVM.RecruiterOwners)); // RecruiterOwner SelectList
            ModelState.Remove(nameof(submitResultVM.Jobs)); // Job SelectList
            ModelState.Remove(nameof(submitResultVM.Location)); // Location input is optional
            ModelState.Remove(nameof(submitResultVM.MeetingId)); // MeetingId input is optional
            ModelState.Remove(nameof(submitResultVM.Notes)); // Notes input is optional
			ModelState.Remove(nameof(submitResultVM.SelectedInterviewer));
			ModelState.Remove(nameof(submitResultVM.SelectedRecruiterOwner));
			ModelState.Remove(nameof(submitResultVM.LastUpdatedBy));
			if (submitResultVM.Result == null)
                ModelState.AddModelError("Result", "Invalid result ! Result must not be null");
            if (ModelState.IsValid)
            {
                try
                {
					// Get interviewSchedule need update
					var interviewSchedule = await _interviewScheduleService.GetInterviewScheduleById(id);

					// Udpate result
					interviewSchedule.Notes = submitResultVM.Notes;
					interviewSchedule.Result = submitResultVM.Result;
					interviewSchedule.LastUpdatedBy = User.Identity.Name;
					interviewSchedule.LastUpdatedOn = DateTime.Now;
					interviewSchedule.Status = InterviewScheduleStatus.Interviewed;

                    // Update interview schedule 
                    await _interviewScheduleService.UpdateInterviewSchedule(interviewSchedule);

					// Change candidate status
					if(interviewSchedule.Result == true)
						_candidateService.UpdateCandidateStatus(interviewSchedule.CandidateId, CandidateStatus.PassedInterview);
                    if (interviewSchedule.Result == false)
                        _candidateService.UpdateCandidateStatus(interviewSchedule.CandidateId, CandidateStatus.FailedInterview);

                    // Select list for Interviewers, Recruiters, Candidate, Job 
                    submitResultVM.Candidates = _candidateService.GetAvailableCandidates();
                    submitResultVM.Interviewers = await _userService.GetAllInterviewer();
                    submitResultVM.RecruiterOwners = await _userService.GetAllRecruiter();
                    submitResultVM.Jobs = _jobService.GetJobsByStatus((int)JobStatus.Open);

					// Return to list page (need change) 
					return RedirectToAction(nameof(InterviewScheduleController.Index));

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
            // Select list for Interviewers, Recruiters, Candidate, Job 
            submitResultVM.Candidates = _candidateService.GetCandidatesNotBannedAndDeleted();
            submitResultVM.Interviewers = await _userService.GetAllInterviewer();
            submitResultVM.RecruiterOwners = await _userService.GetAllRecruiter();
            submitResultVM.Jobs = _jobService.GetJobsByStatus((int)JobStatus.Open);

            // Get existed Interview Schedule
            var existedInterviewSchedule = await _interviewScheduleService.GetInterviewScheduleById(id);

            try
            {
                // Get existed interviewers who has been assigned to the interview
                submitResultVM.SelectedInterviewer = await _interviewAssignService.GetInterviewersByInterviewId(id);
                // Other data
                submitResultVM.SelectedCandidate = existedInterviewSchedule.CandidateId;
                submitResultVM.SelectedJob = existedInterviewSchedule.JobId;
                submitResultVM.SelectedRecruiterOwner = existedInterviewSchedule.RecruiterId;
                submitResultVM.LastUpdatedBy = existedInterviewSchedule.LastUpdatedBy;
                submitResultVM.InterviewScheduleTitle = existedInterviewSchedule.Title;
                submitResultVM.Location = existedInterviewSchedule.Location;
                submitResultVM.ScheduleDate = existedInterviewSchedule.DueDate;
                // DateTime is not nullable
                submitResultVM.ScheduleFrom = existedInterviewSchedule.StartTime;
                submitResultVM.ScheduleTo = existedInterviewSchedule.EndTime;
                submitResultVM.MeetingId = existedInterviewSchedule.MeetingId;
                submitResultVM.Notes = existedInterviewSchedule.Notes;
                submitResultVM.Status = existedInterviewSchedule.Status;
                submitResultVM.LastUpdatedOn = existedInterviewSchedule.LastUpdatedOn;
                submitResultVM.CreatedOn = existedInterviewSchedule.CreatedOn;
                submitResultVM.Result = existedInterviewSchedule.Result;
                return View(submitResultVM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Error");
        }

		[Authorize(Roles ="Manager,Admin,Recruiter")]
		public async Task<IActionResult> SendReminder(int id)
		{
			try
			{
				// Get interivew
				var interview = await _interviewScheduleService.GetInterviewScheduleById(id);


				if (interview.Status == InterviewScheduleStatus.Cancelled)
				{
					return RedirectToAction(nameof(InterviewScheduleController.Index));
				}
				else
				{
					// Get interviewer
					var interviewers = await _interviewAssignService.GetInterviewersByInterviewId(interview.InterviewScheduleId);
					var allInterviewers = await _userService.GetAllInterviewer();
					var interviewersInfo = allInterviewers.Where(user => interviewers.Contains(user.Id));

					// Get candidate
					var candidate = _candidateService.GetCandidate(interview.CandidateId);
					var position = _positionService.GetPositionName(candidate.PositionId);
					var recruiter = await _userService.GetUser(interview.RecruiterId);

					// Change status 
					interview.Status = InterviewScheduleStatus.Invited;
					_interviewScheduleService.UpdateInterviewSchedule(interview);


                    // Get list email
                    var emailList = interviewersInfo.Select(i => i.Email).ToList();

                    // Send reminder
                    _emailService.SendInterviewReminder(candidate, interview, position, recruiter.UserName, emailList);

                    return RedirectToAction(nameof(InterviewScheduleController.Index));
				}
            }
            catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}
        }
    }
}
