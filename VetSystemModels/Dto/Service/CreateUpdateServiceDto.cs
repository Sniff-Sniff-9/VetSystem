using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.Service
{
    public class CreateUpdateServiceDto
    {
        [StringLength(50)]
        [Required]
        public string ServiceName { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int DurationMinutes { get; set; }
    }
}
