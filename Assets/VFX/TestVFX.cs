using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TestVFX : MonoBehaviour
{
    public VisualEffect myVFX;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            myVFX.Play();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            myVFX.Stop();
        }
    }
}
