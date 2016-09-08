using System.ComponentModel.DataAnnotations;

namespace Mtg.Web.Models.AuthModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}