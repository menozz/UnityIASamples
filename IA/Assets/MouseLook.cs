using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }
    public float sensitivityHor = 9.0f;
    public float sensitivityVer = 9.0f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    public float minimumHor = -45.0f;
    public float maximumHor = 45.0f;

    private float _rotationX = 0;
    private float _rotationY = 0;

    public RotationAxes axes = RotationAxes.MouseXAndY;

    void Start()
    {
            Rigidbody body = GetComponent<Rigidbody>();
            if (body != null)
                body.freezeRotation = true;
    }
    void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVer;
           transform.localEulerAngles = 
               new Vector3(Mathf.Clamp(_rotationX, minimumVert, maximumVert), transform.localEulerAngles.y, 0); 
        }
        else
        {

            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVer;
            this._rotationY += Input.GetAxis("Mouse X") * sensitivityHor;
            transform.localEulerAngles =
                new Vector3(Mathf.Clamp(_rotationX, minimumVert, maximumVert), Mathf.Clamp(_rotationY, minimumHor, maximumHor), 0);

        }
    }
}
