using System.ComponentModel.DataAnnotations;

namespace DTOs_BE.UserDTOs;

public class ChangeUserRoleRequest
{
    [Required]
    public Guid AccountId { get; set; }

    [Required]
    [Range(0, 2, ErrorMessage = "Role must be 0 (Manager), 1 (User), or 2 (Doctor).")]
    public int NewRole { get; set; }
}