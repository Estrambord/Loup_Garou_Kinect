using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables

    private List<Player> Players;
    private float timer;
    private Canvas timer_UI;
    public GameObject Loup_Garou1;
    public LoupGarou loup2;
    public List<AvatarController> playersList;
    public KinectManager[] kinectManagers;

    private int nbPlayersAlive;
    private int nbLoupGarouAlive;

    private bool beforeGameStart = true;
    private bool jour = true;
    private bool rolesSet = false;
    private bool rolesDiscovered = false;
    private bool capitaineElected = false;

    public AvatarController playerKilled;

    private bool gameOver = false;

    #endregion

    #region Unity Base Methods
    void Start()
    {
        playersList = new List<AvatarController>();
    }

    void Update()
    {
        if (beforeGameStart) //initialisation du jeu, avant la premiere nuit
        {
            //Son d'introduction
            //Son qui dit aux joueurs de se mettre à leur place et de lever les bras pour Ready
            while (ArePlayersReady() == false)
            {
                //Afficher à l'écran que les joueurs ne sont pas prêts
            }
            if (rolesSet == false)
            {
                SetPlayersRoles();
            }
            if (rolesDiscovered == false)
            {
                //Son qui lance la nuit pour les joueurs
                for (int i = 0; i < playersList.Count; i++)
                {
                    DiscoverOwnRole(playersList[i]);
                }
                //Son qui explique le but du jeu
                //Son qui dit que tout le monde peut relever son masque
                rolesDiscovered = true;
            }
            if (!capitaineElected)
            {
                //Son election du maire
                ElectionCapitaine();
            }

        }

        /*
        else if (tourDeChauffe)
        {
            EST CE QU'ON FAIT UN TOUR POUR RIEN ? SYLVAIN AVAIT PROPOSE MAIS JE TROUVE QUE CA FERAIT PERDRE TROP DE TEMPS AUX JOUEURS
        }
        */

        else if (!jour) //nuit
        {
            //Son pour que tout le monde mette son masque sur ses yeux
            TourVoyante();
            TourLoupGarou();
            TourSorciere();
            jour = true;
        }
        else if (jour) //jour
        {
            //Son pour que tout le monde enleve son masque des yeux

            //Annonce des morts (fonction ? plusieurs ifs a la suite ?)

            if (nbPlayersAlive <= 2*nbLoupGarouAlive)
            {
                gameOver = true;
            }

            if (!gameOver)
            {
                //Son debut du vote
                VoteVillage();
                //Son fin du vote et elimination d'un joueur
            }

            if (playerKilled.Role == "chasseur")
            {
                TourChasseur();
            }
            else if (playerKilled.IsCapitaine)
            {
                NouveauCapitaine();
            }
            jour = false;
        }

        if (gameOver)
        {
            if (nbLoupGarouAlive == 0)
            {
                //Son Bonne fin
            }
            else
            {
                //Son mauvaise fin
            }
        }
    }
    #endregion

    #region Methodes generales

    /// <summary>
    /// Indique si tous les joueurs sont detectes par la Kinect et prets
    /// </summary>
    public bool ArePlayersReady()
    {
        for (int i = 0; i < playersList.Count; i++)
        {
            if (!playersList[i].IsAvatarReady)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Attribue un role a chaque joueur aleatoirement
    /// </summary>
    public void SetPlayersRoles()
    {
        //Affecter les roles aux joueurs
        rolesSet = true;
    }

    /// <summary>
    /// Permet à un joueur donne de decouvrir son role
    /// </summary>
    public void DiscoverOwnRole(AvatarController player)
    {
        //Son qui demande à un joueur specifique d'enlever son masque

        //Afficher le rôle du joueur

        //Son qui lui demande d'interagir avec son role pour passer à la suite

        //Son qui lui demande de remettre son masque
    }

    /// <summary>
    /// Lance l'election du maire
    /// </summary>
    public void ElectionCapitaine()
    {

    }

    /// <summary>
    /// Déclenche un vote d'elimination
    /// </summary>
    public void VoteVillage()
    {

    }
    #endregion

    #region Methodes specifiques a un joueur
    /// <summary>
    /// Lance les actions de la Voyante
    /// </summary>
    /// 
    public void TourVoyante()
    {

    }

    /// <summary>
    /// Lance le tour des Loups Garous
    /// </summary>
    public void TourLoupGarou()
    {

    }

    /// <summary>
    /// Lance le tour de la Sorciere
    /// </summary>
    public void TourSorciere() 
    {

    }

    /// <summary>
    /// Lance le tour du Chasseur
    /// </summary>
    public void TourChasseur()
    {

    }

    public void NouveauCapitaine()
    {

    }
    #endregion

    #region Methodes que je sais pas si elles seront utiles ou pas
    public void DisplayUI() { }

    //public Player Get_Vote_Result(int Nb_Vote) { 
    // script qui gère le résultat du vote quotidien
    //}

    public void Execution() 
    {
    // script qui gère la mort d'un player
    }
    
    


    public void Victory() 
    {
        // script qui donne le vainqueur de la partie
    }
    public void Reload_Players()
    {
        // script qui reload la partie si il y a un problème de tracking
    }
    #endregion
}
