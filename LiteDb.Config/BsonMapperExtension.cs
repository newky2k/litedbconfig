using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LiteDb.Config;

namespace LiteDB
{
    public static class BsonMapperExtension
    {
        private static readonly MethodInfo entityMethod = typeof(BsonMapper).GetTypeInfo().GetMethods().Single(x => (x.Name == "Entity") && (x.IsGenericMethod == true) && (x.GetParameters().Length == 0));

        private static Type FindEntityType(Type type)
        {
            var interfaceType = type.GetInterfaces().First(x => (x.GetTypeInfo().IsGenericType == true) && (x.GetGenericTypeDefinition() == typeof(IEntityBuilderConfiguration<>)));
            return interfaceType.GetGenericArguments().First();
        }

        private static readonly Dictionary<Assembly, IEnumerable<Type>> typesPerAssembly = new Dictionary<Assembly, IEnumerable<Type>>();

        public static BsonMapper ApplyConfiguration<T>(this BsonMapper modelBuilder, IEntityBuilderConfiguration<T> configuration) where T : class
        {
            var entityType = FindEntityType(configuration.GetType());

            dynamic entityTypeBuilder = entityMethod
                .MakeGenericMethod(entityType)
                .Invoke(modelBuilder, new object[0]);

            configuration.Configure(entityTypeBuilder);

            return modelBuilder;
        }

        public static BsonMapper ApplyConfigurationsFromAssembly(this BsonMapper mapper, Assembly asm)
        {
            IEnumerable<Type> configurationTypes;

            if (typesPerAssembly.TryGetValue(asm, out configurationTypes) == false)
            {
                typesPerAssembly[asm] = configurationTypes = asm
                    .GetTypes()
                    .Where(x => (x.GetTypeInfo().IsClass == true) && (x.GetTypeInfo().IsAbstract == false) && (x.GetInterfaces().Any(y => (y.GetTypeInfo().IsGenericType == true) && (y.GetGenericTypeDefinition() == typeof(IEntityBuilderConfiguration<>)))));
            }

            var configurations = configurationTypes.Select(x => Activator.CreateInstance(x));

            foreach (dynamic configuration in configurations)
            {
                ApplyConfiguration(mapper, configuration);
            }

            return mapper;
        }
    }
}
