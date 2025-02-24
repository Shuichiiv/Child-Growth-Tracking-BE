namespace DTOs_BE.UserDTOs
{
    public class ChildDto
    {
        public Guid ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string ImageUrl { get; set; }
    }
}