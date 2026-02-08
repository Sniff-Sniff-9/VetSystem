using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.Pet
{
    internal class CreateUpdatePetDto
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; } = null!;
        [StringLength(50)]
        [Required]
        public int SpeciesId { get; set; }
        [StringLength(50)]
        [Required]
        public string Breed { get; set; } = null!;
        [Required]
        public DateOnly BirthDate { get; set; }
        [Required]
        public int GenderId { get; set; }
    }
}
