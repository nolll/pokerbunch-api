using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_cashgame")]
public partial class PbCashgame
{
    [Key]
    [Column("cashgame_id")]
    public int CashgameId { get; set; }

    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("timestamp", TypeName = "timestamp without time zone")]
    public DateTime Timestamp { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("bunch_id")]
    public int BunchId { get; set; }

    [Column("location_id")]
    public int LocationId { get; set; }

    [ForeignKey("BunchId")]
    [InverseProperty("PbCashgame")]
    public virtual PbBunch Bunch { get; set; } = null!;

    [ForeignKey("LocationId")]
    [InverseProperty("PbCashgame")]
    public virtual PbLocation Location { get; set; } = null!;

    [InverseProperty("Cashgame")]
    public virtual ICollection<PbCashgameCheckpoint> PbCashgameCheckpoint { get; set; } = new List<PbCashgameCheckpoint>();

    [ForeignKey("CashgameId")]
    [InverseProperty("Cashgame")]
    public virtual ICollection<PbComment> Comment { get; set; } = new List<PbComment>();

    [ForeignKey("CashgameId")]
    [InverseProperty("Cashgame")]
    public virtual ICollection<PbEvent> Event { get; set; } = new List<PbEvent>();
}
