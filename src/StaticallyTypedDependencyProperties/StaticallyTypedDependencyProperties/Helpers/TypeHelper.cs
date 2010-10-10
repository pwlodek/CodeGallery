using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace StronglyTypedDependencyProperties.Helpers
{
    public static class TypeHelper
    {
        private static PropertyInfo GetPropertyInternal(LambdaExpression propertyAccessor)
        {
            MemberExpression memberExpression;

            if (propertyAccessor.Body is UnaryExpression)
            {
                var ue = (UnaryExpression)propertyAccessor.Body;
                memberExpression = (MemberExpression)ue.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)propertyAccessor.Body;
            }

            return (PropertyInfo)(memberExpression).Member;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Adds static compile-time checking to expression")]
        public static Type GetPropertyType<TObject>(Expression<Func<TObject, object>> propertyAccessor)
        {
            return GetPropertyInternal(propertyAccessor).PropertyType;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Adds static compile-time checking to expression")]
        public static string GetPropertyName<TObject>(Expression<Func<TObject, object>> propertyAccessor)
        {
            return GetPropertyInternal(propertyAccessor).Name;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Adds static compile-time checking to expression")]
        public static string GetPropertyName<TObject, T>(Expression<Func<TObject, T>> propertyAccessor)
        {
            return GetPropertyInternal(propertyAccessor).Name;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Adds static compile-time checking to expression")]
        public static string GetPropertyName<T>(Expression<Func<T>> propertyAccessor)
        {
            return GetPropertyInternal(propertyAccessor).Name;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Adds static compile-time checking to expression")]
        public static PropertyInfo GetProperty<TObject>(Expression<Func<TObject, object>> propertyAccessor)
        {
            return GetPropertyInternal(propertyAccessor);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Adds static compile-time checking to expression")]
        public static PropertyInfo GetProperty<TObject, T>(Expression<Func<TObject, T>> propertyAccessor)
        {
            return GetPropertyInternal(propertyAccessor);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Adds static compile-time checking to expression")]
        public static PropertyInfo GetProperty<T>(Expression<Func<T>> propertyAccessor)
        {
            return GetPropertyInternal(propertyAccessor);
        }
    }
}