using ClosedXML.Excel;
using IMS.CoreServices;
using IMS.CoreServices.Implementations;
using IMS.Models;
using IMS.Utilities.Constants;
using IMS.WebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;

namespace IMS.WebApp.Controllers
{
    [Authorize]
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly ICandidateService _candidateService;
        private readonly IContractTypeService _contractTypeService;
        private readonly IPositionService _positionService;
        private readonly ILevelService _levelService;
        private readonly IUserService _userService;
        private readonly IDepartmentService _departmentService;
        private readonly IInterviewScheduleService _interviewScheduleService;

        public OfferController(IOfferService offerService, ICandidateService candidateService, IContractTypeService contractTypeService,
            IPositionService positionService, ILevelService levelService, IUserService userService, IDepartmentService departmentService, IInterviewScheduleService interviewScheduleService)
        {
            _offerService = offerService;
            _candidateService = candidateService;
            _contractTypeService = contractTypeService;
            _positionService = positionService;
            _levelService = levelService;
            _userService = userService;
            _departmentService = departmentService;
            _interviewScheduleService = interviewScheduleService;

        }

        [Authorize(Roles = "Recruiter, Manager, Admin")]
        public IActionResult Index()
        {
            var offers = _offerService.GetAllOffer();
            var listOfferViewModel = new List<ListOfferVM>();
            foreach (var offer in offers)
            {
                ListOfferVM offerViewModel = new ListOfferVM
                {
                    Offer = offer,
                    Candidate = offer.Candidate,
                    Approver = offer.Manager,
                    Recruiter = offer.Recruiter,
                    Department = offer.Department
                };

                listOfferViewModel.Add(offerViewModel);
            }
            return View(listOfferViewModel);
        }

