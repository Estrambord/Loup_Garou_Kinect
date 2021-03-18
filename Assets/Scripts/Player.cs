using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    protected int nbVote;
    protected bool isAlive;
    protected bool isMayor;
    protected bool hasVoted;
    protected bool isAwake;
    protected bool canDie;
    protected bool roleVisible;
    protected float votingCountdown;
    public GameObject voteUI;
    public Button Reveiller, Voter;
    public TMPro.TMP_Text role;
    public TMPro.TMP_Text player;

    void Start()
    {
        role.enabled = false;
        nbVote = 0;
    }

    void Update()
    {
        
    }


    public void PlayerSleep()
    {
        //endort un player
        role.enabled = false;
        player.enabled = true;
        isAwake = false;
    }

    public void PlayerAwake()
    {
        //réveille un player
        role.enabled = true;
        player.enabled = false;
        isAwake = true;
    }

    public void PlayerDie()
    {
        //tue possiblement un joueur sous réserve d'intervention de la sorcière
        isAlive = false;
        canDie = true;
    }

    public void PlayerRevive()
    {
        //réssuscite un joueur g^râce à la sorcière
        canDie = false;
        isAlive = true;
    }

    public void PlayerStandardVote(Player player)
    {
        //le script de vote standard de tous les joueurs
        player.nbVote ++ ;
        Debug.Log("le " +player+ " a " + player.nbVote + " votes contre lui");
    }

    public virtual void PlayerSpecialVote()
    {
        //le script de vote des personnages à rôles spéciaux

    }
}
