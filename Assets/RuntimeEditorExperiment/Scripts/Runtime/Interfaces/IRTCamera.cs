using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeEditorExperiment.Scripts.Runtime.Interfaces
{
    public interface IRTCamera
    {
        public Camera HandlesCamera { get; }
        public Camera MainCamera { get; }
        public Camera FuncitonsCamera { get; }
    }
}

