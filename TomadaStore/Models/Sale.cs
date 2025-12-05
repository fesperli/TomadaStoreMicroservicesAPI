using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomadaStore.Models.Models
{
    public class Sale
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public ObjectId Id { get; private set; }
        [BsonElement("customer")]
        public Customer Customer { get; private set; }  
        [BsonElement("products")]
        public List<Product> Products { get; private set; }
        [BsonElement("saleDate")]
        public DateTime SaleDate { get; private set; }
        [BsonElement("totalPrice")]
        public decimal TotalPrice { get; private set; }

        public Sale() { }
        public Sale(ObjectId id, Customer customer, List<Product> products, DateTime saleDate, decimal totalPrice)
        {
            Id = id;
            Customer = customer;
            Products = products;
            SaleDate = saleDate;
            TotalPrice = totalPrice;
        }

        public Sale(Customer customer, List<Product> products, decimal totalPrice)
        {
            Id = ObjectId.GenerateNewId();
            Customer = customer;
            Products = products;
            SaleDate = DateTime.UtcNow;
            TotalPrice = totalPrice;
        }
    }

}
