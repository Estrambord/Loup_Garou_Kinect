using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Variables

    public bool IsPlayerReady { get; set; } = false;


    [System.NonSerialized] public bool isAlive = true;
    [System.NonSerialized] public int nbVote = 0;
    protected bool isMayor = false;
    [System.NonSerialized] public bool hasVoted = false;
    protected bool isAwake = true;

    public string Role { get; set; } = "citizen";

    protected int nbVote;
    public bool IsMayor { get; set; } = false;
    protected bool hasVoted;
    protected bool isAwake;
    protected bool canDie;
    protected bool roleVisible;
    protected float votingCountdown;
    public List<GameObject> remainingPotions;
    public List<string> remainingPotionsString;
    public GameObject voteUI;
    public TMPro.TMP_Text roleText;
    public TMPro.TMP_Text player;
    public Player voice = null;
    public HandClickScript handClick;
    public GameObject marmite;

    #endregion

    #region Unity Base Methods
    void Start()
    {
        //role.enabled = false;
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
    #endregion

    public void Sleep()
    {
        //endort un player
        Role.enabled = false;
        player.enabled = true;
        isAwake = false;
    }

    public void Awake()
    {
        //r�veille un player
        Role.enabled = true;
        player.enabled = false;
        isAwake = true;
    }

    public void Die()
    {
        //tue possiblement un joueur sous r�serve d'intervention de la sorci�re
        isAlive = false;
        canDie = true;
    }

    public void Revive()
    {
        //r�ssuscite un joueur gr�ce � la sorci�re
        canDie = false;
        isAlive = true;
    }

    public void StandardVote(Player player)
    {
        voice = player;
        player.nbVote ++ ;
        handClick.enabled = true;
        Debug.Log("Le joueur " + this + " a vot� contre le joueur " + player);
        Debug.Log("le " + player + " a " + player.nbVote + " votes contre lui");
    }
    



    //M�thode� d�placer dans le GameManager pour avoir acc�s � tous les joueurs
    public virtual void SpecialVote(Player player)
    {

        switch (this.Role)
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

                //Si la potion est l�ch�e, r�initialiser la position (GrabScript)

                break;
            case ("hunter"):
                break;
            case ("teller"):
                break;
            case ("wolf"):
                break;
        }
        //le script de vote des personnages � r�les sp�ciaux

    }

    public void ActivateVote()
    {
        //Activer le vote � la main
    }

    public void DeactivateVote()
    {
        //Activer le vote � la main
    }

}
