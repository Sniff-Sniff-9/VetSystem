using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto
{
    public class UpdateUserDto
    {
        [StringLength(50)]
        [Required]
        public string Username { get; set; } = null!;
        [StringLength(100)]
        [Required]
        public string Email { get; set; } = null!;
    }
}
