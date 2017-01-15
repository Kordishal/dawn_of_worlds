﻿using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateCave : ShapeLand
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Create Cave";
            Tags = new List<CreationTag>() { CreationTag.Subterranean, CreationTag.Earth };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            // needs a possible province in the area.
            if (candidate_provinces().Count == 0)
                return false;

            return true;
        }

        public override void Effect(Deity creator)
        {
            List<Province> cave_locations = candidate_provinces();

            // Caves are placed in a random location within the territory.
            Province cave_location = cave_locations[Constants.Random.Next(cave_locations.Count)];

            Cave cave = new Cave("PlaceHolder", cave_location, creator);

            int chance = Constants.Random.Next(100);
            switch (cave_location.LocalClimate)
            {
                case Climate.SubArctic:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
                case Climate.Temperate:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
                case Climate.SubTropical:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
                case Climate.Tropical:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
            }

            cave.Name.Singular = Constants.Names.GetName("caves");
            cave_location.SecondaryTerrainFeatures.Add(cave);

            cave.Modifiers.NaturalDefenceValue += 1;

            // Add forest to the deity.
            creator.TerrainFeatures.Add(cave);
            creator.LastCreation = cave;

            Program.WorldHistory.AddRecord(cave, cave.printTerrainFeature);
        }

        public CreateCave(Area location) : base(location) { }

        private List<Province> candidate_provinces()
        {
            List<Province> province_list = new List<Province>();
            foreach (Province province in _location.Provinces)
            {
                if (province.Type == TerrainType.MountainRange || province.Type == TerrainType.MountainRange)
                    province_list.Add(province);
            }

            return province_list;
        }
    }
}
