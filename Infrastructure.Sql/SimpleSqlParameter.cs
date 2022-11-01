﻿using Npgsql;
using System;

namespace Infrastructure.Sql;

public class SimpleSqlParameter : IEquatable<SimpleSqlParameter>
{
    private string ParameterName { get; }
    private object Value { get; }

    public SimpleSqlParameter(string parameterName, object value)
    {
        ParameterName = parameterName;
        Value = value;
    }

    public NpgsqlParameter SqlParameter
    {
        get
        {
            var value = Value ?? DBNull.Value;
            return new NpgsqlParameter(ParameterName, value);
        }
    }

    public bool Equals(SimpleSqlParameter other)
    {
        return ParameterName.Equals(other.ParameterName) && Value.Equals(other.Value);
    }
}