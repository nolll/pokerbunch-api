using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.Models;

[Table("pb_video")]
public partial class PbVideo
{
    [Key]
    [Column("video_id")]
    public int VideoId { get; set; }

    [Column("bunch_id")]
    public int BunchId { get; set; }

    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("thumbnail")]
    [StringLength(255)]
    public string Thumbnail { get; set; } = null!;

    [Column("length")]
    public int Length { get; set; }

    [Column("width")]
    public int Width { get; set; }

    [Column("height")]
    public int Height { get; set; }

    [Column("source")]
    [StringLength(20)]
    public string Source { get; set; } = null!;

    [Column("type")]
    [StringLength(20)]
    public string Type { get; set; } = null!;

    [Column("hidden")]
    public bool Hidden { get; set; }

    [ForeignKey("BunchId")]
    [InverseProperty("PbVideo")]
    public virtual PbBunch Bunch { get; set; } = null!;
}
