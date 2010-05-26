using System;
using System.Linq.Expressions;
using System.Windows;

namespace StronglyTypedDependencyProperties.Helpers
{
    public static class DependencyPropertyHelper
    {
        public static DependencyProperty Register<TOwner>(
            Expression<Func<TOwner, object>> property)
        {
            return DependencyProperty.Register(typeof(TOwner).GetPropertyName(property),
                                               typeof(TOwner).GetPropertyType(property),
                                               typeof(TOwner));
        }

        public static DependencyProperty Register<TOwner>(
            Expression<Func<TOwner, object>> property, PropertyMetadata metadata)
        {
            return DependencyProperty.Register(typeof(TOwner).GetPropertyName(property),
                                               typeof(TOwner).GetPropertyType(property),
                                               typeof(TOwner),
                                               metadata);
        }

        public static DependencyProperty Register<TOwner>(
            Expression<Func<TOwner, object>> property,
            PropertyMetadata metadata,
            ValidateValueCallback validateValueCallback)
        {
            return DependencyProperty.Register(typeof(TOwner).GetPropertyName(property),
                                               typeof(TOwner).GetPropertyType(property),
                                               typeof(TOwner),
                                               metadata,
                                               validateValueCallback);
        }

        public static DependencyPropertyKey RegisterReadOnly<TOwner>(
            Expression<Func<TOwner, object>> property, PropertyMetadata metadata)
        {
            return DependencyProperty.RegisterReadOnly(typeof(TOwner).GetPropertyName(property),
                                                       typeof(TOwner).GetPropertyType(property),
                                                       typeof(TOwner),
                                                       metadata);
        }

        public static DependencyPropertyKey RegisterReadOnly<TOwner>(
            Expression<Func<TOwner, object>> property,
            PropertyMetadata metadata,
            ValidateValueCallback validateValueCallback)
        {
            return DependencyProperty.RegisterReadOnly(typeof(TOwner).GetPropertyName(property),
                                                       typeof(TOwner).GetPropertyType(property),
                                                       typeof(TOwner),
                                                       metadata,
                                                       validateValueCallback);
        }
    }
}