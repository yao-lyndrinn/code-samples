using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject itemPrefab;
    public string marker;
    public int number;
    public bool obtained;

    void Awake()
    {
        if(marker == "Dash"){
            obtained = GlobalVars.obtainedDash;
        }
        else if(marker == "Vit"){
            obtained = GlobalVars.obtainedVit[number];
        }
        else if(marker == "Mana"){
            obtained = GlobalVars.obtainedMana[number];
        }
        else if(marker == "Cast"){
            obtained = GlobalVars.obtainedCast;
        }
        else if(marker == "Pickaxe"){
            obtained = GlobalVars.obtainedPickaxe;
        }
        else if(marker == "IceSpell"){
            obtained = GlobalVars.obtainedIceSpell;
        }
        else if(marker == "WindSpell"){
            obtained = GlobalVars.obtainedWindSpell;
        }
        if (!obtained)
        {
        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        if(marker == "Vit"){
            VitOrbCollectible vitOrb = item.GetComponent<VitOrbCollectible>();
            vitOrb.number = number;
        }
        if(marker == "Mana"){
            ManaOrbCollectible manaOrb = item.GetComponent<ManaOrbCollectible>();
            manaOrb.number = number;
        }
        }
    }
}
