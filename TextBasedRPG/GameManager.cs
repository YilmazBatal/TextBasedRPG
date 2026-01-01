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
    public class GameManager
    {
        
        // Game State - Default
        private GameState currentState = GameState.HeroSelection;
        private Heroes selectedHero;

        // Flags
        bool isHeroSelected = false;

        public void Run()
        {
            while (currentState != GameState.Exit)
            {
                Console.Clear();
                switch (currentState)
                {
                    case GameState.HeroSelection:
                        HandleHeroSelection();
                        break;
                    case GameState.MainMenu:
                        HandleMainMenu();
                        break;
                    case GameState.Blacksmith:
                        HandleBlacksmith();
                        break;
                    default:
                        currentState = GameState.Exit;
                        break;
                }
            }
        }

        public void HandleHeroSelection()
        {
            while (selectedHero == null)
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

                string ?input = Console.ReadLine();

                Heroes? candidate = input switch
                {
                    "1" => new Warrior(),
                    "2" => new Archer(),
                    _ => null
                };

                if (candidate != null && ConfirmSelection(candidate))
                {
                    selectedHero = candidate;
                    currentState = GameState.MainMenu; // State Transition
                }
            }
            return;
        }
        private bool ConfirmSelection(Heroes candidate)
        {
            candidate.GetStatsSummary();
            Console.WriteLine($"Confirm {candidate.ClassName}? (Y/N)");
            return Console.ReadLine()?.ToUpper() == "Y";
        }

        public void HandleMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Where do you want to go?");
            Console.WriteLine($"""
             ---------------------
                (1). BlackSmith 
                (2). Training 
                (3). Adventure
                (4). Region Boss
             ---------------------
             """);
            string ?input = Console.ReadLine();
            currentState = input switch
            {
                "1" => GameState.Blacksmith,
                "2" => GameState.Training,
                "3" => GameState.Adventure,
                "4" => GameState.RegionBoss,
                _ => currentState
            };
            if (currentState == GameState.MainMenu)
            {
                Console.WriteLine("Invalid Selection, please try again.\nPress any key to continue...");
                Console.ReadKey();
            }
            return;
        }
        private void HandleBlacksmith()
        {
            Console.WriteLine("Welcome to the Forge! Press any key to go back.");
            Console.ReadKey();
            currentState = GameState.MainMenu; // Transition back
        }
    }

}
