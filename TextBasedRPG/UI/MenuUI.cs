using TextBasedRPG.Core.Items;
using TextBasedRPG.Managers;
using TextBasedRPG.Models;

namespace TextBasedRPG.UI
{
    public static class MenuUI
    {
        public const int ITEMS_PER_PAGE = 9;
        // will do dynamic text in the future
        public static void MapTitle()
        {
            #region Title
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.Write("║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("                          MAP                         ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.ResetColor();
            #endregion
        }
        public static void ShopTitle()
        {
            #region Title
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.Write("║");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("                          SHOP                        ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.ResetColor();
            #endregion
        }
        public static void ActiveLocationData(GameContext context)
        {
            int index = LocationManager.GetLocationIndex(context);

            Console.WriteLine();
            Console.Write($"Currently in: ");
            ColoredMsg(ConsoleColor.White, text: context.Locations![index].Name);
            Console.WriteLine($"Description: {context.Locations[index].Description}");
            Console.Write($"Monsters Level Cap: ");
            ColoredMsg(ConsoleColor.Red, context.Locations[index].LevelCap.ToString());
            Console.WriteLine();
        }
        public static void SetRarityColor(string rarity)
        {
            switch (rarity.ToLower())
            {
                case "common": Console.ForegroundColor = ConsoleColor.Gray; break;
                case "uncommon": Console.ForegroundColor = ConsoleColor.Green; break;
                case "rare": Console.ForegroundColor = ConsoleColor.Blue; break;
                case "epic": Console.ForegroundColor = ConsoleColor.Magenta; break;
                case "legendary": Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                default: Console.ResetColor(); break;
            }
        }
        public static void ColoredMsg(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void MenuOption(string option, string text, string description)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"[{option}] ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{text,-15} ");
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("- ");
            
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(description);
            
            Console.ResetColor();
        }
        /// <summary> Visualize specified Data with 10 Bars </summary>
        public static string BarFiller(string text, int cur, int max)
        {
            string fill = text + " [";
            var ratio = (double)cur / max;
            for (int i = 0; i < 10; i++)
            {
                fill += (ratio > (i * 0.1)) ? "█" : "░";
            }
            fill += "] ";
            fill += cur.ToString() + "/" + max.ToString();
            return fill;
        }
    }
}
