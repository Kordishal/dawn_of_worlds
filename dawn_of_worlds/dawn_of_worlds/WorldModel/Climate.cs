﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.WorldModel
{
    [Serializable]
    enum Climate
    {
        Arctic,
        SubArctic,
        Temperate,
        SubTropical,
        Tropical,

        Inferno
    }

    [Serializable]
    enum ClimateModifier
    {
        None,
        MagicInfused,
    }
}
