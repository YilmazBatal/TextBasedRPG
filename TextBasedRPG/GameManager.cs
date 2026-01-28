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
            (1) Backpack        - Browse in your inventory
            (2) BlackSmith      - Upgrade Equipments
            (3) Training        - Improve Yourself
            (4) Adventure       - Fight Monsters
            (5) Region Boss     - Challange Boss
            (S) Save Game       - Save Progress
            (Q) Quit
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
                    (1). Warrior 
                    (2). Archer
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
            Console.WriteLine($"Confirm {candidate.ClassName}? (Y/N)");
            return Console.ReadLine()?.ToUpper() == "Y";
        }
    }

    // i'll update the context later
    public class InventoryState : IMenuState
    {
        public GameState Update(GameContext context)
        {
            Console.Clear();
            Console.WriteLine("Inventory pagination in the future.");
            Console.ReadKey();
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

    public class GameContext
    {
        public Heroes? Player { get; set; }
    }
}
