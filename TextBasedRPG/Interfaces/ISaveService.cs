using TextBasedRPG.Entities;
using TextBasedRPG.Managers;

public interface ISaveService
{
    void SaveGame(GameContext context);
    GameContext LoadGame();
}