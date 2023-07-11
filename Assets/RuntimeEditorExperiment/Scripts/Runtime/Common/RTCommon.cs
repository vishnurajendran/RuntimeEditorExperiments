using System;
using System.Collections.Generic;

namespace RuntimeEditorExperiment.Scripts.Runtime.Common
{
    public static class RTCommon
    {
        private static Dictionary<Type, object> instanceMap;
        public static void RegisterInstance<T>(T instance)
        {
            if (instanceMap == null)
                instanceMap = new Dictionary<Type, object>();
            
            var type = typeof(T);
            if (instanceMap.ContainsKey(type))
            {
                return;
            }
            
            instanceMap.Add(type, instance);
        }
        
        public static void DeRegisterInstance<T>(T instance)
        {
            if (instanceMap == null)
                return;
            
            var type = typeof(T);
            if (instanceMap.ContainsKey(type))
            {
                instanceMap.Remove(type);
            }
        }

        public static T Resolve<T>()
        {
            var type = typeof(T);
            if (instanceMap == null || !instanceMap.ContainsKey(type))
                return default;

            return (T)instanceMap[type];
        }
    }
}