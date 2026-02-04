using TextBasedRPG.Entities;
using TextBasedRPG.Interfaces;

namespace TextBasedRPG.Managers
{
    internal class CombatManager
    {
        public static void StartCombat(GameContext context)
        {
            Combat(context);
        }
        
        public static void Combat(GameContext context)
        {
            Entity enemy = GenerateEnemy(context);
            List<string> log = new List<string>();
            bool isPlayerTurn = context.Player?.TotalSPD >= enemy.CurrentSPD;
            bool isPlayerStarted = isPlayerTurn;
            bool isCombatActive = true;
            bool didUserFlee = false;

            while (isCombatActive)
            {
                Console.Clear();
                string enemyStatus = GetEnemyStatus(enemy);
                CombatHeader(context, enemy);

                ShowLogs(log);

                if (isPlayerTurn)
                    Acting(context, enemy, log);
                else
                    EnemyAct(context, enemy, log);

                if (context.Player?.CurHP <= 0)
                {
                    Console.WriteLine("Player is dead");
                    isCombatActive = false;
                }
                else if (enemy.CurHP <= 0)
                {
                    Console.WriteLine("Enemy is dead");
                    isCombatActive = false;
                }
                
                isPlayerTurn = !isPlayerTurn;
            }

            Console.WriteLine("Combat has ended.");
        }

        private static void ShowLogs(List<string> log)
        {
            Console.WriteLine("|======================== LOGS ========================\n");
            var lastFive = log.Skip(Math.Max(0, log.Count - 5)).ToList();

            foreach (var item in lastFive)
            {
                if (item.StartsWith("P"))
                {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"|   {item.Substring(2)}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"|   {item.Substring(2)}");
                }
                Console.ResetColor();
            }
            for (int i = 0; i < (5 - lastFive.Count); i++)
            {
                Console.WriteLine("|");
            }
        }

        /// <returns>Enemy's condition based on HP percentage</returns>
        public static string GetEnemyStatus(Entity enemy)
        {
            if (enemy.CurHP >= enemy.TotalHP*75/100)
                return $"{enemy.Name} looks ready to fight!";
            else if (enemy.CurHP >= enemy.TotalHP * 50/100)
                return $"{enemy.Name} is watching you carefuly";
            else if (enemy.CurHP >= enemy.TotalHP * 20 / 100)
                return $"{enemy.Name} looks slightly tired to fight...";
            else
                return $"{enemy.Name} is panicking!";
        }
        #region Enemy Acts
        public static void EnemyAct(GameContext context, Entity entity, List<string> log)
        {
            Console.WriteLine($"""
                
                \ =-=-=-=-=-=-=-=-=- {entity.Name.ToUpper()} TURN =-=-=-=-=-=-=-=-=- /
                
                {entity.Name} Enemy Attacked!

                Press Any Key...
                """);
            Console.ReadKey();
            RefreshUI(context, entity, log);
            log.Add($"E:{entity.Name} dealt xx dmg");
        }
        #endregion
        #region Player Acts
        public static void Acting(GameContext context, Entity entity, List<string> log)
        {
            bool acted = false;
            while (!acted)
            {
                Console.WriteLine($"""
                 
                 \ =-=-=-=-=-=-=-=-=- YOUR TURN =-=-=-=-=-=-=-=-=- /
                 
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
            }
        }
        public static bool CombatAttack(GameContext context, Entity entity, List<string> log)
        {
            var p = context.Player;
            IDamageCalculator damageCalc = new DamageCalculator();
            int damage = damageCalc.CalculateDMG(p.TotalATK, entity.TotalDEF, p.CritRate, p.CritDamage);
            entity.TakeDamage(damage);
            log.Add($"P:User dealt {damage.ToString()} damage");
            RefreshUI(context, entity, log);
            return true;
        }
        public static bool CombatFocus(GameContext context, Entity entity, List<string> log)
        {
            log.Add("P: User focesed on the enemy");
            RefreshUI(context, entity, log);
            return true;
        }
        public static bool CombatGuardUp(GameContext context, Entity entity, List<string> log)
        {
            log.Add("P: User increased his defense");
            RefreshUI(context, entity, log);
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
                RefreshUI(context, entity, log);
                return true;
            }
        }
        public static bool CombatRunAway(GameContext context, Entity entity, List<string> log)
        {
            log.Add("P:User tried to run away");
            RefreshUI(context, entity, log);
            return true;
        }
        #endregion
        public static void RefreshUI(GameContext context, Entity enemy, List<string> log)
        {
            Console.Clear();
            CombatHeader(context, enemy);
            ShowLogs(log);
        }
        public static void CombatHeader(GameContext context, Entity enemy)
        {
            var p = context.Player;
            Console.WriteLine($"""
            |===== YOUR CARD =====|         |==== ENEMY CARD ====|
            |                     |   VS    |                    |
            |   O   ATK: {p?.TotalATK,-9}|         |  O   ATK: {enemy.TotalATK,-9}|
            |  /|\  DEF: {p?.TotalDEF,-9}|         | /|\  DEF: {enemy.TotalDEF,-9}|
            |  / \  LVL: {p?.Level,-9}|         | / \  LVL: {enemy.GeneratedLevel,-9}|   
            -----------------------         -----------------------
            """);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"|{UIHelper.BarFiller("HP", p.CurHP, p.TotalHP)}");
            Console.WriteLine($"        {UIHelper.BarFiller("HP", enemy.CurHP, enemy.TotalHP)}");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"|{UIHelper.BarFiller("XP", p.CurExp, p.ReqExp)}");
            Console.ResetColor();
        } 
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

            newEntity.Level = newEntity.GeneratedLevel;

            newEntity.Initialize();

            return newEntity;
        }
    }
}
