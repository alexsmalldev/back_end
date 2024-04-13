using System.ComponentModel.DataAnnotations;

namespace TimeSheetApplicaiton.Models
{
    public class Contractor
    {
        [Key]
        public int ContractorId { get; set; }
        public string Name { get; set;}
        public string? Email { get; set;}
        public string? Phone { get; set;}
        
        //relationships

        //Jobs
        public ICollection<Job>? Jobs { get; set; }
    }
}
