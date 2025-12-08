using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleResponseDTO
    {
        public List<SaleItemDTO> Items { get; init; }
    }
}