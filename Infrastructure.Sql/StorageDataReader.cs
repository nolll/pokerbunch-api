using System;
using System.Collections.Generic;
using System.Data;
using Infrastructure.Sql.Interfaces;

namespace Infrastructure.Sql;

public class StorageDataReader : IStorageDataReader
{
    private readonly IDataReader _reader;

    public StorageDataReader(IDataReader reader)
    {
        _reader = reader;
    }

    public string GetStringValue(string key)
    {
        var ordinal = _reader.GetOrdinal(key);
        return _reader.IsDBNull(ordinal) ? default(string) : _reader.GetString(ordinal);
    }

    public int GetIntValue(string key)
    {
        var ordinal = _reader.GetOrdinal(key);
        return _reader.IsDBNull(ordinal) ? default(int) : _reader.GetInt32(ordinal);
    }

    public string ReadString(string key)
    {
        if (Read())
            return GetStringValue(key);
        return null;
    }

    public IList<string> ReadStringList(string key)
    {
        var list = new List<string>();
        while (Read())
        {
            list.Add(GetStringValue(key));
        }
        return list;
    }

    public bool HasRows()
    {
        return Read();
    }

    public int? ReadInt(string key)
    {
        if (Read())
            return GetIntValue(key);
        return null;
    }

    public IList<int> ReadIntList(string key)
    {
        var list = new List<int>();
        while (Read())
        {
            list.Add(GetIntValue(key));
        }
        return list;
    }

    public bool GetBooleanValue(string key)
    {
        var ordinal = _reader.GetOrdinal(key);
        return !_reader.IsDBNull(ordinal) && _reader.GetBoolean(ordinal);
    }

    public DateTime GetDateTimeValue(string key)
    {
        var ordinal = _reader.GetOrdinal(key);
        var dateTime = _reader.IsDBNull(ordinal) ? default(DateTime) : _reader.GetDateTime(ordinal);
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

    public bool Read()
    {
        return _reader.Read();
    }

    public IList<T> ReadList<T>(Func<IStorageDataReader, T> func)
    {
        var list = new List<T>();
        while (Read())
        {
            list.Add(func(this));
        }
        return list;
    }

    public T ReadOne<T>(Func<IStorageDataReader, T> func)
    {
        return Read() ? func(this) : default(T);
    }
}