using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VetSystemModels.DataBase;

[Index("SpeciesName", Name = "UQ__Species__304D4C0D4DB9F377", IsUnique = true)]
public partial class Species
{
    [Key]
    public int SpeciesId { get; set; }

    [StringLength(50)]
    public string SpeciesName { get; set; } = null!;

    [InverseProperty("Species")]
    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
