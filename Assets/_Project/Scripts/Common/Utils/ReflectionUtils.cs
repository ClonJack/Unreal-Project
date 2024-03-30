using System;
using System.Reflection;
using UnityEngine;

namespace UnrealTeam.SB.Common.Utils
{
    public static class ReflectionUtils
    {
        public static object GetFieldValue(object instance, string fieldName, Type type)
        {
            return type
                !.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                !.GetValue(instance);
        }

        public static object GetFieldValue(object instance, string fieldName)
        {
            return instance.GetType()
                !.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                !.GetValue(instance);
        }

        public static void SetPropertyValue(object instance, string propertyName, object value)
        {
            instance.GetType()
                !.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
                !.GetSetMethod(nonPublic: true)
                .Invoke(instance, new[] { value });
        }

        public static void SetFieldValue(object instance, string fieldName, object value)
        {
            instance.GetType()
                !.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                !.SetValue(instance, value);
        }

        public static void SetFieldValue(object instance, string fieldName, object value, Type type)
        {
            type
                !.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                !.SetValue(instance, value);
        }
    }
}