using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGameObject : MonoBehaviour
{
    private List<Player> Players;
    private bool timer;
    private bool jour;
    private Canvas timer_UI;
    public LoupGarou loup1;
    public LoupGarou loup2;
    public Voyante voyante;
    public Sorciere sorciere;
    public Chasseur chasseur;
    public Player villageois;
    public Canvas Vote;

    void Start()
    {
        Vote.enabled = false;
        timer = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            timer = true;
            StartCoroutine("Timer",10f); 
        }
        if (!timer)
        {
            Debug.Log("guigui la salope");

        }
    }

    public void GetReady()
    {
        // script qui indique que le jeu commence lorsque tous les joueurs sont pr�ts et d�tecter par le Kinect
    }

    public void DiscoverRoles()
    {
        // script qui permet aux joueurs chacun leur tour de d�couvrir leur r�le
    }

    public void ElectMayor()
    {
        // script qui permet de faire un premier vote pour �lire le maire
    }

    public void AsVoted(Canvas vote)
    {
    vote.enabled = false;
    }

    public void TourVoyante()
    {
        // script qui g�re le tour de la voyante
    }

    public void TourLoupGarou()
    {
        // script qui g�re le tour des Loups-Garous
    }
    public void TourSorciere()
    {
        // script qui g�re le tour de la sorci�re
    }
    public void TourChasseur()
    {
        // script qui g�re le tour du chasseur
    }
    public void VoteVillage()
    {
    // script qui g�re le vote quotidien
    Vote.enabled = true;
    }
    public void Nuit()
    {
        // script qui lance la nuit
    }
    public void Jour()
    {
        // script qui lance la journ�e
        jour = true;
    }
    public void DisplayUI() { }
    //public Player Get_Vote_Result(int Nb_Vote) { 
    // script qui g�re le r�sultat du vote quotidien
    //}
    public void Execution()
    {
        // script qui g�re la mort d'un player
    }
    public void SetPlayersRoles()
    {
        // script qui attribue les r�les aux players
    }
    public void Victory()
    {
        // script qui donne le vainqueur de la partie
    }
    public void ReloadPlayers()
    {
        // script qui reload la partie si il y a un probl�me de tracking
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        //Debug.Log(time +" plus tard");
        timer = false;
    }

    }
