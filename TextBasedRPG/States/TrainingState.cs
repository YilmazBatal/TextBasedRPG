using TextBasedRPG.Interfaces;
using TextBasedRPG.Managers;

public class TrainingState : IMenuState
{
    public GameState Update(GameContext context)
    {
        Console.Clear();
        var p = context.Player;
        int totalPointsSpent = p.InvestedSTRPoints + p.InvestedVITPoints + p.InvestedDEXPoints + p.InvestedAGIPoints;
        Console.WriteLine("==========================================================");
        Console.WriteLine("Welcome to Training Grounds!");
        Console.WriteLine("Unused Training Points: " + p.UnusedStatPoints);
        Console.WriteLine("Total Points Invested: " + totalPointsSpent);
        Console.WriteLine("----------------------------------------------------------");
        Console.WriteLine($"[1] STR     - Increases attack & critical damage");
        Console.WriteLine($"[2] VIT     - Increases defence & health");
        Console.WriteLine($"[3] DEX     - Increases critical hit chance.");
        Console.WriteLine($"[4] AGI     - Increases chances to dodge");
        Console.WriteLine("----------------------------------------------------------");
        Console.WriteLine("==========================================================");

        Console.Write($"Selection: ");
        string? selection = Console.ReadLine();
        Console.Write($"Amount to invest: ");
        string? amountInput = Console.ReadLine();

        if (int.TryParse(amountInput, out int amount))
        {
            if (amount > 0 && amount <= p.UnusedStatPoints)
            {
                switch (selection)
                {
                    case "1": p.InvestedSTRPoints += amount; break;
                    case "2": p.InvestedVITPoints += amount; break;
                    case "3": p.InvestedDEXPoints += amount; break;
                    case "4": p.InvestedAGIPoints += amount; break;
                    default:
                        Console.WriteLine("[SYSTEM] Invalid selection");
                        Thread.Sleep(1000);
                        return GameState.Training;
                }

                p.UnusedStatPoints -= amount;
                Console.WriteLine($"[SYSTEM] Invested {amount} points");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("[SYSTEM] Invalid amount or not enough points");
                Thread.Sleep(1000);
                return GameState.Training;
            }
        }

        return GameState.MainMenu;
    }
}