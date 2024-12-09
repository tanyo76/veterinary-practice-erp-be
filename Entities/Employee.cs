using System.ComponentModel.DataAnnotations;

namespace UsersRestApi.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
