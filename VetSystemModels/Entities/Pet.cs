using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.Entities;

public partial class Pet
{
    [Key]
    public int PetId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public int SpeciesId { get; set; }

    [StringLength(50)]
    public string Breed { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public int GenderId { get; set; }

    public int ClientId { get; set; }
    public bool IsDeleted { get; set; } = false;

    [InverseProperty("Pet")]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    [ForeignKey("ClientId")]
    [InverseProperty("Pets")]
    public virtual Client Client { get; set; } = null!;

    [ForeignKey("GenderId")]
    [InverseProperty("Pets")]
    public virtual Gender Gender { get; set; } = null!;

    [ForeignKey("SpeciesId")]
    [InverseProperty("Pets")]
    public virtual Species Species { get; set; } = null!;
}
