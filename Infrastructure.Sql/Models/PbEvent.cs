using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_event")]
public partial class PbEvent
{
    [Key]
    [Column("event_id")]
    public int EventId { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("bunch_id")]
    public int BunchId { get; set; }

    [ForeignKey("BunchId")]
    [InverseProperty("PbEvent")]
    public virtual PbBunch Bunch { get; set; } = null!;

    [ForeignKey("EventId")]
    [InverseProperty("Event")]
    public virtual ICollection<PbCashgame> Cashgame { get; set; } = new List<PbCashgame>();
}
