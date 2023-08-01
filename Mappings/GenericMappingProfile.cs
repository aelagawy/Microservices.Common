using AutoMapper;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Microservices.Common.Mappings
{
    public partial class GenericMappingProfile : Profile
    {

        public GenericMappingProfile()
        {
            string serviceAssembly = ConsumerServicePattern().Match(Assembly.GetExecutingAssembly().Location).Value;

            ApplyMappingsFromAssembly(Assembly.Load(serviceAssembly));
            //todo: ignore attribute globaly
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping")
                    ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] { this });

            }
        }

        [GeneratedRegex("(\\w*).Api")]
        private static partial Regex ConsumerServicePattern();
    }
}