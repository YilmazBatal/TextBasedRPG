using TextBasedRPG.Core.Items;
using TextBasedRPG.Managers;

namespace TextBasedRPG.UI
{
    internal class CharacterUI
    {
        public static void HeroPreview(GameContext context)
        {
            var p = context.Player;
            MenuUI.ColoredMsg(ConsoleColor.DarkGray, "--- Hit F11 for the best experience | ESC to Leave full screen mode ---");
            MenuUI.ColoredMsg(ConsoleColor.Cyan, "════════════════ PLAYER INFO ════════════════");
            Console.WriteLine($" [ AVATAR ]   Class: {p?.ClassName}      ");
            Console.WriteLine($"     O        Location: {LocationManager.locations[p.ActiveLocation]} - {(LocationManager.GetLocationIndex(context) + 1)}");
            Console.WriteLine($"    /|\\       Level: {p?.Level}");
            Console.WriteLine($"    / \\       Gold: {p?.Gold}");
            #region Bars
            Console.WriteLine("──────────────────────────────────────────────────────────");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"{MenuUI.BarFiller("XP", p.CurExp, p.ReqExp)}");
            Console.ResetColor();
            Console.Write($" - ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{MenuUI.BarFiller("HP", p.CurHP, p.TotalHP)}");
            Console.ResetColor();
            Console.WriteLine("──────────────────────────────────────────────────────────");
            #endregion
            EquipmentCheck(context);
            Console.WriteLine();
        }

        // TODO : when you change the Item inheritance come back to here and refactor/optimize the code.
        /// <summary>
        /// Prints out equipped item data
        /// </summary>
        /// <param name="context"></param>
        public static void EquipmentCheck(GameContext context)
        {
            List<(string Name, Item? item)> equipments = new List<(string Name, Item? item)> {
                ("Weapon", context.Player?.EquippedWeapon),
                ("Armor", context.Player?.EquippedArmor),
                ("Necklace", null),
                //("Ring", null)
                }; // this is gonna be dynamic in the future

            foreach (var equipment in equipments)
            {
                if (equipment.item == null)
                {
                    Console.WriteLine($"{equipment.Name} : No {equipment.Name} is equipped");
                }
                else
                {
                    Console.Write($"{equipment.Name} : ");
                    MenuUI.SetRarityColor(equipment.item.Rarity.ToString());
                    Console.WriteLine($"{equipment.item.Name,-20}");
                    Console.ResetColor();
                }
            }
        }
    }
}
