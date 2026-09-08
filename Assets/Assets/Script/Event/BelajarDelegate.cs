using UnityEngine;
using System;

public class BelajarDelegate : MonoBehaviour
{
    public delegate void AksiDelegate();
    void Start()
    {
        CobaDelegate();
        CobaDelegate2();
        CobaDelegate3();
    }

    void CobaDelegate()
    {
        AksiDelegate halo = PanggilHalo;
        halo();
    }

    void CobaDelegate2()
    {
        AksiDelegate halo = PanggilHalo;
        halo += PanggilWorld;
        halo();
    }

    void CobaDelegate3()
    {
        Action halo = PanggilHalo;
        halo += PanggilWorld;
        halo();
    }

    void PanggilHalo()
    {
        Debug.Log("Halo!");
    }

    void PanggilWorld()
    {
        Debug.Log("World!");
    }
}
