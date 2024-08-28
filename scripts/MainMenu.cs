using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button newGameButton;
    public Button exitGameButton;
    public void NewGame() {
        GlobalVars.ResetAll();
        SceneManager.LoadScene("IntroScreen");
    }
    public void Restart(){
        if(GlobalVars.obtainedVit[3]){
            SceneManager.LoadScene("Level23");
        }
        else if(GlobalVars.obtainedPickaxe){
            SceneManager.LoadScene("Level10");
        }
        else if(GlobalVars.obtainedVit[1]){
            SceneManager.LoadScene("Level8");
        }
        else{
            SceneManager.LoadScene("SampleScene");
        }        
    }
    public void ExitGame(){
        Application.Quit();
    }

}
