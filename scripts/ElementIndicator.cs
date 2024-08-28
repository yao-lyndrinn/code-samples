using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementIndicator : MonoBehaviour
{
    Image m_Image;
    public Sprite wind;
    public Sprite ice;
    public Sprite earth;
    public Sprite fire;
    // Start is called before the first frame update
    void Start()
    {
        m_Image = GetComponent<Image>();
        if (GlobalVars.obtainedElement == false){
            m_Image.enabled = false;
        }
    }

    void Update(){
        if (GlobalVars.obtainedElement){
            m_Image.enabled = true;
        }
        if(SwordsmanController.currentElement == 0){
            m_Image.sprite = wind;
        }
        else if(SwordsmanController.currentElement == 1){
            m_Image.sprite = ice;
        }
        else if (SwordsmanController.currentElement == 2){
            m_Image.sprite = earth;
        }
        else{
            m_Image.sprite = fire;
        }
    }
}
