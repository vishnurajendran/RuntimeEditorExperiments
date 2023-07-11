using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace RuntimeEditorExperiment.Scripts.Runtime.Function
{
    public abstract class Port
    {
        [SerializeField]
        public enum PortType
        {
            In,
            Out
        }
        
        protected string key;
        protected PortRenderer renderer;
        
        private Port connectedPort;
        protected Transform targetObject;

        public bool IsConnected => connectedPort != null;
        protected abstract string RenderAsset { get; }
        
        public abstract PortType Type { get; }
        public Vector3 RendererPosition => targetObject == null ? Vector3.zero : renderer.transform.position;
        public Port ConnectedPort => connectedPort;

        protected void SpawnRenderer()
        {
            Debug.Log(RenderAsset);
            var go = GameObject.Instantiate(Resources.Load<GameObject>(RenderAsset));
            renderer = go.GetComponent<PortRenderer>();
        }
        
        public void Show()
        {
            if (renderer != null)
            {
                renderer.gameObject.SetActive(true);
            }
        }
        
        public void Release()
        {
            if (renderer != null)
            {
                GameObject.Destroy(renderer.gameObject);
            }
        }

        private void ConnectToPort(Port port)
        {
            if (Type == PortType.In)
            {
                connectedPort = port;
            }
            else
            {
                connectedPort = port;
            }
        }
        
        #region Statics

        public static bool CanConnect(Port a, Port b)
        {
            var res = a.targetObject != b.targetObject && a.Type != b.Type;
            Debug.Log($"Can Connect? a <---> b {res}");
            return res;
        }
        
        public static void Connect(Port a, Port b)
        {
           a.ConnectToPort(b);
           b.ConnectToPort(a);
        }
        
        #endregion
    }
}