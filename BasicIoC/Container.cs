using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicIoC
{
    public sealed class Container
    {
        private static readonly Lazy<Container> _lazy = new Lazy<Container>(() => new Container());

        public static Container Instance => _lazy.Value;

        private Dictionary<Type, Func<object>> _registeredTypes = new Dictionary<Type, Func<object>>();

        private Container() { }

        /// <summary>
        /// Registers an instantiate object with the IoC Container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="initiatedObject"></param>
        public void Register<T>(T initiatedObject)
        {
            this._registeredTypes.Add(typeof(T), () => initiatedObject);
        }

        /// <summary>
        /// Registers a func method which returns an object with the IoC Container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instantiateMethod"></param>
        public void Register<T>(Func<T> instantiateMethod)
        {
            this._registeredTypes.Add(typeof(T), () => instantiateMethod.Method);
        }

        /// <summary>
        /// Registers a type from another type.
        /// This will be either from a previously registered type,
        /// or the container will try to instantiate the type from registered types.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        public void Register<TIn, TOut>()
        {
            if (!this._registeredTypes.ContainsKey(typeof(TOut)))
            {
                _registeredTypes.Add(typeof(TOut), () => InstantiateService<TOut>());
            }

            this._registeredTypes.Add(typeof(TIn), this._registeredTypes[typeof(TOut)]);
        }
        

        private T InstantiateService<T>()
        {
            var constructor = typeof(T).GetConstructors()
                .FirstOrDefault(c => c.GetParameters().Length == 0);

            if (constructor != null)
                return (T) constructor.Invoke(null);

            constructor = typeof(T).GetConstructors()
                .FirstOrDefault(c =>
                    c.GetParameters().All(p => this._registeredTypes.ContainsKey(p.ParameterType)));

            if (constructor == null)
            {
                var requiredParameters = string.Join(", ", typeof(T).GetConstructors()
                    .FirstOrDefault()?.GetParameters()?.Select(p => p.Name)?.ToArray() ?? new string[]{});

                throw new ArgumentNullException(
                    $"Unable to find registered values for requested service input parameters!: {requiredParameters}");
            }

            var inputParameters = constructor.GetParameters();

            return (T) constructor.Invoke(_registeredTypes
                .Where(r => inputParameters.Select(p => p.ParameterType).Contains(r.Key))
                .Select(r => r.Value.Invoke())
                .ToArray());
        }

        /// <summary>
        /// Retrieves the object of type from the IoC container.
        /// This will be either from a previously registered type,
        /// or the container will try to instantiate the type from registered types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>()
        {
            if (_registeredTypes.ContainsKey(typeof(T)))
            {
                return (T) _registeredTypes[typeof(T)].Invoke();
            }
            else
            {
                return InstantiateService<T>();
            }
        }
    }
}
