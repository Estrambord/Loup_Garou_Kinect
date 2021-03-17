using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    protected int Nb_Vote;
    protected bool Alive;
    protected bool Mayor;
    protected bool Has_Voted;
    protected bool Awaken;
    protected bool Can_Die;
    protected bool Role_Visible;
    protected float Voting_Countdown;
    protected Canvas Vote_UI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Player_Sleep()
    {
        //endort un player
        Awaken = false;
    }

    public void Player_Awake()
    {
        //réveille un player
        Awaken = true;
    }

    public void Player_Die()
    {
        //tue possiblement un joueur sous réserve d'intervention de la sorcière
        Alive = false;
        Can_Die = true;
    }

    public void Player_Revive()
    {
        //réssuscite un joueur g^râce à la sorcière
        Can_Die = false;
        Alive = true;
    }

    public void Player_Standard_Vote()
    {
        //le script de vote standard de tous les joueurs
        Has_Voted = true;
    }

    /*public virtual void Player_Special_Vote(Player player, )
    {
        //le script de vote des personnages à rôles spéciaux

    }*/
}
