using System.ComponentModel.DataAnnotations;

namespace CPW219_eCommerceSite.Models
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string Phone { get; set; }

        public string Username { get; set; }
    }
}
