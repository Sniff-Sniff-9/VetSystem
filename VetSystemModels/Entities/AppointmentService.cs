using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Entities
{
    public partial class AppointmentService
    {
        public int AppointmentServiceId { get; set; }

        public int AppointmentId { get; set; }

        public int ServiceId { get; set; }

        public decimal PriceAtMoment { get; set; }

        public bool IsMain { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public Appointment Appointment { get; set; } = null!;

        public Service Service { get; set; } = null!;
    }
}
