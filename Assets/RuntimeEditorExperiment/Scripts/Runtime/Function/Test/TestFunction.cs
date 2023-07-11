using System;
using UnityEngine;

namespace RuntimeEditorExperiment.Scripts.Runtime.Function.Test
{
    [RTFunction(IconResourcePath = "prototype")]
    public class TestFunction : DefaultFunction
    {
        private void Awake()
        {
            RegisterAction("test action", Test);
            RegisterTrigger("test trigger");
        }

        private void Test()
        {
            Debug.Log("Test");
        }
    }
}