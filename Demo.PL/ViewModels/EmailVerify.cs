using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class EmailVerify
    {
        [Required]
        public int Code { get; set; }
    }
}
