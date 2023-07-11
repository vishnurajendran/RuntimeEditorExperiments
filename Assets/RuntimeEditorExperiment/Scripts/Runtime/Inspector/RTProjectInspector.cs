using System;
using RuntimeEditorExperiment.Scripts.Runtime.Common;
using RuntimeEditorExperiment.Scripts.Runtime.Function;
using RuntimeEditorExperiment.Scripts.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeEditorExperiment.Scripts.Runtime.Inspector
{
    
    public class RTProjectInspector : MonoBehaviour, IRTInspector
    {
        private const string FUNC_INSPECTOR_ASSET = "FunctionInspector";
        
        [SerializeField] private RTTRFInspector trfInspector;
        [SerializeField] private Transform functionInspectorParent;
        [SerializeField] private Button addFunctionButton;
        private Transform target;
        private void Awake()
        {
            addFunctionButton.gameObject.SetActive(false);
            addFunctionButton.onClick.AddListener(() =>
            {
                FunctionBrowser.OpenBrowser(OnFunctionSelected);
            });
            RTCommon.RegisterInstance<IRTInspector>(this);
        }

        private void OnFunctionSelected(Type functionType)
        {
            if (target == null)
                return;
            var instance = target.gameObject.AddComponent(functionType) as DefaultFunction;
            SpawnFunctionInspector(instance);
        }

        private void SpawnFunctionInspector(DefaultFunction instance)
        {
            var go = Instantiate(Resources.Load<GameObject>(FUNC_INSPECTOR_ASSET), functionInspectorParent);
            var funcInspector = go.GetComponent<FunctionInspector>();
            funcInspector.Init(instance);
            addFunctionButton.transform.SetAsLastSibling();
        }
        
        private void OnDestroy()
        {
            RTCommon.DeRegisterInstance<IRTInspector>(this);
        }

        public void Inspect(Transform target)
        {
            this.target = target;
            addFunctionButton.gameObject.SetActive(target != null);
            GenerateInspector();
        }

        private void GenerateInspector()
        {
            trfInspector.Inspect(target);
        }
    }
}