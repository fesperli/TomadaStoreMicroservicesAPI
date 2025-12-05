using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TomadaStore.Models.Models
{
    public class Category
    {

        public ObjectId Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Category() { }

        [BsonConstructor]
        public Category(ObjectId id, string name, string description)
        {
            Id = ObjectId.GenerateNewId();
            Name = name;
            Description = description;
        }
        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }

    }
}