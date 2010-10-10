using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace StronglyTypedDependencyProperties.Helpers
{
    public static class TypeExtensions
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "type",
            Justification = "It's extenstion method on type, which allow syntax like typeof(...).GetPropertyName(...)")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static string GetPropertyName<T>(this Type type, Expression<Func<T, object>> property)
        {
            return TypeHelper.GetPropertyName(property);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "type",
            Justification = "It's extenstion method on type, which allow syntax like typeof(...).GetPropertyType(...)")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Type GetPropertyType<T>(this Type type, Expression<Func<T, object>> property)
        {
            return TypeHelper.GetPropertyType(property);
        }
    }
}