using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models;
using TomadaStore.Models.DTOs;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleRequestDTO
    {
        public int CustomerId { get; init; }
        public List<SaleItemDTO> Items { get; init; } = new();
    }
}