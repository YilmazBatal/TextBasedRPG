using TextBasedRPG.Entities;
using TextBasedRPG.Heroes;
using TextBasedRPG.Interfaces;
using TextBasedRPG.Items;
using TextBasedRPG.Locations;
using TextBasedRPG.States;

namespace TextBasedRPG.Managers
{
    public class GameManager
    {
        private GameState _currentState = GameState.HeroSelection; // Initial Menu
        private GameContext _context = new GameContext(); // init data
        private readonly Dictionary<GameState, IMenuState> _states; // Game state
        private readonly ISaveService _saveService; // Saving service

        public GameManager(ISaveService saveService)
        {
            // Save Service
            _saveService = saveService; // <-- this initialized from program.cs
            _context = _saveService.LoadGame(); // ?? new GameContext();

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
                { GameState.Map, new MapState() },
                { GameState.Quests, new QuestState() },
                { GameState.Wipe, new WipeState() },
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
        public List<Location>? Locations { get; set; }
        public List<Weapon>? Weapons { get; set; }
        public List<Armor>? Armors { get; set; }
        public List<Material>? Materials { get; set; }
        public List<Consumable>? Consumables { get; set; }
        public Dictionary<string, Item> MasterItemBook { get; private set; } = new();
        public void InitializeMasterBook()
        {
            var all = Weapons!.Cast<Item>()
                .Concat(Armors!)
                .Concat(Materials!)
                .Concat(Consumables!);

            MasterItemBook = all.ToDictionary(item => item.ID, item => item);
        }
    }
}
