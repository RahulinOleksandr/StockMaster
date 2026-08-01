using System.ComponentModel.DataAnnotations;

namespace Stock_Master.Models
{
    public class Class
    {
        [Key] public int jobId { get; set; }
        public string jobName { get; set; } = string.Empty;
        public string jobDescription { get; set; } = string.Empty;
    }
}
