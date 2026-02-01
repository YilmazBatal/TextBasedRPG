using TextBasedRPG.Interfaces;
using TextBasedRPG.Managers;

namespace TextBasedRPG.States
{
    public class InventoryState : IMenuState
    {
        public GameState Update(GameContext context)
        {
            Console.Clear();
            UIHelper.BackpackPagination(context);
            return GameState.MainMenu;
        }

    }
}
