using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppscoreAncestry.Models
{
    public class PersonData : IPersonData
    {
        private Person _person { get; set; }

        public PersonData(Person person)
        {
            if (person == null)
            {
                throw new NullReferenceException("person cannot be null");
            }
            _person = person;
        }

        public int ID { get => _person.ID; }

        public string Name { get => _person.Name; }

        public string Gender { get => _person.Gender == "M" ? "Male" : "Female"; }

        public string BirthPlace
        {
            get
            {
                string result = "";
                if (_person.PlaceObj != null)
                {
                    result = _person.PlaceObj.Name;
                }
                return result;
            }
        }
    }
}
