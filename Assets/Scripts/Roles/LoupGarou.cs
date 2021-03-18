using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoupGarou : Player
{
    public Button Tuer;
    // Start is called before the first frame update
    void Start()
    {
        role.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void PlayerSpecialVote()
    {
    // script qui désigne le joueur que les loups-garous vont tuer

    }
}
