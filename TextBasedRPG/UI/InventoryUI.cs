using TextBasedRPG.Core.Items;
using TextBasedRPG.Managers;
using TextBasedRPG.Models;

namespace TextBasedRPG.UI
{
    public static class InventoryUI
    {
        public static void BackpackPagination(GameContext context)
        {
            var inventory = context.Player?.Inventory;
            if (inventory == null || inventory.Count == 0)
            {
                Console.WriteLine("\n[SYSTEM] Your backpack is empty.");
                Console.ReadKey(true);
                return;
            }

            int pageCount = (int)Math.Ceiling((double)inventory.Count / MenuUI.ITEMS_PER_PAGE);
            int currentPage = 0;
            bool inMenu = true;

            while (inMenu)
            {
                Console.Clear();
                MenuUI.ColoredMsg(ConsoleColor.Cyan, $"══════════════════ BACKPACK PAGE {currentPage + 1} / {pageCount} ══════════════════");
                MenuUI.ColoredMsg(ConsoleColor.Cyan, "─────────────────────────────────────────────────────────");
                MenuUI.ColoredMsg(ConsoleColor.White, $"    {"Item Name",-20} {"Category",-15} {"Qty",-5} {"Rarity",10}");
                MenuUI.ColoredMsg(ConsoleColor.Cyan, "───F──────────────────────────────────────────────────────");

                // List Items
                for (int i = 0; i < MenuUI.ITEMS_PER_PAGE; i++)
                {
                    int currentIndex = i + (currentPage * MenuUI.ITEMS_PER_PAGE);

                    if (currentIndex >= inventory.Count) break;

                    var invData = inventory[currentIndex];

                    // Get Whole Item value from the dictionary
                    if (context.MasterItemBook.TryGetValue(invData.ID, out var masterItem))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"[{i + 1}] ");
                        Console.ResetColor();

                        Console.Write($"{masterItem.Name,-20} {masterItem.ItemType.ToString(),-15}");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"x{invData.Quantity,-4}");
                        Console.ResetColor();

                        MenuUI.SetRarityColor(masterItem.Rarity.ToString());
                        Console.WriteLine($"{masterItem.Rarity.ToString(),10}");
                        Console.ResetColor();
                    }
                }

                MenuUI.ColoredMsg(ConsoleColor.Cyan, "─────────────────────────────────────────────────────────");
                Console.WriteLine($"Equipped Weapon : {(context.Player?.EquippedWeapon != null ? context.Player.EquippedWeapon.Name : "None")}");
                Console.WriteLine($"Equipped Armor  : {(context.Player?.EquippedArmor != null ? context.Player.EquippedArmor.Name : "None")}");
                MenuUI.ColoredMsg(ConsoleColor.Cyan, "─────────────────────────────────────────────────────────");
                Console.WriteLine($"[N] Next | [P] Previous | [U] Unequip | [B] Back");
                MenuUI.ColoredMsg(ConsoleColor.Cyan, "═════════════════════════════════════════════════════════");
                Console.Write("Selection > ");

                string input = Console.ReadLine()?.ToUpper() ?? "";

                if (input == "N") { if (currentPage < pageCount - 1) currentPage++; }
                else if (input == "P") { if (currentPage > 0) currentPage--; }
                else if (input == "B") inMenu = false;
                else if (input == "U") HandleUnequip(context);
                else
                {
                    // selection
                    if (int.TryParse(input, out int selection) && selection >= 1 && selection <= MenuUI.ITEMS_PER_PAGE)
                    {
                        int realIndex = (currentPage * MenuUI.ITEMS_PER_PAGE) + (selection - 1);
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
            }
            else if (choice == "2" && context.Player?.EquippedArmor != null)
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
                string requiredType = context.ClassWeaponCheck[context.Player.ClassName!];

                if (requiredType != weapon.WeaponType.ToString())
                {
                    MenuUI.ColoredMsg(ConsoleColor.Red, $"[SYSTEM] {context.Player.ClassName}s cannot use {weapon.WeaponType}s! They need {requiredType}.");
                    Console.ReadKey(true);
                    return;
                }

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
                Console.WriteLine($"════════════════════════════════════════");
                Console.WriteLine($"{"Player Level:",-15} {context.Player?.Level,20}");
                Console.WriteLine($"{"Current ATK:",-15} {(context.Player?.EquippedWeapon != null ? context.Player.EquippedWeapon.WeaponATK : 0),20}");
                Console.WriteLine($"{"Current DEF:",-15} {(context.Player?.EquippedArmor != null ? context.Player.EquippedArmor.ArmorDef : 0),20}");
                Console.WriteLine($"────────────────────────────────────────");

                Console.Write($"{"Name:",-15} "); MenuUI.SetRarityColor(item.Rarity.ToString());
                Console.WriteLine($"{item.Name,20}"); Console.ResetColor();

                Console.WriteLine($"{"Description:",-15} {item.Description,20}");
                Console.WriteLine($"{"Price:",-15} {item.Price,20}");

                bool isEquippable = false;
                bool isConsumable = false;
                bool isLevelEnough = false;

                // Item Tipine Göre Detay Gösterimi
                if (item is Weapon weapon)
                {
                    isEquippable = true;
                    Console.WriteLine($"{"Weapon Type:",-15} {weapon.WeaponType,20}");
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
                else if (item is Consumable consumable)
                {
                    isConsumable = true;
                    Console.WriteLine($"{"Effect:",-15} {consumable.Effect,20}");
                    Console.WriteLine($"{"Value:",-15} {consumable.Value,20}%");
                }
                else
                {
                    Console.WriteLine($"{"Quantity:",-15} {invData.Quantity,20}");
                }

                Console.WriteLine($"────────────────────────────────────────");
                Console.WriteLine($"{(isEquippable ? "[E]quip | " : "")}{(isConsumable ? "[C]onsume | " : "")}[D]iscard {(isAtShop ? "| [S]ell" : "")} | [B]ack");
                Console.WriteLine($"════════════════════════════════════════");
                                    
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
                else if (input == "C" && isConsumable)
                {
                    ConsumeItem();
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
        private static void ConsumeItem()
        {
            Console.WriteLine("[System] You consumed the item! (Effect not implemented)");
            Thread.Sleep(1000);
        }

    }
}