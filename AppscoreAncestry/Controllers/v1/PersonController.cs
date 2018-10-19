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
                return new List<IPersonData>();
            }

            // Return all when ask for both
            if (gender == "MF")
            {
                gender = "";
            }

            var person = DataManager.NameDictionary[name];
            HashSet<Person> ansestors = new HashSet<Person>();
            int countToFind = count ?? 10;

            ansestors = BFS_Relatives(person, DataManager.DirectAncestors, countToFind, gender);

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
                return new List<IPersonData>();
            }

            // Return all when ask for both
            if (gender == "MF")
            {
                gender = "";
            }

            var person = DataManager.NameDictionary[name];
            HashSet<Person> descendants = new HashSet<Person>();
            int countToFind = count ?? 10;

            descendants = BFS_Relatives(person, DataManager.DirectDesendants, countToFind, gender);

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
        /// Breadth first search
        /// </summary>
        /// <param name="person"></param>
        /// <param name="descendants"></param>
        /// <param name="count"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        HashSet<Person> BFS_Relatives(int person, Dictionary<int, HashSet<int>> relativesDictionary, int count, string gender)
        {
            // HashSet to save the result.
            HashSet<Person> set = new HashSet<Person>();

            // HashSet to prevent if there is a loop in the data.
            HashSet<int> checkedSet = new HashSet<int>();
            checkedSet.Add(person);
            Queue<Person> queue = new Queue<Person>();
            if (relativesDictionary.ContainsKey(person))
            {
                relativesDictionary[person].ToList().ForEach(p => queue.Enqueue(DataManager.PersonDictionary[p]));
            }

            while (queue.Count > 0)
            {
                int found = set.Count();
                if (!string.IsNullOrEmpty(gender) && gender != "MF")
                {
                    found = set.Count(x => x.Gender.Equals(gender));
                }

                if (found >= count)
                {
                    break;
                }

                var current = queue.Dequeue();
                set.Add(current);
                int newID = current.ID;
                if (checkedSet.Contains(newID))
                {
                    continue;
                }
                checkedSet.Add(newID);
                if (relativesDictionary.ContainsKey(newID))
                {
                    relativesDictionary[newID].ToList().ForEach(x =>
                    {
                        var p = DataManager.PersonDictionary[x];
                        if (!set.Contains(p)) {
                            queue.Enqueue(p);
                        }
                    });
                }
            }
            
            return set;
        }
    }
}