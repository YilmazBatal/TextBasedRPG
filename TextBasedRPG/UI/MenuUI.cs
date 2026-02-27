using TextBasedRPG.Locations;
using TextBasedRPG.Managers;

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
        public static void ActiveLocationData(GameContext context)
        {
            int index = LocationManager.GetLocationIndex(context);

            Console.WriteLine();
            Console.Write($"Currently in: ");
            ColoredMsg(ConsoleColor.White, text: context.Locations[index].Name);
            Console.WriteLine($"Description: {context.Locations[index].Description}");
            Console.Write($"Monsters Level Cap: ");
            ColoredMsg(ConsoleColor.Red, context.Locations[index].LevelCap.ToString());
            Console.WriteLine();
        }
        public static void BackpackPagination(GameContext context)
        {
            var inventory = context.Player?.Inventory;
            if (inventory == null) return;

            int pageCount = (int)Math.Ceiling((double)inventory.Count / ITEMS_PER_PAGE);
            int currentPage = 0;
            bool inMenu = true;

            while (inMenu)
            {
                Console.Clear();
                Console.WriteLine($"================== BACKPACK PAGE {currentPage + 1} / {pageCount} ==================");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($"    {"Item Name",-20} {"Category",-10} {"Rarity",10}");
                Console.WriteLine("---------------------------------------------------------");

                for (int j = 0; j < ITEMS_PER_PAGE; j++)
                {
                    int currentIndex = j + (currentPage * ITEMS_PER_PAGE);

                    if (currentIndex >= inventory.Count)
                        break;

                    var item = inventory[currentIndex];
                    Console.Write($"[{j + 1}] {item.Name,-20} {item.Type.ToString(),-10} "); SetRarityColor(item.Rarity.ToString()); Console.WriteLine($"{item.Rarity.ToString(),10}"); Console.ResetColor();
                }
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($"Player Level: {(context.Player?.Level)}");
                Console.WriteLine($"Equipped Weapon : {(context.Player?.EquippedWeapon != null ? context.Player.EquippedWeapon.Name : "No Equipped Weapon")}");
                Console.WriteLine($"Equipped Armor : {(context.Player?.EquippedArmor != null ? context.Player.EquippedArmor.Name : "No Equipped Weapon")}");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($"[P]revious | [N]ext | [B]ack | [U]nequip");
                Console.WriteLine("==========================================================");
                Console.Write("Selection: ");

                string input = Console.ReadLine()?.ToUpper() ?? "";

                if (input == "N")
                {
                    if (currentPage < pageCount - 1) currentPage++;
                }
                else if (input == "P")
                {
                    if (currentPage > 0) currentPage--;
                }
                else if (input == "U")
                {
                    Console.WriteLine($"""
                        [1] Unequip : {(context.Player?.EquippedWeapon != null ? context.Player.EquippedWeapon.Name : "No Equipped Weapon")}
                        [2] Unequip :  {(context.Player?.EquippedArmor != null ? context.Player.EquippedArmor.Name : "No Equipped Armor")}
                        """);
                    Console.Write("Selection: ");
                    string selection = Console.ReadLine() ?? "";
                    if (selection == "1" && context.Player?.EquippedWeapon != null)
                    {
                        context.Player?.Inventory?.Add(context.Player.EquippedWeapon);
                        Console.WriteLine("[System] Unequipped the " + context.Player?.EquippedWeapon);
                        context.Player?.EquippedWeapon = null;
                    }
                    else if (selection == "2" && context.Player?.EquippedArmor != null)
                    {
                        context.Player?.Inventory?.Add(context.Player.EquippedArmor);
                        Console.WriteLine("[System] Unequipped the " + context.Player?.EquippedArmor);
                        context.Player?.EquippedArmor = null;
                    }
                    else
                    {
                        Console.WriteLine("[System] Nothing to unequip!");
                        Thread.Sleep(1000);
                    }
                }
                else if (input == "B") inMenu = false;
                else
                {
                    if (int.TryParse(input, out int selection) && selection >= 1 && selection <= 9)
                    {
                        int realIndex = (currentPage * ITEMS_PER_PAGE) + (selection - 1);

                        if (realIndex < inventory.Count)
                        {
                            var selectedItem = inventory[realIndex];

                            ShowItemDetails(inventory, selectedItem, isAtShop: false, context);
                        }
                    }
                }
            }
        }
        private static void ShowItemDetails(List<Item> inventory, Item item, bool isAtShop, GameContext context)
        {
            bool inMenu = true;
            while (inMenu)
            {
                Console.Clear();
                Console.WriteLine($"========================================");
                Console.WriteLine($"{"Player Level:",-15} {context.Player?.Level,20}");
                Console.WriteLine($"{"Current Weapon:",-15} {(context.Player?.EquippedWeapon != null ? context.Player.EquippedWeapon.WeaponATK : 0),20}");
                Console.WriteLine($"{"Current Armor:",-15} {(context.Player?.EquippedArmor != null ? context.Player.EquippedArmor.ArmorDef : 0),20}");
                Console.WriteLine($"----------------------------------------");
                Console.Write($"{"Name:",-15} "); SetRarityColor(item.Rarity.ToString()); Console.WriteLine($"{item.Name,20}"); Console.ResetColor();
                Console.WriteLine($"{"Description:",-15} {item.Description,20}");
                bool isEquippable = false;
                bool isLevelEnough = false;
                if (item is Weapon || item is Armor)
                {
                    isEquippable = true;
                    if (item is Weapon weapon)
                    {
                        Console.WriteLine($"{"Type:",-15} {weapon.WeaponType,20}");
                        Console.WriteLine($"{"Attack:",-15} {weapon.WeaponATK,20}");
                        Console.WriteLine($"{"Req. Level:",-15} {weapon.RequiredLevel,20}");
                        isLevelEnough = context.Player?.Level >= weapon.RequiredLevel;
                    }
                    else if (item is Armor armor)
                    {
                        Console.WriteLine($"{"Defense:",-15} {armor.ArmorDef,20}");
                        Console.WriteLine($"{"Req. Level:",-15} {armor.RequiredLevel,20}");
                        isLevelEnough = context.Player?.Level >= armor.RequiredLevel;
                    }
                }
                else if (item is Material material)
                {
                    Console.WriteLine($"{"Quantity:",-15} {material.Quantity,20}");
                }

                Console.WriteLine($"----------------------------------------");
                Console.WriteLine($"{(isEquippable ? "[E]quip | " : "")}[D]iscard {(isAtShop ? "| [S]ell" : "")} | [B]ack");
                Console.WriteLine($"========================================");
                Console.Write("Selection: ");

                string input = Console.ReadLine()?.ToUpper() ?? "";

                if (input == "D")
                {
                    Console.WriteLine("Are you sure? [Y/N]");
                    Console.Write("Selection: ");
                    string confirmation = Console.ReadLine()?.ToUpper() ?? "N";
                    if (confirmation == "Y") inventory.Remove(item);
                    return;
                }
                else if (input == "E" && isEquippable)
                {
                    // TODO: If I add more items like rings or necklaces later, 
                    // I should create an "Equipment" class under "Item". 
                    // This way, I can move RequiredLevel there and clean these 'if' blocks.
                    if (isLevelEnough)
                    {
                        if (item is Weapon w && w.RequiredLevel > context.Player?.Level || item is Armor a && a.RequiredLevel > context.Player?.Level)
                        {
                            Console.WriteLine("[System] Player level is not enough!");
                            Thread.Sleep(1000);
                            continue;
                        }

                        if (item is Weapon weapon)
                        {
                            if (context.Player?.EquippedWeapon != null)
                            {
                                context.Player?.Inventory?.Add(context.Player.EquippedWeapon);
                                context.Player?.EquippedWeapon = weapon;
                                context.Player?.Inventory?.Remove(weapon);
                                Console.WriteLine("[System] Equipped the " + context.Player?.EquippedWeapon.Name);
                            }
                            else if (context.Player?.EquippedWeapon == null)
                            {
                                context.Player?.EquippedWeapon = weapon;
                                context.Player?.Inventory?.Remove(weapon);
                                Console.WriteLine("[System] Equipped the " + context.Player?.EquippedWeapon?.Name);

                            }
                        }
                        else if (item is Armor armor)
                        {
                            if (context.Player?.EquippedArmor != null)
                            {
                                context.Player?.Inventory?.Add(context.Player.EquippedArmor);
                                context.Player?.EquippedArmor = armor;
                                context.Player?.Inventory?.Remove(armor);
                                Console.WriteLine("[System] Equipped the " + context.Player?.EquippedArmor.Name);
                            }
                            else if (context.Player?.EquippedArmor == null)
                            {
                                context.Player?.EquippedArmor = armor;
                                context.Player?.Inventory?.Remove(armor);
                                Console.WriteLine("[System] Equipped the " + context.Player?.EquippedArmor?.Name);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("[System] Player level is not enough!");
                    }
                    Thread.Sleep(1000);
                    return;
                }
                else if (input == "S" && isAtShop)
                {
                    Console.WriteLine("Are you sure you want to sell the " + item.Name + " for " + ((double)item.Price * 35 / 100).ToString() + "? [Y]/[N]");
                    Console.Write("Selection: ");
                    string confirmation = Console.ReadLine()?.ToUpper() ?? "";

                    if (confirmation == "Y")
                    {
                        inventory.Remove(item);
                        context.Player?.Gold += item.Price;
                        Console.WriteLine("You sold the " + item.Name + " for " + ((double)item.Price * 35 / 100).ToString() + "!");
                        Thread.Sleep(1000);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (input == "B")
                {
                    inMenu = false;
                }
            }
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
