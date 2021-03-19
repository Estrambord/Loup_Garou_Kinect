using System.Collections
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    private List<Player> Players;
    private float timer;
    private Canvas timer_UI;
    public GameObject Loup_Garou1;
    public Loup_Garou loup2;
    // Start is called before the first frame update
    public GameObject Loup_Garou1;
    public Loup_Garou loup2;
    public AvatarController[] avatars;
    public KinectManager[] kinectManagers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Get_Ready()
    { 
    // script qui indique que le jeu commence lorsque tous les joueurs sont prêts et détecter par le Kinect
    }

    public void Discover_roles()
    {
        // script qui permet aux joueurs chacun leur tour de découvrir leur rôle
    }

    public void Elect_Mayor()
    {
        // script qui permet de faire un premier vote pour élire le maire
    }

    public void Tour_Voyante()
    {
        // script qui gère le tour de la voyante
    }

    public void Tour_Loup_Garou()
    {
        // script qui gère le tour des Loups-Garous
    }
    public void Tour_Sorciere() 
    {
        // script qui gère le tour de la sorcière
    }
    public void Tour_Chasseur()
    {
        // script qui gère le tour du chasseur
    }
    public void Vote_village() 
    {
        // script qui gère le vote quotidien
    }
    public void Nuit() 
    {
        // script qui lance la nuit
    }
    public void jour() 
    {
        // script qui lance la journée
    }
    public void Display_UI() { }
    //public Player Get_Vote_Result(int Nb_Vote) { 
    // script qui gère le résultat du vote quotidien
    //}
    public void Execution() 
    {
    // script qui gère la mort d'un player
    }
    public void Set_Players_Roles() 
    {
        // script qui attribue les rôles aux players
    }
    public void Victory() 
    {
        // script qui donne le vainqueur de la partie
    }
    public void Reload_Players()
    {
        // script qui reload la partie si il y a un problème de tracking
    }
}
