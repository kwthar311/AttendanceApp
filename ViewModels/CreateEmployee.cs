 using System.ComponentModel.DataAnnotations;

namespace AttendanceApp.ViewModels
{
    public class CreateEmployee
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public DateTime WorkDay { get; set; }

        [Required]
        public TimeSpan StandardCheckIn { get; set; }

        [Required]
        public DateTime CheckIn { get; set; }

        [Required]
        public TimeSpan StandardCheckOut { get; set; }

        [Required]
        public DateTime CheckOut { get; set; }
    }
}
