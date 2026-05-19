using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    //Damages enemy if hit.
    [SerializeField] private float Damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        Enemy EnemyScript = other.GetComponentInParent<Enemy>();
        if (EnemyScript != null && other.tag == "EnemyBody")
        {
            EnemyScript.EditHealth(-Damage);
        }
    }
    //.
}
