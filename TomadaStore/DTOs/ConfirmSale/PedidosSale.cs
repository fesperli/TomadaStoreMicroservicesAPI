using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomadaStore.Models.DTOs.Sale;

namespace TomadaStore.Models.DTOs.ConfirmSale
{
    public class PedidosSale
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<SaleItemDTO> Items { get; set; } = new();
        public string Status { get; set; }
    }
}
