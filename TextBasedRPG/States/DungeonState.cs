using TextBasedRPG.Interfaces;
using TextBasedRPG.Managers;

public class DungeonState : IMenuState
{
    public GameState Update(GameContext context)
    {
        Console.Clear();
        Console.WriteLine("You are at the Region Boss... Press any key to continue.");
        Console.ReadKey();
        return GameState.MainMenu;
    }
}