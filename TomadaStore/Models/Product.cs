using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TomadaStore.Models.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("category")]
        public Category Category { get;  set; }
        [JsonPropertyName("quantity")]
        public int Quantity { get;  set; }

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
