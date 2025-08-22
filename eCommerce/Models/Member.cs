using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models
{
    public class Member
    {

    }

    public class RegistrationViewModel
    {

    }

    public class LoginViewModel 
    { 
    
        public required string UsernameOrEmail { get; set; }
        public required string Password { get; set; }
    }
}

