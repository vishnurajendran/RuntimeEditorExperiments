using System;
using RuntimeEditorExperiment.Scripts.Runtime.Common;
using RuntimeEditorExperiment.Scripts.Runtime.Hierarchy;
using RuntimeEditorExperiment.Scripts.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RuntimeEditorExperiment.Scripts.Runtime.SceneView
{
    public class ProjectSceneView : MonoBehaviour, IRTSceneView
    {
        private const string MOVE_HANDLE_RES_PATH = "MoveHandle";
        private const string ROTATE_HANDLE_RES_PATH = "RotateHandle";
        private const string SCALE_HANDLE_RES_PATH = "ScaleHandle";

        [SerializeField]
        private LayerMask selectionRayMask;
        
        private enum ToolType
        {
            Select,
            Move,
            Rotate,
            Scale
        }

        private ToolType toolType = ToolType.Select;
        private IRTHandle currentActiveHandle;
        private Transform currentSelection;

        public Action<Transform> OnSelectedFromScene { get; set; }
        public Transform CurrentSelection => currentSelection;
        
        private void Awake()
        {
            RTCommon.RegisterInstance<IRTSceneView>(this);
        }

        private void OnDestroy()
        {
            RTCommon.DeRegisterInstance<IRTSceneView>(this);
        }

        private void ReleaseCurrentHandle()
        {
            if(currentActiveHandle != null)
                currentActiveHandle.Release();
            
            currentActiveHandle = null;

        }
        
        public void SetToSelectTool()
        {
            toolType = ToolType.Select;
            SetSelection(currentSelection);
        }
        
        public void SetToMoveTool()
        {
            toolType = ToolType.Move;
            SetSelection(currentSelection);
        }
        
        public void SetToRotateTool()
        {
            toolType = ToolType.Rotate;
            SetSelection(currentSelection);
        }
        
        public void SetToScaleTool()
        {
            toolType = ToolType.Scale;
            SetSelection(currentSelection);
        }

        public void AddCubePrimitive()
        {
            AddPrimitive(PrimitiveType.Cube);
        }
        
        public void AddSpherePrimitive()
        {
            AddPrimitive(PrimitiveType.Sphere);
        }
        
        public void AddCylinderPrimitive()
        {
            AddPrimitive(PrimitiveType.Cylinder);
        }

        public void AddCapsulePrimitive()
        {
            AddPrimitive(PrimitiveType.Capsule);
        }
        
        public void AddPlanePrimitive()
        {
            AddPrimitive(PrimitiveType.Plane);
        }

        private void AddPrimitive(PrimitiveType type)
        {
            RTGameObject.CreatePrimitive(string.Empty, type);
        }
        
        public void SetSelection(Transform target)
        {
            ReleaseCurrentHandle();
            this.currentSelection = target;
            CreateHandle(target);
            RTCommon.Resolve<IRTInspector>()?.Inspect(target);
        }

        private void CreateHandle(Transform target)
        {
            if (target == null)
                return;
            
            string handlePrefab;
            switch (toolType)
            {
                case ToolType.Move:
                    handlePrefab = MOVE_HANDLE_RES_PATH;
                    break;
                
                case ToolType.Rotate:
                    handlePrefab = ROTATE_HANDLE_RES_PATH;
                    break;
                
                case ToolType.Scale:
                    handlePrefab = SCALE_HANDLE_RES_PATH;
                    break;
                
                default:
                    handlePrefab = string.Empty;
                    break;
            }

            if (string.IsNullOrEmpty(handlePrefab))
                return;

            GameObject go = Instantiate(Resources.Load<GameObject>(handlePrefab));
            currentActiveHandle = go.GetComponent<IRTHandle>();
            currentActiveHandle.SetHandleTarget(target);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = RTCommon.Resolve<IRTCamera>().MainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectionRayMask))
                {
                    if (hit.collider == null)
                        return;

                    var go = hit.collider.gameObject;
                    var rtgo = go.GetComponentInChildren<RTGameObject>();
                    if(rtgo == null)
                        rtgo = go.GetComponentInParent<RTGameObject>();
                    
                    if (rtgo != null && currentSelection != rtgo.transform)
                    {
                        SetSelection(rtgo.transform);
                        OnSelectedFromScene?.Invoke(rtgo.transform);
                    }
                }
            }
        }
    }
}