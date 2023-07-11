using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace RuntimeEditorExperiment.Scripts.Runtime.Inspector
{
    public class RTTRFInspector:MonoBehaviour
    {
        [SerializeField]
        private Transform body;
        
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private TMP_InputField posX;
        [SerializeField] private TMP_InputField posY;
        [FormerlySerializedAs("posz")] [SerializeField] private TMP_InputField posZ;
        
        [SerializeField] private TMP_InputField rotX;
        [SerializeField] private TMP_InputField rotY;
        [FormerlySerializedAs("rotz")] [SerializeField] private TMP_InputField rotZ;
        
        [SerializeField] private TMP_InputField scaleX;
        [SerializeField] private TMP_InputField scaleY;
        [FormerlySerializedAs("scalez")] [SerializeField] private TMP_InputField scaleZ;

        private Transform activeTarget;

        private void Start()
        {
            nameField.onValueChanged.AddListener(UpdateName);
            posX.onValueChanged.AddListener(UpdatePosX);
            posY.onValueChanged.AddListener(UpdatePosY);
            posZ.onValueChanged.AddListener(UpdatePosZ);
            
            rotX.onValueChanged.AddListener(UpdateRotX);
            rotY.onValueChanged.AddListener(UpdateRotY);
            rotZ.onValueChanged.AddListener(UpdateRotZ);
            
            scaleX.onValueChanged.AddListener(UpdateScaleX);
            scaleY.onValueChanged.AddListener(UpdateScaleY);
            scaleZ.onValueChanged.AddListener(UpdateScaleZ);
        }

        private void UpdateName(string value)
        {
            if (activeTarget == null)
                return;
            activeTarget.name = value;
        }

        private void UpdatePosX(string value)
        {
            if (activeTarget == null)
                return;
            activeTarget.position = new Vector3(float.Parse(value), activeTarget.position.y, activeTarget.position.z);
        }
        
        private void UpdatePosY(string value)
        {
            if (activeTarget == null)
                return;
            activeTarget.position = new Vector3(activeTarget.position.x,float.Parse(value), activeTarget.position.z);
        }
        
        private void UpdatePosZ(string value)
        {
            if (activeTarget == null)
                return;
            activeTarget.position = new Vector3(activeTarget.position.x, activeTarget.position.y,float.Parse(value));
        }
        
        private void UpdateRotX(string value)
        {
            if (activeTarget == null)
                return;
            activeTarget.eulerAngles = new Vector3(float.Parse(value), activeTarget.eulerAngles.y, activeTarget.eulerAngles.z);
        }
        
        private void UpdateRotY(string value)
        {
            if (activeTarget == null)
                return;
            activeTarget.eulerAngles = new Vector3(activeTarget.eulerAngles.x,float.Parse(value), activeTarget.eulerAngles.z);
        }
        
        private void UpdateRotZ(string value)
        {
            if (activeTarget == null)
                return;
            activeTarget.eulerAngles = new Vector3(activeTarget.eulerAngles.x, activeTarget.eulerAngles.y,float.Parse(value));
        }
        
        private void UpdateScaleX(string value)
        {
            if (activeTarget == null)
                return;
            activeTarget.localScale = new Vector3(float.Parse(value), activeTarget.localScale.y, activeTarget.localScale.z);
        }
        
        private void UpdateScaleY(string value)
        {
            if (activeTarget == null)
                return;
            activeTarget.localScale = new Vector3(activeTarget.localScale.x,float.Parse(value), activeTarget.localScale.z);
        }
        
        private void UpdateScaleZ(string value)
        {
            if (activeTarget == null)
                return;
            activeTarget.localScale = new Vector3(activeTarget.localScale.x, activeTarget.localScale.y,float.Parse(value));
        }
        
        public void Inspect(Transform target)
        {
            this.activeTarget = target;
            body.gameObject.SetActive(activeTarget != null);
        }

        private void LateUpdate()
        {
            if (activeTarget == null)
                return;

            UpdateNameField();
            UpdatePostionFields();
            UpdateRotationFields();
            UpdateScaleFields();
        }

        private void UpdateNameField()
        {
            if (!nameField.isFocused)
            {
                nameField.text = activeTarget.name;
            } 
        }
        
        private void UpdatePostionFields()
        {
            if (!posX.isFocused)
            {
                posX.SetTextWithoutNotify(activeTarget.position.x.ToString("F4"));
            } 
            
            if (!posY.isFocused)
            {
                posY.SetTextWithoutNotify(activeTarget.position.y.ToString("F4"));
            } 
            
            if (!posZ.isFocused)
            {
                posZ.SetTextWithoutNotify(activeTarget.position.z.ToString("F4"));
            } 
        }
        
        private void UpdateRotationFields()
        {
            if (!rotX.isFocused)
            {
                rotX.SetTextWithoutNotify(activeTarget.eulerAngles.x.ToString("F4"));
            } 
            
            if (!rotY.isFocused)
            {
                rotY.SetTextWithoutNotify(activeTarget.eulerAngles.y.ToString("F4"));
            } 
            
            if (!rotZ.isFocused)
            {
                rotZ.SetTextWithoutNotify(activeTarget.eulerAngles.z.ToString("F4"));
            } 
        }
        
        private void UpdateScaleFields()
        {
            if (!scaleX.isFocused)
            {
                scaleX.SetTextWithoutNotify(activeTarget.localScale.x.ToString("F4"));
            } 
            
            if (!scaleY.isFocused)
            {
                scaleY.SetTextWithoutNotify(activeTarget.localScale.y.ToString("F4"));
            } 
            
            if (!rotZ.isFocused)
            {
                scaleZ.SetTextWithoutNotify(activeTarget.localScale.z.ToString("F4"));
            } 
        }
    }
}