using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.Appointment
{
    public class CreateUpdateAppointmentDto
    {
        public int ServiceId { get; set; }

        public int PetId { get; set; }

        public int AppointmentStatusId { get; set; }

        public int ScheduleId { get; set; }
    }
}
