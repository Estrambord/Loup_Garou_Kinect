using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoupGarou : Player
{
    public Canvas voteLoupGarou;
    // Start is called before the first frame update
    void Start()
    {
        Role.enabled = false;
        voteLoupGarou.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Tuer()
    {
        voteLoupGarou.enabled = true;
    }
    public override void PlayerSpecialVote(Player player)
    {
        // script qui désigne le joueur que les loups-garous vont tuer
        voteLoupGarou.enabled = true;
        player.PlayerDie();
        Debug.Log(player + " est mort");

    }
}
