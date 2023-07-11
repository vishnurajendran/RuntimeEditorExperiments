using System;
using RuntimeEditorExperiment.Scripts.Runtime.Interfaces;
using UnityEngine;


namespace RuntimeEditorExperiment.Scripts.Runtime.Common
{
    public class RTGameObject : MonoBehaviour
    {
        public string RTID { get; private set; }
        private RTHierarchyVisible visible;
        
        public static RTGameObject NewInstance(string goName)
        {
            var go = new GameObject(goName);
            var instance =  go.AddComponent<RTGameObject>();
            instance.Init();
            return instance;
        }
        
        public static RTGameObject CreatePrimitive(string goName, PrimitiveType type)
        {
            var go = GameObject.CreatePrimitive(type);
            if(!string.IsNullOrEmpty(goName))
                go.name = goName;
            
            var instance =  go.AddComponent<RTGameObject>();
            instance.Init();
            return instance;
        }
        
        private void Init()
        {
            RTID = Guid.NewGuid().ToString();
            visible =  this.gameObject.AddComponent<RTHierarchyVisible>();
            RTCommon.Resolve<IRTHierarchy>()?.Add(visible);
        }

        private void OnDestroy()
        {
            if(visible != null)
                RTCommon.Resolve<IRTHierarchy>()?.Remove(visible);
        }
    }

}
