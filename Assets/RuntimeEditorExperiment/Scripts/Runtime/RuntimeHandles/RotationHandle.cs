using RuntimeEditorExperiment.Scripts.Runtime.Common;
using RuntimeEditorExperiment.Scripts.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

namespace RuntimeEditorExperiment.Scripts.Runtime.Hierarchy
{
    public class RotationHandle : MonoBehaviour, IRTHandle
    {
        [SerializeField] private float scaleRatio = 1;
        [SerializeField] private LayerMask rayMask;
        [SerializeField] private float resolutionMultiplier = 1.5f;
        private Camera handlesCamera;
        private Transform handleTarget;
        private bool handleLocked = false;

        private Vector3 mouseCurrPos;
        private Vector3 motionMask;
        
        private void Start()
        {
            handlesCamera = RTCommon.Resolve<IRTCamera>()?.HandlesCamera;
        }
        
        public void SetHandleTarget(Transform target)
        {
            this.handleTarget = target;
        }

        public void Release()
        {
            Destroy(this.gameObject);
        }

        private void Update()
        {
            if (handlesCamera == null)
                return;

            if (handleTarget != null)
                transform.position = handleTarget.position;

            transform.rotation = handleTarget.rotation;
            transform.localScale = Vector3.one * (Vector3.Distance(transform.position, handlesCamera.transform.position) * scaleRatio);

            TryManipulate();
        }

        private void TryManipulate()
        {
            var ray = handlesCamera.ScreenPointToRay(Input.mousePosition);
            if (!Input.GetMouseButton(0))
            {
                handleLocked = false;
                return;
            }

            if (handleLocked)
            {
                DoHandleMove();
                return;
            }
            
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit,Mathf.Infinity, rayMask))
            {
                if (hit.collider == null)
                {
                    handleLocked = false;
                    return;
                }

                var mousePosition = new Vector3(Input.mousePosition.x, -Input.mousePosition.y,
                    handlesCamera.nearClipPlane);
                var worldMousePos = handlesCamera.ScreenToWorldPoint(mousePosition);

                mouseCurrPos = worldMousePos;
                motionMask = GetMotionMask(hit.collider.name);
                handleLocked = true;
            }
            else
            {
                handleLocked = false;
            }
        }

        private void DoHandleMove()
        {
            if (!handleLocked)
                return;
            
            var mousePosition = new Vector3(-Input.mousePosition.x, Input.mousePosition.y,
                handlesCamera.nearClipPlane);
            var newMousePos = handlesCamera.ScreenToWorldPoint(mousePosition);

            var delta = (mouseCurrPos - newMousePos).normalized;;
            var resolution = 1;
            handleTarget.Rotate((Vector3.Scale(motionMask, delta) * (resolution * resolutionMultiplier)));
            mouseCurrPos = newMousePos;
        }
        
        private Vector3 GetMotionMask(string axisName)
        {
            switch (axisName.ToLower())
            {
                case "x": return Vector3.right;
                case "y": return Vector3.up;
                case "z": return Vector3.forward;
                default: return Vector3.zero;
            }
        }
    }

}
