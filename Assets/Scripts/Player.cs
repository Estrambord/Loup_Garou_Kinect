using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private string role;
    public string Role
    {
        get { return role; }
        set { role = value; }
    }

    protected int nbVote;
    protected bool isAlive;
    protected bool isMayor;
    protected bool hasVoted;
    protected bool isAwake;
    protected bool canDie;
    protected bool roleVisible;
    protected float votingCountdown;
    public List<GameObject> remainingPotions;
    public List<string> remainingPotionsString;
    public GameObject voteUI;
    public TMPro.TMP_Text role;
    public TMPro.TMP_Text player;
    public GameObject marmite;


    void Start()
    {
        role.enabled = false;
        nbVote = 0;
        hasVoted = false;
        isAwake = false;
        roleVisible = false;
        canDie = false;
        remainingPotionsString.Add("life", "dead");
    }

    void Update()
    {
        
    }
    

    public void Sleep()
    {
        //endort un player
        role.enabled = false;
        player.enabled = true;
        isAwake = false;
    }

    public void Awake()
    {
        //réveille un player
        role.enabled = true;
        player.enabled = false;
        isAwake = true;
    }

    public void Die()
    {
        //tue possiblement un joueur sous réserve d'intervention de la sorcière
        isAlive = false;
        canDie = true;
    }

    public void Revive()
    {
        //réssuscite un joueur grâce à la sorcière
        canDie = false;
        isAlive = true;
    }

    public void StandardVote(Player player)
    {
        //le script de vote standard de tous les joueurs
        player.nbVote ++ ;
        Debug.Log("le " +player+ " a " + player.nbVote + " votes contre lui");
    }
    



    //Méthodeà déplacer dans le GameManager pour avoir accès à tous les joueurs
    public virtual void SpecialVote(Player player)
    {

        switch (this.role)
        {
            case ("witch"):
                if (remainingPotions.Contains("life")){
                    //Activer les potions grabbable
                    remainingPotions[0].enabled = true;
                }
                if (remainingPotions.Contains("dead"))
                {
                    //Activer les potions grabbable
                    remainingPotions[1].enabled = true;
                }
                //Demander de choisir une potion (son)
                //Au trigger de la marmite faire
                if (marmite.isChosen)
                {
                    if(marmite.chosen == "life")
                    {
                        //foreach(Player player in Players){
                        //  if player.canDie == true{
                        //      player.canDie = false;
                        //  }
                        //}
                    }
                    else
                    {
                        ActivateVote();
                        //Tuer player sur qui est la voix
                        DeactivateVote();
                    }
                }

                //Si la potion est lâchée, réinitialiser la position (GrabScript)

                break;
            case ("hunter"):
                break;
            case ("teller"):
                break;
            case ("wolf"):
                break;
        }
        //le script de vote des personnages à rôles spéciaux

    }

    public void ActivateVote()
    {
        //Activer le vote à la main
    }

    public void DeactivateVote()
    {
        //Activer le vote à la main
    }

}
