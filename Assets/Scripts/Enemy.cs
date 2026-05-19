using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    //Initialisation.
    private float Speed;
    private float Health;

    private Rigidbody Body;
    private TextMeshPro HealthLabel;

    private Transform Player;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
        Body.freezeRotation = true;
        Body.linearDamping = 10f;

        HealthLabel = transform.Find("Health Label").GetComponent<TextMeshPro>();

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        print(Player);
    }

    private void Start()
    {
        Speed = 3f;
        Health = 100f;

        EditHealth(0f);
    }
    //.

    private void FixedUpdate()
    {
        //Applies gravity.
        Vector3 Gravity = Vector3.down * 9.8f * Time.deltaTime;
        Body.MovePosition(Body.position + Gravity);
        //.

        ChasePlayer();
    }

    private void EditHealth(float HealthChange)
    {
        Health += HealthChange;
        HealthLabel.text = "Health: " + Health;
    }

    private void ChasePlayer()
    {
        Vector3 PlayerDirection = Player.position - Body.position;

        Body.MovePosition(Body.position + PlayerDirection * Speed * Time.deltaTime);
        Body.rotation = Quaternion.LookRotation(PlayerDirection);

        Vector3 CameraDirection = Camera.main.transform.position - HealthLabel.transform.position;
        HealthLabel.transform.rotation = Quaternion.LookRotation(CameraDirection * -1);
    }
}
