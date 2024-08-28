using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlessingCollectible : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        SwordsmanController controller = other.GetComponent<SwordsmanController>();
        if (controller != null)
        {
            controller.obtainedDash = true;
            GameObject textBox = Instantiate(canvas, transform.position, Quaternion.identity);
            Destroy(gameObject);
            GlobalVars.obtainedDash = true;
            GlobalVars.obtainedWind = true;
            GlobalVars.obtainedElement = true;
            Debug.Log(GlobalVars.obtainedWind);
        }
    }
}
