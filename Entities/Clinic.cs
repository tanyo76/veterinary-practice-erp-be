using System.ComponentModel.DataAnnotations.Schema;

namespace UsersRestApi.Entities
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }
    }
}
