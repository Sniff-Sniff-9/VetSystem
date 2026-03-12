using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.Service
{
    public class ServiceDto
    {
        [Required]
        [StringLength(50)]
        public string ServiceName { get; set; } = null!;
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
    }
}
