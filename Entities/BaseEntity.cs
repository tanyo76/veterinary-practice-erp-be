using System.ComponentModel.DataAnnotations;

namespace UsersRestApi.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
