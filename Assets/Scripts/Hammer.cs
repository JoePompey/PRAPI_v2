using UnityEngine;
using System.Collections;

public class Hammer : MonoBehaviour
{
    //Initialisation.
    private GameObject HammerModel;
    private WeaponDamage DamageScript;

    private float SwingSpeed = 100f;
    private bool SlamDown = false;
    private bool PullUp = false;
    private bool Swinging = false;

    private void Awake()
    {
        HammerModel = transform.Find("Model").gameObject;
        DamageScript = GetComponentInChildren<WeaponDamage>();
    }
    //.

    //Swinging the hammer.
    private void FixedUpdate()
    {
        if (SlamDown)
        {
            SwingSmoothener(1f);
        }
        if (PullUp)
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
        //Slamming down.
        Swinging = true;
        SlamDown = true;
        DamageScript.DamageActive = true;
        //.

        //Pulling up.
        yield return new WaitForFixedUpdate();
        for (int i = 0; i < 25; i++)
            yield return new WaitForFixedUpdate();
        SlamDown = false;
        PullUp = true;
        //.

        //Finishing swing.
        for (int i = 0; i < 25; i++)
            yield return new WaitForFixedUpdate();
        PullUp = false;
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
        HammerModel.transform.Rotate(Vector3.right * SwingSpeed * Time.fixedDeltaTime * Direction);
    }
    //.
}
