using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [System.NonSerialized] public bool isAlive = true;
    [System.NonSerialized] public int nbVote = 0;
    protected bool isMayor = false;
    [System.NonSerialized] public bool hasVoted = false;
    protected bool isAwake = true;
    protected bool canDie;
    protected bool roleVisible;
    protected float votingCountdown;
    public GameObject voteUI;
    public Button Reveiller, Voter;
    public TMPro.TMP_Text role;
    public TMPro.TMP_Text player;
    public Player voice = null;
    public HandClickScript handClick;

    void Start()
    {
        //role.enabled = false;
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
        //r�veille un player
        role.enabled = true;
        player.enabled = false;
        isAwake = true;
    }

    public void PlayerDie()
    {
        //tue possiblement un joueur sous r�serve d'intervention de la sorci�re
        isAlive = false;
        canDie = true;
    }

    public void PlayerRevive()
    {
        //r�ssuscite un joueur g^r�ce � la sorci�re
        canDie = false;
        isAlive = true;
    }

    public void PlayerStandardVote(Player player)
    {
        voice = player;
        player.nbVote ++ ;
        handClick.enabled = true;
        Debug.Log("Le joueur " + this + " a vot� contre le joueur " + player);
        Debug.Log("le " + player + " a " + player.nbVote + " votes contre lui");
    }

    public virtual void PlayerSpecialVote()
    {
        //le script de vote des personnages � r�les sp�ciaux

    }
}
