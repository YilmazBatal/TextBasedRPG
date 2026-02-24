using TextBasedRPG.Interfaces;
using TextBasedRPG.Managers;

public class MapState : IMenuState
{
    public GameState Update(GameContext context)
    {
        Console.Clear();
        Console.WriteLine("==========================================================");
        Console.WriteLine("Welcome to Map Tp any region!");
        Console.WriteLine("----------------------------------------------------------");
        Console.WriteLine($"[1] STR     - Increases attack & critical damage");
        Console.WriteLine($"[2] VIT     - Increases defence & health");
        Console.WriteLine($"[3] DEX     - Increases critical hit chance.");
        Console.WriteLine($"[4] AGI     - Increases chances to dodge");
        Console.WriteLine("----------------------------------------------------------");
        Console.WriteLine("==========================================================");

        Console.Write($"Selection: ");
        string? selection = Console.ReadLine();

        return GameState.MainMenu;
    }
}