using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public static bool obtainedWind = false;

    public static bool obtainedIce = false;
    public static bool obtainedEarth = false;
    public static bool obtainedFire = false;
    public static bool obtainedCast = false;
    public static bool obtainedDash = false;

    public static bool clearedBeast = false;
    public static bool clearedZeya = false;
    public static bool obtainedElement = false;
    public static bool obtainedPickaxe = false;
    public static bool obtainedWindSpell = false;
    public static bool obtainedIceSpell = false;

    public static bool[] obtainedVit = new bool[10];
    public static bool[] obtainedMana = new bool[10];
    public static bool[] clearedCrystal = new bool[10];

    public static void ResetVit(){
        for (int i = 0; i < 10; i++){
            obtainedVit[i] = false;
        }
    }

    public static void ResetMana(){
        for (int i = 0; i < 10; i++){
            obtainedMana[i] = false;
        }
    }
    public static void ResetCrystal(){
        for (int i = 0; i < 10; i++){
            clearedCrystal[i] = false;
        }
    }
    public static void ResetAll(){
        ResetVit();
        ResetMana();
        ResetCrystal();
        obtainedWind = false;
        obtainedIce = false;
        obtainedEarth = false;
        obtainedFire = false;
        obtainedCast = false;
        obtainedDash = false;
        clearedBeast = false;
        clearedZeya = false;
        obtainedElement = false;
        obtainedPickaxe = false;
        obtainedWindSpell = false;
        obtainedIceSpell = false;
    }
}
