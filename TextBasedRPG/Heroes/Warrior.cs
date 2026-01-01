using System;
using System.Collections.Generic;
using System.Text;

namespace TextBasedRPG.Heroes
{
    internal class Warrior : Heroes
    {
        public Warrior()
        {
            ClassName = "Warrior";
            BaseHP = 150;
            BaseATK = 20;
            BaseDEF = 15;
            Description = "A strong and resilient fighter, excelling in close combat.";
        }
    }
}
