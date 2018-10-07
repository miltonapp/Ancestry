using AppscoreAncestry.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AppscoreAncestry.Controllers
{
    [Produces("application/json")]
    [Route("api/Person")]
    public class PersonController : Controller
    {
        [HttpGet("{name}")]
        [HttpGet("{name}/Gender/{gender}")]
        public IEnumerable<IPersonData> Get(string name, string gender)
        {
            return new List<IPersonData> { new PersonData(new Person { ID = 1, Name = "Test" }) };
        }
    }
}