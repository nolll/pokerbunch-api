using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_cashgame_checkpoint")]
public partial class PbCashgameCheckpoint
{
    [Key]
    [Column("checkpoint_id")]
    public int CheckpointId { get; set; }

    [Column("cashgame_id")]
    public int CashgameId { get; set; }

    [Column("player_id")]
    public int PlayerId { get; set; }

    [Column("type")]
    public int Type { get; set; }

    [Column("amount")]
    public int Amount { get; set; }

    [Column("stack")]
    public int Stack { get; set; }

    [Column("timestamp", TypeName = "timestamp without time zone")]
    public DateTime Timestamp { get; set; }

    [ForeignKey("CashgameId")]
    [InverseProperty("PbCashgameCheckpoint")]
    public virtual PbCashgame Cashgame { get; set; } = null!;

    [ForeignKey("PlayerId")]
    [InverseProperty("PbCashgameCheckpoint")]
    public virtual PbPlayer Player { get; set; } = null!;
}
