using TextBasedRPG.Managers;

Console.Title = "Text RPG";
Console.WriteLine("""   
    ======================================

     ######## ######## ##     ## ######## 
        ##    ##        ##   ##     ##    
        ##    ##         ## ##      ##    
        ##    ######      ###       ##    
        ##    ##         ## ##      ##    
        ##    ##        ##   ##     ##    
        ##    ######## ##     ##    ##    

     ########  ########   ######          
     ##     ## ##     ## ##    ##         
     ##     ## ##     ## ##               
     ########  ########  ##   ####        
     ##   ##   ##        ##    ##         
     ##    ##  ##        ##    ##         
     ##     ## ##         ######          

    ======================================
    """);

ISaveService saveService = new DataManager();

GameManager gameManager = new GameManager(saveService);
gameManager.Run();


Console.WriteLine("Shutting down Text RPG.");