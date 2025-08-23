using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models;

/// <summary>
/// Represents an individual website user
/// </summary>
public class Member
{
    /// <summary>
    /// The unique identifier for the member
    /// </summary>
    [Key]
    public int MemberId { get; set; }

    /// <summary>
    /// Public facing username for the member
    /// Alphanumeric characters only
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// The email for the Member
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The password for the Member
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// The date of birth of the Member
    /// </summary>
    public DateOnly DateOfBirth { get; set; }
}

public class RegistrationViewModel
{

}

public class LoginViewModel 
{ 

    public required string UsernameOrEmail { get; set; }
    public required string Password { get; set; }
}

