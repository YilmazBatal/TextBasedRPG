using TextBasedRPG.Interfaces;

namespace TextBasedRPG.Heroes
{
    public abstract class Hero : IDamageable
    {
        public List<Item>? Inventory {  get; set; } = new List<Item>();
        // Basic Info
        public string? ClassName { get; init; }
        public string? Description { get; init; }
        // Equipments 
        public Weapon? EquippedWeapon { get; set; }
        public Armor? EquippedArmor { get; set; }
        // Base Stats
        public int BaseHP { get; protected set; }
        public int BaseATK { get; protected set; }
        public int BaseDEF { get; protected set; }
        // Experience and Level
        public int Level { get; internal set; } = 1;
        public int CurExp { get; internal set; } = 0; 
        public int ReqExp => (int)(100 * Math.Pow(Level, 1.5));
        // Stat points
        public int UnusedStatPoints { get; internal set; } = 0;
        public int InvestedSTRPoints { get; internal set; } = 0;
        public int InvestedVITPoints { get; internal set; } = 0;
        public int InvestedDEXPoints { get; internal set; } = 0;
        public int InvestedAGIPoints { get; internal set; } = 0;
        // Currency
        public int Gold { get; internal set; } = 100;
        // Advanced stats             
        public int TotalATK => BaseATK + (EquippedWeapon?.WeaponATK ?? 0) + (int)Math.Round(InvestedSTRPoints * 1.5);
        public int TotalDEF => BaseDEF + (EquippedArmor?.ArmorDef ?? 0) + (int)Math.Round(InvestedVITPoints * 1.5);
        public int TotalHP => BaseHP + (EquippedArmor?.ExtraHP ?? 0) + (int)Math.Round(InvestedVITPoints * 1.5);
        public int TotalSPD => 30 + (int)Math.Round(InvestedAGIPoints * 1.5);
        public int CurHP { get; internal set; }
        public double CritRate => 5 + InvestedDEXPoints * 1.0 / 3.0; // %
        public double CritDamage => 150 + InvestedSTRPoints; // %
        public double EvasionRate => 5 + InvestedAGIPoints * 1.0 / 3.0;

        /// <summary>
        /// Method to display hero's base stats summary in choosing screen 
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
