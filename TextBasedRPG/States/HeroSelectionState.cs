using TextBasedRPG.Heroes;
using TextBasedRPG.Interfaces;

namespace TextBasedRPG.States
{
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

                Hero? candidate = input switch
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

        private bool ConfirmSelection(Hero candidate)
        {
            candidate.GetStatsSummary();
            Console.WriteLine($"Confirm {candidate.ClassName}? [Y/N]");
            return Console.ReadLine()?.ToUpper() == "Y";
        }
    }
}
