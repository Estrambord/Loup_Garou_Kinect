using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chasseur : Player
{
    public Canvas voteChasseur;
    void Start()
    {
        role.enabled = false;
        voteChasseur.enabled = false;
    }

    void Update()
    {
        
    }
    public void Action()
    {
        voteChasseur.enabled = true;
    }
    public override void PlayerSpecialVote(Player player)
    {
        // script qui désigne le joueur que le chasseur va tuer
        voteChasseur.enabled = true;
        player.PlayerDie();
    }
}
