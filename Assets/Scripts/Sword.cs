using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
    //Initialisation.
    private GameObject SwordModel;
    private WeaponDamage DamageScript;

    private float SwingSpeed = 100f;
    private bool SwingLeft = false;
    private bool SwingRight = false;
    private bool Swinging = false;

    private void Awake()
    {
        SwordModel = transform.Find("Model").gameObject;
        DamageScript = GetComponentInChildren<WeaponDamage>();
    }
    //.

    //Swinging the sword.
    private void FixedUpdate()
    {
        if (SwingLeft)
        {
            SwingSmoothener(1f);
        }
        if (SwingRight)
        {
            SwingSmoothener(-1f);
        }
    }

    public void StartSwinging()
    {
        if (!Swinging)
        {
            StartCoroutine(Swing());
        }
    }

    private IEnumerator Swing()
    {
        //Swinging left.
        Swinging = true;
        SwingLeft = true;
        DamageScript.DamageActive = true;
        //.

        //Swinging right.
        yield return new WaitForFixedUpdate();
        for (int i = 0; i < 25; i++)
            yield return new WaitForFixedUpdate();
        SwingLeft = false;
        SwingRight = true;
        //.

        //Finishing swing.
        for (int i = 0; i < 25; i++)
            yield return new WaitForFixedUpdate();
        SwingRight = false;
        DamageScript.DamageActive = false;
        //.

        //Swing cooldown.
        for (int i = 0; i < 5; i++)
            yield return new WaitForFixedUpdate();
        Swinging = false;
        //.
    }

    private void SwingSmoothener(float Direction)
    {
        SwordModel.transform.Rotate(Vector3.right * SwingSpeed * Time.fixedDeltaTime * Direction);
    }
    //.
}
