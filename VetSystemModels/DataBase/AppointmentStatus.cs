using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.DataBase;

[Index("AppointmentStatusName", Name = "UQ__Appointm__64403C6D3549EC38", IsUnique = true)]
public partial class AppointmentStatus
{
    [Key]
    public int AppointmentStatusId { get; set; }

    [StringLength(20)]
    public string AppointmentStatusName { get; set; } = null!;

    [InverseProperty("AppointmentStatus")]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
