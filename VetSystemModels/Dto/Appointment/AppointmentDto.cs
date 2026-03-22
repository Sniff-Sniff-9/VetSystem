using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.Appointment
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int ServiceId { get; set; }

        public string ServiceName { get; set; } = null!;

        public int PetId { get; set; }

        public string PetName { get; set; } = null!;

        public decimal TotalPriceAtMoment { get; set; }

        public int AppointmentStatusId { get; set; }

        public string AppointmentStatusName { get; set; } = null!;

        public int ScheduleId { get; set; }

        public TimeOnly SсheduleTimeStart { get; set; }

        public TimeOnly SсheduleTimeEnd { get; set; }
    }
}
