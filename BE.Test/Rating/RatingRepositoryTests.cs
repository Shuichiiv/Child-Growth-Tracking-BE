using DataObjects_BE.Entities;
using Repositories_BE.Interfaces;
using Repositories_BE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Test.Rating
{
    public class RatingRepositoryTests: SetupTest
    {
        private readonly RatingRepository _ratingRepository;

        public RatingRepositoryTests()
        {
            _ratingRepository = new RatingRepository(_context);
        }

        /*[Fact]
        public void GetRatingByIdIncludeProperties_ShouldReturnCorrectRating()
        {
            // Arrange: Tạo một tài khoản hợp lệ cho bác sĩ
            var account = new Account
            {
                AccountId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe123",
                Password = "SecurePassword123", // Nên hash password trước khi lưu
                Email = "johndoe@example.com",
                PhoneNumber = "0123456789",
                Address = "123 Main Street",
                Role = 2, // 2: Doctor
                DateCreateAt = DateTime.UtcNow,
                IsActive = true,
                ImageUrl = "https://example.com/avatar.jpg"
            };
            _context.Accounts.Add(account);
            _context.SaveChanges();

            // Tạo bác sĩ hợp lệ
            var doctor = new Doctor
            {
                DoctorId = Guid.NewGuid(),
                AccountId = account.AccountId,
                Specialization = "Cardiology",
                ExperienceYears = 10,
                HospitalAddressWork = "456 Health Street"
            };
            _context.Doctors.Add(doctor);
            _context.SaveChanges();

            // Tạo một Parent hợp lệ
            var parent = new Parent
            {
                ParentId = Guid.NewGuid(),
                AccountId = account.AccountId
            };
            _context.Parents.Add(parent);
            _context.SaveChanges();

            // Tạo Feedback hợp lệ
            var feedback = new Feedback
            {
                FeedbackId = Guid.NewGuid(),
                ReportId = Guid.NewGuid(), // Giả sử có một báo cáo hợp lệ
                DoctorId = doctor.DoctorId, // Liên kết với bác sĩ
                FeedbackContentRequest = "Initial feedback request",
                FeedbackCreateDate = DateTime.UtcNow,
                FeedbackUpdateDate = DateTime.UtcNow,
                FeedbackIsActive = true,
                FeedbackName = "Feedback 1",
                FeedbackContentResponse = "Doctor's response"
            };
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            // Tạo một Rating hợp lệ
            var rating = new DataObjects_BE.Entities.Rating
            {
                RatingId = Guid.NewGuid(),
                FeedbackId = feedback.FeedbackId,
                ParentId = parent.ParentId,
                RatingValue = 4.5,
                RatingDate = DateTime.UtcNow,
                IsActive = true,
                Feedback = feedback,
                Parent = parent
            };
            _context.Ratings.Add(rating);
            _context.SaveChanges();

            // Act: Lấy rating từ repository
            var result = _ratingRepository.GetRatingByIdIncludeProperties(rating.RatingId);

            // Assert: Kiểm tra dữ liệu trả về
            Assert.NotNull(result);
            Assert.Equal(rating.RatingId, result.RatingId);
            Assert.Equal(rating.RatingValue, result.RatingValue);
            Assert.NotNull(result.Feedback);
            Assert.NotNull(result.Parent);
        }*/
    }
}