        [Authorize(Roles = "Recruiter, Manager, Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateOfferVM createOfferVM = new CreateOfferVM();
            createOfferVM.Candidates = _candidateService.GetCandidatesNotBannedAndDeleted();
            createOfferVM.ContractTypes = _contractTypeService.GetContractTypes();
            createOfferVM.Positions = _positionService.GetPositions();
            createOfferVM.Levels = _levelService.GetLevels();
            createOfferVM.Recruiter = await _userService.GetAllRecruiter();
            createOfferVM.Manager = await _userService.GetAllManager();
            createOfferVM.Departments = _departmentService.GetDepartments();
            return View(createOfferVM);
        }

        [Authorize(Roles = "Recruiter, Manager, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOfferVM createOfferVM)
        {
            // Remove unnecessary validation  
            ModelState.Remove(nameof(CreateOfferVM.Candidates));
            ModelState.Remove(nameof(CreateOfferVM.Manager));
            ModelState.Remove(nameof(CreateOfferVM.Recruiter));
            ModelState.Remove(nameof(CreateOfferVM.ContractTypes));
            ModelState.Remove(nameof(CreateOfferVM.Departments));
            ModelState.Remove(nameof(CreateOfferVM.Levels));
            ModelState.Remove(nameof(CreateOfferVM.Positions));
            ModelState.Remove(nameof(CreateOfferVM.Interviews));
            ModelState.Remove(nameof(CreateOfferVM.Notes));
            ModelState.Remove(nameof(CreateOfferVM.LastUpdatedBy));
            if (ModelState.IsValid)
            {
                try
                {
                    //Add value from VM to Offer model
                    Offer offer = new Offer
                    {
                        CandidateId = createOfferVM.SelectedCandidate.Value,
                        ContractTypeID = createOfferVM.SelectedContractTypes.Value,
                        PositionId = createOfferVM.SelectedPosition.Value,
                        LevelId = createOfferVM.SelectedLevels.Value,
                        ApproverId = createOfferVM.SelectedManager,
                        DepartmentId = createOfferVM.SelectedDepartment.Value,
                        InterviewId = createOfferVM.SelectedInterviewSchedule.Value,
                        RecruiterOwnerId = createOfferVM.SelectedRecruiter,
                        ContractFrom = createOfferVM.ContractFrom.Value,
                        ContractTo = createOfferVM.ContractTo.Value,
                        DueDate = createOfferVM.DueDate.Value,
                        Notes = createOfferVM.Notes,
                        Status = OfferStatus.WaitingForApproval,
                        BasicSalary = createOfferVM.BasicSalary.Value,
                        CreatedOn = DateTime.Now,
                        LastUpdatedOn = DateTime.Now,
                        LastUpdatedBy = User.Identity.Name
                    };
                    await _offerService.CreateOffer(offer);

                    //update status of Candidate to Watting to Approval
                    _candidateService.UpdateCandidateStatus(createOfferVM.SelectedCandidate.Value, CandidateStatus.WaitingForApproval);
                    TempData["success"] = "Sucessfully created offer";
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
            createOfferVM.Candidates = _candidateService.GetCandidatesNotBannedAndDeleted();
            createOfferVM.ContractTypes = _contractTypeService.GetContractTypes();
            createOfferVM.Positions = _positionService.GetPositions();
            createOfferVM.Levels = _levelService.GetLevels();
            createOfferVM.Recruiter = await _userService.GetAllRecruiter();
            createOfferVM.Manager = await _userService.GetAllManager();
            createOfferVM.Departments = _departmentService.GetDepartments();
            TempData["error"] = "Failed to created offer";
            return View(createOfferVM);
        }


        //Get InterviewSchedule by candidate id
        [HttpGet]
        public IActionResult GetInterviewSchedules(int candidateId)
        {
            var interviewSchedules = _interviewScheduleService.GetInterviewSchedulesByCandidateId(candidateId);
            return Json(interviewSchedules);
        }


        //Get Interview By Interview ID
        [HttpGet]
        public async Task<IActionResult> GetInterviewById(int interviewId)
        {
            var interviewSchedule = await _interviewScheduleService.GetInterviewScheduleById(interviewId);
            return Json(interviewSchedule);
        }

        [Authorize(Roles = "Recruiter, Manager, Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            EditOfferVM editOfferVM = new EditOfferVM();
            editOfferVM.Candidates = _candidateService.GetCandidatesNotBannedAndDeleted();
            editOfferVM.ContractTypes = _contractTypeService.GetContractTypes();
            editOfferVM.Positions = _positionService.GetPositions();
            editOfferVM.Levels = _levelService.GetLevels();
            editOfferVM.Recruiter = await _userService.GetAllRecruiter();
            editOfferVM.Manager = await _userService.GetAllManager();
            editOfferVM.Departments = _departmentService.GetDepartments();

            // Get existed Offer
            var existedOffer = await _offerService.GetOfferById(id);
            if (existedOffer == null)
            {               
                ErrorViewModel error = new ErrorViewModel();
                error.ErrorMessage = "No offers exist";
                return View("404",error);
            } else if (existedOffer.Status != OfferStatus.WaitingForApproval)
            {
                TempData["error"] = "Cannot edit this offer";
                return RedirectToAction(nameof(Index));
            }
            editOfferVM.Interviews = _interviewScheduleService.GetInterviewSchedulesByCandidateId(existedOffer.CandidateId);

            try
            {
                editOfferVM.SelectedCandidate = existedOffer.CandidateId;
                editOfferVM.SelectedContractTypes = existedOffer.ContractTypeID;
                editOfferVM.SelectedPosition = existedOffer.PositionId;
                editOfferVM.SelectedLevels = existedOffer.LevelId;
                editOfferVM.SelectedRecruiter = existedOffer.RecruiterOwnerId;
                editOfferVM.SelectedManager = existedOffer.ApproverId;
                editOfferVM.SelectedDepartment = existedOffer.DepartmentId;
                editOfferVM.SelectedInterviewSchedule = existedOffer.InterviewId;
                editOfferVM.SelectedInterInfo = await _interviewScheduleService.GetInterviewScheduleById(existedOffer.InterviewId);
                editOfferVM.BasicSalary = existedOffer.BasicSalary;
                editOfferVM.Status = existedOffer.Status;
                editOfferVM.Notes = existedOffer.Notes;
                editOfferVM.ContractFrom = existedOffer.ContractFrom;
                editOfferVM.ContractTo = existedOffer.ContractTo;
                editOfferVM.DueDate = existedOffer.DueDate;
                return View(editOfferVM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Recruiter, Manager, Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditOfferVM editOfferVM)
        {
            ModelState.Remove(nameof(EditOfferVM.Candidates));
            ModelState.Remove(nameof(EditOfferVM.Manager));
            ModelState.Remove(nameof(EditOfferVM.Recruiter));
            ModelState.Remove(nameof(EditOfferVM.ContractTypes));
            ModelState.Remove(nameof(EditOfferVM.Departments));
            ModelState.Remove(nameof(EditOfferVM.Levels));
            ModelState.Remove(nameof(EditOfferVM.Positions));
            ModelState.Remove(nameof(EditOfferVM.Interviews));
            ModelState.Remove(nameof(EditOfferVM.Notes));
            ModelState.Remove(nameof(EditOfferVM.SelectedInterInfo));
            if (ModelState.IsValid)
            {
                try
                {
                    // Get offer need update
                    var offer = await _offerService.GetOfferById(id);

                    offer.LastUpdatedBy = User.Identity.Name;
                    offer.LastUpdatedOn = DateTime.Now;
                    // Map value of VM to Offer model
                    offer.CandidateId = editOfferVM.SelectedCandidate.Value;
                    offer.PositionId = editOfferVM.SelectedPosition.Value;
                    offer.ContractTypeID = editOfferVM.SelectedContractTypes.Value;
                    offer.DepartmentId = editOfferVM.SelectedDepartment.Value;
                    offer.LevelId = editOfferVM.SelectedLevels.Value;
                    offer.RecruiterOwnerId = editOfferVM.SelectedRecruiter;
                    offer.ApproverId = editOfferVM.SelectedManager;
                    offer.InterviewId = editOfferVM.SelectedInterviewSchedule.Value;
                    offer.ContractTo = editOfferVM.ContractTo.Value;
                    offer.ContractFrom = editOfferVM.ContractFrom.Value;
                    offer.DueDate = editOfferVM.DueDate.Value;
                    offer.BasicSalary = editOfferVM.BasicSalary.Value;
                    offer.Notes = editOfferVM.Notes;

                    //Update interview schedule by Id
                    await _offerService.UpdateOffer(offer);

                    TempData["success"] = "Change has been successfully updated";
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
            editOfferVM.Candidates = _candidateService.GetCandidatesNotBannedAndDeleted();
            editOfferVM.ContractTypes = _contractTypeService.GetContractTypes();
            editOfferVM.Positions = _positionService.GetPositions();
            editOfferVM.Levels = _levelService.GetLevels();
            editOfferVM.Recruiter = await _userService.GetAllRecruiter();
            editOfferVM.Manager = await _userService.GetAllManager();
            editOfferVM.Departments = _departmentService.GetDepartments();

            // Get existed Offer
            var existedOffer = await _offerService.GetOfferById(id);
            if (existedOffer == null || existedOffer.Status != OfferStatus.WaitingForApproval)
            {
                return NotFound();
            }
            editOfferVM.Interviews = _interviewScheduleService.GetInterviewSchedulesByCandidateId(existedOffer.CandidateId);

            try
            {
                editOfferVM.SelectedCandidate = existedOffer.CandidateId;
                editOfferVM.SelectedContractTypes = existedOffer.ContractTypeID;
                editOfferVM.SelectedPosition = existedOffer.PositionId;
                editOfferVM.SelectedLevels = existedOffer.LevelId;
                editOfferVM.SelectedRecruiter = existedOffer.RecruiterOwnerId;
                editOfferVM.SelectedManager = existedOffer.ApproverId;
                editOfferVM.SelectedDepartment = existedOffer.DepartmentId;
                editOfferVM.SelectedInterviewSchedule = existedOffer.InterviewId;
                editOfferVM.SelectedInterInfo = await _interviewScheduleService.GetInterviewScheduleById(existedOffer.InterviewId);
                editOfferVM.BasicSalary = existedOffer.BasicSalary;
                editOfferVM.Status = existedOffer.Status;
                editOfferVM.Notes = existedOffer.Notes;
                editOfferVM.ContractFrom = existedOffer.ContractFrom;
                editOfferVM.ContractTo = existedOffer.ContractTo;
                editOfferVM.DueDate = existedOffer.DueDate;
                TempData["error"] = "Failed to updated change";
                return View(editOfferVM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Recruiter, Manager, Admin")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            Offer? offer = await _offerService.GetOfferById(id);
            if (offer == null)
            {
                ErrorViewModel error = new ErrorViewModel();
                error.ErrorMessage = "No offers exist";
                return View("404", error);
            }
            OfferVM offerVM = new OfferVM()
            {
                OfferId = offer.OfferId,
                ContractFrom = offer.ContractFrom,
                ContractTo = offer.ContractTo,
                DueDate = offer.DueDate,
                BasicSalary = offer.BasicSalary,
                Status = offer.Status,
                Note = offer.Notes,
                CreatedOn = offer.CreatedOn,
                LastUpdatedOn = offer.LastUpdatedOn,
                LastUpdatedBy = offer.LastUpdatedBy
            };
            offerVM.Candidate = _candidateService.GetCandidate(offer.CandidateId);
            offerVM.ContractType = _contractTypeService.GetContractTypeById(offer.ContractTypeID);
            offerVM.Position = _positionService.GetPositionById(offer.PositionId);
            offerVM.Level = _levelService.GetLevelbyId(offer.LevelId);
            offerVM.Department = _departmentService.GetDepartmentById(offer.DepartmentId);
            InterviewSchedule interview = _interviewScheduleService.getDetailInterviewById(offer.InterviewId);
            offerVM.InterviewSchedule = interview;
            offerVM.Interviewer = interview.InterviewAssigns.Select(ia => ia.User).ToList();
            offerVM.Recruiter = await _userService.GetUser(offer.RecruiterOwnerId);
            offerVM.Manager = await _userService.GetUser(offer.ApproverId);
            return View(offerVM);
        }

        [Authorize(Roles = "Manager, Admin")]
        [HttpGet]
        public async Task<IActionResult> ApproveRejectOffer(int offerId, OfferStatus status)
        {
            try
            {
                Offer offer = await _offerService.GetOfferById(offerId);
                if (offer == null)
                {
                    ErrorViewModel error = new ErrorViewModel();
                    error.ErrorMessage = "No offers exist";
                    return View("404", error);
                }
                await _offerService.ApproveRejectOffer(offerId, status);
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine(argEx.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Recruiter, Manager, Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int offerId, OfferStatus status)
        {
            try
            {
                Offer offer = await _offerService.GetOfferById(offerId);
                if (offer == null)
                {
                    ErrorViewModel error = new ErrorViewModel();
                    error.ErrorMessage = "No offers exist";
                    return View("404", error);
                }
                await _offerService.UpdateOfferStatus(offerId, status);
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine(argEx.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetOfferByDueDateAjax(DateTime startDate, DateTime endDate)
        {
            var offers = _offerService.GetOffersByDueDate(startDate, endDate);
            return Json(offers);
        }

        [Authorize(Roles = "Recruiter, Manager, Admin")]
        [HttpGet]
        public async Task<IActionResult> ExportOffer(string offers, DateTime startDate, DateTime endDate)
        {
            //Generate file name
            string fileName = "OfferList-" + startDate.ToString("dd-MM-yyyy") + "_" + endDate.ToString("dd-MM-yyyy") + ".xlsx";
            //convert offer json to IEnumrable<Offer>
            IEnumerable<Offer> offerList = JsonConvert.DeserializeObject<IEnumerable<Offer>>(offers);

            //Generate excel data
            byte[] excelData = await _offerService.GenerateExcelOfferAsync(offerList);

            return File(excelData,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}
