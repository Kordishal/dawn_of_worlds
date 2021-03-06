﻿using dawn_of_worlds.Main;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.WorldModel
{
    [Serializable]
    class Region
    {
        public string Name { get; set; }

        public RegionType Type { get; set; }

        [JsonIgnore]
        public World World { get; set; }

        public List<Area> Areas { get; set; }


        public Region(int num_areas)
        {
            Areas = new List<Area>();
            for (int j = 0; j < num_areas; j++)
            {
                Areas.Add(new Area(this));
            }
        }

        public Region(World world, RegionType type)
        {
            Areas = new List<Area>();
            World = world;
            Type = type;

            if (Type == RegionType.Continent)
                Name = Program.GenerateNames.GetName("continent_names");
            if (Type == RegionType.Ocean)
                Name = Program.GenerateNames.GetName("ocean_names");
        }

        public override string ToString()
        {
            return Name;
        }
    }

    enum RegionType
    {
        Continent,
        Ocean,
    }
}
