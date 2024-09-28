using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Shared.Common.General
{
    public static class ObjectExtensions
    {
        public static object GetPropertyValue<T>(this T obj, string propertyName)
        {

            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            PropertyInfo property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'.");
            }

            return property.GetValue(obj);
        }
        public static T GetKeyValue<T>(this object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            // Look for the property with the [Key] attribute
            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), false);
                if (keyAttributes.Length > 0)
                {
                    return (T)property.GetValue(obj);
                }
            }

            // If no [Key] attribute is found, return the first property value
            if (properties.Length > 0)
            {
                return (T)properties[0].GetValue(obj);
            }

            throw new NullReferenceException("ID not found");
        }
        public static List<string> GetAllFieldNames<T>(this T obj)
        {
            var fieldNames = new List<string>();
            var type = obj.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                fieldNames.Add(field.Name);
            }

            return fieldNames;
        }
    }
}