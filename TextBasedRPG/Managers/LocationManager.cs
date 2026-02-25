namespace TextBasedRPG.Managers
{
    internal static class LocationManager
    {
        public static Dictionary<string, string> locations = new ();
        public static void LocationMapping(GameContext context)
        {
            foreach (var item in context.Locations)
            {
                locations.Add(item.ID, item.Name);
            }
        }
    }
}
