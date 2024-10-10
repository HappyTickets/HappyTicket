using System.Reflection;

public static class ObjectExtensions
{
    public static T ToObject<T>(this IDictionary<string, string> source)
        where T : class, new()
    {
        var someObject = new T();
        var someObjectType = someObject.GetType();

        foreach (var item in source)
        {
            // Check if the property exists and is of type string or can accept a string
            var property = someObjectType.GetProperty(item.Key);
            if (property != null && property.CanWrite)
            {
                // Convert the string to the appropriate type if necessary
                var value = Convert.ChangeType(item.Value, property.PropertyType);
                property.SetValue(someObject, value, null);
            }
        }

        return someObject;
    }

    public static IDictionary<string, string> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
    {
        return source.GetType().GetProperties(bindingAttr).ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(source)?.ToString() // Ensure we convert to string
        );
    }
}
