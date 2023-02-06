using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core;
using Moq;

namespace Tests.Core;

public class Mocker
{
    private readonly Dictionary<Type, Mock> _mocks = new();

    public Mock<T> MockOf<T>() where T : class
    {
        var type = typeof(T);
        if (_mocks[type] is not Mock<T> mock)
            throw new PokerBunchException($"Mock not found: {type}");

        return mock;
    }

    private object InterfaceBuilder(Type type)
    {
        if (_mocks.ContainsKey(type))
            return _mocks[type].Object;

        var mock = (Mock?)Activator.CreateInstance(typeof(Mock<>).MakeGenericType(type));
        if(mock is null)
            throw new PokerBunchException($"Mock could not be created: {type}");

        _mocks.Add(type, mock);
        return mock.Object;
    }

    public T New<T>(dynamic? ctorParams = null, int ctorIndex = 0)
    {
        var ctorParamsDictionary = (Dictionary<string, object>)ToDictionary(ctorParams);
        var classCtor = typeof(T).GetConstructors()[ctorIndex].GetParameters();

        foreach (var param in ctorParamsDictionary)
            if (!classCtor.Select(c => c.Name).Contains(param.Key))
                throw new ArgumentException($"{ param.Key } is not an argument in constructor of class { typeof(T).Name }");

        var ctor = new List<object>();
        foreach (var param in classCtor)
        {
            if(param.Name is null)
                continue;

            ctor.Add(ctorParamsDictionary.ContainsKey(param.Name)
                ? ctorParamsDictionary[param.Name]
                : Create(param.ParameterType));
        }

        var type = typeof(T);
        var mock = (T?)Activator.CreateInstance(type, ctor.ToArray());
        if (mock is null)
            throw new PokerBunchException($"Mock could not be created: {type}");

        return mock;
    }

    private object Create(Type type)
    {
        if (type.IsValueType)
        {
            var instance = Activator.CreateInstance(type);
            if(instance is null)
                throw new PokerBunchException($"Mock could not be created: {type}");

            return instance;
        }

        if (type.IsInterface)
            return InterfaceBuilder(type);

        throw new PokerBunchException($"Mock could not be created: {type}");
    }

    private static Dictionary<string, object> ToDictionary(dynamic? dynObj)
    {
        var dictionary = new Dictionary<string, object>();

        if (dynObj is null)
            return dictionary;
        
        foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynObj))
        {
            object obj = propertyDescriptor.GetValue(dynObj);
            dictionary.Add(propertyDescriptor.Name, obj);
        }

        return dictionary;
    }
}