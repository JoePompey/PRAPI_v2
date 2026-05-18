using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Initialisation.
    private Rigidbody Body;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
        Body.freezeRotation = true;
        Body.linearDamping = 10f;
    }
    //.

    private void FixedUpdate()
    {
        //Applies gravity.
        Vector3 Gravity = Vector3.down * 9.8f * Time.deltaTime;
        Body.MovePosition(Body.position + Gravity);
        //.
    }
}
