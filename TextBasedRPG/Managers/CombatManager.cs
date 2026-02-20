using TextBasedRPG.Entities;
using TextBasedRPG.Events;
using TextBasedRPG.Heroes;
using TextBasedRPG.UI;

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
            Entity enemy = EnemyGenerator.GenerateEnemy(context); // Generate the enemy
            List<string> log = new List<string>(); // Create the logs for the PvE

            // Who is faster?
            bool isPlayerTurn = context.Player?.TotalSPD >= enemy.CurrentSPD;
            bool isPlayerStarted = isPlayerTurn;
            bool isCombatActive = true;
            
            while (isCombatActive)
            {
                CombatUI.RefreshUI(context, enemy, log);

                // 1. Action Phase
                if (isPlayerTurn)
                    CombatLogic.Acting(context, enemy, log);
                else
                    CombatLogic.EnemyAct(context, enemy, log);

                // 2. Did anyone died after an action?
                if (context.Player?.CurHP <= 0 || enemy.CurHP <= 0)
                {
                    isCombatActive = false;
                    if (context.Player?.CurHP <= 0) Console.WriteLine("Player is dead");
                    else Console.WriteLine("Enemy is dead");
                    break;
                }

                // 3. The round has ended
                if (isPlayerTurn != isPlayerStarted)
                {
                    EventManager.Combat.TriggerOnRoundEnded();
                }

                // 4. Switch Turns. if i add paralyze or stun , i can check here if the player or enemy is paralyzed or stunned and skip their turn
                isPlayerTurn = !isPlayerTurn;
            }
        }
        public static void OnHit(int damage, bool isCrit)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string critText = isCrit ? "Critical hit!" : "";
            Console.WriteLine($"\nDealt {damage} damage! {critText} ");
            Console.ResetColor();
        }
    }
}
