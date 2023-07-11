using System;
using System.Collections;
using System.Collections.Generic;
using RuntimeEditorExperiment.Scripts.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

public class ProjectHierarchyItem : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text nameText;
    [SerializeField] private RTHierarchyVisible target;
    [SerializeField] private Toggle toggle;

    private Action<RTHierarchyVisible> onSelect;
    
    public void Init(RTHierarchyVisible target, Action<RTHierarchyVisible> onSelect)
    {
        this.target = target;
        this.onSelect = onSelect;
        toggle.onValueChanged.AddListener(OnToggle);
    }

    private void LateUpdate()
    {
        if (target == null)
            return;
        
        nameText.text = target.name;
    }

    public void SelectWithoutNotify()
    {
        toggle.SetIsOnWithoutNotify(true);
    }
    
    private void OnToggle(bool toggled)
    {
        if (toggled)
        {
            onSelect?.Invoke(target);
        }
    }
    
}
