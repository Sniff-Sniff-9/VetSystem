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
        public int PetId { get; set; }
        public string Name { get; set; } = null!;
        public int SpeciesId { get; set; }
        public string SpeciesName { get; set; } = null!;
        public string Breed { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; } = null!;
        public int ClientId { get; set; }
        public int? ClientUserId { get; set; }
        public string ClientName { get; set; } = null!;

    }
}
