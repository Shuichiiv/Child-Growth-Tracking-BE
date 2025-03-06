using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataObjects_BE.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tạo GUID cố định để đảm bảo dữ liệu hợp lệ
            var doctor1 = Guid.NewGuid();
            var doctor2 = Guid.NewGuid();
            var doctor3 = Guid.NewGuid();

            var manager1 = Guid.NewGuid();
            var manager2 = Guid.NewGuid();

            var parent1 = Guid.NewGuid();
            var parent2 = Guid.NewGuid();
            var parent3 = Guid.NewGuid();

            var child1 = Guid.NewGuid();
            var child2 = Guid.NewGuid();
            var child3 = Guid.NewGuid();

            var report1 = Guid.NewGuid();
            var report2 = Guid.NewGuid();
            var report3 = Guid.NewGuid();

            var service1 = 1;
            var service2 = 2;
            var service3 = 3;

            var serviceOrder1 = Guid.NewGuid();
            var serviceOrder2 = Guid.NewGuid();
            var serviceOrder3 = Guid.NewGuid();

            var payment1 = Guid.NewGuid();
            var payment2 = Guid.NewGuid();
            var payment3 = Guid.NewGuid();

            var product1 = Guid.NewGuid();
            var product2 = Guid.NewGuid();
            var product3 = Guid.NewGuid();

            var feedback1 = Guid.NewGuid();
            var feedback2 = Guid.NewGuid();
            var feedback3 = Guid.NewGuid();

            var rating1 = Guid.NewGuid();
            var rating2 = Guid.NewGuid();
            var rating3 = Guid.NewGuid();

            // Chèn dữ liệu vào bảng Accounts
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountId", "FirstName", "LastName", "UserName", "Password", "Email", "PhoneNumber", "Address", "Role", "DateCreateAt", "ImageUrl", "IsActive" },
                values: new object[,]
                {
                    { doctor1, "John", "Doe", "johndoe", "hashedpassword1", "johndoe@example.com", "123456789", "123 Main St", 2, DateTime.UtcNow, "image1.jpg", true },
                    { doctor2, "Jane", "Smith", "janesmith", "hashedpassword2", "janesmith@example.com", "987654321", "456 Oak St", 2, DateTime.UtcNow, "image2.jpg", true },
                    { doctor3, "Alice", "Johnson", "alicejohnson", "hashedpassword3", "alicejohnson@example.com", "1122334455", "789 Pine St", 2, DateTime.UtcNow, "image3.jpg", true },
                    { manager1, "Robert", "Williams", "robertw", "hashedpassword7", "robert@example.com", "2233445566", "789 Birch St", 3, DateTime.UtcNow, "image7.jpg", true },
                    { manager2, "Lisa", "White", "lisaw", "hashedpassword8", "lisa@example.com", "3344556677", "951 Cedar St", 3, DateTime.UtcNow, "image8.jpg", true },
                    { parent1, "Michael", "Brown", "michaelbrown", "hashedpassword4", "michaelbrown@example.com", "555666777", "159 Maple St", 1, DateTime.UtcNow, "image4.jpg", true },
                    { parent2, "Emily", "Davis", "emilydavis", "hashedpassword5", "emilydavis@example.com", "999888777", "852 Walnut St", 1, DateTime.UtcNow, "image5.jpg", true },
                    { parent3, "Chris", "Evans", "chrisevans", "hashedpassword6", "chrisevans@example.com", "123789456", "951 Birch St", 1, DateTime.UtcNow, "image6.jpg", true }
                });

            // Chèn dữ liệu vào bảng Doctors
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "AccountId", "Specialization", "ExperienceYears", "HospitalAddressWork" },
                values: new object[,]
                {
                    { doctor1, doctor1, "Cardiology", 10, "City Hospital" },
                    { doctor2, doctor2, "Pediatrics", 8, "Children's Hospital" },
                    { doctor3, doctor3, "Neurology", 12, "Neuro Clinic" }
                });

            // Chèn dữ liệu vào bảng Managers
            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "ManagerId", "AccountId" },
                values: new object[,]
                {
                    { manager1, manager1 },
                    { manager2, manager2 }
                });

            // Chèn dữ liệu vào bảng Parents
            migrationBuilder.InsertData(
                table: "Parents",
                columns: new[] { "ParentId", "AccountId" },
                values: new object[,]
                {
                    { parent1, parent1 },
                    { parent2, parent2 },
                    { parent3, parent3 }
                });

            // Chèn dữ liệu vào bảng Children
            migrationBuilder.InsertData(
                table: "Children",
                columns: new[] { "ChildId", "ParentId", "FirstName", "LastName", "Gender", "DOB", "DateCreateAt", "DateUpdateAt", "ImageUrl" },
                values: new object[,]
                {
                    { child1, parent1, "Tom", "Brown", "Male", new DateTime(2020, 5, 15), DateTime.UtcNow, DateTime.UtcNow, "child1.jpg" },
                    { child2, parent2, "Sophia", "Davis", "Female", new DateTime(2018, 8, 20), DateTime.UtcNow, DateTime.UtcNow, "child2.jpg" },
                    { child3, parent3, "Jake", "Evans", "Male", new DateTime(2017, 3, 10), DateTime.UtcNow, DateTime.UtcNow, "child3.jpg" }
                });

            // Chèn dữ liệu vào bảng Services
            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceId", "ServiceName", "ServicePrice", "ServiceDescription", "ServiceDuration", "ServiceCreateDate", "IsActive" },
                values: new object[,]
                {
                    { service1, "Service 1", 100f, "Description 1", 1.5f, DateTime.UtcNow, true },
                    { service2, "Service 2", 200f, "Description 2", 2.0f, DateTime.UtcNow, true },
                    { service3, "Service 3", 300f, "Description 3", 2.5f, DateTime.UtcNow, true }
                });

            // Chèn dữ liệu vào bảng ServiceOrders
            migrationBuilder.InsertData(
                table: "ServiceOrders",
                columns: new[] { "ServiceOrderId", "ParentId", "ServiceId", "Quantity", "UnitPrice", "TotalPrice", "CreateDate", "EndDate" },
                values: new object[,]
                {
                    { serviceOrder1, parent1, service1, 1, 100f, 100f, DateTime.UtcNow, DateTime.UtcNow.AddDays(1) },
                    { serviceOrder2, parent2, service2, 2, 200f, 400f, DateTime.UtcNow, DateTime.UtcNow.AddDays(2) },
                    { serviceOrder3, parent3, service3, 3, 300f, 900f, DateTime.UtcNow, DateTime.UtcNow.AddDays(3) }
                });

            // Chèn dữ liệu vào bảng Reports
            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "ReportId", "ChildId", "ReportMark", "ReportContent", "ReprotCreateDate", "ReportIsActive", "ReportName", "Height", "Weight", "BMI" },
                values: new object[,]
                {
                    { report1, child1, "A", "Content 1", DateTime.UtcNow, true, "Report 1", 120.5, 30.5, 18.5 },
                    { report2, child2, "B", "Content 2", DateTime.UtcNow, true, "Report 2", 130.5, 35.5, 20.5 },
                    { report3, child3, "C", "Content 3", DateTime.UtcNow, true, "Report 3", 140.5, 40.5, 22.5 }
                });

            // Chèn dữ liệu vào bảng Payments
            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "ServiceOrderId", "PaymentMethod", "PaymentStatus", "PaymentDate", "Amount" },
                values: new object[,]
                {
                    { payment1, serviceOrder1, "Credit Card", 1, DateTime.UtcNow, 100m },
                    { payment2, serviceOrder2, "PayPal", 2, DateTime.UtcNow, 200m },
                    { payment3, serviceOrder3, "Bank Transfer", 3, DateTime.UtcNow, 300m }
                });

            // Chèn dữ liệu vào bảng Feedbacks
            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "FeedbackId", "ReportId", "DoctorId", "FeedbackContentRequest", "FeedbackCreateDate", "FeedbackUpdateDate", "FeedbackIsActive", "FeedbackName", "FeedbackContentResponse" },
                values: new object[,]
                {
                    { feedback1, report1, doctor1, "Request 1", DateTime.UtcNow, DateTime.UtcNow, true, "Feedback 1", "Response 1" },
                    { feedback2, report2, doctor2, "Request 2", DateTime.UtcNow, DateTime.UtcNow, true, "Feedback 2", "Response 2" },
                    { feedback3, report3, doctor3, "Request 3", DateTime.UtcNow, DateTime.UtcNow, true, "Feedback 3", "Response 3" }
                });

            // Chèn dữ liệu vào bảng ProductLists
            migrationBuilder.InsertData(
                table: "ProductLists",
                columns: new[] { "ProductListId", "ReportId", "ProductName", "ProductDescription", "Price", "MinAge", "MaxAge", "SafetyFeature", "Rating", "RecommendedBy", "ImageUrl", "Brand", "IsActive" },
                values: new object[,]
                {
                    { product1, report1, "Product 1", "Description 1", 100m, 3, 5, "Feature 1", 4.5, "Recommender 1", "url1", "Brand 1", true },
                    { product2, report2, "Product 2", "Description 2", 200m, 6, 8, "Feature 2", 4.0, "Recommender 2", "url2", "Brand 2", true },
                    { product3, report3, "Product 3", "Description 3", 300m, 9, 12, "Feature 3", 3.5, "Recommender 3", "url3", "Brand 3", true }
                });

            // Chèn dữ liệu vào bảng Ratings
            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "RatingId", "FeedbackId", "ParentId", "RatingValue", "RatingDate", "IsActive" },
                values: new object[,]
                {
                    { rating1, feedback1, parent1, 4.5, DateTime.UtcNow, true },
                    { rating2, feedback2, parent2, 4.0, DateTime.UtcNow, true },
                    { rating3, feedback3, parent3, 3.5, DateTime.UtcNow, true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Accounts;");
            migrationBuilder.Sql("DELETE FROM Doctors;");
            migrationBuilder.Sql("DELETE FROM Managers;");
            migrationBuilder.Sql("DELETE FROM Parents;");
            migrationBuilder.Sql("DELETE FROM Children;");
            migrationBuilder.Sql("DELETE FROM Services;");
            migrationBuilder.Sql("DELETE FROM ServiceOrders;");
            migrationBuilder.Sql("DELETE FROM Reports;");
            migrationBuilder.Sql("DELETE FROM Payments;");
            migrationBuilder.Sql("DELETE FROM Feedbacks;");
            migrationBuilder.Sql("DELETE FROM ProductLists;");
            migrationBuilder.Sql("DELETE FROM Ratings;");
        }
    }
}
