using System.Collections;
using System.Collections.Generic;
using RuntimeEditorExperiment.Scripts.Runtime.Common;
using UnityEngine;

namespace RuntimeEditorExperiment.Scripts.Runtime.Interfaces
{
    public interface IRTHierarchy
    {
        public void Add(RTHierarchyVisible visible);
        public void Remove(RTHierarchyVisible visible);
    }
}
