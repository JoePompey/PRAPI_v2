using UnityEngine;

public class Apple : MonoBehaviour
{
    public void EndLife()
    {
        Destroy(gameObject);
        Destroy(this);
    }
}
