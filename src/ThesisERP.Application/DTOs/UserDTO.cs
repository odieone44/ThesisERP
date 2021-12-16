using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Application.DTOs;

public class LoginUserDTO
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(512, ErrorMessage = "Your Password is limited to {2} to {1} character", MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}

public class RegisterUserDTO : LoginUserDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = string.Empty;
}

public class UserDTO : RegisterUserDTO
{
    public ICollection<string> Roles { get; set; } = new HashSet<string>();
}