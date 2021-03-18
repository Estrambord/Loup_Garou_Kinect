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
        // script qui indique que le jeu commence lorsque tous les joueurs sont prêts et détecter par le Kinect
    }

    public void DiscoverRoles()
    {
        // script qui permet aux joueurs chacun leur tour de découvrir leur rôle
    }

    public void ElectMayor()
    {
        // script qui permet de faire un premier vote pour élire le maire
    }

    public void TourVoyante()
    {
        // script qui gère le tour de la voyante
    }

    public void TourLoupGarou()
    {
        // script qui gère le tour des Loups-Garous
    }
    public void TourSorciere()
    {
        // script qui gère le tour de la sorcière
    }
    public void TourChasseur()
    {
        // script qui gère le tour du chasseur
    }
    public void VoteVillage()
    {
        // script qui gère le vote quotidien
    }
    public void Nuit()
    {
        // script qui lance la nuit
    }
    public void Jour()
    {
        // script qui lance la journée
    }
    public void DisplayUI() { }
    //public Player Get_Vote_Result(int Nb_Vote) { 
    // script qui gère le résultat du vote quotidien
    //}
    public void Execution()
    {
        // script qui gère la mort d'un player
    }
    public void SetPlayersRoles()
    {
        // script qui attribue les rôles aux players
    }
    public void Victory()
    {
        // script qui donne le vainqueur de la partie
    }
    public void ReloadPlayers()
    {
        // script qui reload la partie si il y a un problème de tracking
    }
}

