using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[PrimaryKey("TournamentId", "PlayerId")]
[Table("pb_tournament_result")]
public partial class PbTournamentResult
{
    [Key]
    [Column("tournament_id")]
    public int TournamentId { get; set; }

    [Key]
    [Column("player_id")]
    public int PlayerId { get; set; }

    [Column("position")]
    public int Position { get; set; }

    [ForeignKey("PlayerId")]
    [InverseProperty("PbTournamentResult")]
    public virtual PbPlayer Player { get; set; } = null!;

    [ForeignKey("TournamentId")]
    [InverseProperty("PbTournamentResult")]
    public virtual PbTournament Tournament { get; set; } = null!;
}
