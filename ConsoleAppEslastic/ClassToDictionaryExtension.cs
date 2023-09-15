using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using System.Reflection;

namespace ConsoleAppEslastic
{
    public static class ClassToDictionaryExtension
    {
        public static Dictionary<string, Elastic.Clients.Elasticsearch.Mapping.Properties> ToDictionary(this object obj)
        {
            var result = new Dictionary<string, Elastic.Clients.Elasticsearch.Mapping.Properties>();

            PropertyInfo[] properties = obj.GetType().GetProperties();

            //foreach (var property in properties)
            //{
            //    var typeMapping = new TypeMapping
            //    {
                
            //    };
            //    var value = property.GetValue(property.Name);
            //    result.Add(property.Name, value);
            //    result[property.Name] = new Elastic.Clients.Elasticsearch.Mapping.Properties(property.GetValue(obj));
            //}

            return result;
        }
    }
}

