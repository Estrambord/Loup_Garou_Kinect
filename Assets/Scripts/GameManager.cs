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
    //public GameObject Loup_Garou1;
    //public LoupGarou loup2;
    
    
    [SerializeField] private List<AvatarController> playersList;
    [SerializeField] private List<KinectManager> kinectManagers;

    /*
    [SerializeField] private AvatarController player0;
    [SerializeField] private AvatarController player1;
    [SerializeField] private AvatarController player2;
    [SerializeField] private AvatarController player3;
    [SerializeField] private AvatarController player4;
    [SerializeField] private AvatarController player5;
    */

    [Header("Roles Settings")]
    public bool randomizeNbLoupGarou = false;
    public int nbLoupGarou = 2;
    public bool randomizeOtherRoles = false;
    public bool voyante = true;
    public bool sorciere = true;
    public bool chasseur = true;


    private int nbPlayersAlive;
    private int nbLoupGarouAlive;

    private bool beforeGameStart = true;
    private bool jour = true;
    private bool rolesSet = false;
    private bool rolesDiscovered = false;
    private bool capitaineElected = false;

    [System.NonSerialized] public List<AvatarController> playersKilledThisTurn;

    private bool gameOver = false;

    #endregion

    #region Unity Base Methods
    void Start()
    {
        //playersList = new List<AvatarController>() { player0, player1, player2, player3, player4, player5 };
        playersKilledThisTurn = new List<AvatarController>();
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
            playersKilledThisTurn.Clear();

            //Son pour que tout le monde mette son masque sur ses yeux
            TourVoyante();
            TourLoupGarou();
            TourSorciere();

            for (int i = 0; i < playersKilledThisTurn.Count; i++)
            {
                KillPlayer(playersKilledThisTurn[i]);
            }

            jour = true;
        }
        else if (jour) //jour
        {
            playersKilledThisTurn.Clear();

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

            for (int i = 0; i < playersKilledThisTurn.Count; i++)
            {
                if (playersKilledThisTurn[i].Role == "chasseur")
                {
                    TourChasseur();
                }
                else if (playersKilledThisTurn[i].IsCapitaine)
                {
                    NouveauCapitaine();
                }

                KillPlayer(playersKilledThisTurn[i]);
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
        nbPlayersAlive = 6;

        if (randomizeNbLoupGarou)
        {
            nbLoupGarou = Random.Range(1, 3);
        }

        if (randomizeOtherRoles)
        {
            voyante = Random.value >= 0.5f;
            sorciere = Random.value >= 0.5f;
            chasseur = Random.value >= 0.5f;
        }

        List<int> numberList = new List<int>() { 0, 1, 2, 3, 4, 5 };
        numberList.Shuffle();

        for (int i = 0; i < 6; i++)
        {
            switch (i)
            {
                case 0:
                    playersList[numberList[i]].Role = "loup garou";
                    break;
                case 1:
                    if (nbLoupGarou == 2)
                    {
                        playersList[numberList[i]].Role = "loup garou";
                    }
                    else if(voyante)
                    {
                        playersList[numberList[i]].Role = "voyante";
                        voyante = false;
                    }
                    else if (sorciere)
                    {
                        playersList[numberList[i]].Role = "sorciere";
                        sorciere = false;
                    }
                    else if (chasseur)
                    {
                        playersList[numberList[i]].Role = "chasseur";
                        chasseur = false;
                    }
                    else
                    {
                        playersList[numberList[i]].Role = "villageois";
                    }
                    break;
                case 2:
                    if (voyante)
                    {
                        playersList[numberList[i]].Role = "voyante";
                        voyante = false;
                    }
                    else if (sorciere)
                    {
                        playersList[numberList[i]].Role = "sorciere";
                        sorciere = false;
                    }
                    else if (chasseur)
                    {
                        playersList[numberList[i]].Role = "chasseur";
                        chasseur = false;
                    }
                    else
                    {
                        playersList[numberList[i]].Role = "villageois";
                    }
                    break;
                case 3:
                    if (sorciere)
                    {
                        playersList[numberList[i]].Role = "sorciere";
                        sorciere = false;
                    }
                    else if (chasseur)
                    {
                        playersList[numberList[i]].Role = "chasseur";
                        chasseur = false;
                    }
                    else
                    {
                        playersList[numberList[i]].Role = "villageois";
                    }
                    break;
                case 4:
                    if (chasseur)
                    {
                        playersList[numberList[i]].Role = "chasseur";
                        chasseur = false;
                    }
                    else
                    {
                        playersList[numberList[i]].Role = "villageois";
                    }
                    break;
                case 5:
                    playersList[numberList[i]].Role = "villageois";
                    break;
                default:
                    break;
            }
        }

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

    /// <summary>
    /// Tue le joueur, le supprime de la liste des joueurs vivants, et change son apparence en jeu
    /// </summary>
    public void KillPlayer(AvatarController player)
    {
        playersList.Remove(player);
        player.Die();
        nbPlayersAlive = playersList.Count;
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

    /*
    public Player Get_Vote_Result(int Nb_Vote)
    {
        //script qui gère le résultat du vote quotidien
    }
    */

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