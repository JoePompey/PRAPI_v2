using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;

public class Burn : MonoBehaviour
{
    //Initialisation.
    private float TickSpeed = 0.5f;
    private float TickDamage = 2f;
    private bool TickCooldown = false;
    private bool IsPlayer = false;

    private GameObject VictimObject;
    private GameObject[] ItemsList;
    private Player PlayerScript;

    private Renderer[] EditableRenderers;
    private Material[] BaseMaterials;
    private Material BurnOrange;

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

        BurnOrange = Resources.Load<Material>("Burn Orange");

        AddBurnMaterial();
        //.
    }
    //.

    //Deal damage.
    private void FixedUpdate()
    {
        if (!TickCooldown && IsPlayer)
        {
            StartCoroutine(ActivateDamageTick());
        }
    }

    IEnumerator ActivateDamageTick()
    {
        TickCooldown = true;
        PlayerScript.EditHealth(-TickDamage);
        yield return new WaitForSeconds(TickSpeed);
        TickCooldown = false;
    }
    //.

    //Fire spread.
    public void FireSpread()
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
                    VictimObject.GetComponent<BasePickupable>().BurnScript = this;
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
            VictimObject.AddComponent<Burn>();
        }
    }
    //.

    //End burning when detached from firestarter.
    public void Extinguish()
    {
        RemoveBurnMaterial();
        Destroy(this);
    }
    //.

    //Apply burn material when active.
    private void AddBurnMaterial()
    {
        for (int i = 0; i < EditableRenderers.Length; i++)
        {
            EditableRenderers[i].material = BurnOrange;
        }
    }

    private void RemoveBurnMaterial()
    {
        for (int i = 0; i < EditableRenderers.Length; i++)
        {
            EditableRenderers[i].material = BaseMaterials[i];
        }
    }
    //.
}
