namespace TextBasedRPG.Entities
{
    internal class Enemy : Entity
    {
        public override void Initialize(int playerLevel, int regionCap)
        {
            isElite = Random.Shared.Next(0, 100) < EliteChance;
            int enemyLevel = Random.Shared.Next(playerLevel - LevelInterval, playerLevel + LevelInterval + 1);
            if (enemyLevel > regionCap)
                enemyLevel = regionCap;
            GeneratedLevel = Math.Max(1, enemyLevel);
            CurHP = TotalHP;
        }
    }
}
