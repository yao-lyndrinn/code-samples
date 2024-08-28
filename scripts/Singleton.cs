using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton _instance = null;
    public static Singleton Instace
    {
        get {return _instance ;}
    }
    void Awake(){
        if(_instance == null){
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else{
            Destroy(this.gameObject);
        }
    }
}
