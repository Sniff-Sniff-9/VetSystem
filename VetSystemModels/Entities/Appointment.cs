using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.Entities;

public partial class Appointment
{
    [Key]
    public int AppointmentId { get; set; }

    public int ServiceId { get; set; }

    public int PetId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal PriceAtMoment { get; set; }

    public int AppointmentStatusId { get; set; }

    public int ScheduleId { get; set; }
    public bool IsDeleted { get; set; } = false;

    [ForeignKey("AppointmentStatusId")]
    [InverseProperty("Appointments")]
    public virtual AppointmentStatus AppointmentStatus { get; set; } = null!;

    [ForeignKey("PetId")]
    [InverseProperty("Appointments")]
    public virtual Pet Pet { get; set; } = null!;

    [ForeignKey("ScheduleId")]
    [InverseProperty("Appointments")]
    public virtual Schedule Schedule { get; set; } = null!;

    [ForeignKey("ServiceId")]
    [InverseProperty("Appointments")]
    public virtual Service Service { get; set; } = null!;
}
