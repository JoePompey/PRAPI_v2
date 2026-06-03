using UnityEngine;
using TMPro;
using System.Collections;

public class Enemy : MonoBehaviour
{
    //Initialisation.
    private float Speed = 3f;
    private float Health = 100f;
    private float Damage = 10f;
    private bool CanDamage = true;

    private Rigidbody Body;
    private TextMeshPro HealthLabel;

    private Transform PlayerTransform;
    private Player PlayerScript;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
        Body.freezeRotation = true;
        Body.linearDamping = 10f;

        HealthLabel = transform.Find("Health Label").GetComponent<TextMeshPro>();

        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
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
        if (CheckPlayerDistance())
        {
            AttackPlayer();
        }
    }

    //Changes health and updates label.
    public void EditHealth(float HealthChange)
    {
        Health += HealthChange;
        HealthLabel.text = "Health: " + Health;

        if (Health <= 0f)
        {
            Destroy(gameObject);
        }
    }
    //.

    //Follows and faces player.
    private void ChasePlayer()
    {
        Vector3 PlayerDirection = PlayerTransform.position - Body.position;

        Body.MovePosition(Body.position + PlayerDirection * Speed * Time.deltaTime);
        Body.rotation = Quaternion.LookRotation(PlayerDirection);

        Vector3 CameraDirection = Camera.main.transform.position - HealthLabel.transform.position;
        HealthLabel.transform.rotation = Quaternion.LookRotation(CameraDirection * -1);
    }
    //.

    //Checks if player is close enough to attack.
    private bool CheckPlayerDistance()
    {
        Vector3 PlayerDirection = PlayerTransform.position - Body.position;
        float PlayerDistance = PlayerDirection.magnitude;

        if (PlayerDistance <= 1.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //.

    //Attack the player.
    private void AttackPlayer()
    {
        if (CanDamage)
        {
            PlayerScript.EditHealth(-Damage);
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        CanDamage = false;
        yield return new WaitForSeconds(0.5f);
        CanDamage = true;
    }
    //.
}
