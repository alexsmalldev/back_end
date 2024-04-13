using System.ComponentModel.DataAnnotations;

namespace TimeSheetApplicaiton.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleShortDesc { get; set; }
        public string RoleLongDesc { get; set; }

        //relationships

        //user 
        public ICollection<User> Users { get; set; }
    }
}
