using Core.Entities;
using System;

namespace Infrastructure.Sql.Sql;

public static class Schema
{
    public static readonly SqlBunch Bunch = new();
    public static readonly SqlLocation Location = new();
    public static readonly SqlPlayer Player = new();
    public static readonly SqlUser User = new();
}