using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Initialisation.
    private InputSystem_Actions ControlsFile;
    private InputActionMap PlayerControls;
    
    private float Speed;
    private float SpinSpeed;
    
    private bool ForwardPressed = false;
    private bool BackwardPressed = false;
    private bool StrafeRightPressed = false;
    private bool StrafeLeftPressed = false;
    private bool SteerRightPressed = false;
    private bool SteerLeftPressed = false;

    private Rigidbody Body;

    private void Awake()
    {
        ControlsFile = new InputSystem_Actions();
        PlayerControls = ControlsFile.PlayerControls;
        Body = GetComponent<Rigidbody>();
        Body.freezeRotation = true;
    }
    private void OnEnable()
    {
        PlayerControls.Enable();
        
        //Hooks up controls to variables to check for functions.
        PlayerControls.FindAction("Forward").performed += ctx => ForwardPressed = true;
        PlayerControls.FindAction("Forward").canceled += ctx => ForwardPressed = false;

        PlayerControls.FindAction("Backward").performed += ctx => BackwardPressed = true;
        PlayerControls.FindAction("Backward").canceled += ctx => BackwardPressed = false;

        PlayerControls.FindAction("StrafeRight").performed += ctx => StrafeRightPressed = true;
        PlayerControls.FindAction("StrafeRight").canceled += ctx => StrafeRightPressed = false;

        PlayerControls.FindAction("StrafeLeft").performed += ctx => StrafeLeftPressed = true;
        PlayerControls.FindAction("StrafeLeft").canceled += ctx => StrafeLeftPressed = false;

        PlayerControls.FindAction("SteerRight").performed += ctx => SteerRightPressed = true;
        PlayerControls.FindAction("SteerRight").canceled += ctx => SteerRightPressed = false;

        PlayerControls.FindAction("SteerLeft").performed += ctx => SteerLeftPressed = true;
        PlayerControls.FindAction("SteerLeft").canceled += ctx => SteerLeftPressed = false;
        //.
    }
    private void Start()
    {
        Speed = 10f;
        SpinSpeed = 5f;
    }
    //.

    private void FixedUpdate()
    { 
        //Checks button input values.
        if (ForwardPressed)
        {
            Move(Speed, 0);
        }

        if (BackwardPressed)
        {
            Move(-Speed, 0);
        }

        if (StrafeRightPressed)
        {
            Move(0, Speed);
        }

        if (StrafeLeftPressed)
        {
            Move(0, -Speed);
        }

        if (SteerRightPressed)
        {
            Rotate(SpinSpeed);
        }
        if (SteerLeftPressed)
        {
            Rotate(-SpinSpeed);
        }
        //.

        //Applies gravity.
        Vector3 Gravity = Vector3.down * 9.8f * Time.deltaTime;
        Body.MovePosition(Body.position + Gravity);
        //.
    }

    private void Move(float ForwardAcceleration, float RightAcceleration)
    {
        Vector3 DeltaForward = Body.transform.forward * ForwardAcceleration * Time.deltaTime;
        Vector3 DeltaRight = Body.transform.right * RightAcceleration * Time.deltaTime;
        Vector3 NewPos = Body.position + DeltaForward + DeltaRight;

        Body.MovePosition(NewPos);
    }

    private void Rotate(float Spin)
    {
        Body.MoveRotation(Body.rotation * Quaternion.Euler(0f, Spin, 0f));
    }
}
