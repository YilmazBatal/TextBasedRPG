using System.Diagnostics;

namespace TextBasedRPG.Heroes
{
    public interface IMenuState
    {
        GameState Update(GameContext context);
    }

    #region Menus
    public class MainMenuState : IMenuState
    {
        private readonly ISaveService _saveService;

        public MainMenuState(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public GameState Update(GameContext context)
        {
            Console.Clear();

            if (context.Player == null)
            {
                return GameState.HeroSelection;
            }

            HeroPreview(context);
            Console.WriteLine("--- MAIN MENU ---");
            Console.WriteLine($"""
            [1] Backpack        - Browse in your inventory
            [2] BlackSmith      - Upgrade Equipments
            [3] Training        - Improve Yourself
            [4] Adventure       - Fight Monsters
            [5] Region Boss     - Challange Boss
            [S] Save Game       - Save Progress
            [Q] Quit
            """);
            string? input = Console.ReadLine()?.ToUpper();

            if (input == "S")
            {
                _saveService.SaveGame(context);
                Console.WriteLine("\nProgress saved to savegame.json!");
                Thread.Sleep(1000);
                return GameState.MainMenu;
            }

            return input switch
            {
                "1" => GameState.Inventory,
                "2" => GameState.Blacksmith,
                "3" => GameState.Training,
                "4" => GameState.Adventure,
                "5" => GameState.RegionBoss,
                "Q" => GameState.Exit,
                _ => GameState.MainMenu
            };
        }

        void HeroPreview(GameContext context)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("   [ AVATAR ]          PLAYER STATUS      ");
            Console.WriteLine("      O                Class: " + context.Player?.ClassName);
            Console.WriteLine("     /|\\               Level: " + context.Player?.Level);
            Console.WriteLine("     / \\               Gold : " + context.Player?.Gold);
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("HP [███████___] 18/22" + " - " + "EXP [█████_____] 25/52");
            //string hpBar = new string('█', (int)(context.Player?.CurrentHP / 10));
            //Console.WriteLine($" HP: [{hpBar,-10}] {context.Player.CurrentHP}/{context.Player?.MaxHP}");
            Console.WriteLine("==============================================");
        }
    }
    public class HeroSelectionState : IMenuState
    {
        private readonly ISaveService _saveService;

        public HeroSelectionState(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public GameState Update(GameContext context)
        {
            while (context.Player == null)
            {
                Console.Clear();
                Console.WriteLine("Please choose a hero to overview Eg. 1");
                Console.WriteLine($"""
                 ---------------------
                    [1]. Warrior 
                    [2]. Archer
                    More Heroes soon...
                 ---------------------
                 """);

                string? input = Console.ReadLine();

                Heroes? candidate = input switch
                {
                    "1" => new Warrior(),
                    "2" => new Archer(),
                    _ => null
                };
                
                if (candidate != null && ConfirmSelection(candidate))
                {
                    context.Player = candidate;
                    _saveService.SaveGame(context);
                    return GameState.MainMenu;
                }
            }
            return GameState.HeroSelection;
        }
        
        private bool ConfirmSelection(Heroes candidate)
        {
            candidate.GetStatsSummary();
            Console.WriteLine($"Confirm {candidate.ClassName}? [Y/N]");
            return Console.ReadLine()?.ToUpper() == "Y";
        }
    }

    // i'll update the context later
    public class InventoryState : IMenuState
    {
        public GameState Update(GameContext context)
        {
            Console.Clear();
            UIHelper.BackpackPagination(context);
            return GameState.MainMenu;
        }
        
    }
    public class BlacksmithState : IMenuState
    {
        public GameState Update(GameContext context)
        {
            Console.Clear();
            Console.WriteLine("You are at the Blacksmith... Press any key to continue.");
            
            Console.ReadKey();
            return GameState.MainMenu;
        }
    }
    public class TrainingState : IMenuState
    {
        public GameState Update(GameContext context)
        {
            Console.Clear();
            Console.WriteLine("You are at the Training... Press any key to continue.");
            Console.ReadKey();
            return GameState.MainMenu;
        }
    }
    public class AdventureState : IMenuState
    {
        public GameState Update(GameContext context)
        {
            Console.Clear();
            Console.WriteLine("You are at the Adventure... Press any key to continue.");
            Console.ReadKey();
            return GameState.MainMenu;
        }
    }
    public class RegionBossState : IMenuState
    {
        public GameState Update(GameContext context)
        {
            Console.Clear();
            Console.WriteLine("You are at the Region Boss... Press any key to continue.");
            Console.ReadKey();
            return GameState.MainMenu;
        }
    }
    #endregion
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
                Console.WriteLine("[P]revious | [N]ext | [B]ack");
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
                else if (input == "B") inMenu = false;
                else
                {
                    if (int.TryParse(input, out int selection) && selection >= 1 && selection <= 9)
                    {
                        int realIndex = (currentPage * itemsPerPage) + (selection - 1);

                        if (realIndex < inventory.Count)
                        {
                            var selectedItem = inventory[realIndex];

                            ShowItemDetails(inventory, selectedItem, false);
                        }
                    }
                }
            }
        }
        private static void ShowItemDetails(List<Item> inventory, Item item, bool isAtShop)
        {
            bool inMenu = true;
            while (inMenu)
            {
                Console.Clear();
                Console.WriteLine($"========================================");
                Console.Write($"{"Name:",-15} "); SetRarityColor(item.Rarity.ToString());  Console.WriteLine($"{item.Name, 20}"); Console.ResetColor();
                Console.WriteLine($"{"Description:",-15} {item.Description,20}");
                bool isEquippable = false;
                if (item is Weapon || item is Armor)
                {
                    isEquippable = true;
                    if (item is Weapon weapon)
                    {
                        Console.WriteLine($"{"Type:",-15} {weapon.WeaponType,20}");
                        Console.WriteLine($"{"Attack:",-15} {weapon.WeaponATK,20}");
                        Console.WriteLine($"{"Req. Level:",-15} {weapon.RequiredLevel,20}");
                    }
                    else if (item is Armor armor)
                    {
                        Console.WriteLine($"{"Defense:",-15} {armor.ArmorDef,20}");
                        Console.WriteLine($"{"Req. Level:",-15} {armor.RequiredLevel,20}");
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
                    // Equip then discard from inventory
                    // if already has smth equipped then replace it
                    // if user level is not enough to equip give error.
                    Console.WriteLine("Equipped");
                }
                else if (input == "S" && isAtShop)
                {
                    // Add Gold to the accound then discard it
                    // if it's a material and there is more than 1, ask for quantity.
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
    }
    /// <summary>
    /// Manages the overall game flow, including state transitions, context management, and integration with the save
    /// system.
    /// </summary>
    /// <remarks>The GameManager coordinates the main game loop and handles switching between different game
    /// states, such as menus and gameplay areas. It loads and saves game progress using the provided save service. This
    /// class is typically instantiated once at application startup and remains active for the duration of the game
    /// session.</remarks>
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
                { GameState.Inventory, new InventoryState() },
                { GameState.Blacksmith, new BlacksmithState() },
                { GameState.Training, new TrainingState() },
                { GameState.Adventure, new AdventureState() },
                { GameState.RegionBoss, new RegionBossState() },
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
        public Heroes? Player { get; set; }
    }
}
