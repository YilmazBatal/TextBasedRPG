using TextBasedRPG;
using TextBasedRPG.Heroes;

Console.Title = "Text Based RPG";
Console.WriteLine("}==============>>>    Welcome to TB-RPG   <<<=============={");

GameManager gameManager = new GameManager();
gameManager.Run();

Console.WriteLine("Shutting down TB-RPG.");