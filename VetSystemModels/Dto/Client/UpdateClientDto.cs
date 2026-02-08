using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.Client
{
    public class UpdateClientDto
    {
        [StringLength(50)]
        [Required]
        public string LastName { get; set; } = null!;
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        [Required]
        public string MiddleName { get; set; } = null!;
        [Required]
        public DateOnly BirthDate { get; set; }
        [StringLength(20)]
        [Required]
        public string Phone { get; set; } = null!;
    }
}
