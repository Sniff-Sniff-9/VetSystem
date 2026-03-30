using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.WorkdayOverride
{
    public class CreateUpdateWorkdayOverrideDto
    {
        public int EmployeeId { get; set; }

        public DateOnly WorkdayOverrideDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }
}
