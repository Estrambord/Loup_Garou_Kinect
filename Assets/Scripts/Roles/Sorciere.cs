using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sorciere : Player
{
    public Canvas voteSorciere;
    bool potionVie;
    bool potionMort;
    public Canvas PotionMort;
    public Button potionDeVie, potionDeMort;
    // Start is called before the first frame update
    void Start()
    {
        role.enabled = false;
        voteSorciere.enabled = false;
        PotionMort.enabled = false;
        potionVie = false;
        potionMort = false;
    }

    public void ChoisirPotion()
    {
        voteSorciere.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        potionDeVie.onClick.AddListener(TaskOnClickVie);
        potionDeMort.onClick.AddListener(TaskOnClickMort);
    }

    public void TaskOnClickVie()
    { potionVie = true; }
    public void TaskOnClickMort()
    {   
        potionMort = true;
        PotionMort.enabled = true;
    }
    public override void PlayerSpecialVote(Player player)
    { 
   // sript qui désigne la potion choisi par la sorcière et la personne sur laquelle elle l'utilise
        if(potionVie) 
        { 
            player.PlayerRevive();
            Debug.Log(player + " est réscucité");
        }
        if(potionMort) 
        { 
            player.PlayerDie();
            Debug.Log(player + " est mort");
        }   

    }
}
