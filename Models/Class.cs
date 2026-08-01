using System.ComponentModel.DataAnnotations;

namespace First_web_project.Models
{
    public class Class
    {
        [Key] public int jobId { get; set; }
        public string jobName { get; set; } = string.Empty;
        public string jobDescription { get; set; } = string.Empty;
    }
}
