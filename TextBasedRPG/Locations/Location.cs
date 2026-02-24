using System.Diagnostics.CodeAnalysis;
using TextBasedRPG.Entities;

namespace TextBasedRPG.Locations
{
    public class Location
    {
        public required string ID { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
        public List<string>? AdventureTexts { get; init; }
        public List<string>? Entities { get; init; }
        public List<Entity> ActiveEntities { get; protected set; } = new();

        [SetsRequiredMembers]
        public Location(string id, string name, string? description, List<string>? texts, List<string>? entities)
        {
            ID = id;
            Name = name;
            Description = description;
            AdventureTexts = texts ?? new List<string>();
            Entities = entities ?? new List<string>();
        }
    }
}
