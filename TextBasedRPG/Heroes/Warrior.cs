namespace TextBasedRPG.Heroes
{
    internal class Warrior : Hero
    {
        public Warrior()
        {
            ClassName = "Warrior";
            BaseHP = 150;
            BaseATK = 20;
            BaseDEF = 15;
            Description = "A strong and resilient fighter, excelling in close combat.";
        }
    }
}
