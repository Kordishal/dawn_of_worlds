﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateMountain : ShapeLand
    {

        public override bool Precondition(Deity creator)
        {
            // needs a possible terrain in the area.
            if (candidate_terrain().Count == 0)
                return false;

            return true;
        }

        private List<Tile> candidate_terrain()
        {
            List<Tile> terrain_list = new List<Tile>();
            foreach (Tile terrain in _location.TerrainArea)
            {
                if (terrain.Type == TerrainType.MountainRange)
                    terrain_list.Add(terrain);
            }

            return terrain_list;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Earth))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override void Effect(Deity creator)
        {
            List<Tile> mountain_locations = candidate_terrain();
            Tile mountain_location = mountain_locations[Constants.Random.Next(mountain_locations.Count)];
            Mountain mountain = new Mountain("PlaceHolder", mountain_location, creator);

            int chance = Constants.Random.Next(100);
            switch (_location.ClimateArea)
            {
                case Climate.Arctic:
                    mountain.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.SubArctic:                   
                    if (chance < 50)
                        mountain.BiomeType = BiomeType.Tundra;
                    else 
                        mountain.BiomeType = BiomeType.BorealForest;
                    break;
                case Climate.Temperate:
                    if (chance < 50)
                        mountain.BiomeType = BiomeType.TemperateGrassland;
                    else
                        mountain.BiomeType = BiomeType.TemperateDeciduousForest;
                    break;
                case Climate.SubTropical:
                    if (chance < 50)
                        mountain.BiomeType = BiomeType.TropicalGrassland;
                    else
                        mountain.BiomeType = BiomeType.TropicalDryForest;
                    break;
                case Climate.Tropical:
                    if (chance < 50)
                        mountain.BiomeType = BiomeType.TropicalGrassland;
                    else
                        mountain.BiomeType = BiomeType.TropicalRainforest;
                    break;           
            }

            mountain.Name = Constants.Names.GetName("mountains");
            ((MountainRange)mountain_location.PrimaryTerrainFeature).Mountains.Add(mountain);
            mountain.Range = (MountainRange)mountain_location.PrimaryTerrainFeature;
            mountain_location.UnclaimedTerritories.Add(mountain);
            mountain_location.UnclaimedTravelAreas.Add(mountain);
            mountain_location.UnclaimedHuntingGrounds.Add(mountain);
            creator.TerrainFeatures.Add(mountain);
            creator.LastCreation = mountain;

            Program.WorldHistory.AddRecord(mountain);
        }


        public CreateMountain(Area location) : base (location)
        {
            Name = "Create Mountain in Area " + location.Name;
        }
    }
}
