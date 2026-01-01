using System;
using System.Collections.Generic;
using System.Text;
using TextBasedRPG.Interfaces;

namespace TextBasedRPG.Heroes
{
    public abstract class Heroes : IDamageable
    {
        // Basic Info
        public string ? ClassName { get; protected set; }
        public string ?Description { get; protected set; }
        // Base Stats
        public int BaseHP { get; protected set; }
        public int BaseATK { get; protected set; }
        public int BaseDEF { get; protected set; }
        // Experience and Level
        public int Level { get; protected set; } = 1;
        public int CurrentExp { get; protected set; } = 0;
        public int ExpToNextLevel { get; protected set; } = 100;
        // Stat points
        public int UnusedStatPoints { get; protected set; } = 0;
        public int InvestedAtkPoints { get; protected set; } = 0;
        public int InvestedDefPoints { get; protected set; } = 0;
        public int InvestedHpPoints { get; protected set; } = 0;
        // Advanced stats
        public double CritRate { get; protected set; } = 5.0; // %
        public double CritDamage { get; protected set; } = 50.0; // %
        // Currency
        public int Balance { get; protected set; } = 100;


        public int TotalATK
        {
            get { return BaseATK + (InvestedAtkPoints * 2); }
        }

        /// <summary>
        /// Method to display hero's base stats summary 
        /// </summary>
        public void GetStatsSummary()
        {
            Console.WriteLine($"""
                
                =========================
                === {ClassName} Stats ===
                BASE HP  : {BaseHP}
                BASE ATK : {BaseATK}
                BASE DEF : {BaseDEF}
                -----------------------
                Desc: {Description}
                =========================

                """);
        }

        public void TakeDamage(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
