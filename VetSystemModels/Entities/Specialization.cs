using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.Entities;

[Index("SpecializationName", Name = "UQ__Speciali__08A8EB9EB1D3501A", IsUnique = true)]
public partial class Specialization
{
    [Key]
    public int SpecializationId { get; set; }

    [StringLength(50)]
    public string SpecializationName { get; set; } = null!;

    [InverseProperty("Specialization")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
