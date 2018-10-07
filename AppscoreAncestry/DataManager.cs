using AppscoreAncestry.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AppscoreAncestry
{
    public class DataManager
    {
        static DataManager()
        {
            using (FileStream fs = new FileStream(@".\Data\data_large.json", FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                reader.Read();
                if (reader.TokenType == JsonToken.StartObject)
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            var place = serializer.Deserialize<Place>(reader);
                            PlaceDictionary[place.ID] = place;
                        }
                        else if (reader.TokenType == JsonToken.EndArray)
                        {
                            break;
                        }
                    }
                }

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        var person = serializer.Deserialize<Person>(reader);
                        if (person.Place_Id.HasValue && PlaceDictionary.ContainsKey(person.Place_Id.Value))
                        {
                            person.PlaceObj = PlaceDictionary[person.Place_Id.Value];
                        }
                        PersonDictionary[person.ID] = person;

                        if (!NameDictionary.ContainsKey(person.Name))
                        {
                            NameDictionary[person.Name] = person.ID;
                        }

                        if (!DirectAncestors.ContainsKey(person.ID))
                        {
                            DirectAncestors[person.ID] = new HashSet<int>();
                        }

                        var fatherId = person.Father_Id ?? -1;

                        if (fatherId != -1)
                        {
                            if (!DirectDesendants.ContainsKey(fatherId))
                            {
                                DirectDesendants[fatherId] = new HashSet<int>();
                            }
                            DirectAncestors[person.ID].Add(fatherId);
                            DirectDesendants[fatherId].Add(person.ID);
                        }

                        var motherId = person.Mother_Id ?? -1;
                        if (motherId != -1)
                        {
                            if (!DirectDesendants.ContainsKey(motherId))
                            {
                                DirectDesendants[motherId] = new HashSet<int>();
                            }
                            DirectAncestors[person.ID].Add(motherId);
                            DirectDesendants[motherId].Add(person.ID);
                        }
                    }
                }
            }
        }

        public static Dictionary<int, Place> PlaceDictionary { get; } = new Dictionary<int, Place>();

        public static Dictionary<int, Person> PersonDictionary { get; } = new Dictionary<int, Person>();

        // Save the unique name and the first id with the name.
        public static Dictionary<string, int> NameDictionary { get; } =
            new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        public static Dictionary<int, HashSet<int>> DirectAncestors { get; } = new Dictionary<int, HashSet<int>>();

        public static Dictionary<int, HashSet<int>> DirectDesendants { get; } = new Dictionary<int, HashSet<int>>();
    }
}
