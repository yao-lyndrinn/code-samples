using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//it's okay to configure these all in static or global vars later
//for now just pretend they exist
//INTENDED BEHAVIOR
//SWITCH MAX 2 ELEMENTS ON
//RECLICKING AN ELEMENT SWITCHES IT OFF
//ELEMENTS NOT OBTAINED CANNOT BE SWITCHED ON
//BUTTONS INDICATE WHAT ELEMENTS ARE SWITCHED ON
public class PowerMenu : MonoBehaviour
{
    public Button WindButton;
    public Button IceButton;
    public Button EarthButton;
    public Button FireButton;

    public Color windColor;
    public Color iceColor;
    public Color earthColor;
    public Color fireColor;
    public int count;

    public void countElements(){
        bool[] elements = {SwordsmanController.wind, SwordsmanController.ice, SwordsmanController.earth, SwordsmanController.fire};
        count = 0;
        
        foreach (bool element in elements)
            {
                if(element == true){
                    count +=1;
                }
            }
    }
    void Update(){
        if(GlobalVars.obtainedWind){
            WindButton.interactable = true;
        } else{
            WindButton.interactable = false;
        }
        if(GlobalVars.obtainedIce){
            IceButton.interactable = true;
        } else{
            IceButton.interactable = false;
        }
        if(GlobalVars.obtainedEarth){
            EarthButton.interactable = true;
        } else{
            EarthButton.interactable = false;
        }
        if(GlobalVars.obtainedFire){
            FireButton.interactable = true;
        } else{
            FireButton.interactable = false;
        }
    }

    public void WindClick() {
        ColorBlock cbw = WindButton.colors;
        if(SwordsmanController.wind == true){
            cbw = ColorBlock.defaultColorBlock;
            WindButton.colors = cbw;
            SwordsmanController.wind = false;
        }
        else{
            countElements();
            if (count < 2){
                cbw.normalColor = windColor;
                cbw.highlightedColor = windColor;
                cbw.pressedColor = windColor;
                cbw.selectedColor = windColor;
                WindButton.colors = cbw;
                SwordsmanController.wind = true;
            }
        }
    }
    public void IceClick() {
        ColorBlock cbi = IceButton.colors;
        if(SwordsmanController.ice == true){
            SwordsmanController.ice = false;
            cbi = ColorBlock.defaultColorBlock;
            IceButton.colors = cbi;
        }
        else{
            countElements();
            if (count < 2){
                cbi.normalColor = iceColor;
                cbi.highlightedColor = iceColor;
                cbi.pressedColor = iceColor;
                cbi.selectedColor = iceColor;
                IceButton.colors = cbi;
                SwordsmanController.ice = true;
            }
        }
    }
    public void EarthClick() {
        ColorBlock cbe = EarthButton.colors;
        if(SwordsmanController.earth == true){
            SwordsmanController.earth = false;
            cbe = ColorBlock.defaultColorBlock;
            EarthButton.colors = cbe;
        }
        else{
            countElements();
            if (count < 2){
                cbe.normalColor = earthColor;
                cbe.highlightedColor = earthColor;
                cbe.pressedColor = earthColor;
                cbe.selectedColor = earthColor;
                EarthButton.colors = cbe;
                SwordsmanController.earth = true;
            }
        }
    }
    public void FireClick() {
        ColorBlock cbf = FireButton.colors;
        if(SwordsmanController.fire == true){
            SwordsmanController.fire = false;
            cbf = ColorBlock.defaultColorBlock;
            FireButton.colors = cbf;
        }
        else{
            countElements();
            if (count < 2){
                Debug.Log(count);
                cbf.normalColor = fireColor;
                cbf.highlightedColor = fireColor;
                cbf.pressedColor = fireColor;
                cbf.selectedColor = fireColor;
                FireButton.colors = cbf;
                SwordsmanController.fire = true;
            }
        }
    }
}
