using System;
using UnityEngine;

namespace RuntimeEditorExperiment.Scripts.Runtime.Interfaces
{
    public interface IRTSceneView
    {
        public Action<Transform> OnSelectedFromScene { get; set; }
        public Transform CurrentSelection { get; }
        public void SetSelection(Transform target);
    }
}