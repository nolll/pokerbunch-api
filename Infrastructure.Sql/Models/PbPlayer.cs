using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_player")]
public partial class PbPlayer
{
    [Key]
    [Column("player_id")]
    public int PlayerId { get; set; }

    [Column("bunch_id")]
    public int BunchId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("approved")]
    public bool Approved { get; set; }

    [Column("player_name")]
    [StringLength(50)]
    public string? PlayerName { get; set; }

    [Column("color")]
    [StringLength(10)]
    public string? Color { get; set; }

    [ForeignKey("BunchId")]
    [InverseProperty("PbPlayer")]
    public virtual PbBunch Bunch { get; set; } = null!;

    [InverseProperty("Player")]
    public virtual ICollection<PbCashgameCheckpoint> PbCashgameCheckpoint { get; set; } = new List<PbCashgameCheckpoint>();

    [InverseProperty("Player")]
    public virtual ICollection<PbComment> PbComment { get; set; } = new List<PbComment>();

    [InverseProperty("Player")]
    public virtual ICollection<PbTournamentResult> PbTournamentResult { get; set; } = new List<PbTournamentResult>();

    [ForeignKey("UserId")]
    [InverseProperty("PbPlayer")]
    public virtual PbUser? User { get; set; }
}
