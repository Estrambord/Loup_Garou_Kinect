using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Player> Players;
    private float timer;
    private Canvas timer_UI;
    void Start()
    {

    }

    void Update()
    {

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
    }
    public void Nuit()
    {
        // script qui lance la nuit
    }
    public void Jour()
    {
        // script qui lance la journ�e
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
}

