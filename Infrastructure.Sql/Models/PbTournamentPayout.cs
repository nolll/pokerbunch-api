using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[PrimaryKey("TournamentId", "Position")]
[Table("pb_tournament_payout")]
public partial class PbTournamentPayout
{
    [Key]
    [Column("tournament_id")]
    public int TournamentId { get; set; }

    [Key]
    [Column("position")]
    public int Position { get; set; }

    [Column("payout")]
    public int Payout { get; set; }

    [ForeignKey("TournamentId")]
    [InverseProperty("PbTournamentPayout")]
    public virtual PbTournament Tournament { get; set; } = null!;
}
