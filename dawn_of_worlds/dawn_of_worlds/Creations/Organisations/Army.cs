﻿using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.WorldModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Organisations
{

    [Serializable]
    class Army : Creation
    {
        public Civilisation Owner { get; set; }
        public Province Location { get; set; }

        public bool isScattered { get; set; }


        public Army(string name, Deity creator) : base(name, creator)
        {
        }
        // TODO: Implement this funtion... 
        // Should collect all the modifiers an army can suffer/profit from.
        public int getTotalModifier()
        {
            int modifier = 0;


            return modifier;
        }
    }
}
