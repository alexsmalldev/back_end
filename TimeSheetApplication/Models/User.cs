using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSheetApplicaiton.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; } //should be the users email address
        public string Password { get; set; }
        //relationships

        //Employee
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        //Role
        public int RoleId { get; set; } = 2;
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
