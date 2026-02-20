using TextBasedRPG.Entities;
using TextBasedRPG.Events;
using TextBasedRPG.Interfaces;
using TextBasedRPG.UI;

namespace TextBasedRPG.Managers
{
    internal static class CombatLogic
    {
        /// <returns>Enemy's condition based on HP percentage</returns>
        public static string GetEnemyStatus(Entity enemy)
        {
            if (enemy.CurHP >= enemy.TotalHP * 75 / 100)
                return $"{enemy.Name} looks ready to fight!";
            else if (enemy.CurHP >= enemy.TotalHP * 50 / 100)
                return $"{enemy.Name} is watching you carefuly";
            else if (enemy.CurHP >= enemy.TotalHP * 20 / 100)
                return $"{enemy.Name} looks slightly tired to fight...";
            else
                return $"{enemy.Name} is panicking!";
        }

        #region Enemy Acts
        public static void EnemyAct(GameContext context, Entity entity, List<string> log)
        {
            IDamageCalculator damage = new DamageCalculator();
            int dmg = damage.CalculateDMG(entity.TotalATK, context.Player.TotalDEF, 7.0, 100.0, out bool isCrit);
            Console.WriteLine($"""
                
                \ =-=-=-=-=-=-=-=-=- {entity.Name.ToUpper()}'S TURN =-=-=-=-=-=-=-=-=- /
            
            """);
            Thread.Sleep(500);
            Console.WriteLine(GetEnemyStatus(entity));
            Thread.Sleep(500);
            Console.WriteLine($"""

                {entity.Name} Attacked!

                """);
            context.Player.TakeDamage(dmg);
            log.Add($"E:{entity.Name} dealt {dmg} dmg");
            CombatManager.OnHit(dmg, isCrit);
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();

        }
        #endregion
        #region Player Acts
        public static void Acting(GameContext context, Entity entity, List<string> log)
        {
            bool acted = false;
            string enemyStatus = GetEnemyStatus(entity);
            while (!acted)
            {
                Console.WriteLine($"""
                 
                 \ =-=-=-=-=-=-=-=-=- YOUR TURN =-=-=-=-=-=-=-=-=- /
                 
                 {enemyStatus}

                 What is your act gonna be?
                 (1) Attack          - Best defense is a good offense
                 (2) Focus           - Increase chances of Crit, Dodge or Escape
                 (3) Guard Up        - Take less damage this turn
                 (4) Backpack        - Browse through your Equipments and Consumables
                 (5) Run Away        - Running away is not guaranteed
                 
                 """);
                Console.Write("Selection : ");
                string? input = Console.ReadLine()?.Trim();

                if (input == "1")
                    acted = CombatAttack(context, entity, log);
                else if (input == "2")
                    acted = CombatFocus(context, entity, log);
                else if (input == "3")
                    acted = CombatGuardUp(context, entity, log);
                else if (input == "4")
                    acted = CombatBackpack(context, entity, log);
                else if (input == "5")
                    acted = CombatRunAway(context, entity, log);
                else
                {
                    Console.WriteLine("You flinched be careful! (Turn skipped)");
                    log.Add("P:User flinched damage (Turn skipped)");
                    acted = true;
                }
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }

        }
        public static bool CombatAttack(GameContext context, Entity entity, List<string> log)
        {
            var p = context.Player;
            IDamageCalculator damageCalc = new DamageCalculator();
            int damage = damageCalc.CalculateDMG(p.TotalATK, entity.TotalDEF, p.CritRate, p.CritDamage, out bool isCrit);
            string critText = isCrit ? "Critical hit!" : "";

            Console.WriteLine("\nYou performed and attack");
            Thread.Sleep(1000);
            entity.TakeDamage(damage);

            log.Add($"P:User dealt {damage.ToString()} damage. {critText}");
            CombatManager.OnHit(damage, isCrit);
            return true;
        }
        public static bool CombatFocus(GameContext context, Entity entity, List<string> log)
        {
            new AttackBuff(context.Player, 1, 15);
            log.Add("P: User focesed on the enemy");
            return true;
        }
        public static bool CombatGuardUp(GameContext context, Entity entity, List<string> log)
        {
            new DefenseBuff(context.Player, 1, 15);

            log.Add("P: User braced themselves! Defense increased.");
            return true;
        }
        public static bool CombatBackpack(GameContext context, Entity entity, List<string> log)
        {
            //if did not use item
            if (false)
            {
                return false;
            }
            else
            {
                log.Add("P: User used xx item");
                return true;
            }
        }
        public static bool CombatRunAway(GameContext context, Entity entity, List<string> log)
        {
            log.Add("P:User tried to run away");
            return true;
        }
        #endregion
    }
}
