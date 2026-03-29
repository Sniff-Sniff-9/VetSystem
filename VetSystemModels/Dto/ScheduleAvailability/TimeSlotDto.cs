using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.TimeSlot
{
    public class TimeSlotDto
    {
        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }
}
