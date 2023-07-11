using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeEditorExperiment.Scripts.Runtime.Interfaces;
using UnityEngine;

namespace RuntimeEditorExperiment.Scripts.Runtime.Function
{
    public abstract class DefaultFunction:MonoBehaviour, IFunction
    {
        private Dictionary<string, InPort> inputPorts;
        private Dictionary<string, OutPort> outputPorts;

        private MeshRenderer renderer;
        
        public string[] AvailableTriggers
        {
            get
            {
                if (outputPorts == null)
                    return new string[]{};

                return outputPorts.Keys.ToArray();
            }
        }
        
        public string[] AvailableActions {
            get
            {
                if (inputPorts == null)
                    return new string[]{};

                return inputPorts.Keys.ToArray();
            }
        }
        
        protected void RegisterAction(string actionKey, Action action)
        {
            if (inputPorts == null)
                inputPorts = new Dictionary<string, InPort>();

            if (inputPorts.ContainsKey(actionKey))
                return;
            
            inputPorts.Add(actionKey, new InPort(this.gameObject,actionKey,action));
        }
        
        protected void RegisterTrigger(string actionKey)
        {
            if (outputPorts == null)
                outputPorts = new Dictionary<string, OutPort>();

            if (outputPorts.ContainsKey(actionKey))
                return;
            
            outputPorts.Add(actionKey, new OutPort(this.gameObject,actionKey));
        }

        private void OnDestroy()
        {
            if (inputPorts != null)
            {
                foreach (var kv in inputPorts)
                {
                    kv.Value.Release();
                }
            }
            
            if (outputPorts != null)
            {
                foreach (var kv in outputPorts)
                {
                    kv.Value.Release();
                }
            }
        }
    }
}