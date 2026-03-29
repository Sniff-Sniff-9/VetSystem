using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetSystemModels.Dto.Workday
{
    public class CreateUpdateWorkdayDto
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(20)]
        public string DayOfWeek { get; set; } = null!;
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        [Required]
        public TimeOnly LunchStart { get; set; }
        [Required]
        public TimeOnly LunchEnd { get; set; }
        [Required]
        public int SlotDuration { get; set; }
    }
}
