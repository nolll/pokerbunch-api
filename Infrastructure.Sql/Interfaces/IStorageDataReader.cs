using System;
using System.Collections.Generic;

namespace Infrastructure.Sql.Interfaces
{
    public interface IStorageDataReader
    {
        string GetStringValue(string key);
        int GetIntValue(string key);
        bool GetBooleanValue(string key);
        DateTime GetDateTimeValue(string key);
        bool Read();
        IList<T> ReadList<T>(Func<IStorageDataReader, T> func);
        T ReadOne<T>(Func<IStorageDataReader, T> func);
        int? ReadInt(string key);
        string ReadString(string key);
        IList<int> ReadIntList(string key);
        IList<string> ReadStringList(string key);
        bool HasRows();
    }
}