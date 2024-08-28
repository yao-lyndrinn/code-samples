using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaOrbCollectible : MonoBehaviour
{
    public int number;
    public GameObject canvas;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        SwordsmanController controller = other.GetComponent<SwordsmanController>();
        if (controller != null)
        {
            controller.maxMana += 5;
            controller.FillMana();
            GameObject textBox = Instantiate(canvas, transform.position, Quaternion.identity);
            Destroy(gameObject);
            GlobalVars.obtainedMana[number] = true;
        }
    }

}
