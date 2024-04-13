using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSheetApplicaiton.Models
{
    public class Timesheet
    {
        [Key]
        public int TimesheetId { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeOn { get; set; }
        public DateTime TimeOff { get; set; }
        public decimal Mileage { get; set; } = 0;

        //relationships

        //Employee
        
        public int EmployeeId { get; set; } //Relationship to employee table but retrieved from user table when a user uploads their timesheet
        [ForeignKey ("EmployeeId")]
        public Employee? Employee { get; set; }

        //Job
        public int JobId { get; set; } //Job table
        [ForeignKey("JobId")]
        public Job? Job { get; set; }

    }
}
