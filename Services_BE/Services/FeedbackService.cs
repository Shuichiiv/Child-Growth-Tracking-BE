using AutoMapper;
using DataObjects_BE.Entities;
using DTOs_BE.FeedbackDTOs;
using Repositories_BE.Interfaces;
using Repositories_BE.Repositories;
using Services_BE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_BE.Services
{
    public class FeedbackService: IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper, ICurrentTime currentTime,
            IReportRepository reportRepository, IDoctorRepository doctorRepository)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _currentTime = currentTime;
            _reportRepository = reportRepository;
            _doctorRepository = doctorRepository;
            
        }
        public async Task<FeedbackResponseDTO> GetFeedbackById(string id)
        {
            try
            {
                var feedbackId = Guid.Parse(id);
                var feedbackExisting = _feedbackRepository.GetFeedbackByIdIncludeProperties(feedbackId);
                if (feedbackExisting== null)
                {
                    throw new Exception("Feedback is not existing!!!");
                }
                else
                {
                    var result = _mapper.Map<FeedbackResponseDTO>(feedbackExisting);
                    return result;
                }
                
            }catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<FeedbackResponseDTO>> GetListFeedBack()
        {
            try
            {
                var list = _feedbackRepository.Get(includeProperties: "Report,Doctor,Ratings");
                if(list== null)
                {
                    throw new Exception("List is empty!!!");
                }
                var result = _mapper.Map<List<FeedbackResponseDTO>>(list);
                return result;
            }catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<FeedbackResponseDTO> CreateFeedback(CreateFeedbackModel model)
        {
            try
            {
                var reportExisting = _reportRepository.GetByID(model.ReportId);
                if(reportExisting== null)
                {
                    throw new Exception("Report is not existing!!!");
                }
                var doctorExisting = _doctorRepository.GetByID(model.DoctorId);
                Feedback newFeedback = new Feedback()
                {
                    FeedbackId = Guid.NewGuid(),
                    ReportId = model.ReportId,
                    DoctorId = model.DoctorId,
                    FeedbackContentRequest = model.FeedbackContentRequest,
                    FeedbackCreateDate = _currentTime.GetCurrentTime(),
                    FeedbackName = model.FeedbackName,
                    FeedbackIsActive = model.FeedbackIsActive,
                    FeedbackContentResponse = model.FeedbackContentResponse,
                    IsResponsed = false
                };
                _feedbackRepository.Insert(newFeedback);
                _feedbackRepository.Save();
                var result = _mapper.Map<FeedbackResponseDTO>(newFeedback);
                return result;
            }catch(Exception ex) { throw ex; }
        }
        public async Task<FeedbackResponseDTO> UpdateFeedback(UpdateFeedbackModel model, string id)
        {
            try
            {
                var feedbackId = Guid.Parse(id);
                var feedbackExisting = _feedbackRepository.GetFeedbackByIdIncludeProperties(feedbackId);
                if(feedbackExisting== null)
                {
                    throw new Exception("Feedback is not existing!!!");
                }
                feedbackExisting.FeedbackUpdateDate = _currentTime.GetCurrentTime();
                feedbackExisting.FeedbackContentResponse = model.FeedbackContentResponse;
                feedbackExisting.IsResponsed = true;
                _feedbackRepository.Update(feedbackExisting);
                _feedbackRepository.Save();
                var result = _mapper.Map<FeedbackResponseDTO>(feedbackExisting);
                return result;
            }catch (Exception ex) { throw ex; }
        }
        public async Task<FeedbackResponseDTO> ChangeActiveOfFeedback(string id)
        {
            try
            {
                var feedbackId = Guid.Parse(id);
                var feedbackExisting = _feedbackRepository.GetFeedbackByIdIncludeProperties(feedbackId);
                if (feedbackExisting == null)
                {
                    throw new Exception("Feedback is not existing!!!");
                }
                if(feedbackExisting.FeedbackIsActive == true)
                {
                    feedbackExisting.FeedbackIsActive = false;
                }else if (feedbackExisting.FeedbackIsActive == false)
                {
                    feedbackExisting.FeedbackIsActive = true;
                }
                _feedbackRepository.Update(feedbackExisting);
                _feedbackRepository.Save();
                var result = _mapper.Map<FeedbackResponseDTO>(feedbackExisting);
                return result;
            }catch(Exception ex) { throw ex; }
        }
        public async Task<List<FeedbackResponseDTO>> ListFeedbackByChildId(string childId)
        {
            try
            {
                var cId = Guid.Parse(childId);
                var list = _feedbackRepository.GetFeedbacksByChildId(cId);
                if (list == null || !list.Any())
                {
                    throw new Exception("This child doesn't have any feedback");
                }
                var result = _mapper.Map<List<FeedbackResponseDTO>>(list);
                return result;
            }catch (Exception ex) { throw ex; }
        }
        public async Task<List<FeedbackResponseDTO>> ListFeedbackByDoctorId(string doctorId)
        {
            try
            {
                var dId = Guid.Parse(doctorId);
                var list = _feedbackRepository.GetFeedbackByDoctorId(dId);
                if (list == null || !list.Any())
                {
                    throw new Exception("This doctor doesn't have any feedback");
                }
                var result = _mapper.Map<List<FeedbackResponseDTO>>(list);
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
