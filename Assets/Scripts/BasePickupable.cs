using UnityEngine;
using UnityEngine.InputSystem;

public class BasePickupable : MonoBehaviour
{
    //Initialisation.
    private InputSystem_Actions ControlsFile;
    private InputActionMap PlayerControls;

    private bool GrabPressed = false;
    private bool DropPressed = false;

    private Transform Player;
    private Transform Hand;

    private void Awake()
    {
        ControlsFile = new InputSystem_Actions();
        PlayerControls = ControlsFile.PlayerControls;

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Hand = Player.Find("Model/FistR");
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

    private void LateUpdate()
    {
        FollowHand();
    }

    //Keeps item in player hand.
    private void FollowHand()
    {
        transform.position = Hand.position + Hand.forward * 0.1f;
        transform.rotation = Hand.rotation;
    }
    //.
}
