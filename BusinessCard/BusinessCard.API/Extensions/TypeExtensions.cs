namespace BusinessCard.API.Extensions;

public static class TypeExtensions
{
    public static string GetGenericTypeName(this Type type)
    {
        var typeName = string.Empty;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }

    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }
    public static Guid ToGuid(this string str)
    {
        if (!Guid.TryParse(str, out var guid)) throw new ArgumentException($"{str} is not a valid Guid.");
        return guid;
    }
    
}
