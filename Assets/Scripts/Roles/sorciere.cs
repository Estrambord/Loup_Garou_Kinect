using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sorciere : Player
{
    public Button Vie, Mort, Potion;
    // Start is called before the first frame update
    void Start()
    {
        role.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*public override void PlayerSpecialVote()
   { 
   // sript qui désigne la potion choisi par la sorcière et la personne sur laquelle elle l'utilise
   }*/
}
