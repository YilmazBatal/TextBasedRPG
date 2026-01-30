using TextBasedRPG.Interfaces;

namespace TextBasedRPG.Heroes
{
    public abstract class Heroes : IDamageable
    {
        public List<Item>? Inventory {  get; set; } = new List<Item>();
        // Basic Info
        public string? ClassName { get; protected set; }
        public string? Description { get; protected set; }
        // Equipments 
        public Weapon? EquippedWeapon { get; set; }
        public Armor? EquippedArmor { get; set; }
        // Base Stats
        public int BaseHP { get; protected set; }
        public int BaseATK { get; protected set; }
        public int BaseDEF { get; protected set; }
        // Experience and Level
        public int Level { get; internal set; } = 1;
        public int Experience { get; internal set; } = 0;
        public int ExpToNextLevel { get; protected set; } = 100;
        // Stat points
        public int UnusedStatPoints { get; internal set; } = 0;
        public int InvestedSTRPoints { get; internal set; } = 0;
        public int InvestedVITPoints { get; internal set; } = 0;
        public int InvestedDEXPoints { get; internal set; } = 0;
        // Advanced stats
        public double CritRate { get; protected set; } = 5.0; // %
        public double CritDamage { get; protected set; } = 50.0; // %
        // Currency
        public int Gold { get; internal set; } = 100;


        public int TotalATK
        {
            get { return BaseATK + (InvestedSTRPoints * 2); }
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
