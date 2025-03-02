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

        [Fact]
        public void GetRatingByIdIncludeProperties_ShouldReturnCorrectRating()
        {
            // Arrange: Tạo Account hợp lệ
            var account = new Account
            {
                AccountId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe123",
                Password = "SecurePassword123",
                Email = "johndoe@example.com",
                PhoneNumber = "0123456789",
                Address = "123 Main Street",
                Role = 2, // Doctor
                DateCreateAt = DateTime.UtcNow,
                IsActive = true,
                ImageUrl = "https://example.com/avatar.jpg"
            };
            _context.Accounts.Add(account);
            _context.SaveChanges();

            // Tạo Doctor
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

            // Tạo Parent
            var parent = new Parent
            {
                ParentId = Guid.NewGuid(),
                AccountId = account.AccountId
            };
            _context.Parents.Add(parent);
            _context.SaveChanges();

            // Tạo Child
            var child = new Child
            {
                ChildId = Guid.NewGuid(),
                ParentId = parent.ParentId,
                FirstName = "Baby",
                LastName = "Doe",
                Gender = "Male",
                DOB = new DateTime(2020, 1, 1),
                DateCreateAt = DateTime.UtcNow,
                DateUpdateAt = DateTime.UtcNow,
                ImageUrl = "https://example.com/baby.jpg"
            };
            _context.Childs.Add(child);
            _context.SaveChanges();

            // Tạo Report
            var report = new Report
            {
                ReportId = Guid.NewGuid(),
                ChildId = child.ChildId,
                ReportMark = "A+",
                ReportContent = "Child health checkup",
                ReprotCreateDate = DateTime.UtcNow,
                ReportIsActive = "Active",
                ReportName = "General Health Report",
                Height = 100.5,
                Weight = 20.2,
                BMI = 15.0
            };
            _context.Reports.Add(report);
            _context.SaveChanges();

            // Tạo Feedback liên kết với Report
            var feedback = new Feedback
            {
                FeedbackId = Guid.NewGuid(),
                ReportId = report.ReportId,
                DoctorId = doctor.DoctorId,
                FeedbackContentRequest = "Initial feedback request",
                FeedbackCreateDate = DateTime.UtcNow,
                FeedbackUpdateDate = DateTime.UtcNow,
                FeedbackIsActive = true,
                FeedbackName = "Feedback 1",
                FeedbackContentResponse = "Doctor's response"
            };
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            // Tạo Rating
            var rating = new DataObjects_BE.Entities.Rating
            {
                RatingId = Guid.NewGuid(),
                FeedbackId = feedback.FeedbackId,
                ParentId = parent.ParentId,
                RatingValue = 4.5,
                RatingDate = DateTime.UtcNow,
                IsActive = true
            };
            _context.Ratings.Add(rating);
            _context.SaveChanges();

            // Act: Gọi phương thức cần test
            var result = _ratingRepository.GetRatingByIdIncludeProperties(rating.RatingId);

            // Assert: Kiểm tra kết quả trả về
            Assert.NotNull(result);
            Assert.Equal(rating.RatingId, result.RatingId);
            Assert.Equal(rating.RatingValue, result.RatingValue);
            Assert.NotNull(result.Feedback);
            Assert.NotNull(result.Feedback.Report);
            Assert.NotNull(result.Feedback.Report.Child);
            Assert.Equal(child.ChildId, result.Feedback.Report.Child.ChildId);
            Assert.Equal(report.ReportId, result.Feedback.Report.ReportId);
            Assert.Equal("A+", result.Feedback.Report.ReportMark);
            Assert.Equal(100.5, result.Feedback.Report.Height);
            Assert.Equal(20.2, result.Feedback.Report.Weight);
            Assert.Equal(15.0, result.Feedback.Report.BMI);
        }
    }
}
