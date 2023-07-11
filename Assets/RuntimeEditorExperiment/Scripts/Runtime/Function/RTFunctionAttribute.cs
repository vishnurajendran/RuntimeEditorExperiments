using System;

namespace RuntimeEditorExperiment.Scripts.Runtime.Function
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RTFunctionAttribute : Attribute
    {
        public string IconResourcePath = "function_default";
    }
}