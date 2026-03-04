using TextBasedRPG.Locations;
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
        public static void BackpackPagination(GameContext context)
        {
            var inventory = context.Player?.Inventory;
            if (inventory == null || inventory.Count == 0)
            {
                Console.WriteLine("\n[SYSTEM] Your backpack is empty.");
                Console.ReadKey(true);
                return;
            }

            int pageCount = (int)Math.Ceiling((double)inventory.Count / ITEMS_PER_PAGE);
            int currentPage = 0;
            bool inMenu = true;

            while (inMenu)
            {
                Console.Clear();
                Console.WriteLine($"================== BACKPACK PAGE {currentPage + 1} / {pageCount} ==================");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($"    {"Item Name",-20} {"Category",-15} {"Qty",-5} {"Rarity",10}");
                Console.WriteLine("---------------------------------------------------------");

                // List Items
                for (int i = 0; i < ITEMS_PER_PAGE; i++)
                {
                    int currentIndex = i + (currentPage * ITEMS_PER_PAGE);

                    if (currentIndex >= inventory.Count) break;

                    var invData = inventory[currentIndex];

                    // Get Whole Item value from the dictionary
                    if (context.MasterItemBook.TryGetValue(invData.ID, out var masterItem))
                    {
                        Console.Write($"[{i + 1}] {masterItem.Name,-20} {masterItem.ItemType.ToString(),-15} x{invData.Quantity,-4} ");
                        SetRarityColor(masterItem.Rarity.ToString());
                        Console.WriteLine($"{masterItem.Rarity.ToString(),10}");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($"Equipped Weapon : {(context.Player?.EquippedWeapon != null ? context.Player.EquippedWeapon.Name : "None")}");
                Console.WriteLine($"Equipped Armor  : {(context.Player?.EquippedArmor != null ? context.Player.EquippedArmor.Name : "None")}");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($"[N] Next | [P] Previous | [U] Unequip | [B] Back");
                Console.WriteLine("==========================================================");
                Console.Write("Selection > ");

                string input = Console.ReadLine()?.ToUpper() ?? "";

                if (input == "N") { if (currentPage < pageCount - 1) currentPage++; }
                else if (input == "P") { if (currentPage > 0) currentPage--; }
                else if (input == "B") inMenu = false;
                else if (input == "U") HandleUnequip(context);
                else
                {
                    // selection
                    if (int.TryParse(input, out int selection) && selection >= 1 && selection <= ITEMS_PER_PAGE)
                    {
                        int realIndex = (currentPage * ITEMS_PER_PAGE) + (selection - 1);
                        if (realIndex < inventory.Count)
                        {
                            ShowItemDetails(inventory, inventory[realIndex], isAtShop: false, context);
                        }
                    }
                }
            }
        }

        private static void HandleUnequip(GameContext context)
        {
            Console.WriteLine("\n[1] Unequip Weapon  [2] Unequip Armor");
            string choice = Console.ReadLine() ?? "";

            if (choice == "1" && context.Player?.EquippedWeapon != null)
            {
                InventoryData equippedWeapon = new InventoryData();
                equippedWeapon.ID = context.Player.EquippedWeapon.ID;
                equippedWeapon.Quantity = 1;

                context.Player.Inventory!.Add(equippedWeapon);
                Console.WriteLine($"[SYSTEM] {context.Player.EquippedWeapon.Name} unequipped.");
                context.Player.EquippedWeapon = null;
            } else if (choice == "2" && context.Player?.EquippedArmor != null)
            {
                InventoryData equippedArmor = new InventoryData();
                equippedArmor.ID = context.Player.EquippedArmor.ID;
                equippedArmor.Quantity = 1;

                context.Player.Inventory!.Add(equippedArmor);
                Console.WriteLine($"[SYSTEM] {context.Player.EquippedArmor.Name} unequipped.");
                context.Player.EquippedArmor = null;
            }
            Thread.Sleep(1000);
        }

        private static void HandleEquipment(GameContext context, Item itemToEquip, InventoryData invData)
        {
            if (itemToEquip is Weapon weapon)
            {
                if (context.Player.EquippedWeapon != null)
                {
                    InventoryData equippedWeapon = new InventoryData();
                    equippedWeapon.ID = context.Player.EquippedWeapon.ID;
                    equippedWeapon.Quantity = 1;

                    context.Player.Inventory.Add(equippedWeapon);
                }
                context.Player.EquippedWeapon = weapon;
            }
            else if (itemToEquip is Armor armor)
            {
                if (context.Player.EquippedArmor != null)
                {
                    InventoryData equippedArmor = new InventoryData();
                    equippedArmor.ID = context.Player.EquippedArmor.ID;
                    equippedArmor.Quantity = 1;

                    context.Player.Inventory.Add(equippedArmor);
                }
                context.Player.EquippedArmor = armor;
            }

            context.Player.Inventory.Remove(invData);
            Console.WriteLine($"[System] Equipped {itemToEquip.Name}!");
            Thread.Sleep(1000);
        }

        private static void ShowItemDetails(List<InventoryData> inventory, InventoryData invData, bool isAtShop, GameContext context)
        {
            if (!context.MasterItemBook.TryGetValue(invData.ID, out var item)) return;

            bool inMenu = true;
            while (inMenu)
            {
                Console.Clear();
                Console.WriteLine($"========================================");
                Console.WriteLine($"{"Player Level:",-15} {context.Player?.Level,20}");
                Console.WriteLine($"{"Current ATK:",-15} {(context.Player?.EquippedWeapon != null ? context.Player.EquippedWeapon.WeaponATK : 0),20}");
                Console.WriteLine($"{"Current DEF:",-15} {(context.Player?.EquippedArmor != null ? context.Player.EquippedArmor.ArmorDef : 0),20}");
                Console.WriteLine($"----------------------------------------");

                Console.Write($"{"Name:",-15} "); SetRarityColor(item.Rarity.ToString());
                Console.WriteLine($"{item.Name,20}"); Console.ResetColor();

                Console.WriteLine($"{"Description:",-15} {item.Description,20}");
                Console.WriteLine($"{"Price:",-15} {item.Price,20}");

                bool isEquippable = false;
                bool isLevelEnough = false;

                // Item Tipine Göre Detay Gösterimi
                if (item is Weapon weapon)
                {
                    isEquippable = true;
                    Console.WriteLine($"{"Attack:",-15} {weapon.WeaponATK,20}");
                    Console.WriteLine($"{"Req. Level:",-15} {weapon.RequiredLevel,20}");
                    isLevelEnough = context.Player?.Level >= weapon.RequiredLevel;
                }
                else if (item is Armor armor)
                {
                    isEquippable = true;
                    Console.WriteLine($"{"Defense:",-15} {armor.ArmorDef,20}");
                    Console.WriteLine($"{"Req. Level:",-15} {armor.RequiredLevel,20}");
                    isLevelEnough = context.Player?.Level >= armor.RequiredLevel;
                }
                else
                {
                    Console.WriteLine($"{"Quantity:",-15} {invData.Quantity,20}");
                }

                Console.WriteLine($"----------------------------------------");
                Console.WriteLine($"{(isEquippable ? "[E]quip | " : "")}[D]iscard {(isAtShop ? "| [S]ell" : "")} | [B]ack");
                Console.WriteLine($"========================================");
                Console.Write("Selection: ");

                string input = Console.ReadLine()?.ToUpper() ?? "";

                if (input == "B") break;

                if (input == "D")
                {
                    InventoryManager.DiscardFromInventory(context, invData);
                }
                else if (input == "E" && isEquippable)
                {
                    if (isLevelEnough)
                    {
                        HandleEquipment(context, item, invData);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("[System] Player level is not enough!");
                        Thread.Sleep(1000);
                    }
                }
                else if (input == "S" && isAtShop) 
                {
                    int sellPrice = (int)(item.Price * 0.35);
                    Console.Write($"Sell for {sellPrice} gold? [Y/N]: ");
                    if (Console.ReadLine()?.ToUpper() == "Y")
                    {
                        context.Player.Gold += sellPrice;
                        inventory.Remove(invData);
                        Console.WriteLine($"[System] Sold for {sellPrice}!");
                        Thread.Sleep(1000);
                        return;
                    }
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
