using System.ComponentModel.DataAnnotations;

namespace BoardCore.Models
{
    public class ApiUsers
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}