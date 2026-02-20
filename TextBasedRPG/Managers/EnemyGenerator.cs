using TextBasedRPG.Entities;

namespace TextBasedRPG.Managers
{
    internal static class EnemyGenerator
    {
        public static Entity GenerateEnemy(GameContext context)
        {
            string id = "E0001";
            var template = context.Entities?.FirstOrDefault(e => e.ID == id); // For now its hardcoded

            if (template == null) throw new Exception($"[DATA ERROR] Couldnt find: {id}. Please check if Entities.json exists.");

            Entity? newEntity;
            if (template.EntityType == EntityType.Boss)
                newEntity = new Boss();
            else
                newEntity = new Enemy();

            // Mapping
            newEntity.ID = template.ID;
            newEntity.Name = template.Name;
            newEntity.BaseHP = template.BaseHP;
            newEntity.BaseATK = template.BaseATK;
            newEntity.BaseDEF = template.BaseDEF;
            newEntity.Level = template.Level;
            newEntity.Scaling = template.Scaling;
            newEntity.EliteChance = template.EliteChance;
            newEntity.LootTable = template.LootTable;
            newEntity.GoldDrop = template.GoldDrop;
            newEntity.EntityType = template.EntityType;

            newEntity.Initialize();

            newEntity.Level = newEntity.GeneratedLevel;

            return newEntity;
        }
    }
}
