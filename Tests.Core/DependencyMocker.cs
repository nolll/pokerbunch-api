using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Moq;

namespace Tests.Core
{
    public class DependencyMocker
    {
        readonly Dictionary<Type, Mock> _mocks = new Dictionary<Type, Mock>();

        public Mock<T> MockOf<T>() where T : class => _mocks[typeof(T)] as Mock<T>;

        private object InterfaceBuilder(Type type)
        {
            if (_mocks.ContainsKey(type))
                return _mocks[type].Object;

            var mock = (Mock)Activator.CreateInstance(typeof(Mock<>).MakeGenericType(type));
            _mocks.Add(type, mock);
            return mock.Object;
        }

        public T New<T>(dynamic ctorParams = null, int ctorIndex = 0)
        {
            var ctorParamsDictionary = (Dictionary<string, object>)(ToDictionary(ctorParams) ?? new Dictionary<string, object>());
            var classCtor = typeof(T).GetConstructors()[ctorIndex].GetParameters();

            foreach (var param in ctorParamsDictionary)
                if (!classCtor.Select(c => c.Name).Contains(param.Key))
                    throw new ArgumentException($"{ param.Key } is not an argument in constructor of class { typeof(T).Name }");

            var ctor = new List<object>();
            foreach (var param in classCtor)
                ctor.Add(ctorParamsDictionary.ContainsKey(param.Name)
                    ? ctorParamsDictionary[param.Name]
                    : Create(param.ParameterType));

            return (T)Activator.CreateInstance(typeof(T), ctor.ToArray());
        }

        private object Create(Type type) => type.IsValueType
            ? Activator.CreateInstance(type)
            : (type.IsInterface
                ? InterfaceBuilder(type)
                : null);

        private Dictionary<string, object> ToDictionary(dynamic dynObj)
        {
            if (dynObj == null) return null;
            var dictionary = new Dictionary<string, object>();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynObj))
            {
                object obj = propertyDescriptor.GetValue(dynObj);
                dictionary.Add(propertyDescriptor.Name, obj);
            }
            return dictionary;
        }
    }
}
