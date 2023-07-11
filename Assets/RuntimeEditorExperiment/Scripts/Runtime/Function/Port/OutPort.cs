using UnityEngine;

namespace RuntimeEditorExperiment.Scripts.Runtime.Function
{
    public class OutPort:Port
    {
        private InPort NextPort;
        public override PortType Type => PortType.Out;
        protected override string RenderAsset => "OutPortRenderer";
        public OutPort(GameObject target, string key)
        {
            this.key = key;
            this.targetObject = target.transform;
            SpawnRenderer();
            renderer.Init(this,target, key);
        }
        
        public void TriggerPort()
        {
            
        }

        
    }
}