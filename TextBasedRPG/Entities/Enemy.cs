namespace TextBasedRPG.Entities
{
    internal class Enemy : Entity
    {
        public override void Initialize()
        {
            isElite = Random.Shared.Next(0, 100) < EliteChance;
            GeneratedLevel = Math.Max(1, Random.Shared.Next(Level - LevelInterval, Level + LevelInterval + 1));
            CurHP = TotalHP;
        }
    }
}
