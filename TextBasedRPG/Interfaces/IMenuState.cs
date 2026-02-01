using TextBasedRPG.Heroes;

namespace TextBasedRPG.Interfaces
{
    public interface IMenuState
    {
        GameState Update(GameContext context);
    }
}
