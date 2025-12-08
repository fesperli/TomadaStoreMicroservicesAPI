using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomadaStore.Models.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; private set; }
        [BsonElement("name")]
        public string Name { get; private set; }
        [BsonElement("description")]
        public string Description { get; private set; }
        [BsonElement("price")]
        public decimal Price { get; private set; }
        [BsonElement("category")]
        public Category Category { get; private set; }
        [BsonElement("quantity")]
        public int Quantity { get; private set; }

        public Product() { }
        
        public Product(ObjectId id, string name, string description, decimal price, Category category, int quantity)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            Quantity = quantity;
        }

        public Product(string name, string description, decimal price, Category category, int quantity)
        {
            Id = ObjectId.GenerateNewId();
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            Quantity = quantity;
        }
    }
}
