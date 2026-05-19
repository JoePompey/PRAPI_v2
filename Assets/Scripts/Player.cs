using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour
{
    //Initialisation.
    private InputSystem_Actions ControlsFile;
    private InputActionMap PlayerControls;

    private float Speed = 10f;
    private float SpinSpeed = 5f;
    private float JumpSpeed = 20f;
    private bool Grounded = false;
    private float Health = 100f;
    private bool Punching = false;
    private bool PunchingOut = false;
    private bool PullingPunch = false;

    private bool ForwardPressed = false;
    private bool BackwardPressed = false;
    private bool StrafeRightPressed = false;
    private bool StrafeLeftPressed = false;
    private bool SteerRightPressed = false;
    private bool SteerLeftPressed = false;
    private bool UsePressed = false;

    private Rigidbody Body;
    private TextMeshPro HealthLabel;
    private GameObject FistR;

    private void Awake()
    {
        ControlsFile = new InputSystem_Actions();
        PlayerControls = ControlsFile.PlayerControls;
        
        Body = GetComponent<Rigidbody>();
        Body.freezeRotation = true;
        Body.linearDamping = 10f;

        HealthLabel = transform.Find("Health Label").GetComponent<TextMeshPro>();

        FistR = transform.Find("Model/FistR").gameObject;
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

        PlayerControls.FindAction("Use").performed += ctx => UsePressed = true;
        PlayerControls.FindAction("Use").canceled += ctx => UsePressed = false;
        //.
    }
    private void Start()
    {
        EditHealth(0f);
    }
    //.

    Vector3 MoveDirection = Vector3.zero;
    private void FixedUpdate()
    { 
        //Links inputs to functions.
        if (ForwardPressed || BackwardPressed)
        {
            if (ForwardPressed)
            {
                MoveDirection.x += 1f;
            }
            if (BackwardPressed)
            {
                MoveDirection.x -= 1f;
            }
        }
        else
        {
            MoveDirection.x = 0;
        }

        if (StrafeRightPressed || StrafeLeftPressed)
        {
            if (StrafeRightPressed)
            {
                MoveDirection.y += 1f;
            }
            if (StrafeLeftPressed)
            {
                MoveDirection.y -= 1f;
            }
        }
        else
        {
            MoveDirection.y = 0;
        }

        MoveDirection = MoveDirection.normalized;
        if (MoveDirection.magnitude != 0)
        {
            Move(MoveDirection.x * Speed, MoveDirection.y * Speed, 0);
        }

        if (SteerRightPressed)
        {
            Rotate(SpinSpeed);
        }
        if (SteerLeftPressed)
        {
            Rotate(-SpinSpeed);
        }

        if (PlayerControls.FindAction("Jump").IsPressed() && Grounded)
        {
            Move(0, 0, JumpSpeed);
        }

        if (UsePressed && Punching == false)
        {
            StartCoroutine(Punch());
        }
        //.

        //Calls punch smoothener dependent on direction.
        if (PunchingOut)
        {
            PunchSmoothener(1f);
        }
        if (PullingPunch)
        {
            PunchSmoothener(-1f);
        }
        //.

        //Applies gravity.
        if (!Grounded)
        {
            Vector3 Gravity = Vector3.down * 9.8f * Time.deltaTime;
            Body.MovePosition(Body.position + Gravity);
        }
        //.
    }

    //Checks if grounded so player can jump with slight buffers for smoothness.
    private void OnCollisionStay(Collision Collider)
    {
        if (Collider.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(WaitForGravity(false));
        }
    }

    private void OnCollisionExit(Collision Collider)
    {
        if (Collider.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(WaitForGravity(true));
        }
    }

    IEnumerator WaitForGravity(bool GoingUp)
    {
        yield return new WaitForSeconds(0.1f);
        if (GoingUp)
        {
            Grounded = false;
        }
        else
        {
            Grounded = true;
        }
    }
    //.

    //Movement and rotation.
    private void Move(float ForwardAcceleration, float RightAcceleration, float JumpAcceleration)
    {
        Vector3 DeltaForward = Body.transform.forward * ForwardAcceleration * Time.deltaTime;
        Vector3 DeltaRight = Body.transform.right * RightAcceleration * Time.deltaTime;
        Vector3 DeltaUp = Vector3.up * JumpAcceleration * Time.deltaTime;
        Vector3 NewPos = Body.position + DeltaForward + DeltaRight + DeltaUp;

        Body.MovePosition(NewPos);
    }

    private void Rotate(float Spin)
    {
        Body.MoveRotation(Body.rotation * Quaternion.Euler(0f, Spin, 0f));
    }
    //.

    //Changes health and updates label.
    private void EditHealth(float HealthChange)
    {
        Health += HealthChange;
        HealthLabel.text = "Health: " + Health;
    }
    //.

    //Lets player punch with cooldowns.
    IEnumerator Punch()
    {
        Punching = true;
        
        //Punching out.
        PunchingOut = true;
        //.

        //Pulling punch.
        yield return new WaitForSeconds(0.05f);
        PunchingOut = false;
        PullingPunch = true;
        //.
        
        //Finishing punch.
        yield return new WaitForSeconds(0.05f);
        PullingPunch = false;
        //.

        //Punch cooldown.
        yield return new WaitForSeconds(0.1f);
        Punching = false;
        //.
    }
    
    //-Smooths out the fist's movement when punching.
    private void PunchSmoothener(float Direction)
    {
        FistR.transform.position += FistR.transform.forward * Speed * Time.deltaTime * Direction;
    }
    //-.
    //.
}
