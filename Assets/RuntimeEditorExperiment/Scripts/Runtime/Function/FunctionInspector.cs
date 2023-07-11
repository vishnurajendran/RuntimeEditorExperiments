using RuntimeEditorExperiment.Scripts.Runtime.Function;
using UnityEngine;
using UnityEngine.UI;

public class FunctionInspector : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text title;
    [SerializeField] private Button removeButton;

    private DefaultFunction functionInstance;
    public void Init(DefaultFunction functionInstance)
    {
        this.functionInstance = functionInstance;
        title.text = this.functionInstance.GetType().Name;
        removeButton.onClick.AddListener(() =>
        {
            Destroy(functionInstance);
            Destroy(this.gameObject);
        });
    }
}
