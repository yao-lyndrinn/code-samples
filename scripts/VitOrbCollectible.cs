using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class VitOrbCollectible : MonoBehaviour
{
    public GameObject canvas;
    public int number;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        SwordsmanController controller = other.GetComponent<SwordsmanController>();
        if (controller != null)
        {
            controller.maxHealth += 1;
            controller.ChangeHealth(controller.maxHealth);
            GameObject textBox = Instantiate(canvas, transform.position, Quaternion.identity);
            Destroy(gameObject);
            GlobalVars.obtainedVit[number] = true;
        }
    }

}
