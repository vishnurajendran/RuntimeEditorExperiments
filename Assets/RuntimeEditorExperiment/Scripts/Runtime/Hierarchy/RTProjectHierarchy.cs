using System;
using System.Collections.Generic;
using RuntimeEditorExperiment.Scripts.Runtime.Common;
using RuntimeEditorExperiment.Scripts.Runtime.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RuntimeEditorExperiment.Scripts.Runtime.Hierarchy
{
    public class RTProjectHierarchy : MonoBehaviour, IRTHierarchy
    {
        private Dictionary<RTHierarchyVisible, ProjectHierarchyItem> map;

        [SerializeField] private Transform parent;
        [SerializeField] private GameObject itemRef;
        
        private void Awake()
        {
            map = new Dictionary<RTHierarchyVisible, ProjectHierarchyItem>();
            RTCommon.RegisterInstance<IRTHierarchy>(this);
        }

        private void Start()
        {
            RTCommon.Resolve<IRTSceneView>().OnSelectedFromScene += OnSelectedFromScene;
        }

        private void OnDestroy()
        {
            RTCommon.DeRegisterInstance<IRTHierarchy>(this);
            
            var scene = RTCommon.Resolve<IRTSceneView>();
            if(scene != null)
                scene.OnSelectedFromScene -= OnSelectedFromScene;
        }

        private void OnSelectedFromScene(Transform target)
        {
            var visible = target.GetComponent<RTHierarchyVisible>();
            if (visible == null)
                return;
            if (!map.ContainsKey(visible))
                return;
            map[visible].SelectWithoutNotify();
        }
        
        public void Add(RTHierarchyVisible visible)
        {
            AddToMap(visible);
        }
        
        public void Remove(RTHierarchyVisible visible)
        {
            RemoveFromMap(visible);
        }

        private void AddToMap(RTHierarchyVisible visible)
        {
            var go = Instantiate(itemRef, parent);
            var instance = go.GetComponent<ProjectHierarchyItem>();
            map.Add(visible, instance);
            go.SetActive(true);
            instance.Init(visible, OnItemSelected);
        }
        
        private void RemoveFromMap(RTHierarchyVisible visible)
        {
            if (map.ContainsKey(visible))
            {
                var item = map[visible];

                if (RTCommon.Resolve<IRTSceneView>()?.CurrentSelection == item.transform)
                {
                    RTCommon.Resolve<IRTSceneView>()?.SetSelection(null);
                }
                
                Destroy(item.gameObject);
                map.Remove(visible);
            }
        }

        private void OnItemSelected(RTHierarchyVisible selected)
        {
            RTCommon.Resolve<IRTSceneView>()?.SetSelection(selected.transform);
        }
    }
}
