using System.ComponentModel.DataAnnotations;

namespace core.Models
{
    public class UserModel
    {
        private string? _username;

        [EmailAddress]
        public string Email { get; set; }

        public string Username
        {
            get => string.IsNullOrEmpty(_username) ? _username = Email : _username;
            set
            {
                _username = value;
            }
        }

        [Required]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}
