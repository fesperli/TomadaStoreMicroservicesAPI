using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomadaStore.Models.DTOs.Sale;

namespace TomadaStore.Models.DTOs.ConfirmSale
{
    public class ProcessSaleDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<SaleItemDTO> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }

    }

}
