using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppscoreAncestry.Models
{
    public class Person
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public int? Father_Id { get; set; }

        public int? Mother_Id { get; set; }

        public int? Place_Id { get; set; }

        public int? Level { get; set; }
        
        public Place PlaceObj { get; set; }
    }
}
