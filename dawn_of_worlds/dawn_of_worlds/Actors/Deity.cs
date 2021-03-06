﻿using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.CelestialPowers.CreateRacePowers;
using dawn_of_worlds.CelestialPowers.ShapeClimatePowers;
using dawn_of_worlds.CelestialPowers.ShapeLandPowers;
using dawn_of_worlds.Creations;
using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;
using dawn_of_worlds.WorldModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace dawn_of_worlds.Actors
{
    /// <summary>
    /// The deities are the only actors in dawn of worlds. 
    /// Everything which happens is because of divine intervention. 
    /// 
    /// Deities affect their changes to the world with powers. Each power costs power points.
    /// Each deity gets a random amount of power points per turn. 
    /// 
    /// The deities can use several powers per turn. 
    /// </summary>
    class Deity
    {

        public Random rnd { get; set; }

        /// <summary>
        /// Name of the deity. Randomly assigned. Used to name things created by this deity.
        /// </summary>
        public string Name { get; set; }

        // Resource to use powers.
        public int PowerPoints { get; set; }


        /// <summary>
        /// Each deity has a set amount of domains. They influence what powers the deity uses.
        /// </summary>
        public Modifier[] Domains { get; set; }

        /// <summary>
        /// A list of all powers the deity can currently use.
        /// </summary>
        public List<Power> Powers { get; set; }

        /// <summary>
        /// Modifiers applied to power point generation each turn.
        /// </summary>
        public DeityModifiers Modifiers { get; set; }


        /// <summary>
        /// Stores all the terrain features created by this deity.
        /// </summary>
        /// 
        [JsonIgnore]
        public List<TerrainFeatures> TerrainFeatures { get; set; }
        [JsonIgnore]
        public List<Race> CreatedRaces { get; set; }
        [JsonIgnore]
        public List<Order> CreatedOrders { get; set; }
        [JsonIgnore]
        public List<Avatar> CreatedAvatars { get; set; }
        [JsonIgnore]
        public List<Civilisation> FoundedNations { get; set; }
        [JsonIgnore]
        public List<City> FoundedCities { get; set; }


        /// <summary>
        /// The last used power of this deity. Currently not used.
        /// </summary>
        public Power LastUsedPower { get; set; }


        /// <summary>
        /// Last creation of the deity.
        /// </summary>
        [JsonIgnore]
        public Creation LastCreation { get; set; }

        /// <summary>
        /// Creates a deity with a name, x domains and the powers useable in the beginning.
        /// </summary>
        /// <param name="name"></param>
        public Deity(int seed)
        {   
            TerrainFeatures = new List<TerrainFeatures>();
            CreatedRaces = new List<Race>();
            CreatedOrders = new List<Order>();
            CreatedAvatars = new List<Avatar>();
            FoundedNations = new List<Civilisation>();
            FoundedCities = new List<City>();

            rnd = new Random(seed);
        }


        private int _low_point_turn_bonus { get; set; }

        /// <summary>
        /// Add power points for a new turn.
        /// </summary>
        public void addPowerPoints()
        {
            int gain = 0;

            // Any deity with less than 5 points gets +1 point gain. This gain is cummulative to a max of +3
            // this is to encourage action.
            if (PowerPoints <= 5 && _low_point_turn_bonus < 3)
                 _low_point_turn_bonus += 1;
            else if (PowerPoints > 5)
                _low_point_turn_bonus = 0;

            gain += _low_point_turn_bonus;
            gain += rnd.Next(Constants.DEITY_BASE_POWERPOINT_MIN_GAIN, Constants.DEITY_BASE_POWERPOINT_MAX_GAIN);
            gain += Modifiers.BonusPowerPoints;
            gain += (int)Math.Floor(gain * Modifiers.PowerPointModifier);

            PowerPoints = PowerPoints + gain;
        }

        /// <summary>
        /// What a deity does when they take a turn.
        /// </summary>
        public void Turn()
        {
            List<Power> current_powers = new List<Power>(Powers);
            List<Power> possible_powers = new List<Power>();

            // take actions as long as there are actions to be taken.
            do
            {
                possible_powers = new List<Power>();

                int total_weight = 0;

                foreach (Power p in current_powers)
                {
                    // no powers that are too expensive.
                    if (PowerPoints - p.Cost(this) >= 0)
                    {
                        // no powers where the precondition is not met.
                        if (p.Precondition(this))
                        {
                            possible_powers.Add(p);
                            total_weight += p.Weight(this);
                        }
                    }
                }

                // leave this loop when there are no possible powers.
                if (possible_powers.Count == 0)
                    break;

                int chance = rnd.Next(total_weight);
                int prev_weight = 0, current_weight = 0;
                foreach (Power p in possible_powers)
                {
                    current_weight += p.Weight(this);
                    if (prev_weight <= chance && chance < current_weight)
                    {
                        int return_value = p.Effect(this);
                        if (return_value == 0)
                        {
                            PowerPoints = PowerPoints - p.Cost(this);
                            // For the Action Log entry.
                            _total_power_points_used += p.Cost(this);
                            LastUsedPower = p;
                            break;
                        }
                        else
                        {
                            break;
                        }

                    }


                    prev_weight += p.Weight(this);
                }

            } while (possible_powers.Count != 0);
            
        }

        /// <summary>
        /// A local property which stores the total number of power points used by the deity.
        /// </summary>
        private int _total_power_points_used { get; set; }

        /// <summary>
        /// Puts all the attributes of the deity into a string.
        /// </summary>
        /// <returns>Description of the deity.</returns>
        public string printDeity()
        {
            int counter = 0;
            string result = "";
            result += "Name: " + Name + "\n";
            result += "Domains: ";
            foreach (Modifier domain in Domains)
                result += domain.ToString() + ", ";
            result += "\n";
            result += "Total PowerPoints Used: " + _total_power_points_used + "\n";
            result += "CreationsCount: " + TerrainFeatures.Count.ToString() + "\n";
            result += "Creations: \n";
            foreach (Creation creation in TerrainFeatures)
            {
                result += creation.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            result += "RacesCount: " + CreatedRaces.Count.ToString() + "\n";
            result += "Races: \n";
            foreach (Race race in CreatedRaces)
            {
                result += race.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            result += "OrdersCount: " + CreatedOrders.Count.ToString() + "\n";
            result += "Orders: \n";
            foreach (Order order in CreatedOrders)
            {
                result += order.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            result += "NationsCount: " + FoundedNations.Count.ToString() + "\n";
            result += "Nations: \n";
            foreach (Civilisation nation in FoundedNations)
            {
                result += nation.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            result += "CitiesCount: " + FoundedCities.Count.ToString() + "\n";
            result += "Cities: \n";
            foreach (City city in FoundedCities)
            {
                result += city.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            result += "AvatarsCount: " + CreatedAvatars.Count.ToString() + "\n";
            result += "Avatars: \n";
            foreach (Avatar avatar in CreatedAvatars)
            {
                result += avatar.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            return result;
        }
    }

    /// <summary>
    /// Modifies the amount of power points the deity gets each turn. 
    /// </summary>
    public class DeityModifiers
    {
        public int BonusPowerPoints { get; set; }
        public double PowerPointModifier { get; set; }

        public DeityModifiers()
        {
            BonusPowerPoints = 0;
            PowerPointModifier = 1.0;
        }
    }

}
