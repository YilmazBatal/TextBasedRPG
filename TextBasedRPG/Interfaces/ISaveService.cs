using TextBasedRPG.Heroes;

public interface ISaveService
{
    void SaveGame(GameContext context);
    GameContext LoadGame();
}