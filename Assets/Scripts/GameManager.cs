
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    #region Variables
    //PUBLIC
    //[SerializeField] private List<KinectManager> kinectManagers;
    [System.NonSerialized] public List<Player> playersKilledThisTurn;
    public List<Player> Players;
    [SerializeField] private List<Player> playersList;
    private float timer;
    private Canvas timer_UI;

    [Header("Roles Settings")]
    public bool randomizeNbWolves = false;
    public int nbWolves = 2;
    public bool randomizeOtherRoles = false;
    public bool teller = true;
    public bool witch = true;
    public bool hunter = true;
    public List<GameObject> remainingPotions;

    //PRIVATE
    private int nbPlayersAlive;
    private int nbWolvesAlive;
    private bool skip;

    #region UI
    public Text timerText;
	#endregion


	#region Voting Variables
	private int nbVoters = 0;
    private int nbVotes = 0;
    private bool everybodyVoted = false;
    private bool votingTime = false;
    private bool votingTimer = false;
    private Player eliminatedPlayer;
    #endregion

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

    #endregion


    #region Unity Base Methods
    void Start()
    {
        playersKilledThisTurn = new List<Player>();
        rolesDiscovered = new List<bool>() { false, false, false, false, false, false };
        rolesDisplayed = new List<bool>() { false, false, false, false, false, false };
    }



    void Update()
    {

        #region Vote du village
        VoteVillage();
		
        #endregion


        

        /*
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
                    else if (!rolesDisplayed[i] && rolesDiscovered[i - 1])
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

            if (!tellerTurnOngoing && !wolvesTurnOngoing) WolvesTurn();

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

            if (nbPlayersAlive <= 2 * nbWolvesAlive - 1)
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
                    if (!hunterTurnOngoing && playersKilledThisTurn[i].Role == "hunter")
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
        }*/
    }
    #endregion


    #region Methodes generales

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
            hunter = Random.value >= 0.5f;
        }

        List<string> rolesList = new List<string>() { "citizen", "wolf" };

        if (nbWolves == 1) { rolesList.Add("citizen"); }
        else { rolesList.Add("wolf"); }

        if (teller) rolesList.Add("teller");
        else rolesList.Add("citizen");

        if (witch) rolesList.Add("witch");
        else rolesList.Add("citizen");

        if (hunter) rolesList.Add("hunter");
        else rolesList.Add("citizen");

        rolesList.Shuffle();

        //Affecter les roles aux joueurs
        for (int i = 0; i < playersList.Count; i++)
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

        if (!mayorElected)
        {
            Player newMayor = GetVoteResult();
            mayorElected = true;
        }

        voteOngoing = false;
    }

    /// <summary>
    /// Déclenche un vote d'elimination
    /// </summary>


    /// <summary>
    /// Tue le joueur et change son apparence en jeu
    /// </summary>
    public void KillPlayer(Player player)
    {
        player.Die();
        nbPlayersAlive--;
    }


    /// <summary>
    /// Permet le vote des joueurs vivants
    /// </summary>
    /// <returns></returns>
    public Player GetVoteResult()
    {
        Player chosenPlayer = null;
        timer = 60f;
        int maxVotes = 0;
        nbVoters = 0;

        //Compte le nombre de participants au vote et active le vote pour chacun d'eux
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].isAlive)
            {
                nbVoters++;
				if (!Players[i].hasVoted)
				{
                    Players[i].ActivateVote();
                }
            }
        }
        //Vérifie le nombre de votes déjà réalisés
        if(nbVotes < nbVoters)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                //Vérifie si un joueur vient de voter et désactive le vote pour ce joueur le cas échéant
                if (Players[i].isAlive && !Players[i].hasVoted)
                {
                    if (Players[i].voice != null)
                    {
                        nbVotes++;
                        Players[i].hasVoted = true;
                        Players[i].DeactivateVote();
                        Debug.Log("Number of votes : " + nbVotes);
                    }
                }
            }
        }
        //Vérifie si tout le monde a voté
        everybodyVoted = true;
        foreach (Player player in Players)
        {
            if (player.isAlive && player.hasVoted == false)
            {
                everybodyVoted = false;
            }
        }
        //Trouve le joueur ayant le plus de voix contre lui
        if (everybodyVoted)
        {
            foreach (Player player in Players)
            {
                if (player.nbVote > maxVotes)
                {
                    chosenPlayer = player;
                    maxVotes = player.nbVote;
                }
                else if (player.nbVote == maxVotes)
                {
                    chosenPlayer = null;
                }
            }
        }
        else
        {
            chosenPlayer = null;
        }
        return chosenPlayer;
    }

    public void VoteVillage()
	{
        for (int i = 2; i < Players.Count; i++)
        {
            Players[i].Die();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            votingTime = true;
            Debug.Log("It's Voting Time !");
            eliminatedPlayer = null;
        }

        
        if (votingTime)
        {
            if (!voteOngoing)
            {
                votingTimer = true;
                voteOngoing = true;
                timerText.gameObject.SetActive(true);
                StartCoroutine("VotingTimer", 10f);
                Debug.Log("Voting Timer started");
            }

            //Get result of the vote
            if (!votingTimer || everybodyVoted)
            {
                if (eliminatedPlayer != null)
                {
                    Debug.Log("Everybody voted : Player " + eliminatedPlayer + "Was eliminated");
                    eliminatedPlayer.enabled = false;
                }
                else
                {
                    Debug.Log("Everybody voted : Nobody was eliminated");
                }
                votingTime = false;
                Debug.Log("Vote ended");
                voteOngoing = false;
                timerText.gameObject.SetActive(false);
            }
            //Continue vote
            else if (!everybodyVoted)
            {
                //Debug.Log("Voting time");
                eliminatedPlayer = GetVoteResult();
            }
        }
    }

    public void ResetVotes()
    {
        foreach (Player player in Players)
        {
            player.hasVoted = false;
            player.nbVote = 0;
            player.voice = null;
        }
    }

    #endregion

    #region Methodes specifiques a un joueur

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
    /// Lance le tour du hunter
    /// </summary>
    public void HunterTurn()
    {
        hunterTurnOngoing = true;

        //DO something

        Debug.Log("tour du hunter");

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

	#region Coroutines
    IEnumerator VotingTimer(float time)
    {
		for (int i = (int)time; i >=0; i--)
		{
            yield return new WaitForSeconds(1);
            timerText.text = i.ToString() ;
		}
        
        votingTimer = false;
        Debug.Log("Voting Timer OFF");
    }


    #endregion;
}

