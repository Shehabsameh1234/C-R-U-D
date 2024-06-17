using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage = "FName is Required")]
		[MaxLength(25,ErrorMessage ="max length is 25")]
		[MinLength(3, ErrorMessage = "min length is 3")]
		public string FName { get; set; }
		[Required(ErrorMessage = "LName is Required")]
		[MaxLength(25, ErrorMessage = "max length is 25")]
		[MinLength(3, ErrorMessage = "min length is 3")]
		public string LName { get; set; }
		[Required(ErrorMessage ="Email is Required")]
		
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password is Required")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password),ErrorMessage ="Confirm Password does't math password")]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }
		[Display(Name = "Is Agree")]
		[Required(ErrorMessage ="Required To Agree")]
		public bool IsAgree { get; set; }

    }
}
