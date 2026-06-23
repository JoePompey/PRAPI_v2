using System.Linq;
using UnityEngine;

public class Speedy : MonoBehaviour
{
    //Initialisation.
    [SerializeField] float SpeedModifier = 1.5f;
    private bool IsPlayer = false;

    private GameObject VictimObject;
    private GameObject[] ItemsList;
    private Player PlayerScript;

    private Renderer[] EditableRenderers;
    private Material[] BaseMaterials;
    private Material SpeedyBlue;

    private void Awake()
    {
        if (gameObject.tag == "Player")
        {
            IsPlayer = true;
            PlayerScript = gameObject.GetComponent<Player>();
        }

        //Gets materials ready for editing.
        EditableRenderers = GetComponentsInChildren<Renderer>().Where(r => r.CompareTag("EffectColourable")).ToArray();

        BaseMaterials = new Material[EditableRenderers.Length];
        for (int i = 0; i < EditableRenderers.Length; i++)
        {
            BaseMaterials[i] = EditableRenderers[i].material;
        }

        SpeedyBlue = Resources.Load<Material>("Speedy Blue");

        AddSpeedyMaterial();
        //.
    }
    //.

    //Set speed boost.
    private void Start()
    {
        Accelerate(true);
    }

    private void Accelerate(bool UpOrDown)
    {
        print("Accelerate");
        if (IsPlayer)
        {
            print("IsPlayer");
            if (UpOrDown)
            {
                print("Up");
                PlayerScript.Speed *= SpeedModifier;
            }

            else
            {
                print("Down");
                PlayerScript.Speed /= SpeedModifier;
            }
        }
    }
    //.

    //Speedy spread.
    public void SpeedySpread()
    {
        //Spread from player to item.
        if (IsPlayer)
        {
            ItemsList = GameObject.FindGameObjectsWithTag("Item");
            for (int i = 0; i < ItemsList.Length; i++)
            {
                if (ItemsList[i].GetComponent<BasePickupable>().IsGrabbed == true)
                {
                    VictimObject = ItemsList[i];
                    VictimObject.GetComponent<BasePickupable>().SpeedyScript = this;
                    break;
                }
            }
        }
        //.

        //Spread from item to player.
        else
        {
            VictimObject = GameObject.Find("Player");
        }
        //.

        if (VictimObject != null)
        {
            VictimObject.AddComponent<Speedy>();
        }
    }
    //.

    //End speed when detached from accelerator.
    public void SlowDown()
    {
        Accelerate(false);
        RemoveSpeedyMaterial();
        Destroy(this);
    }
    //.

    //Apply speedy material when active.
    private void AddSpeedyMaterial()
    {
        for (int i = 0; i < EditableRenderers.Length; i++)
        {
            EditableRenderers[i].material = SpeedyBlue;
        }
    }

    private void RemoveSpeedyMaterial()
    {
        for (int i = 0; i < EditableRenderers.Length; i++)
        {
            EditableRenderers[i].material = BaseMaterials[i];
        }
    }
    //.
}
