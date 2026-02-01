using TextBasedRPG.Interfaces;

namespace TextBasedRPG.Entities
{
    public abstract class Entity : IDamageable
    {
        public double Scaling = 1.3; 
        public int EliteChance = 5; // %
        public int LevelInterval = 5;

        // JSON datas
        public string ID = string.Empty;
        public string Name = string.Empty;
        public int BaseHP;
        public int BaseATK;
        public int BaseDEF;
        public int Level;
        public Dictionary<string, int>? LootTable;  // ID, Chances%
        public int GoldDrop;
        public EntityType EntityType;

        // Runtime datas
        public int MaxHP => (int)Math.Round((decimal)BaseHP + (decimal)(BaseHP * GeneratedLevel * 20 / 100 * (isElite ? Scaling : 1)));
        public int CurrentHP { get; set; }
        public int CurrentATK => (int)Math.Round((decimal)BaseATK + (decimal)(BaseATK * GeneratedLevel * 20/100 * (isElite ? Scaling : 1)));
        public int CurrentDEF => (int)Math.Round((decimal) BaseDEF + (decimal) (BaseDEF * GeneratedLevel * 5/100 * (isElite? Scaling : 1)));
        public int GeneratedLevel { get; set; }
        public bool isElite { get; set; }
        public bool IsAlive => CurrentHP > 0;
        
        public virtual void Initialize()
        {
            isElite = Random.Shared.Next(0, 100) < EliteChance;
            GeneratedLevel = Math.Max(1, Random.Shared.Next(Level - LevelInterval, Level + LevelInterval)); // Seviye aralığını daralttım
            CurrentHP = MaxHP;
        }

        public void TakeDamage(int amount)
        {
            CurrentHP -= Math.Max(0, amount);
            if (CurrentHP < 0) CurrentHP = 0;
        }
    }
}
