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

        [HttpGet("{name}/Ancestors/{count:int?}")]
        [HttpGet("{name}/Ancestors/Gender/{gender}/{count:int?}")]
        public IEnumerable<IPersonData> GetAncestors(string name, string gender, int? count)
        {
            if (!DataManager.NameDictionary.ContainsKey(name))
            {
                return null;
            }

            // Return all when ask for both
            if (gender == "MF")
            {
                gender = "";
            }

            var person = DataManager.NameDictionary[name];
            HashSet<Person> ansestors = new HashSet<Person>();
            int countToFind = count ?? 10;
            FindAncestors(person, ansestors, countToFind, gender);
            
            List<IPersonData> result = new List<IPersonData>();
            foreach (var anscestor in ansestors)
            {
                if (!string.IsNullOrEmpty(gender) && !gender.Equals(anscestor.Gender))
                {
                    continue;
                }
                result.Add(new PersonData(anscestor));
            }

            return result;
        }


        [HttpGet("{name}/Descendants/{count:int?}")]
        [HttpGet("{name}/Descendants/Gender/{gender}/{count:int?}")]
        public IEnumerable<IPersonData> GetDescendants(string name, string gender, int? count)
        {
            if (!DataManager.NameDictionary.ContainsKey(name))
            {
                return null;
            }

            // Return all when ask for both
            if (gender == "MF")
            {
                gender = "";
            }

            var person = DataManager.NameDictionary[name];
            HashSet<Person> descendants = new HashSet<Person>();
            int countToFind = count ?? 10;
            FindDescendants(person, descendants, countToFind, gender);

            List<IPersonData> result = new List<IPersonData>();
            foreach (var descendant in descendants)
            {
                if (!string.IsNullOrEmpty(gender) && !gender.Equals(descendant.Gender))
                {
                    continue;
                }
                result.Add(new PersonData(descendant));
            }

            return result;
        }

        /// <summary>
        /// Recursively looking for descendants with given number and gender
        /// </summary>
        /// <param name="person"></param>
        /// <param name="descendants"></param>
        /// <param name="count"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        HashSet<Person> FindDescendants(int person, HashSet<Person> descendants, int count, string gender)
        {
            int found = descendants.Count();
            if (!string.IsNullOrEmpty(gender) && gender != "MF")
            {
                found = descendants.Count(x => x.Gender.Equals(gender));
            }

            if (found >= count || !DataManager.DirectDesendants.ContainsKey(person))
            {
                return descendants;
            }

            foreach (int id in DataManager.DirectDesendants[person])
            {
                descendants.Add(DataManager.PersonDictionary[id]);
                FindDescendants(id, descendants, count, gender);
            }
            return descendants;
        }

        /// <summary>
        /// Recursively looking for ancestors with given number and gender
        /// </summary>
        /// <param name="person"></param>
        /// <param name="ancestors"></param>
        /// <param name="count"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        HashSet<Person> FindAncestors(int person, HashSet<Person> ancestors, int count, string gender)
        {
            int found = ancestors.Count();
            if (!string.IsNullOrEmpty(gender) && gender != "MF")
            {
                found = ancestors.Count(x => x.Gender.Equals(gender));
            }

            if (found >= count || !DataManager.DirectAncestors.ContainsKey(person))
            {
                return ancestors;
            }

            foreach (int id in DataManager.DirectAncestors[person])
            {
                ancestors.Add(DataManager.PersonDictionary[id]);
                FindAncestors(id, ancestors, count, gender);
            }
            return ancestors;
        }
    }
}