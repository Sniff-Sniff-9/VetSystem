using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.ScheduleAvailability
{
    public class ScheduleAvailabilityDto
    {
        public int EmployeeId { get; set; }

        public DateOnly ScheduleAvailabilityDate { get; set; }
    }
}
