using AppscoreAncestry.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AppscoreAncestry.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Person")]
    public class PersonController : Controller
    {
        /// <summary>
        /// All searchs are case sensive unless requirement specify otherwise.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet("{name}/{count:int?}")]
        [HttpGet("{name}/Gender/{gender}/{count:int?}")]
        public IEnumerable<IPersonData> Get(string name, string gender, int? count)
        {
            IEnumerable<Person> peopleList = null;

            // Return all when ask for both
            if (gender == "MF")
            {
                gender = "";
            }

            if (count.HasValue)
            {
                peopleList = DataManager.PersonDictionary.Values.
                    Where(
                        x => x.Name.Contains(name) &&
                        (string.IsNullOrEmpty(gender) ? true : x.Gender.Equals(gender)) 
                    ).
                    Take(count.Value);
            }
            else
            {
                peopleList = DataManager.PersonDictionary.Values.
                    Where(
                        x => x.Name.Contains(name) &&
                        (string.IsNullOrEmpty(gender) ? true : x.Gender.Equals(gender))
                    );
            }

            List<IPersonData> result = new List<IPersonData>();
            foreach (var person in peopleList)
            {
                result.Add(new PersonData(person));
            }
            return result;
        }
    }
}