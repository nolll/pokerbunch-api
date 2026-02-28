using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_location")]
public partial class PbLocation
{
    [Key]
    [Column("location_id")]
    public int LocationId { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("bunch_id")]
    public int BunchId { get; set; }

    [ForeignKey("BunchId")]
    [InverseProperty("PbLocation")]
    public virtual PbBunch Bunch { get; set; } = null!;

    [InverseProperty("Location")]
    public virtual ICollection<PbCashgame> PbCashgame { get; set; } = new List<PbCashgame>();
}
