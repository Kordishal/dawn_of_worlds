﻿using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.CommandArmyPowers
{
    abstract class CommandArmy : Power
    {
        protected Army _commanded_army { get; set; }


        public override bool isObsolete
        {
            get
            {
                return _commanded_army.isScattered;
            }
        }

        public override int Cost()
        {
            int cost = 0;
            switch (Simulation.Time.CurrentAge)
            {
                case Age.Creation:
                    cost += 6;
                    break;
                case Age.Races:
                    cost += 4;
                    break;
                case Age.Relations:
                    cost += 2;
                    break;
            }

            return cost;
        }

        public override int Weight(Deity deity)
        {
            int weight = 0;

            switch (Simulation.Time.CurrentAge)
            {
                case Age.Creation:
                    weight += Constants.WEIGHT_STANDARD_LOW;
                    break;
                case Age.Races:
                    weight += Constants.WEIGHT_STANDARD_MEDIUM;
                    break;
                case Age.Relations:
                    weight += Constants.WEIGHT_STANDARD_HIGH;
                    break;
            }

            int cost = Cost();
            if (cost > Constants.WEIGHT_COST_DEVIATION_MEDIUM)
                weight += cost * Constants.WEIGHT_STANDARD_COST_DEVIATION;
            else
                weight -= cost * Constants.WEIGHT_STANDARD_COST_DEVIATION;

            return weight >= 0 ? weight : 0;
        }


        public CommandArmy(Army commanded_army)
        {
            _commanded_army = commanded_army;
        }
    }
}
