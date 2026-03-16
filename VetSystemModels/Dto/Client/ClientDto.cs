using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.Client
{
    public class ClientDto
    {
        public int ClientId { get; set; }

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = null!;

        public DateOnly BirthDate { get; set; }

        public string Phone { get; set; } = null!;

        public int? UserId { get; set; }
    }
}
