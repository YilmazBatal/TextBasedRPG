using TextBasedRPG.Interfaces;
using TextBasedRPG.Managers;

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