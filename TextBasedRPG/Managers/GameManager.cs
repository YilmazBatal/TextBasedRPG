using TextBasedRPG.Entities;
using TextBasedRPG.Heroes;
using TextBasedRPG.Interfaces;
using TextBasedRPG.States;

namespace TextBasedRPG.Managers
{
    public static class UIHelper
    {
        public static void BackpackPagination(GameContext context)
        {
            var inventory = context.Player?.Inventory;
            if (inventory == null) return;

            int itemsPerPage = 9;
            int pageCount = (int)Math.Ceiling((double)inventory.Count / itemsPerPage);
            int currentPage = 0; 
            bool inMenu = true;

            while (inMenu)
            {
                Console.Clear();
                Console.WriteLine($"================== BACKPACK PAGE {currentPage + 1} / {pageCount} ==================");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($"    {"Item Name",-20} {"Category",-10} {"Rarity",10}");
                Console.WriteLine("---------------------------------------------------------");

                for (int j = 0; j < itemsPerPage; j++)
                {
                    int currentIndex = j + (currentPage * itemsPerPage);
                
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
                        int realIndex = (currentPage * itemsPerPage) + (selection - 1);

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
                Console.Write($"{"Name:",-15} "); SetRarityColor(item.Rarity.ToString());  Console.WriteLine($"{item.Name, 20}"); Console.ResetColor();
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
                }   else if (item is Material material) {
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
        private static void SetRarityColor(string rarity)
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
        public static void HeroPreview(GameContext context)
        {
            var p = context.Player;
            Console.WriteLine("==============================================");
            Console.WriteLine($" [ AVATAR ]   PLAYER STATUS      ");
            Console.WriteLine($"     O        Class: {p?.ClassName}");
            Console.WriteLine($"    /|\\       Level: {p?.Level}");
            Console.WriteLine($"    / \\       Gold: {p?.Gold}");
            EquipmentCheck(context);
            Console.WriteLine("----------------------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"{BarFiller("XP", p.CurExp, p.ReqExp)}");
            Console.ResetColor();
            Console.Write($" - ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{BarFiller("HP",p.CurHP, p.TotalHP)}");
            Console.ResetColor();
            Console.WriteLine("==============================================");
        }
        /// <summary>
        /// Dynamic Bar with Custom Titling
        /// </summary>
        /// <param name="text">Name of the bar</param>
        /// <param name="min">Min </param>
        /// <param name="max"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// TODO : when you change the Item inheritance come back to here and refactor/optimize the code.
        /// Prints out the Equipped item datas
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
                    SetRarityColor(equipment.item.Rarity.ToString());
                    Console.WriteLine($"{equipment.item.Name,-20}");
                    Console.ResetColor();
                }
            }
        }
    }

    public class GameManager
    {
        private GameState _currentState = GameState.HeroSelection; // Initial Menu
        private GameContext _context = new GameContext(); // Data to save
        private readonly Dictionary<GameState, IMenuState> _states; // Game state
        private readonly ISaveService _saveService; // Saving service

        public GameManager(ISaveService saveService)
        {
            // Save Service
            _saveService = saveService;
            _context = _saveService.LoadGame() ?? new GameContext();

            if (_context.Player != null)
            {
                _currentState = GameState.MainMenu;
            }

            // Menus
            _states = new Dictionary<GameState, IMenuState> {
                { GameState.HeroSelection, new HeroSelectionState(_saveService) },
                { GameState.MainMenu, new MainMenuState(_saveService) },
                { GameState.DetailedStats, new DetailedStatsState() },
                { GameState.Inventory, new InventoryState() },
                { GameState.Blacksmith, new BlacksmithState() },
                { GameState.Training, new TrainingState() },
                { GameState.Adventure, new AdventureState() },
                { GameState.Dungeon, new DungeonState() },
            };
        }

        public void Run()
        {
            while (_currentState != GameState.Exit)
            {
                if (_states.ContainsKey(_currentState))
                {
                    _currentState = _states[_currentState].Update(_context);
                }   
                else
                {
                    _currentState = GameState.Exit;
                }
            }
        }
    }

    /// <summary>
    /// Context to hold game data during runtime
    /// </summary>
    public class GameContext
    {
        public Hero? Player { get; set; }
        public bool IsAutoSaveOn { get; set; }
        public List<Entity>? Entities { get; set; }
    }
}
