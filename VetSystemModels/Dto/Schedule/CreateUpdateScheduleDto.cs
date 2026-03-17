using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.Schedule
{
    public class CreateUpdateScheduleDto
    {
        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int WorkdayId { get; set; }

    }
}
