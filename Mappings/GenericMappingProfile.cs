using AutoMapper;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Microservices.Common.Mappings
{
    public partial class GenericMappingProfile : Profile
    {

        public GenericMappingProfile()
        {
            string serviceAssembly = Regex.Match(Assembly.GetExecutingAssembly().Location, "(\\w*).Api").Value;

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

        //[GeneratedRegex("(\\w*).Api")]
        //public static partial Regex ConsumerServicePattern();
    }
}