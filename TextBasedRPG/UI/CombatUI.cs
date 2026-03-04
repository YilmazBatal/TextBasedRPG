using TextBasedRPG.Core.Entities;
using TextBasedRPG.Managers;

namespace TextBasedRPG.UI
{
    internal static class CombatUI
    {
        public static void CombatHeader(GameContext context, Entity enemy)
        {
            var p = context.Player;
            Console.WriteLine($"""
            |===== YOUR CARD =====|         |==== ENEMY CARD ====|
            |                     |   VS    |                    |
            |   O   ATK: {p?.TotalATK,-9}|         |  O   ATK: {enemy.TotalATK,-9}|
            |  /|\  DEF: {p?.TotalDEF,-9}|         | /|\  DEF: {enemy.TotalDEF,-9}|
            |  / \  LVL: {p?.Level,-9}|         | / \  LVL: {enemy.Level,-9}|   
            -----------------------         -----------------------
            -----------------------  BONUS  -----------------------
            | Attack Bonus: {p?.BonusATK,-9} 
            | Defense Bonus: {p?.BonusDef,-9}
            -----------------------         -----------------------
            """);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"|{MenuUI.BarFiller("HP", p.CurHP, p.TotalHP)}");
            Console.WriteLine($"        {MenuUI.BarFiller("HP", enemy.CurHP, enemy.TotalHP)}");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"|{MenuUI.BarFiller("XP", p.CurExp, p.ReqExp)}");
            Console.ResetColor();
        }
        public static void ShowLogs(List<string> log)
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
        public static void RefreshUI(GameContext context, Entity enemy, List<string> log)
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J"); // 3 means everything , J means clear, this is to clear the scrollback buffer
            CombatHeader(context, enemy);
            ShowLogs(log);
        }
    }
}
