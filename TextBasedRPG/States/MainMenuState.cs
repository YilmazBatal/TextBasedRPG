using TextBasedRPG.Interfaces;
using TextBasedRPG.Managers;

namespace TextBasedRPG.States
{
    public class MainMenuState : IMenuState
    {
        private readonly ISaveService _saveService;

        public MainMenuState(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public GameState Update(GameContext context)
        {
            if (context.IsAutoSaveOn) _saveService.SaveGame(context);

            Console.Clear();

            if (context.Player == null)
            {
                return GameState.HeroSelection;
            }


            UIHelper.HeroPreview(context);
            Console.WriteLine("--- MAIN MENU ---");
            Console.WriteLine($"""
            [0] Detailed Stats  - Detailed Player Information
            [1] Backpack        - Browse In Your Inventory
            [2] BlackSmith      - Upgrade Equipments
            [3] Training        - Improve Yourself
            [4] Adventure       - Fight Monsters
            [5] Region Boss     - Challange Boss
            [A] Auto Save       - Toggle Auto Save (Currently {(context.IsAutoSaveOn ? "ON" : "OFF")})
            [S] Save Game       - Save Progress
            [Q] Quit
            """);
            string? input = Console.ReadLine()?.ToUpper();

            if (input == "A")
            {
                if (context.IsAutoSaveOn)
                {
                    context.IsAutoSaveOn = false;
                    Console.WriteLine("[SYSTEM] Auto Save is now OFF");
                }
                else
                {
                    context.IsAutoSaveOn = true;
                    Console.WriteLine("[SYSTEM] Auto Save is now ON");
                }
                Thread.Sleep(1000);
                return GameState.MainMenu;
            }

            if (input == "S")
            {
                _saveService.SaveGame(context);
                return GameState.MainMenu;
            }

            return input switch
            {
                "0" => GameState.DetailedStats,
                "1" => GameState.Inventory,
                "2" => GameState.Blacksmith,
                "3" => GameState.Training,
                "4" => GameState.Adventure,
                "5" => GameState.Dungeon,
                "Q" => GameState.Exit,
                _ => GameState.MainMenu
            };

        }
    }
}
