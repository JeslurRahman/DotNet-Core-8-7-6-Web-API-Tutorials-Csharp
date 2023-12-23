using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyLogController : ControllerBase
    {
        //1. Strongly/ Tightly Coupled Technique
        /*
        private readonly IMyLogger _myLogger;
        public MyLogController()
        {
            _myLogger = new LogToDB();

             //Lets say we have creatred instance for logger in many controller
             //If we need to modify the instance into "_myLogger = new LogToFile();"
             //we need to modify in the all the controller so this is difficult and time taking process
        }
        */

        //2. Loosely Coupled Technique
        /*
        private readonly IMyLogger _myLogger;
        public MyLogController(IMyLogger myLogger)
        {
            _myLogger = myLogger;

            //We have 3 diff loging mechanism which instance will it inject here for that
            //we need to configure/ register that which type of instance we wanted.
            //if we need to register that in the Dependency injection we need to got Program.cs.
            //Go and Check Program.cs file.
            
        }
        */

        //Built-In Logger
        public readonly ILogger<MyLogController> _logger;

        public MyLogController(ILogger<MyLogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //_myLogger.Log("Index Method Started");

            _logger.LogTrace("Log Message from trace method");
            _logger.LogDebug("Log Message from Debug method");
            _logger.LogInformation("Log Message from Information method");
            _logger.LogWarning("Log Message from Warning method");
            _logger.LogError("Log Message from Error method");
            _logger.LogCritical("Log Message from Critical method");

            return Ok();
        }
    }
}
