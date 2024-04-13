using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSheetApplicaiton.Models
{
    public class Job
    {

        [Key]
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string JobPostcode { get; set; }
        public string? JobDesc { get; set; }
        //Relationships

        //Contractor relationship
        public int ContractorId { get; set; }
        [ForeignKey("ContractorId")]
        public Contractor? Contractor { get; set; }
        public ICollection<Timesheet>? Timesheets { get; set; }
    }
}
