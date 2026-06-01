using UnityEngine;
using System.Collections;

public class Hammer : MonoBehaviour
{
    //Initialisation.
    private GameObject HammerModel;

    private float SwingSpeed = 100f;
    private bool SlamDown = false;
    private bool PullUp = false;
    private bool Swinging = false;

    private void Awake()
    {
        HammerModel = transform.Find("Model").gameObject;
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
        //.

        //Pulling up.
        for (int i = 0; i <= 25; i++)
            yield return new WaitForFixedUpdate();
        SlamDown = false;
        PullUp = true;
        //.

        //Finishing swing.
        for (int i = 0; i <= 25; i++)
            yield return new WaitForFixedUpdate();
        PullUp = false;
        //.

        //Swing cooldown.
        yield return new WaitForSeconds(0.1f);
        Swinging = false;
        //.
    }

    private void SwingSmoothener(float Direction)
    {
        HammerModel.transform.Rotate(Vector3.right * SwingSpeed * Time.fixedDeltaTime * Direction);
    }
    //.
}
