namespace DTOs_BE.DoctorDTOs;

public class DoctorDto
{
        public Guid DoctorId { get; set; }
        public Guid AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialization { get; set; }
        public int ExperienceYears { get; set; }
        public string HospitalAddressWork { get; set; }
        public string ImageUrl { get; set; }
    
        public string FullName { get; set; }

        public double StarRating { get; set; } = 0; // Default rating
}
