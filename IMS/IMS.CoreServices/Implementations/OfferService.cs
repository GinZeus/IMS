using IMS.Data;
using IMS.HtmlEmail.Views.Emails.OfferReminder;
﻿using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using IMS.Models;
using IMS.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IMS.CoreServices.Implementations
{
	public class OfferService : IOfferService
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly ICandidateService _candidateService;
        private readonly IContractTypeService _contractTypeService;
        private readonly IPositionService _positionService;
        private readonly ILevelService _levelService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserService _userService;
        private readonly IInterviewScheduleService _interviewScheduleService;

        public OfferService(ApplicationDbContext dbContext, ICandidateService candidateService,
            IContractTypeService contractTypeService, IPositionService positionService, ILevelService levelService,
            IDepartmentService departmentService, IUserService userService, IInterviewScheduleService interviewScheduleService)
        {
            _dbContext = dbContext;
            _candidateService = candidateService;
            _contractTypeService = contractTypeService;
            _positionService = positionService;
            _levelService = levelService;
            _departmentService = departmentService;
            _userService = userService;
            _interviewScheduleService = interviewScheduleService;

        }

        public IEnumerable<Offer> GetAllOffer()
		{
			return _dbContext.Offers
				.Include(o => o.Candidate)
				.Include(o => o.Manager)
				.Include(o => o.Department)
				.ToList();
		}

        public async Task CreateOffer(Offer offer)
        {
            ArgumentNullException.ThrowIfNull(offer);

            try
            {
                _dbContext.Offers.Add(offer);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Offer>? GetOfferById(int offerId)
        {
            ArgumentNullException.ThrowIfNull(offerId);

            try
            {
                return await _dbContext.Offers.FirstOrDefaultAsync(i => i.OfferId == offerId);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task UpdateOffer(Offer offer)
        {
            if (offer == null)
                throw new ArgumentNullException(nameof(offer));

            try
            {
                _dbContext.Entry(offer).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task ApproveRejectOffer(int offerId, OfferStatus status)
        {
            //Get offer by offer id
            Offer? offer = await GetOfferById(offerId);
            if (offer == null)
                throw new ArgumentException("Offer does not exist", nameof(offerId));

            // Create a mapping table between OfferStatus and CandidateStatus
            var statusMapping = new Dictionary<OfferStatus, CandidateStatus>
            {
                { OfferStatus.Approved, CandidateStatus.ApprovedOffer },
                { OfferStatus.Rejected, CandidateStatus.RejectedOffer }
            };

            // Check if the status exists in the mapping table
            if (statusMapping.ContainsKey(status))
            {
                // Get the corresponding value of status from the mapping table
                var candidateStatus = statusMapping[status];

                // Update status of offer and candidate
                offer.Status = status;
                _candidateService.UpdateCandidateStatus(offer.CandidateId, candidateStatus);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateOfferStatus(int offerId, OfferStatus status)
        {
            Offer? offer = await GetOfferById(offerId);
            if (offer == null)
                throw new ArgumentException("Offer does not exist", nameof(offerId));

            // Create a mapping table between OfferStatus and CandidateStatus
            var statusMapping = new Dictionary<OfferStatus, CandidateStatus>
            {
                { OfferStatus.WaitingForResponse, CandidateStatus.WaitingForResponse },
                { OfferStatus.AcceptedOffer, CandidateStatus.AcceptedOffer },
                { OfferStatus.DeclinedOffer, CandidateStatus.DeclinedOffer },
                { OfferStatus.Cancelled, CandidateStatus.CancelledOffer }
            };

            // Check if the status exists in the mapping table
            if (statusMapping.ContainsKey(status))
            {
                // Get the corresponding value of status from the mapping table
                var candidateStatus = statusMapping[status];

                // Update status of offer and candidate
                offer.Status = status;
                _candidateService.UpdateCandidateStatus(offer.CandidateId, candidateStatus);
            }

            await _dbContext.SaveChangesAsync();
        }

        public IEnumerable<OfferReminderVM> GetDueDateOffers()
        {
            IEnumerable<OfferReminderVM> offerReminders = _dbContext.Offers
                .Where(o => o.DueDate.Date == DateTime.Now.Date && o.Status == OfferStatus.WaitingForApproval)
                .Select(o => new OfferReminderVM
                {
                    DueDate = o.DueDate,
                    CandidateName = o.Candidate.FullName,
                    CandidatePostion = o.Candidate.Position.PositionName,
                    RecruiterUsername = o.Recruiter.UserName,
                    ManagerEmail = o.Manager.Email,
                    ManagerUsername = o.Manager.UserName,
                    OfferDetailURL = $"https://localhost:7056/Offer/Detail/{o.OfferId}",
                    CvAttachment = o.Candidate.CVAttachment,
                    CvMimeType = o.Candidate.CVMimeType
                }).ToList();

            return offerReminders;
        }

		public IEnumerable<Offer>? GetOffersByDueDate(DateTime startDate, DateTime endDate)
		{
			return  _dbContext.Offers
				.Where(o => o.DueDate >= startDate && o.DueDate <= endDate)
				.ToList();
		}

        public async Task<byte[]> GenerateExcelOfferAsync(IEnumerable<Offer> offers)
        {

			//create sheet excel with name "Offer"
            DataTable dataTable = new DataTable("Offer");
            //add Columns to Data Table
            dataTable.Columns.AddRange(new DataColumn[]
            {
            new DataColumn("No."),
            new DataColumn("Candidate ID"),
            new DataColumn("Candidate Name"),
            new DataColumn("Approved By"),
            new DataColumn("Contract type"),
            new DataColumn("Position"),
            new DataColumn("Level"),
            new DataColumn("Department"),
            new DataColumn("Recruiter Owner"),
            new DataColumn("Interviewer"),
            new DataColumn("Contract From"),
            new DataColumn("Contract To"),
            new DataColumn("Basic Salary"),
            new DataColumn("Interview notes"),
            new DataColumn("Notes")
            });

            foreach (var offer in offers)
            {
                var candidate = _candidateService.GetCandidate(offer.CandidateId);
                var contractType = _contractTypeService.GetContractTypeById(offer.ContractTypeID);
                var position = _positionService.GetPositionById(offer.PositionId);
                var level = _levelService.GetLevelbyId(offer.LevelId);
                var dept = _departmentService.GetDepartmentById(offer.DepartmentId);
                var interview = _interviewScheduleService.getDetailInterviewById(offer.InterviewId);
                var interviewers = interview.InterviewAssigns.Select(ia => ia.User).ToList();
                var interviewerValue = string.Join(", ", interviewers.Select(s => s.UserName));
                var recruiter = await _userService.GetUser(offer.RecruiterOwnerId);
                var manager = await _userService.GetUser(offer.ApproverId);

                // Add a new row to the DataTable using the offer data
                dataTable.Rows.Add(
                    offer.OfferId,
                    candidate.CandidateId,
                    candidate.FullName,
                    manager.UserName,
                    contractType.ContractTypeTitle,
                    position.PositionName,
                    level.LevelName,
                    dept.DepartmentName,
                    recruiter.UserName,
                    interviewerValue,
                    offer.ContractFrom.ToString("dd-MM-yyyy"),
                    offer.ContractTo.ToString("dd-MM-yyyy"),
                    offer.BasicSalary,
                    interview.Notes,
                    offer.Notes
                );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var worksheet = wb.Worksheets.Add(dataTable);

                // Adjust column width to fit the content
                worksheet.Columns().AdjustToContents();

                // Save the workbook to a MemoryStream and return it as a byte array
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return stream.ToArray();
                }
            }
        }

        public int GetTotalOffer()
        {
            return _dbContext.Offers.Count();
        }
        public int GetTotalOfferByStatus(OfferStatus status)
        {
            return _dbContext.Offers.Count(o => o.Status == status);
        }
        public async Task<IEnumerable<Offer>> GetOffersByCandidateId(int candidateId)
		{
			return await _dbContext.Offers.Where(o => o.CandidateId ==  candidateId).ToListAsync();
		}
        public IEnumerable<Offer> GetOfferWaitingForApproval()
        {
            var offers = GetAllOffer();
            return offers
                .Where(o => o.Status == OfferStatus.WaitingForApproval)
                .OrderByDescending(o => o.DueDate)
                .ToList();
        }
    }
}
