using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gNowWP.ViewModels
{
    class places
    {
        public int idplace;
        public string name;
        public string description;
        public double gravity;
    }

    public class gravityPlaces
    {
        List<places> place = new List<places>();

        public gravityPlaces()
        {
            place.Add(new places() { idplace = 0, name = "Sun", description = "star", gravity = 274 });
            place.Add(new places() { idplace = 1, name = "Mercury", description = "planet", gravity = 3.7 });
            place.Add(new places() { idplace = 2, name = "Venus", description = "planet", gravity = 8.87 });
            place.Add(new places() { idplace = 3, name = "Earth", description = "planet", gravity = 9.798 });
            place.Add(new places() { idplace = 4, name = "Moon", description = "moon", gravity = 1.62 });
            place.Add(new places() { idplace = 5, name = "Mars", description = "planet", gravity = 3.71 });
            place.Add(new places() { idplace = 6, name = "Jupiter", description = "planet", gravity = 24.92 });
            place.Add(new places() { idplace = 7, name = "Saturn", description = "planet", gravity = 10.44 });
            place.Add(new places() { idplace = 8, name = "Uranus", description = "planet", gravity = 8.87 });
            place.Add(new places() { idplace = 9, name = "Neptune", description = "planet", gravity = 11.15 });
            place.Add(new places() { idplace = 10, name = "Pluto", description = "dwarf planet", gravity = 0.58 });
        }

        public int comparedGravity(int id1, int id2)
        {
            double gravity1 = place[id1].gravity;
            double gravity2 = place[id2].gravity;

            if (gravity1 > gravity2)
                return 0;
            else if (gravity1 < gravity2)
                return 1;
            else
                return 2;
        }

        public double percentageGravity(int id1, int id2)
        {
            int result = comparedGravity(id1, id2);

            double gravity1 = place[id1].gravity;
            double gravity2 = place[id2].gravity;

            if (result == 0)
                return (gravity1 * 100) / gravity2;
            else if (result == 1)
                return (gravity2 * 100) / gravity1;
            else
                return 1;
        }

        public string getName(int id)
        {
            return place[id].name;
        }

        public string getDescription(int id)
        {
            return place[id].description;
        }

        public double getGravity(int id)
        {
            return place[id].gravity;
        }
    }
}
