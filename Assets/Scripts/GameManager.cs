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
    // script qui indique que le jeu commence lorsque tous les joueurs sont pr�ts et d�tecter par le Kinect
    }

    public void Discover_roles()
    {
        // script qui permet aux joueurs chacun leur tour de d�couvrir leur r�le
    }

    public void Elect_Mayor()
    {
        // script qui permet de faire un premier vote pour �lire le maire
    }

    public void Tour_Voyante()
    {
        // script qui g�re le tour de la voyante
    }

    public void Tour_Loup_Garou()
    {
        // script qui g�re le tour des Loups-Garous
    }
    public void Tour_Sorciere() 
    {
        // script qui g�re le tour de la sorci�re
    }
    public void Tour_Chasseur()
    {
        // script qui g�re le tour du chasseur
    }
    public void Vote_village() 
    {
        // script qui g�re le vote quotidien
    }
    public void Nuit() 
    {
        // script qui lance la nuit
    }
    public void jour() 
    {
        // script qui lance la journ�e
    }
    public void Display_UI() { }
    //public Player Get_Vote_Result(int Nb_Vote) { 
    // script qui g�re le r�sultat du vote quotidien
    //}
    public void Execution() 
    {
    // script qui g�re la mort d'un player
    }
    public void Set_Players_Roles() 
    {
        // script qui attribue les r�les aux players
    }
    public void Victory() 
    {
        // script qui donne le vainqueur de la partie
    }
    public void Reload_Players()
    {
        // script qui reload la partie si il y a un probl�me de tracking
    }
}
