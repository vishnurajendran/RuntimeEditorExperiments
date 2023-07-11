using System;
using UnityEngine;

namespace RuntimeEditorExperiment.Scripts.Runtime.Function
{
    public class InPort : Port
    {
        private string key;
        private Action functionToInvoke;
        public override PortType Type => PortType.In;
        protected override string RenderAsset => "InPortRenderer";
        
        public InPort(GameObject target, string key, Action functionToInvoke)
        {
            this.key = key;
            this.targetObject = target.transform;
            this.functionToInvoke = functionToInvoke;
            SpawnRenderer();
            renderer.Init(this,target, key);
        }

        public void Invoke()
        {
            functionToInvoke?.Invoke();
        }
    }
}