using TextBasedRPG.Entities;

namespace TextBasedRPG.Managers
{
    internal class CombatManager
    {
        //public static void StartCombat(GameContext context)
        //{
        //    GenerateEnemy(context);
        //}
        //public static Entity GenerateEnemy(GameContext context)
        //{
        //    string id = "E0001";
        //    var template = context.Entities?.FirstOrDefault(e => e.ID == id); // For now its hardcoded

        //    if (template == null) throw new Exception($"[DATA ERROR] Couldnt find: {id}. Please check if Entities.json exists.");

        //    //Entity newEntity;
        //    Entity? newEntity = null;
        //    if (template.EntityType == EntityType.Boss.ToString())
        //    {
        //        newEntity = new Boss();
        //    }
        //    else if (template.EntityType == EntityType.Enemy.ToString())
        //    {
        //        newEntity = new Enemy();
        //    }
        //    else
        //    {
        //        newEntity = new Entity();
        //    }

        //        // Mapping
        //        newEntity.ID = template.ID;
        //    newEntity.Name = template.Name;
        //    newEntity.BaseHP = template.BaseHP;
        //    newEntity.BaseATK = template.BaseATK;
        //    newEntity.BaseDEF = template.BaseDEF;
        //    newEntity.Level = template.Level;
        //    newEntity.Scaling = template.Scaling;
        //    newEntity.EliteChance = template.EliteChance;
        //    newEntity.LootTable = template.LootTable; // Dictionary referans kalabilir şimdilik
        //    newEntity.GoldDrop = template.GoldDrop;
        //    //newEntity.EntityType = template.EntityType

        //    // Dice roll
        //    newEntity.Initialize();

        //    return newEntity;
        //}
        ////public 
    }
}
