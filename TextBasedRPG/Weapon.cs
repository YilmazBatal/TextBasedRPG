using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace TextBasedRPG
{
    public class Weapon
    {
        public string ?weaponName { get; protected set; }
        public int weaponATK { get; protected set; }
        public WeaponType weaponType { get; protected set; }
        public int weaponPrice { get; protected set; }
        public int requiredLevel { get; protected set; }

        public Weapon(string name, int atk, WeaponType type, int price, int level)
        {
            this.weaponName = name;
            this.weaponATK = atk;
            this.weaponType = type;

            this.weaponPrice = price;
            this.requiredLevel = level;
        }
    }
}
