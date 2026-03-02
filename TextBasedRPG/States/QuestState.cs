using TextBasedRPG.Interfaces;
using TextBasedRPG.Managers;

namespace TextBasedRPG.States
{
    internal class QuestState : IMenuState
    {
        public GameState Update(GameContext context)
        {
            return GameState.MainMenu;
        }
    }
}
