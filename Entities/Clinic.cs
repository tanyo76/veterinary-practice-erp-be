using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersRestApi.Entities
{
    public class Clinic : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string FirstName { get; set; }
        public int OwnerId { get; set; }

        [Required]
        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }
    }
}
