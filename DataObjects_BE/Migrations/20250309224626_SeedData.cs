using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataObjects_BE.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            -- Insert Accounts
            INSERT INTO Accounts (AccountId, FirstName, LastName, UserName, Password, Email, PhoneNumber, Address, Role, DateCreateAt, ImageUrl, IsActive) VALUES
            (NEWID(), 'Alice', 'Johnson', 'parent1', 'hashed_password', 'parent1@example.com', '1234567890', '123 Main St', 1, GETUTCDATE(), 'image1.jpg', 1),
            (NEWID(), 'Bob', 'Williams', 'parent2', 'hashed_password', 'parent2@example.com', '0987654321', '456 Oak St', 1, GETUTCDATE(), 'image2.jpg', 1),
            (NEWID(), 'John', 'Doe', 'doctor1', 'hashed_password', 'doctor1@example.com', '1112223333', '789 Pine St', 2, GETUTCDATE(), 'image3.jpg', 1),
            (NEWID(), 'Jane', 'Smith', 'doctor2', 'hashed_password', 'doctor2@example.com', '4445556666', '321 Birch St', 2, GETUTCDATE(), 'image4.jpg', 1),
            (NEWID(), 'Admin', 'Manager', 'manager', 'hashed_password', 'manager@example.com', '7778889999', '654 Cedar St', 0, GETUTCDATE(), 'image5.jpg', 1);

            -- Insert Managers
            INSERT INTO Managers (ManagerId, AccountId) VALUES
            (NEWID(), (SELECT AccountId FROM Accounts WHERE UserName = 'manager'));

            -- Insert Doctors
            INSERT INTO Doctors (DoctorId, AccountId, Specialization, ExperienceYears, HospitalAddressWork) VALUES
            (NEWID(), (SELECT AccountId FROM Accounts WHERE UserName = 'doctor1'), 'Pediatrics', 10, 'City Hospital'),
            (NEWID(), (SELECT AccountId FROM Accounts WHERE UserName = 'doctor2'), 'Nutritionist', 8, 'Children''s Hospital');

            -- Insert Parents
            INSERT INTO Parents (ParentId, AccountId) VALUES
            (NEWID(), (SELECT AccountId FROM Accounts WHERE UserName = 'parent1')),
            (NEWID(), (SELECT AccountId FROM Accounts WHERE UserName = 'parent2'));

            -- Insert Childs
            INSERT INTO Childs (ChildId, ParentId, FirstName, LastName, Gender, DOB, DateCreateAt, DateUpdateAt, ImageUrl) VALUES
            (NEWID(), (SELECT ParentId FROM Parents WHERE AccountId = (SELECT AccountId FROM Accounts WHERE UserName = 'parent1')), 'Emma', 'Johnson', 'Female', '2020-05-15', GETUTCDATE(), GETUTCDATE(), 'child1.jpg'),
            (NEWID(), (SELECT ParentId FROM Parents WHERE AccountId = (SELECT AccountId FROM Accounts WHERE UserName = 'parent2')), 'Liam', 'Williams', 'Male', '2019-08-21', GETUTCDATE(), GETUTCDATE(), 'child2.jpg');

            -- Insert Appointments
            INSERT INTO Appointments (AppointmentId, DoctorId, ParentId, ChildId, ScheduledTime, Status, CreatedAt, UpdatedAt) VALUES
            (NEWID(), (SELECT DoctorId FROM Doctors WHERE AccountId = (SELECT AccountId FROM Accounts WHERE UserName = 'doctor1')), (SELECT ParentId FROM Parents WHERE AccountId = (SELECT AccountId FROM Accounts WHERE UserName = 'parent1')), (SELECT ChildId FROM Childs WHERE FirstName = 'Emma'), '2025-04-01 10:00:00', 1, GETUTCDATE(), GETUTCDATE()),
            (NEWID(), (SELECT DoctorId FROM Doctors WHERE AccountId = (SELECT AccountId FROM Accounts WHERE UserName = 'doctor2')), (SELECT ParentId FROM Parents WHERE AccountId = (SELECT AccountId FROM Accounts WHERE UserName = 'parent2')), (SELECT ChildId FROM Childs WHERE FirstName = 'Liam'), '2025-04-02 14:00:00', 1, GETUTCDATE(), GETUTCDATE());

            -- Insert Reports
            INSERT INTO Reports (ReportId, ChildId, ReportMark, ReportContent, ReprotCreateDate, ReportIsActive, ReportName, Height, Weight, BMI) VALUES
            (NEWID(), (SELECT ChildId FROM Childs WHERE FirstName = 'Emma'), 'A', 'Healthy', GETUTCDATE(), 1, 'Report 1', 120.5, 30.5, 18.5),
            (NEWID(), (SELECT ChildId FROM Childs WHERE FirstName = 'Liam'), 'B', 'Needs more vitamins', GETUTCDATE(), 1, 'Report 2', 130.5, 35.5, 20.5);

            -- Insert Feedbacks
            INSERT INTO Feedbacks (FeedbackId, ReportId, DoctorId, FeedbackContentRequest, FeedbackCreateDate, FeedbackUpdateDate, FeedbackIsActive, FeedbackName, FeedbackContentResponse) VALUES
            (NEWID(), (SELECT ReportId FROM Reports WHERE ReportName = 'Report 1'), (SELECT DoctorId FROM Doctors WHERE AccountId = (SELECT AccountId FROM Accounts WHERE UserName = 'doctor1')), 'Great consultation!', GETUTCDATE(), GETUTCDATE(), 1, 'Feedback 1', 'Response 1'),
            (NEWID(), (SELECT ReportId FROM Reports WHERE ReportName = 'Report 2'), (SELECT DoctorId FROM Doctors WHERE AccountId = (SELECT AccountId FROM Accounts WHERE UserName = 'doctor2')), 'Very helpful advice', GETUTCDATE(), GETUTCDATE(), 1, 'Feedback 2', 'Response 2');

            -- Insert Ratings
            INSERT INTO Ratings (RatingId, FeedbackId, ParentId, RatingValue, RatingDate, IsActive) VALUES
            (NEWID(), (SELECT FeedbackId FROM Feedbacks WHERE FeedbackName = 'Feedback 1'), (SELECT ParentId FROM Parents WHERE AccountId = (SELECT AccountId FROM Accounts WHERE UserName = 'parent1')), 5, GETUTCDATE(), 1),
            (NEWID(), (SELECT FeedbackId FROM Feedbacks WHERE FeedbackName = 'Feedback 2'), (SELECT ParentId FROM Parents WHERE AccountId = (SELECT AccountId FROM Accounts WHERE UserName = 'parent2')), 4, GETUTCDATE(), 1);

            

            -- Insert ProductLists
            INSERT INTO ProductLists (ProductListId, ProductName, ProductDescription, Price, MinAge, MaxAge, SafetyFeature, Rating, RecommendedBy, ImageUrl, Brand, IsActive, ProductType) VALUES
            (NEWID(), 'Multivitamins', 'Daily vitamins for kids', 15.00, 3, 5, 'Feature 1', 4.5, 'Recommender 1', 'url1', 'Brand 1', 1, 'Balanced'),
            (NEWID(), 'Baby Formula', 'Infant milk powder', 30.00, 0, 1, 'Feature 2', 4.0, 'Recommender 2', 'url2', 'Brand 2', 1, 'Balanced');

            -- Insert ReportProducts
            INSERT INTO ReportProducts (ReportProductId, ReportId, ProductListId) VALUES
            (NEWID(), (SELECT ReportId FROM Reports WHERE ReportName = 'Report 1'), (SELECT ProductListId FROM ProductLists WHERE ProductName = 'Multivitamins')),
            (NEWID(), (SELECT ReportId FROM Reports WHERE ReportName = 'Report 2'), (SELECT ProductListId FROM ProductLists WHERE ProductName = 'Baby Formula'));
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DELETE FROM ReportProducts;
            DELETE FROM Ratings;
            DELETE FROM Feedbacks;
            
            
            DELETE FROM Reports;
            DELETE FROM Appointments;
            DELETE FROM Childs;
            DELETE FROM Parents;
            DELETE FROM Managers;
            DELETE FROM Doctors;
            DELETE FROM ProductLists;
            
            DELETE FROM Accounts;
        ");
        }
    }
}
