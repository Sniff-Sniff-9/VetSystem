using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.Pet
{
    public class PetDto
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; } = null!;
        [StringLength(50)]
        [Required]
        public string SpeciesName { get; set; } = null!;
        [StringLength(50)]
        [Required]
        public string Breed { get; set; } = null!;
        [Required]
        public DateOnly BirthDate { get; set; }
        [StringLength(10)]
        [Required]
        public string GenderName { get; set; } = null!;

    }
}
