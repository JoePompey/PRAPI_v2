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
    public string ItemName = "Base";

    private Transform PlayerTransform;
    private Player PlayerScript;
    private Transform Hand;
    public SphereCollider ModelCollider;
    private Rigidbody Body;

    private void Awake()
    {
        ControlsFile = new InputSystem_Actions();
        PlayerControls = ControlsFile.PlayerControls;

        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerScript = PlayerTransform.GetComponent<Player>();
        Hand = PlayerTransform.Find("Model/FistR");
        ModelCollider = GetComponentInChildren<SphereCollider>();
        Body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ModelCollider.enabled = true;
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
        if (GrabPressed && CheckPlayerCanPickup())
        {
            ModelCollider.enabled = false;
            IsGrabbed = true;

            PlayerScript.HoldingItem = true;
            PlayerScript.HeldItem = ItemName;
            PlayerScript.CurrentItem = gameObject;
            PlayerScript.ItemScript = this;
            
            if (ItemName == "Apple")
            {
                PlayerScript.AppleScript = gameObject.GetComponent<Apple>();
            }
            else if (ItemName == "Hammer")
            {
                PlayerScript.HammerScript = gameObject.GetComponent<Hammer>();
            }
            else if (ItemName == "Sword")
            {
                PlayerScript.SwordScript = gameObject.GetComponent<Sword>();
            }
        }
        if (DropPressed)
        {
            ModelCollider.enabled = true;
            IsGrabbed = false;

            PlayerScript.HoldingItem = false;
            PlayerScript.HeldItem = "None";
            PlayerScript.CurrentItem = null;
            PlayerScript.AppleScript = null;
        }
    }

    private void LateUpdate()
    {
        if (IsGrabbed)
        {
            FollowHand();
        }
        else
        {
            ApplyGravity();
        }
    }

    //Keeps item in player hand.
    private void FollowHand()
    {
        transform.position = Hand.position + Hand.forward * 0.1f;
        transform.rotation = Hand.rotation;
    }

    private Vector3 PlayerDirection;
    private float PlayerDistance;
    private bool CheckPlayerCanPickup()
    {
        if (!PlayerScript.HoldingItem)
        {
            PlayerDirection = PlayerTransform.position - Body.transform.position;
            PlayerDistance = PlayerDirection.magnitude;

            if (PlayerDistance <= 2.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
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
