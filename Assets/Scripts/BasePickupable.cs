using UnityEngine;
using UnityEngine.InputSystem;

public class BasePickupable : MonoBehaviour
{
    //Initialisation.
    private InputSystem_Actions ControlsFile;
    private InputActionMap PlayerControls;

    private bool GrabPressed = false;
    private bool DropPressed = false;
    private bool IsGrabbed = false;

    private Transform Player;
    private Transform Hand;
    private SphereCollider ModelCollider;
    private Rigidbody Body;

    private void Awake()
    {
        ControlsFile = new InputSystem_Actions();
        PlayerControls = ControlsFile.PlayerControls;

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Hand = Player.Find("Model/FistR");
        ModelCollider = GameObject.Find("Model/Placehold").GetComponent<SphereCollider>();
        Body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        PlayerControls.Enable();

        //Hooks up controls to variables to check for functions.
        PlayerControls.FindAction("Grab").performed += ctx => GrabPressed = true;
        PlayerControls.FindAction("Grab").canceled += ctx => GrabPressed = false;

        PlayerControls.FindAction("Drop").performed += ctx => DropPressed = true;
        PlayerControls.FindAction("Drop").canceled += ctx => DropPressed = false;
        //.
    }
    //.

    private void FixedUpdate()
    {
        if (GrabPressed)
        {
            ModelCollider.enabled = false;
            IsGrabbed = true;
        }
        if (DropPressed)
        {
            ModelCollider.enabled = true;
            IsGrabbed = false;
        }

        if (!IsGrabbed)
        {
            ApplyGravity();
        }
    }

    private void LateUpdate()
    {
        if (IsGrabbed)
        {
            FollowHand();
        }
    }

    //Keeps item in player hand.
    private void FollowHand()
    {
        transform.position = Hand.position + Hand.forward * 0.1f;
        transform.rotation = Hand.rotation;
    }
    //.

    private void ApplyGravity()
    {
        //Applies gravity.
        Vector3 Gravity = Vector3.down * 9.8f * Time.deltaTime;
        Body.MovePosition(Body.position + Gravity);
        //.
    }
}
