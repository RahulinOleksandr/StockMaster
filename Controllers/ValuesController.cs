using First_web_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace First_web_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ValuesController : ControllerBase
    {
        [Route("GetJobList")]
        [HttpGet]
        public List<Class> GetAllInfo()
        {
            List<Class> list = new List<Class>();

            Class _job1 = new Class()
            {
                jobId = 0,
                jobName = ".Net dev",
                jobDescription = "Web"
            };
            list.Add(_job1);

            Class _job2 = new Class()
            {
                jobId = 1,
                jobName = "React",
                jobDescription = "full stack"
            };
            list.Add(_job2);

            Class _job3 = new Class()
            {
                jobId = 2,
                jobName = "C++",
                jobDescription = "back"
            };
            list.Add(_job3);

            Class _job4 = new Class()
            {
                jobId = 3,
                jobName = "C++",
                jobDescription = "game dev"
            };
            list.Add(_job4);

            return list;
        }
    }
}
