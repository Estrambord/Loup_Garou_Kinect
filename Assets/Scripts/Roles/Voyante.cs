using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Voyante : Player
{
    public Canvas voteVoyante;

    void Start()
    {
        voteVoyante.enabled = false;
        role.enabled = false;
    }

    void Update()
    {
        
    }
    
    public void Voir()
    {
        voteVoyante.enabled = true;
    }
    public override void PlayerSpecialVote(Player player)
    {
        player.PlayerAwake();
        Debug.Log(player + "(" + player.role + ") est mort");
    }
}
