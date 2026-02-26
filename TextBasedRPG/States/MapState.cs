using TextBasedRPG.Interfaces;
using TextBasedRPG.Managers;

public class MapState : IMenuState
{
    public GameState Update(GameContext context)
    {
        Console.Clear();
        #region Title
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════════════════╗");
        Console.Write    ("║");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write    ("                          MAP                         ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝");
        Console.ResetColor();
        #endregion

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