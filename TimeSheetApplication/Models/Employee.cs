using System.ComponentModel.DataAnnotations;

namespace TimeSheetApplicaiton.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        public decimal? Wage { get; set; } = 0;
        public string Email { get; set; }
        public bool MileageFlag { get; set; } = false;


        //relationships 

        //users
        public User? User { get; set; }
        public ICollection<Timesheet>? Timesheets { get; set; }

    }
}
