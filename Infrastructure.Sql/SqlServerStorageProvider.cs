using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Sql.Interfaces;
using Npgsql;
using System.Data;

namespace Infrastructure.Sql;

public class SqlServerStorageProvider
{
    private readonly string _connectionString;

    public SqlServerStorageProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public IStorageDataReader Query(string sql, IEnumerable<SimpleSqlParameter> parameters = null)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(ToSqlCommands(parameters));
                }
                var mySqlReader = command.ExecuteReader();
                var dt = new DataTable();
                dt.Load(mySqlReader);
                return new StorageDataReader(dt.CreateDataReader());
            }
        }
    }

    public IStorageDataReader Query(string sql, ListSqlParameter parameter)
    {
        var sqlWithIdList = sql.Replace(parameter.ParameterName, parameter.ParameterNameList);
        return Query(sqlWithIdList, parameter.ParameterList);
    }

    public int Execute(string sql, IEnumerable<SimpleSqlParameter> parameters = null)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(ToSqlCommands(parameters));
                }
                return command.ExecuteNonQuery();
            }
        }
    }

    public int ExecuteInsert(string sql, IEnumerable<SimpleSqlParameter> parameters = null)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(ToSqlCommands(parameters));
                }
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }

    private object[] ToSqlCommands(IEnumerable<SimpleSqlParameter> parameters)
    {
        return parameters.Select(o => o.SqlParameter).ToArray();
    }
}