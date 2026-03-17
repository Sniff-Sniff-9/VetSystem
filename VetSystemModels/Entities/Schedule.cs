using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.Entities;

public partial class Schedule
{
    [Key]
    public int ScheduleId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int WorkdayId { get; set; }

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Workday Workday { get; set; } = null!;
}
