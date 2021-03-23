using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NbWolvesAlive : MonoBehaviour
{
    public GameManager_Guillaume GM;
    public Text nbWolves;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nbWolves.text = "NbWolvesAlive : " + GM.nbWolvesAlive.ToString();
    }
}
