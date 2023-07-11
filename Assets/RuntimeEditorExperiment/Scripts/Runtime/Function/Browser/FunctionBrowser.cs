using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RuntimeEditorExperiment.Scripts.Runtime.Function;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FunctionBrowser : MonoBehaviour
{
    [SerializeField] private GameObject functionItem;
    [SerializeField] private Transform functionsParent;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private ToggleGroup toggleGroup;
    
    private Dictionary<string, Type> typeMap;
    private Dictionary<string, string> typeImgMap;
    private Type selectedType;
    
    public static void OpenBrowser(Action<Type> onSelect)
    {
        var go = Instantiate(Resources.Load<GameObject>("FunctionBrowser"));
        var browser = go.GetComponent<FunctionBrowser>();
        browser.Init(onSelect);
    }

    private void Init(Action<Type> onSelect)
    {
        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var allRTFunctions = 
            from assembly in allAssemblies
            from type in assembly.DefinedTypes
            where type.IsDefined(typeof(RTFunctionAttribute), false)
            select type;
            
        typeMap = new Dictionary<string, Type>();
        typeImgMap = new Dictionary<string, string>();
        foreach (var rtFunction in allRTFunctions)
        {
            typeMap.Add(rtFunction.Name, rtFunction);
            var attrib = rtFunction.GetAttribute(typeof(RTFunctionAttribute)) as RTFunctionAttribute;
            typeImgMap.Add(rtFunction.Name,attrib.IconResourcePath);
        }

        foreach (var kv in typeMap)
        {
            var func = Instantiate(functionItem, functionsParent);
            func.SetActive(true);
            func.GetComponentInChildren<TMPro.TMP_Text>().text = kv.Key;
            func.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(typeImgMap[kv.Key]);
            func.GetComponentInChildren<Toggle>().onValueChanged.AddListener((bool val) =>
            {
                var type = kv.Value;
                if (val)
                {
                    selectedType = type;
                }
                selectButton.interactable = toggleGroup.AnyTogglesOn();
            });
            selectButton.interactable = false;
        }
        
        selectButton.onClick.AddListener(() =>
        {
            if (!toggleGroup.AnyTogglesOn())
                return;

            onSelect?.Invoke(selectedType);
            Destroy(this.gameObject);
        });
        
        cancelButton.onClick.AddListener(() =>
        {
            Destroy(this.gameObject);
        });
        
    }

}
