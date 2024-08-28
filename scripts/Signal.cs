using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour
{
    // Start is called before the first frame update
    public void Pause()
    {
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    public void Unpause()
    {
        Time.timeScale = 1.0f;
    }
}
