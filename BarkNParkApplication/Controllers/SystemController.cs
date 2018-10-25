using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BarkNParkApplication.Controllers
{
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        ASystem system = new ASystem(new Models.BarkParkContext());

        // GET api/appointments
        [HttpGet]
        [Route("/api/appointments")]
        public ObjectResult GetAppointments()
        {
            return new ObjectResult(system.SystemAppointments);
        }

        // GET api/stations
        [HttpGet]
        [Route("/api/stations")]
        public ObjectResult GetStations()
        {
            return new ObjectResult(system.SystemStations);
        }

        // GET api/transactions
        [HttpGet]
        [Route("/api/transactions")]
        public ObjectResult GetTransactions()
        {
            return new ObjectResult(system.SystemTransactions);
        }


        // GET api/getuser/5
        [HttpGet("{id}")]
        [Route("/api/getuser")]
        public ObjectResult GetUser(int id)
        {
            return new ObjectResult(system.SystemUser(id));
        }

        //api/adduser/{first}/{last}
        [HttpPost]
        [Route("/api/adduser")]
        public ObjectResult AddUSer([FromBody]string first, [FromBody] string last)
        {
            return new ObjectResult(system.AddUser(first, last));
        }

        // POST api/checkin
        [HttpPost]
        [Route("/api/checkin")]
        public ObjectResult CheckIn([FromBody]string Name, [FromBody] double duration)
        {
            return new ObjectResult(system.CheckIn(Name, duration));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
