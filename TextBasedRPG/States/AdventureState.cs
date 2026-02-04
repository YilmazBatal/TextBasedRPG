using TextBasedRPG.Interfaces;
using TextBasedRPG.Managers;

public class AdventureState : IMenuState
{
    public GameState Update(GameContext context)
    {
        Console.Clear();
        CombatManager.StartCombat(context);
        Console.ReadKey();
        return GameState.MainMenu;
    }
}