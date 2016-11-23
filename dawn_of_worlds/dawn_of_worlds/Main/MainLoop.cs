﻿using dawn_of_worlds.Actors;
using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Log;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Main
{
    class MainLoop
    {
        public int CurrentAge { get; set; }

        public ActionLog Log { get; set; }
        public World MainWorld { get; set; }

        public MainLoop() { }

        public void Initialize()
        {
            MainWorld = new World("New World", 5, 5);
            Log = new ActionLog();        
        }

        public void Run()
        {
            CurrentAge = 1;
            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@ TURN " + i.ToString() + " @@@@@@@@@@@@@@@@@@@@@@@@@@@");
                foreach (Deity deity in MainWorld.Deities)
                {
                    deity.AddPowerPoints();

                    foreach (City city in deity.FoundedCities)
                    {
                        city.not_hasRaisedArmy = true;
                    }
                }


                for (int j = 0; j < 10; j++)
                {
                    foreach (Deity deity in MainWorld.Deities)
                    {
                        deity.Turn(MainWorld, CurrentAge);

                        Log.Entries.Add(new ActionLogEntry(i, deity, deity.LastUsedPower, deity.LastCreation));
                    }
                }


                foreach (Deity deity in MainWorld.Deities)
                {
                    for (int j = 0; j < deity.Powers.Count; j++)
                    {
                        if (deity.Powers[j].isObsolete)
                        {
                            deity.Powers.Remove(deity.Powers[j]);
                            j -= 1;
                        }
                    }
                }
               
                if (i == 10)
                    CurrentAge += 1;

                if (i == 20)
                    CurrentAge += 1;
            
            }


            Log.Write();
        }

    }
}
