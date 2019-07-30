using System;

namespace CodeMagic.Core.Injection.Configuration
{
    /// <summary>
    /// Represents mapping details for specific type.
    /// Either Type or Object properties should be populated.
    /// </summary>
    public class InjectorMappingType
    {
        /// <summary>
        /// Gets or sets implementation type for mapping type-to-type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets implementation object for mapping type-to-object.
        /// </summary>
        public object Object { get; set; }

        public Func<object[], object> FactoryMethod { get; set; }
    }
}