using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersRestApi.Entities
{
    public class EmployeeToClinic : BaseEntity
    {

        [Required]
        public int UserId { get; set; }
        [Required]
        public int ClinicId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ClinicId")]
        public virtual Clinic Clinic { get; set; }
    }
}
