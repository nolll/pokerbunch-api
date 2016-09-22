using System;
using System.Collections.Generic;
using Moq;

namespace Tests.Common
{
    public class Mocker : MockerBase
    {
        readonly Dictionary<Type, Mock> _mocks = new Dictionary<Type, Mock>();

        public Mock<T> MockOf<T>() where T : class => _mocks[typeof(T)] as Mock<T>;

        protected override object InterfaceBuilder(Type type)
        {
            var mock = (Mock)Activator.CreateInstance(typeof(Mock<>).MakeGenericType(type));
            _mocks.Add(type, mock);
            return mock.Object;
        }
    }

    public abstract class MockerBase
    {
        protected abstract object InterfaceBuilder(Type type);

        public T New<T>(Dictionary<string, object> ctorParams = null, int ctorIndex = 0)
        {
            ctorParams = ctorParams ?? new Dictionary<string, object>();
            var classCtor = typeof(T).GetConstructors()[ctorIndex];
            var ctor = new List<object>();

            foreach (var param in classCtor.GetParameters())
                ctor.Add(ctorParams.ContainsKey(param.Name)
                    ? ctorParams[param.Name]
                    : Create(param.ParameterType));

            return (T)Activator.CreateInstance(typeof(T), ctor.ToArray());
        }

        object Create(Type type) => type.IsValueType
            ? Activator.CreateInstance(type)
            : (type.IsInterface
                ? InterfaceBuilder(type)
                : null);
    }
}
