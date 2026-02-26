using TextBasedRPG.Interfaces;
using TextBasedRPG.Locations;
using TextBasedRPG.Managers;
using TextBasedRPG.UI;

public class MapState : IMenuState
{
    public GameState Update(GameContext context)
    {
        Console.Clear();
        

        
        MenuUI.MapPagination(context);

        //Console.Write($"Selection: ");
        //string? selection = Console.ReadLine();

        return GameState.MainMenu;
    }
}