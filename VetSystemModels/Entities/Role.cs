using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VetSystemModels.Entities;

[Table("Role")]
public partial class Role
{
    [Key]
    public int RoleId { get; set; }

    [StringLength(20)]
    public string RoleName { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
