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
        // GetRatingById Test
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
        // GetListRating Test
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
        // GetListRatingOfParent Test
        [Fact]
        public async Task GetListRatingOfParent_ShouldReturnList_WhenParentIdIsValid()
        {
            // Arrange
            var parentId = Guid.NewGuid();
            var ratings = new List<DataObjects_BE.Entities.Rating>
        {
            new DataObjects_BE.Entities.Rating { RatingId = Guid.NewGuid(), ParentId = parentId, RatingValue = 4.5, IsActive = true },
            new DataObjects_BE.Entities.Rating { RatingId = Guid.NewGuid(), ParentId = parentId, RatingValue = 3.8, IsActive = true }
        };
            var ratingDtos = new List<RatingResponseDTO>
        {
            new RatingResponseDTO { RatingId = ratings[0].RatingId, RatingValue = 4.5 },
            new RatingResponseDTO { RatingId = ratings[1].RatingId, RatingValue = 3.8 }
        };

            _ratingRepositoryMock
                .Setup(repo => repo.GetListRatingActiveOfParent(parentId))
                .Returns(ratings);

            _mapperMock
                .Setup(mapper => mapper.Map<List<RatingResponseDTO>>(ratings))
                .Returns(ratingDtos);

            // Act
            var result = await _ratingService.GetListRatingOfParent(parentId.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(4.5, result[0].RatingValue);
            Assert.Equal(3.8, result[1].RatingValue);

            _ratingRepositoryMock.Verify(repo => repo.GetListRatingActiveOfParent(parentId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<RatingResponseDTO>>(ratings), Times.Once);
        }
        [Fact]
        public async Task GetListRatingOfParent_ShouldThrowException_WhenListIsEmpty()
        {
            // Arrange
            var parentId = Guid.NewGuid();
            List<DataObjects_BE.Entities.Rating> emptyRatings = null;

            _ratingRepositoryMock
                .Setup(repo => repo.GetListRatingActiveOfParent(parentId))
                .Returns(emptyRatings);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _ratingService.GetListRatingOfParent(parentId.ToString()));

            Assert.Equal("List is not empty!!!", exception.Message);
            _ratingRepositoryMock.Verify(repo => repo.GetListRatingActiveOfParent(parentId), Times.Once);
        }
        [Fact]
        public async Task GetListRatingOfParent_ShouldThrowException_WhenParentIdIsInvalid()
        {
            // Arrange
            var invalidParentId = "invalid-guid";

            // Act & Assert
            await Assert.ThrowsAsync<FormatException>(() => _ratingService.GetListRatingOfParent(invalidParentId));
        }
        //CreateRating Test
        [Fact]
        public async Task CreateRating_ShouldReturnRatingResponseDTO_WhenDataIsValid()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var currentDate = DateTime.UtcNow.Date;

            var model = new CreateRatingModel
            {
                FeedbackId = feedbackId,
                ParentId = parentId,
                RatingValue = 4.5,
                IsActive = true
            };

            var newRating = new DataObjects_BE.Entities.Rating
            {
                RatingId = Guid.NewGuid(),
                FeedbackId = feedbackId,
                ParentId = parentId,
                RatingValue = model.RatingValue,
                RatingDate = currentDate,
                IsActive = model.IsActive
            };

            var ratingDto = new RatingResponseDTO
            {
                RatingId = newRating.RatingId,
                RatingValue = newRating.RatingValue
            };

            _feedbackRepositoryMock
                .Setup(repo => repo.GetByID(feedbackId))
                .Returns(new Feedback());

            _parentRepositoryMock
                .Setup(repo => repo.GetByID(parentId))
                .Returns(new Parent());

            _currentTimeMock
                .Setup(time => time.GetCurrentTime())
                .Returns(currentDate);

            _ratingRepositoryMock
                .Setup(repo => repo.Insert(It.IsAny<DataObjects_BE.Entities.Rating>()));

            _ratingRepositoryMock
                .Setup(repo => repo.Save());

            _mapperMock
                .Setup(mapper => mapper.Map<RatingResponseDTO>(It.IsAny<DataObjects_BE.Entities.Rating>()))
                .Returns(ratingDto);

            // Act
            var result = await _ratingService.CreateRating(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.RatingValue, result.RatingValue);

            _feedbackRepositoryMock.Verify(repo => repo.GetByID(feedbackId), Times.Once);
            _parentRepositoryMock.Verify(repo => repo.GetByID(parentId), Times.Once);
            _ratingRepositoryMock.Verify(repo => repo.Insert(It.IsAny<DataObjects_BE.Entities.Rating>()), Times.Once);
            _ratingRepositoryMock.Verify(repo => repo.Save(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<RatingResponseDTO>(It.IsAny<DataObjects_BE.Entities.Rating>()), Times.Once);
        }
        [Fact]
        public async Task CreateRating_ShouldThrowException_WhenFeedbackDoesNotExist()
        {
            // Arrange
            var model = new CreateRatingModel
            {
                FeedbackId = Guid.NewGuid(),
                ParentId = Guid.NewGuid(),
                RatingValue = 4.5,
                IsActive = true
            };

            _feedbackRepositoryMock
                .Setup(repo => repo.GetByID(model.FeedbackId))
                .Returns((Feedback)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _ratingService.CreateRating(model));

            Assert.Equal("Feedback is not existing!!!", exception.Message);
            _feedbackRepositoryMock.Verify(repo => repo.GetByID(model.FeedbackId), Times.Once);
            _parentRepositoryMock.Verify(repo => repo.GetByID(It.IsAny<Guid>()), Times.Never);
            _ratingRepositoryMock.Verify(repo => repo.Insert(It.IsAny<DataObjects_BE.Entities.Rating>()), Times.Never);
        }
        [Fact]
        public async Task CreateRating_ShouldThrowException_WhenParentDoesNotExist()
        {
            // Arrange
            var model = new CreateRatingModel
            {
                FeedbackId = Guid.NewGuid(),
                ParentId = Guid.NewGuid(),
                RatingValue = 4.5,
                IsActive = true
            };

            _feedbackRepositoryMock
                .Setup(repo => repo.GetByID(model.FeedbackId))
                .Returns(new Feedback());

            _parentRepositoryMock
                .Setup(repo => repo.GetByID(model.ParentId))
                .Returns((Parent)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _ratingService.CreateRating(model));

            Assert.Equal("Parent is not existing!!!", exception.Message);
            _feedbackRepositoryMock.Verify(repo => repo.GetByID(model.FeedbackId), Times.Once);
            _parentRepositoryMock.Verify(repo => repo.GetByID(model.ParentId), Times.Once);
            _ratingRepositoryMock.Verify(repo => repo.Insert(It.IsAny<DataObjects_BE.Entities.Rating>()), Times.Never);
        }


    }
}
