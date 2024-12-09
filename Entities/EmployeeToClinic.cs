using System.ComponentModel.DataAnnotations.Schema;

namespace UsersRestApi.Entities
{
    public class EmployeeToClinic
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int ClinicId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ClinicId")]
        public virtual Clinic Clinic { get; set; }
    }
}
