using AutoMapper;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;
using DTOs_BE.RatingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects_BE.Entities;

namespace Services_BE.Services
{
    public class RatingService: IRatingService
    {
        private IRatingRepository _ratingRepository;
        private IMapper _mapper;
        private ICurrentTime _currentTime;
        private IFeedbackRepository _feedbackRepository;
        private IParentRepository _parentRepository;
        public RatingService(IRatingRepository ratingRepository, IMapper mapper, ICurrentTime currentTime, IFeedbackRepository feedbackRepository,IParentRepository parentRepository)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
            _currentTime = currentTime;
            _feedbackRepository = feedbackRepository;
            _parentRepository = parentRepository;
        }
        public async Task<RatingResponseDTO> GetRatingById(string id)
        {
            try
            {
                var ratingId = Guid.Parse(id);
                var ratingExisting = _ratingRepository.GetRatingByIdIncludeProperties(ratingId);
                if(ratingExisting == null) 
                {
                    throw new Exception("Rating is not existing!!!");
                }
                var result = _mapper.Map<RatingResponseDTO>(ratingExisting);
                return result;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<RatingResponseDTO>> GetListRating()
        {
            try
            {
                var list = _ratingRepository.Get(includeProperties: "Feedback,Parent");
                if(list == null||!list.Any())
                {
                    throw new Exception("List is empty!!!");
                }
                var result = _mapper.Map<List<RatingResponseDTO>>(list);
                return result;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<RatingResponseDTO>>GetListRatingOfParent(string parentId)
        {
            try
            {
                var id = Guid.Parse(parentId);
                var list = _ratingRepository.GetListRatingActiveOfParent(id);
                if(list == null)
                {
                    throw new Exception("List is not empty!!!");
                }
                var result = _mapper.Map<List<RatingResponseDTO>>(list);
                return result;
            }catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RatingResponseDTO> CreateRating(CreateRatingModel model)
        {
            try
            {
                var feedbackExisting = _feedbackRepository.GetByID(model.FeedbackId);
                if(feedbackExisting == null)
                {
                    throw new Exception("Feedback is not existing!!!");
                }
                var parentExisting = _parentRepository.GetByID(model.ParentId);
                if(parentExisting == null)
                {
                    throw new Exception("Parent is not existing!!!");
                }
                var newRating = new Rating()
                {
                    RatingId = Guid.NewGuid(),
                    FeedbackId = model.FeedbackId,
                    ParentId = model.ParentId,
                    RatingValue = model.RatingValue,
                    RatingDate = _currentTime.GetCurrentTime(),
                    IsActive = model.IsActive,
                };
                _ratingRepository.Insert(newRating);
                _ratingRepository.Save();
                var result = _mapper.Map<RatingResponseDTO>(newRating);
                return result;
            }catch( Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RatingResponseDTO>UpdateRating(UpdateRatingModel model, string id)
        {
            try
            {
                var ratingId = Guid.Parse(id);
                var ratingExisting = _ratingRepository.GetRatingByIdIncludeProperties(ratingId);
                if(ratingExisting == null)
                {
                    throw new Exception("Rating is not existing!!!");
                }
                ratingExisting.RatingValue = model.RatingValue;
                ratingExisting.RatingDate = _currentTime.GetCurrentTime();
                _ratingRepository.Update(ratingExisting);
                _ratingRepository.Save();
                var result = _mapper.Map<RatingResponseDTO>(ratingExisting);
                return result;
            }catch ( Exception ex)
            {
                throw ex;
            }
        }
        public async Task<RatingResponseDTO> ChangeActiveRating(string id)
        {
            try
            {
                var ratingId = Guid.Parse(id);
                var ratingExisting = _ratingRepository.GetRatingByIdIncludeProperties(ratingId);
                if (ratingExisting == null)
                {
                    throw new Exception("Rating is not existing!!!");
                }
                if(ratingExisting.IsActive== false)
                {
                    ratingExisting.IsActive = true;
                }
                else
                {
                    ratingExisting.IsActive = false;
                }
                _ratingRepository.Update(ratingExisting);
                _ratingRepository.Save();
                var result = _mapper.Map<RatingResponseDTO>(ratingExisting);
                return result;
            }catch( Exception ex)
            {
                throw ex;
            }
        }
    }
}
