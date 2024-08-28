using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCollectible : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        SwordsmanController controller = other.GetComponent<SwordsmanController>();
        if (controller != null)
        {
            controller.obtainedCast = true;
            SwordsmanController.currentElement = 1;
            GameObject textBox = Instantiate(canvas, transform.position, Quaternion.identity);
            Destroy(gameObject);
            GlobalVars.obtainedCast = true;
            GlobalVars.obtainedIce = true;
            GlobalVars.obtainedElement = true;
        }
    }
}
