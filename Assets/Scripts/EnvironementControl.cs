using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironementControl : MonoBehaviour
{
    //Attach to the directionalLight
    [SerializeField] private bool isDay = true;
    [SerializeField, Range(0, 24)]  private float time = 13.9f;
    [SerializeField] private float sunSpeed = 0.01f;
    private bool switchTime = false;

    public bool SwitchTime
    {
        get { return switchTime; }
        set { switchTime = value; }
    }

    private void Update()
    {
        //SwitchTime = true to launch sun sequence
        if (switchTime && isDay)
        {
            time += sunSpeed;
            if(time > 19.1)
            {
                switchTime = false;
                isDay = false;
            }
        }

        if (switchTime && !isDay)
        {
            time -= sunSpeed;
            if (time < 14)
            {
                switchTime = false;
                isDay = true;
            }
        }

        transform.localRotation = Quaternion.Euler(new Vector3((time / 24 * 360f) - 90f, 170f, 0));
    }
}
