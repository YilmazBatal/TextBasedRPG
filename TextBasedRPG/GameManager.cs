using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TextBasedRPG.Heroes
{
    public enum GameState
    {
        HeroSelection,
        MainMenu,
        Blacksmith,
        Training,
        Adventure,
        RegionBoss,
        Exit    
    }
    #region Game Menus
    
    public interface IMenuState
    {
        GameState Update(GameContext context);
    }
    public class MainMenuState : IMenuState
    {
        public GameState Update(GameContext context)
        {
            Console.Clear();
            Console.WriteLine("--- MAIN MENU ---");
            Console.WriteLine($"""
            (1) BlackSmith
            (2) Training
            (3) Adventure
            (4) Region Boss
            """);
            
            string input = Console.ReadLine() ?? "";
            return input switch
            {
                "1" => GameState.Blacksmith,
                "2" => GameState.Training,
                "3" => GameState.Adventure,
                "4" => GameState.RegionBoss,
                _ => GameState.MainMenu
            };
        }
    }
    public class HeroSelectionState : IMenuState
    {
        public GameState Update(GameContext context)
        {
            while (context.SelectedHero == null)
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
                    context.SelectedHero = candidate;
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
        private GameState _currentState = GameState.HeroSelection;
        private readonly GameContext _context = new GameContext();
        private readonly Dictionary<GameState, IMenuState> _states;
        public GameManager()
        {
            // Menus
            _states = new Dictionary<GameState, IMenuState> {
                { GameState.HeroSelection, new HeroSelectionState() },
                { GameState.MainMenu, new MainMenuState() },
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
        public Heroes SelectedHero { get; set; }
        public Weapon Weapon { get; set; }
    }
}
