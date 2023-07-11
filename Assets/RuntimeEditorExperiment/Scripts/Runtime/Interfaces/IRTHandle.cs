using UnityEngine;

namespace RuntimeEditorExperiment.Scripts.Runtime.Hierarchy
{
    public interface IRTHandle
    {
        public void SetHandleTarget(Transform target);
        public void Release();
    }
}