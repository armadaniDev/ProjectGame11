using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PemancarEvent : MonoBehaviour
{
    public static event Action TekanTombol;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Tombol ditekan, memanggil event TekanTombol");
            TekanTombol?.Invoke();
        }
    }
}