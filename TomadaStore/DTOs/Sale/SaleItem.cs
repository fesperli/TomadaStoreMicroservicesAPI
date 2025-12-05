using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleItemDTO
    {
        public string ProductId { get; init; }
        public int Quantity { get; init; }
    }
}