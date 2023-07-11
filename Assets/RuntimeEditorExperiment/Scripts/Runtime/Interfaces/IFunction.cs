namespace RuntimeEditorExperiment.Scripts.Runtime.Interfaces
{
    public interface IFunction
    {
        public string[] AvailableTriggers { get; }
        public string[] AvailableActions { get; }
    }
}