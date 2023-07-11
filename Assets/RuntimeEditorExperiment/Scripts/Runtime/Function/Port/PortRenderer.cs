using System;
using RuntimeEditorExperiment.Scripts.Runtime.Common;
using RuntimeEditorExperiment.Scripts.Runtime.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RuntimeEditorExperiment.Scripts.Runtime.Function
{
    public class PortRenderer:MonoBehaviour
    {
        
        [SerializeField] private TMP_Text title;
        [SerializeField] private float scaleRatio = 0.15f;
        [SerializeField] private float defaultDistanceFromCentre=1.75f;
        [SerializeField] private LayerMask selectionLayer;
        [SerializeField] private GameObject selectedIcon;
        
        [SerializeField] private LineRenderer connectionLine;
        [SerializeField] private float lineScaleRatio = 1;
        
        private Camera functionCamera;
        private GameObject target;
        private float distanceFromCentre;
        private Port port;

        private static bool draggingFromPort;
        private static Port portContext;
        private float ogMultiplier;

        private MeshRenderer meshRenderer;
        
        public void Init(Port targetPort, GameObject targetToLatchTo, string title)
        {
            this.port = targetPort;
            this.title.text = title;
            this.target = targetToLatchTo;
            functionCamera = RTCommon.Resolve<IRTCamera>()?.FuncitonsCamera;
            ogMultiplier = connectionLine.widthMultiplier;
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshRenderer == null)
                meshRenderer = GetComponentInParent<MeshRenderer>();

            if (meshRenderer == null)
                distanceFromCentre = defaultDistanceFromCentre;
            else
            {
                distanceFromCentre = Vector3.Distance(meshRenderer.bounds.extents, meshRenderer.bounds.center)/2;
            }
            
        }

        private void Update()
        {
            if (target == null)
                return;
            
            var dir = (transform.position - functionCamera.transform.position ).normalized;
            var posDirMultiplier = 1;
            if (port.Type == Port.PortType.In)
            {
                posDirMultiplier = -1;
            }

            transform.position = target.transform.position + target.transform.forward * (distanceFromCentre * posDirMultiplier);
            
            if(functionCamera == null)
                return;
            
            transform.forward = dir;
            transform.localScale = Vector3.one * (Vector3.Distance(transform.position, functionCamera.transform.position) * scaleRatio);
            connectionLine.widthMultiplier = ogMultiplier * Vector3.Distance(transform.position, functionCamera.transform.position) * lineScaleRatio;
            var ray = functionCamera.ScreenPointToRay(Input.mousePosition);
            
            var mousePos = Input.mousePosition;
            var camToMeDist = Vector3.Distance(transform.position, functionCamera.transform.position);
            var worldMousePos =
                functionCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y,camToMeDist ));
            
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit, Mathf.Infinity, selectionLayer);
            bool overPort = hasHit && hit.collider != null && hit.collider.GetComponent<PortRenderer>() == this;
            
            if(!port.IsConnected)
                selectedIcon.SetActive(overPort || draggingFromPort && portContext == port);
            else
            {
                connectionLine.positionCount = 2;
                connectionLine.SetPosition(0, transform.position);
                connectionLine.SetPosition(1, port.ConnectedPort.RendererPosition);
            }
            
            if(draggingFromPort)
            {
                if (portContext == port)
                {
                    connectionLine.SetPosition(1,worldMousePos);
                }
            }
            
            if (Input.GetMouseButton(0))
            {
                if (!overPort)
                    return;
                
                if (!draggingFromPort)
                {
                    draggingFromPort = true;
                    portContext = port;
                    connectionLine.positionCount = 2;
                    connectionLine.SetPosition(0, transform.position);
                }
            }
            else
            {
                if(draggingFromPort && portContext != port && overPort && Port.CanConnect(port, portContext))
                {
                    Port.Connect(port, portContext);
                    draggingFromPort = false;
                    portContext = null;
                    Debug.Log("PORT CONNECTED!!");
                    return;
                }
                
                if (!port.IsConnected && draggingFromPort && portContext == port)
                {
                    draggingFromPort = false;
                    portContext = null;
                    connectionLine.positionCount = 0;
                    Debug.Log("PORT CONNECTION CANCELLED!!");
                }
            }
        }
    }
}