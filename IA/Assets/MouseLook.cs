using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }
    public float speed = 6.0f;

    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    //public float minimumHor = -90.0f;
    //public float maximumHor = 90.0f;

    private float _rotationX = 0;
    private float _rotationY = 0;
    public float gravity = -9.8f;

    public RotationAxes axes = RotationAxes.MouseXAndY;

    private CharacterController characterController;

    void Start()
    {
            Rigidbody body = GetComponent<Rigidbody>();
            if (body != null)
                body.freezeRotation = true;

        this.characterController = this.GetComponent<CharacterController>();
    }
    void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            transform.Translate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
           transform.localEulerAngles = 
               new Vector3(Mathf.Clamp(_rotationX, minimumVert, maximumVert), transform.localEulerAngles.y, 0); 
        }
        else
        {
            //float deltaX = Input.GetAxis("Horizontal") * speed;
            //float deltaZ = Input.GetAxis("Vertical") * speed;
            //transform.Translate(deltaX, 0, deltaZ);

            //   _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVer;
            this._rotationY += Input.GetAxis("Mouse X") * sensitivityHor;
            transform.localEulerAngles =
                new Vector3(Mathf.Clamp(_rotationX, minimumVert, maximumVert),
                    Mathf.Clamp(_rotationY, 0, _rotationY), 0);

            float deltaX = Input.GetAxis("Horizontal") * speed;
            float deltaZ = Input.GetAxis("Vertical") * speed;
            Vector3 movement = new Vector3(deltaX, 0, deltaZ);

            movement = Vector3.ClampMagnitude(movement, speed);
            movement *= Time.deltaTime;
            movement = transform.TransformDirection(movement);
            this.characterController.Move(movement);

            ////tt
            //float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityHor;

            //_rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            //_rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

            //transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);


            //float deltaX = Input.GetAxis("Horizontal") * speed;
            //float deltaZ = Input.GetAxis("Vertical") * speed;
            //Vector3 movement = new Vector3(deltaX, 0, deltaZ);
            //movement = Vector3.ClampMagnitude(movement, speed);

            //movement.y = gravity;

            //movement *= Time.deltaTime;
            //movement = transform.TransformDirection(movement);
            //characterController.Move(movement);

        }
    }
}
