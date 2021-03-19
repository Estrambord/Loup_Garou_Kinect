using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chasseur : Player
{
    public Button Tuer;
    void Start()
    {
        role.enabled = false;
    }

    void Update()
    {
        
    }
    /*public override void PlayerSpecialVote()
    {
        // script qui désigne le joueur que le chasseur va tuer
    }*/
}
