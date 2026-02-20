using TextBasedRPG.Interfaces;
using TextBasedRPG.Managers;

public class AdventureState : IMenuState
{
    const int monsterEncounterChance = 70;
    const int itemFindingChance = 85;
    const int miniEventChance = 95;
    const int chestChance = 100;
    const int merchantEncounterChance = 0; // in the future
    public bool isAdventuring = true;
    public GameState Update(GameContext context)
    {
        Console.Clear();
        isAdventuring = true;
        while (isAdventuring)
        {
            Traveling();
            switch (AdventureGenerator())
            {
                case <= monsterEncounterChance:
                    CombatManager.StartCombat(context);
                    break;
                case <= itemFindingChance:
                    GenerateItem();
                    break;
                case <= miniEventChance:
                    GenerateEvent();
                    break;
                default:
                    GenerateChest();
                    break;
            }
            Console.WriteLine("\n[1] Keep Adventuring | [Any Other Key] Go Back to Town");
            string? input = Console.ReadLine();
            if (input != "1")
                isAdventuring = false;
        }
        return GameState.MainMenu;
    }

    private static void Traveling()
    {
        Console.Clear();
        Console.WriteLine("Traveling");
        Thread.Sleep(400);
        Console.Clear();
        Console.WriteLine("Traveling.");
        Thread.Sleep(400);
        Console.Clear();
        Console.WriteLine("Traveling..");
        Thread.Sleep(400);
        Console.Clear();
        Console.WriteLine("Traveling...");
        Thread.Sleep(400);
        Console.Clear();
    }

    public void GenerateItem()
    {
        Console.WriteLine("You have found an item");
        Console.WriteLine("Item given to the player");
    }
    public void GenerateEvent()
    {
        Console.WriteLine("You have encountered a little girl");
        Console.WriteLine("wanna help her? y/n");
    }
    public void GenerateChest()
    {
        Console.WriteLine("You have encountered a chest");
        Console.WriteLine("player got this gold and this exp ");
    }

    public int AdventureGenerator()
    {
        return Random.Shared.Next(0, 100);
    }
}