using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModel.Home
{
    public class SendEmailViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}