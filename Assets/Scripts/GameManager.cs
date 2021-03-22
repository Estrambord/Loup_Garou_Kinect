using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    


    //PUBLIC
    /*
    public GameObject Loup_Garou1;
    public LoupGarou loup2;
    private List<Player> Players;
    private Canvas timer_UI;
    */
    private float timer;
    
    
    [SerializeField] private List<Player> playersList;
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
    public bool randomizeNbWolves = false;
    public int nbWolves = 2;
    public bool randomizeOtherRoles = false;
    public bool teller = true;
    public bool witch = true;
    public bool chasseur = true;
        


    //PRIVATE
    
    private int nbPlayersAlive;
    private int nbWolvesAlive;
    //private List<Player> Players;
    //private float timer;
    //private Canvas timer_UI;
    
    [System.NonSerialized] public List<Player> playersKilledThisTurn; 
    
    private bool skip;

    #endregion

    //_____________________________________________________________________________________________________________________________________________________

    #region GamePlay bools

    private bool beforeGameStart = true;
    private bool jour = true;

    private bool rolesSet = false;

    private List<bool> rolesDiscovered;
    private List<bool> rolesDisplayed;

    private bool mayorElected = false;

    private bool tellerTurnOngoing = false;
    private bool wolvesTurnOngoing = false;
    private bool witchTurnOngoing = false;
    private bool hunterTurnOngoing = false;

    private bool voteOngoing = false;
    private bool newMayorOngoing = false;

    private bool gameOver = false;
    private bool endPlaying = false;
    #endregion

    //_____________________________________________________________________________________________________________________________________________________

    #region Unity Base Methods
    void Start()
    {
        //playersList = new List<AvatarController>() { player0, player1, player2, player3, player4, player5 };
        playersKilledThisTurn = new List<Player>();
        rolesDiscovered = new List<bool>() { false, false, false, false, false, false };
        rolesDisplayed = new List<bool>() { false, false, false, false, false, false };
    }

    void Update()
    {
        /*
        Player eliminatedPlayer = Vote_village();
        eliminatedPlayer.enabled = false;
        */

        if (beforeGameStart) //initialisation du jeu, avant la premiere nuit
        {
            //Son d'introduction
            //Son qui dit aux joueurs de se mettre à leur place et de lever les bras pour Ready
            if (ArePlayersReady() == false)
            {
                //Afficher à l'écran que les joueurs ne sont pas prêts
            }
            if (rolesSet == false)
            {
                SetPlayersRoles();
            }
            //Son qui lance la nuit pour les joueurs
            if (rolesDiscovered[-1] == false)
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (i == 0 && !rolesDisplayed[i])
                    {
                        rolesDisplayed[i] = true;
                        DiscoverOwnRole(playersList[i], i);
                    }
                    else if (!rolesDisplayed[i] && rolesDiscovered[i-1])
                    {
                        rolesDisplayed[i] = true;
                        DiscoverOwnRole(playersList[i], i);
                    }
                }
            }
            //Son qui explique le but du jeu
            //Son qui dit que tout le monde peut relever son masque
            if (!mayorElected && !voteOngoing)
            {
                //Son election du maire
                ElectMayor();
            }
            else if (!voteOngoing)
            {
                beforeGameStart = false;
            }
        }
        else if (!jour) //nuit
        {
            playersKilledThisTurn.Clear();

            //Son pour que tout le monde mette son masque sur ses yeux

            if (teller && !tellerTurnOngoing) TellerTurn();

            if(!tellerTurnOngoing && !wolvesTurnOngoing) WolvesTurn();

            if (witch && !wolvesTurnOngoing && !witchTurnOngoing) WitchTurn();

            if (!witchTurnOngoing)
            {
                for (int i = 0; i < playersKilledThisTurn.Count; i++)
                {
                    KillPlayer(playersKilledThisTurn[i]);
                }
            }
            

            jour = true;
        }
        else if (jour) //jour
        {
            playersKilledThisTurn.Clear();

            //Son pour que tout le monde enleve son masque des yeux

            //Annonce des morts

            if (nbPlayersAlive <= 2*nbWolvesAlive - 1)
            {
                gameOver = true;
            }

            if (!gameOver && !voteOngoing)
            {
                //Son debut du vote
                VoteVillage();
                //Son fin du vote et elimination d'un joueur
            }

            if (!voteOngoing)
            {
                for (int i = 0; i < playersKilledThisTurn.Count; i++)
                {
                    if (!hunterTurnOngoing && playersKilledThisTurn[i].Role == "chasseur")
                    {
                        HunterTurn();
                    }
                    else if (!hunterTurnOngoing && !newMayorOngoing && playersKilledThisTurn[i].IsMayor)
                    {
                        NewMayor();
                    }

                    if (!hunterTurnOngoing && !newMayorOngoing)
                    {
                        KillPlayer(playersKilledThisTurn[i]);
                    }
                }
            }
            

            if (nbPlayersAlive <= 2 * nbWolvesAlive)
            {
                gameOver = true;
            }

            jour = false;
        }

        if (gameOver && !endPlaying)
        {
            endPlaying = true;

            if (nbWolvesAlive == 0)
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

    //_____________________________________________________________________________________________________________________________________________________

    #region Methodes generales

    /// <summary>
    /// Indique si tous les joueurs sont detectes par la Kinect et prets
    /// </summary>
    public bool ArePlayersReady()
    {
        for (int i = 0; i < playersList.Count; i++)
        {
            if (!playersList[i].IsPlayerReady)
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
        rolesSet = true;
        nbPlayersAlive = 6;

        if (randomizeNbWolves)
        {
            nbWolves = Random.Range(1, 3);
        }

        if (randomizeOtherRoles)
        {
            teller = Random.value >= 0.5f;
            witch = Random.value >= 0.5f;
            chasseur = Random.value >= 0.5f;
        }

        List<string> rolesList = new List<string>(){ "citizen", "wolf" };
        
        if(nbWolves == 1) { rolesList.Add("citizen"); }
        else { rolesList.Add("wolf"); }

        if (teller) rolesList.Add("teller");
        else rolesList.Add("citizen");

        if (witch) rolesList.Add("witch");
        else rolesList.Add("citizen");

        if (chasseur) rolesList.Add("hunter");
        else rolesList.Add("citizen");

        rolesList.Shuffle();

        //Affecter les roles aux joueurs
        for(int i = 0; i < playersList.Count; i++)
        {
            playersList[i].Role = rolesList[i];
        }
    }

    /// <summary>
    /// Permet à un joueur donne de decouvrir son role
    /// </summary>
    public void DiscoverOwnRole(Player player, int i)
    {
        //Son qui demande à un joueur specifique d'enlever son masque

        //Afficher le rôle du joueur
        //Son qui lui demande d'interagir avec son role pour passer à la suite
        if (!skip) //Mouvement de la main pour skip ?
        {
            player.roleText.enabled = true;
            //Afficher UI
        }
        else
        {
            player.roleText.enabled = false;
        }
        skip = false;
        //Son qui lui demande de remettre son masque

        rolesDiscovered[i] = true;
    }

    /// <summary>
    /// Lance l'election du maire
    /// </summary>
    public void ElectMayor()
    {
        voteOngoing = true;

        if ( ! mayorElected )
        {
            Player newMayor = VoteVillage();
            mayorElected = true;
        }

        voteOngoing = false;
    }

    /// <summary>
    /// Déclenche un vote d'elimination
    /// </summary>
    public Player VoteVillage()
    {
        return playersList[0];
    }

    /// <summary>
    /// Tue le joueur et change son apparence en jeu
    /// </summary>
    public void KillPlayer(Player player)
    {
        player.Die();
        nbPlayersAlive--;
    }
    #endregion

    //_____________________________________________________________________________________________________________________________________________________

    #region Methodes specifiques a un joueur
    /// <summary>
    /// Lance les actions de la Voyante
    /// </summary>
    public void TellerTurn()
    {
        tellerTurnOngoing = true;

        //DO something

        Debug.Log("tour de la Voyante");

        tellerTurnOngoing = false;
    }

    /// <summary>
    /// Lance le vote du village
    /// </summary>
    public Player Vote_village()
    {
        List<Player> votedPlayers = new List<Player>();

        Player chosenPlayer = null;
        timer = 60f;
        int nbVoters = 0;
        int nbVotes = 0;
        int maxVotes = 0;

        for (int i = 0; i < playersList.Count; i++)
        {
            if (playersList[i].isAlive)
            {
                //Players[i].handClick.enabled = true;
                nbVoters++;
            }
        }

        while (nbVotes < 2)
        //while (nbVotes < nbVoters)
        {
            for (int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i].isAlive && !playersList[i].hasVoted)
                {
                    if (playersList[i].voice != null)
                    {
                        nbVotes++;
                        playersList[i].hasVoted = true;
                    }
                }
            }
        }
        foreach (Player player in playersList)
        {
            if (player.nbVote > maxVotes)
            {
                chosenPlayer = player;
                maxVotes = player.nbVote;
            }
            else if (player.nbVote == maxVotes)
            {
                chosenPlayer = null;
                Debug.Log("Deux joueurs ont le même nombre de voix");
            }
        }
        foreach (Player player in playersList)
        {
            player.hasVoted = false;
            player.nbVote = 0;
        }
        return chosenPlayer;
    }

    /// <summary>
    /// Lance le tour des Loups Garous
    /// </summary>
    public void WolvesTurn()
    {
        wolvesTurnOngoing = true;

        //DO something

        Debug.Log("tour des Loups Garous");

        wolvesTurnOngoing = false;

    }

    /// <summary>
    /// Lance le tour de la Sorciere
    /// </summary>
    public void WitchTurn() 
    {
        witchTurnOngoing = true;

        //DO something

        Debug.Log("tour de la Socriere");

        witchTurnOngoing = false;
    }

    /// <summary>
    /// Lance le tour du Chasseur
    /// </summary>
    public void HunterTurn()
    {
        hunterTurnOngoing = true;

        //DO something

        Debug.Log("tour du Chasseur");

        hunterTurnOngoing = false;
    }

    /// <summary>
    /// Le Maire choisit son successeur à sa mort
    /// </summary>
    public void NewMayor()
    {
        newMayorOngoing = true;

        //DO something

        Debug.Log("Choix d'un nouveau maire");

        newMayorOngoing = false;
    }
    #endregion

    //_____________________________________________________________________________________________________________________________________________________

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

    /*public Player Get_Vote(Player player, KinectManager kinectManager)
	{
        Player votedPlayer = null;
        if(kinectManager.GetComponent<HandClickScript>().enabled == true)
		{
            votedPlayer = kinectManager.GetComponent<HandClickScript>().clickedObject;
		}
        //PlayerStandardVote(player);
        if(votedPlayer != null)
		{
            player.hasVoted = true;
            kinectManager.GetComponent<HandClickScript>().enabled = false;
            votedPlayer.nbVote += 1;
        }
        //Debug.Log("le " + votedPlayer + " a " + votedPlayer.nbVote + " votes contre lui");
        return votedPlayer;
	}*/
    #endregion
}
    

