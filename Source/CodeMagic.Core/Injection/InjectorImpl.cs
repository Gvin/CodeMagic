using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Injection.Configuration;

namespace CodeMagic.Core.Injection
{
    /// <summary>
    /// Represents dependency injector.
    /// </summary>
    public interface IInjector
    {
        /// <summary>
        /// Creates object of specified type according to injector configuration.
        /// </summary>
        /// <typeparam name="T">Type of object to be created.</typeparam>
        /// <returns>Instance of specified type initialized with all required parameters.</returns>
        T Create<T>(params object[] arguments) where T : IInjectable;
    }

    internal class InjectorImpl : IInjector, IDisposable
    {
        private readonly Dictionary<Type, InjectorMappingType> mapping;

        public InjectorImpl(IInjectorConfiguration initialMapping)
        {
            mapping = initialMapping.GetMapping().ToDictionary(pair => pair.Key, pair => pair.Value);
            mapping.Add(typeof(IInjector), new InjectorMappingType { Object = this });
        }

        public T Create<T>(params object[] arguments) where T : IInjectable
        {
            var type = typeof(T);
            return (T)CreateFromInterfaceType(type, arguments);
        }

        private object CreateFromInterfaceType(Type type, object[] arguments)
        {
            if (!type.IsInterface)
                throw new ApplicationException($"Interface type expected but got: {type.FullName}");

            if (!mapping.ContainsKey(type))
                throw new ApplicationException($"Mapping for type \"{type}\" not found.");

            var typeMapping = mapping[type];
            if (typeMapping.Type == null && typeMapping.Object == null && typeMapping.FactoryMethod == null)
                throw new ApplicationException(
                    $"Invalid type mapping for type \"{type.FullName}\": all configurations are null.");

            if (typeMapping.Type != null)
            {
                return CreateFromImplType(typeMapping.Type);
            }
            if (typeMapping.FactoryMethod != null)
            {
                return typeMapping.FactoryMethod(arguments);
            }

            return typeMapping.Object;
        }

        private object CreateFromImplType(Type type)
        {
            var constructors = type.GetConstructors();
            if (constructors.Length > 1)
                throw new ApplicationException($"Type {type.Name} contains several constructors.");

            var constructor = constructors.First();
            var parameters = constructor.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
            if (parameters.Length == 0)
            {
                return constructor.Invoke(new object[0]);
            }

            if (parameters.Any(parameter => !parameter.IsInterface))
            {
                throw new ApplicationException($"Constructor parameters for type \"{type.FullName}\" contains non-interface parameter(s).");
            }

            var parametersImpl = parameters.Select(parameterType => CreateFromInterfaceType(parameterType, new object[0])).ToArray();
            return constructor.Invoke(parametersImpl);
        }

        public void Dispose()
        {
            var objectMapping = mapping.Values.Where(mappingType => mappingType.Object != null)
                .Select(mappingType => mappingType.Object).OfType<IDisposable>().ToArray();
            foreach (var disposableObject in objectMapping)
            {
                if (disposableObject is InjectorImpl) // Skip circular self-disposing.
                    continue;

                disposableObject.Dispose();
            }

            mapping.Clear();
        }
    }
}