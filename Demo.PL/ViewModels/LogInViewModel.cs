using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class LogInViewModel
	{
		[Required(ErrorMessage = "Email is Required")]

		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Display(Name ="Remember me")]
        public bool RememberMe { get; set; }
    }
}
