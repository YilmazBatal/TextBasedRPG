namespace TextBasedRPG.Heroes
{
    public interface IMenuState
    {
        GameState Update(GameContext context);
    }
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

            Console.WriteLine($"--- WELCOME, {context.Player.ClassName?.ToUpper()} ---");
            Console.WriteLine("--- MAIN MENU ---");
            Console.WriteLine($"""
            (1) BlackSmith      - Upgrade Equipments
            (2) Training        - Improve Yourself
            (3) Adventure       - Fight Monsters
            (4) Region Boss     - Challange Boss
            (S) Save Game       - Save Progress
            (Q) Quit
            """);
            for (int i = 0; i < context.Player.Inventory?.Count; i++)
            {
                Console.WriteLine(context.Player.Inventory[i]);
            }
            //Console.WriteLine(context.Player.Inventory);
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
                "1" => GameState.Blacksmith,
                "2" => GameState.Training,
                "3" => GameState.Adventure,
                "4" => GameState.RegionBoss,
                "Q" => GameState.Exit,
                _ => GameState.MainMenu
            };
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

    #region Location Menus
    // i'll update the context later
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
