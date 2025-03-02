using AutoMapper;
using DataObjects_BE.Entities;
using DTOs_BE.RatingDTOs;
using Repositories_BE.Interfaces;
using Services_BE.Interfaces;
using Services_BE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BE.Test.Rating
{
    public class RatingServiceTest: SetupTest
    {
        private readonly IRatingService _ratingService;
        private readonly Mock<IRatingRepository> _ratingRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICurrentTime> _currentTimeMock;
        private readonly Mock<IFeedbackRepository> _feedbackRepositoryMock;
        private readonly Mock<IParentRepository> _parentRepositoryMock;

        public RatingServiceTest()
        {
            _ratingRepositoryMock = new Mock<IRatingRepository>();
            _mapperMock = new Mock<IMapper>();
            _currentTimeMock = new Mock<ICurrentTime>();
            _feedbackRepositoryMock = new Mock<IFeedbackRepository>();
            _parentRepositoryMock = new Mock<IParentRepository>();
            _ratingService = new RatingService(
                _ratingRepositoryMock.Object,
                _mapperMock.Object,
                _currentTimeMock.Object,
                _feedbackRepositoryMock.Object,
                _parentRepositoryMock.Object
                );
        }
        [Fact]
        public async Task GetRatingById_ShouldReturnRating_WhenRatingExists()
        {
            // Arrange
            var ratingId = Guid.NewGuid();
            var feedbackId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var rating = new DataObjects_BE.Entities.Rating
            {
                RatingId = ratingId,
                FeedbackId = feedbackId,
                ParentId = parentId,
                RatingValue = 4.5,
                RatingDate = _currentTimeMock.Object.GetCurrentTime(),
                IsActive = true
            };
            var ratingDto = new RatingResponseDTO
            {
                RatingId = ratingId,
                RatingValue = 4.5,
                RatingDate = rating.RatingDate,
                IsActive = true
            };

            _ratingRepositoryMock
                .Setup(repo => repo.GetRatingByIdIncludeProperties(ratingId))
                .Returns(rating);

            _mapperMock
                .Setup(mapper => mapper.Map<RatingResponseDTO>(rating))
                .Returns(ratingDto);

            // Act
            var result = await _ratingService.GetRatingById(ratingId.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ratingId, result.RatingId);
            Assert.Equal(4.5, result.RatingValue);
            Assert.True(result.IsActive);

            _ratingRepositoryMock.Verify(repo => repo.GetRatingByIdIncludeProperties(ratingId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<RatingResponseDTO>(rating), Times.Once);
        }
        [Fact]
        public async Task GetRatingById_ShouldThrowException_WhenRatingDoesNotExist()
        {
            // Arrange
            var ratingId = Guid.NewGuid();

            _ratingRepositoryMock
                .Setup(repo => repo.GetRatingByIdIncludeProperties(ratingId))
                .Returns((DataObjects_BE.Entities.Rating)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _ratingService.GetRatingById(ratingId.ToString()));

            Assert.Equal("Rating is not existing!!!", exception.Message);
            _ratingRepositoryMock.Verify(repo => repo.GetRatingByIdIncludeProperties(ratingId), Times.Once);
        }
        [Fact]
        public async Task GetListRating_ShouldReturnList_WhenRatingsExist()
        {
            // Arrange
            var ratings = new List<DataObjects_BE.Entities.Rating>
        {
            new DataObjects_BE.Entities.Rating
            {
                RatingId = Guid.NewGuid(),
                FeedbackId = Guid.NewGuid(),
                ParentId = Guid.NewGuid(),
                RatingValue = 4.5,
                RatingDate = _currentTimeMock.Object.GetCurrentTime(),
                IsActive = true
            },
            new DataObjects_BE.Entities.Rating
            {
                RatingId = Guid.NewGuid(),
                FeedbackId = Guid.NewGuid(),
                ParentId = Guid.NewGuid(),
                RatingValue = 3.8,
                RatingDate = _currentTimeMock.Object.GetCurrentTime(),
                IsActive = true
            }
        };

            var ratingDtos = ratings.Select(r => new RatingResponseDTO
            {
                RatingId = r.RatingId,
                RatingValue = r.RatingValue,
                RatingDate = r.RatingDate,
                IsActive = r.IsActive
            }).ToList();

            _ratingRepositoryMock
            .Setup(repo => repo.Get(
                It.IsAny<Expression<Func<DataObjects_BE.Entities.Rating, bool>>>(), // filter
                It.IsAny<Func<IQueryable<DataObjects_BE.Entities.Rating>, IOrderedQueryable<DataObjects_BE.Entities.Rating>>>(), // orderBy
                "Feedback,Parent", // includeProperties
                It.IsAny<int?>(), // pageIndex
                It.IsAny<int?>() // pageSize
            ))
            .Returns(ratings); // Mock danh sách Rating

            _mapperMock
                .Setup(mapper => mapper.Map<List<RatingResponseDTO>>(ratings))
                .Returns(ratingDtos);

            // Act
            var result = await _ratingService.GetListRating();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(ratings[0].RatingId, result[0].RatingId);
            Assert.Equal(ratings[1].RatingId, result[1].RatingId);

            // Verify rằng Get đã được gọi chính xác với "Feedback,Parent"
            _ratingRepositoryMock.Verify(repo => repo.Get(
                It.IsAny<Expression<Func<DataObjects_BE.Entities.Rating, bool>>>(),
                It.IsAny<Func<IQueryable<DataObjects_BE.Entities.Rating>, IOrderedQueryable<DataObjects_BE.Entities.Rating>>>(),
                "Feedback,Parent", // Kiểm tra giá trị includeProperties
                It.IsAny<int?>(),
                It.IsAny<int?>()
            ), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<RatingResponseDTO>>(ratings), Times.Once);
        }
        [Fact]
        public async Task GetListRating_ShouldThrowException_WhenListIsEmpty()
        {
            // Arrange
            _ratingRepositoryMock
            .Setup(repo => repo.Get(
                It.IsAny<Expression<Func<DataObjects_BE.Entities.Rating, bool>>>(), // filter
                It.IsAny<Func<IQueryable<DataObjects_BE.Entities.Rating>, IOrderedQueryable<DataObjects_BE.Entities.Rating>>>(), // orderBy
                "Feedback,Parent", // includeProperties
                It.IsAny<int?>(), // pageIndex
                It.IsAny<int?>() // pageSize
            ))
            .Returns(new List<DataObjects_BE.Entities.Rating>().AsQueryable());


            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _ratingService.GetListRating());

            Assert.Equal("List is empty!!!", exception.Message);
            _ratingRepositoryMock.Verify(repo => repo.Get(
                It.IsAny<Expression<Func<DataObjects_BE.Entities.Rating, bool>>>(),
                It.IsAny<Func<IQueryable<DataObjects_BE.Entities.Rating>, IOrderedQueryable<DataObjects_BE.Entities.Rating>>>(),
                "Feedback,Parent", // Kiểm tra giá trị includeProperties
                It.IsAny<int?>(),
                It.IsAny<int?>()
            ), Times.Once);
        }

    }
}
