using TextBasedRPG.Interfaces;
using TextBasedRPG.Managers;

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