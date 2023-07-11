using System;
using RuntimeEditorExperiment.Scripts.Runtime.Common;
using RuntimeEditorExperiment.Scripts.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

public class RTCamera : MonoBehaviour, IRTCamera
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera handlesCamera;
    [SerializeField] private Camera functionCamera;

    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float panSpeed = 50;
    [SerializeField] private float speedMultiplier = 2;
    [SerializeField] private float rotateSped = 10;
    [FormerlySerializedAs("scrollSensitivity")] [SerializeField] private float zoomIntensity = 2f;
    
    public Camera HandlesCamera => handlesCamera;
    public Camera MainCamera => mainCamera;
    public Camera FuncitonsCamera => functionCamera;

    private float inputX;
    private float inputY;

    private Vector3 forwardDirection;
    private Vector3 upDirection;
    private Vector3 rightDirection;
    
    private void Awake()
    {
        RTCommon.RegisterInstance<IRTCamera>(this);
    }
    
    private void OnDestroy()
    {
        RTCommon.DeRegisterInstance<IRTCamera>(this);
    }

    private void Update()
    {
        inputY = (Input.GetAxis("Vertical") + Input.mouseScrollDelta.y * zoomIntensity)/2;
        inputX = Input.GetAxis("Horizontal");
        forwardDirection = mainCamera.transform.forward;
        upDirection = mainCamera.transform.up;
        rightDirection = mainCamera.transform.right;
        bool canMove = true;
        
        //rotate
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            mainCamera.transform.Rotate(Vector3.up, mouseX * rotateSped * Time.deltaTime);
            mainCamera.transform.Rotate(Vector3.left, mouseY * rotateSped * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Euler(new Vector3(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, 0));
        }
        else if (Input.GetMouseButton(2)) //pan
        {
            canMove = false;
            float mouseX = -Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");
            
            Vector3 pan = upDirection * (mouseY * panSpeed * Time.deltaTime);
            pan += rightDirection * (mouseX * panSpeed * Time.deltaTime);
            transform.Translate(pan);
        }
        
        if (canMove)
        {
            Vector3 translation = forwardDirection * (inputY * moveSpeed * Time.deltaTime);
            translation += rightDirection * (inputX * moveSpeed * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift)?speedMultiplier:1));
            transform.Translate(translation);
        }
    }
}
