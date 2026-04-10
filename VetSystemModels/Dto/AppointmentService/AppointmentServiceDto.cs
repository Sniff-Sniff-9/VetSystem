using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.AppointmentService
{
    public class AppointmentServiceDto
    {
        public int AppointmentServiceId { get; set; }

        public int AppointmentId { get; set; }

        public int ServiceId { get; set; }

        public string ServiceName { get; set; } = null!;

        public decimal PriceAtMoment { get; set; }

        public bool IsMain { get; set; } = false;
    }
}
