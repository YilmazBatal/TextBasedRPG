using TextBasedRPG.Managers;

namespace TextBasedRPG.Interfaces
{
    public interface IMenuState
    {
        GameState Update(GameContext context);
    }
}
