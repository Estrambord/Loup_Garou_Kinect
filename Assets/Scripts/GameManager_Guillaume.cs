
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Guillaume : MonoBehaviour
{

    #region Variables
    //PUBLIC
    //[SerializeField] private List<KinectManager> kinectManagers;
    [System.NonSerialized] public List<Player> playersKilledThisTurn;
    public List<Player> Players;
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
    private bool jour = false;

    private bool rolesSet = false;

    //private List<bool> rolesDisplayed;

    private bool mayorElected = false;

    private bool tellerTurnOngoing = true;
    private bool tellerTimer = false;
    private Player playerTellerClicked = null;
    private bool wolvesTurnOngoing;
    private bool wolvesTimer = true;
    private Player playerWolvesChose = null;
    private bool witchTurnOngoing = false;
    private bool hunterTurnOngoing = false;
    private bool killTurn = false;

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
        //rolesDisplayed = new List<bool>() { false, false, false, false, false, false };

        if (!teller)
        {
            tellerTurnOngoing = false;
            wolvesTurnOngoing = true;
        }
    }



    void Update()
    {
        /*
        #region Vote du village
        VoteVillage();
		
        #endregion
        */


        

       
        if (beforeGameStart) //initialisation du jeu, avant la premiere nuit
        {
            //Son d'introduction
            //Son qui dit aux joueurs de se mettre à leur place et de lever les bras pour Ready
            if (ArePlayersReady() == false)
            {
                //Afficher à l'écran que les joueurs ne sont pas prêts

                Debug.Log("Players not ready");
            }

            if (rolesSet == false && ArePlayersReady())
            {
                SetPlayersRoles();
                Debug.Log("Roles are set up");
                //Son qui lance la nuit pour les joueurs
            }

            if (Players[Players.Count - 1].RoleDiscovered == false && rolesSet)
            {
                Debug.Log("Discovering roles one by one");

                for (int i = 0; i < Players.Count; i++)
                {
                    //if (i == 0 && !rolesDisplayed[i])
                    if (i == 0)
                    {
                        DiscoverOwnRole(Players[i], i);
                    }
                    //else if (!rolesDisplayed[i] && Players[i - 1].RoleDiscovered)
                    else if (Players[i - 1].RoleDiscovered)
                    {
                        DiscoverOwnRole(Players[i], i);
                    }
                }
                //Son qui explique le but du jeu
                //Son qui dit que tout le monde peut relever son masque
            }

            if (Players[Players.Count - 1].RoleDiscovered && !mayorElected && !voteOngoing)
            {
                Players[Players.Count - 1].SetUI("citizen :)");
                Debug.Log("Election du maire ALEATOIRE");
                //Son election du maire
                //ElectMayor();
                int maireALEATOIRE = Random.Range(0, 6);
                Players[maireALEATOIRE].MayorYellow();
                mayorElected = true;
                
            }
            
            if (mayorElected && !voteOngoing)
            {
                beforeGameStart = false;

                Debug.Log("Game launched");
            }
        }
        else if (!beforeGameStart && !jour) //nuit
        {
            Debug.Log("Nuit");

            //Son pour que tout le monde mette son masque sur ses yeux

            if (teller && tellerTurnOngoing) TellerTurn();

            else if (!tellerTurnOngoing && wolvesTurnOngoing) WolvesTurn();

            else if (witch && !wolvesTurnOngoing && witchTurnOngoing) WitchTurn();

            else if (!wolvesTurnOngoing && killTurn)
            {
                for (int i = 0; i < playersKilledThisTurn.Count; i++)
                {
                    KillPlayer(playersKilledThisTurn[i]);
                }
                playersKilledThisTurn.Clear();
                jour = true;
            }


            
        }
        else if (!beforeGameStart && jour) //jour
        {
            

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
                playersKilledThisTurn.Clear();
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

    #region Methodes generales

    public bool ArePlayersReady()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (!Players[i].IsPlayerReady)
            {
                return false;
            }
        }
        return true;
    }

    public void SetPlayersRoles()
    {
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
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].Role = rolesList[i];
        }

        rolesSet = true;
    }

    public void DiscoverOwnRole(Player player, int i)
    {
        //Son qui demande à un joueur specifique d'enlever son masque

        //Afficher le rôle du joueur
        //Son qui lui demande d'interagir avec son role pour passer à la suite
        
        //rolesDisplayed[i] = true;
        
        player.SetRoleUI();

        if (player.RoleDiscovered)
        {
            player.SetUI("citizen :)");
        }
        
        //if (!skip) //Mouvement de la main pour skip ?
        //{
        //    player.roleText.enabled = true;
        //    //Afficher UI
        //}
        //else
        //{
        //    player.roleText.enabled = false;
        //}
        //skip = false;



        //Son qui lui demande de remettre son masque
    }

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

    public void KillPlayer(Player player)
    {
        player.Die();
        nbPlayersAlive--;
    }

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
            //Debug.Log()
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

    public Player GetIndividualVote(Player player)
	{
        Player chosenPlayer = null;
        if(!player.hasVoted)
		{
            player.ActivateVote();
        }
		if (! player.hasVoted && player.voice != null)
		{
            chosenPlayer = player.voice;
            player.hasVoted = true;
		}
        if (player.isAlive && !player.hasVoted)
        {
            if (player.voice != null)
            {
                player.hasVoted = true;
                player.DeactivateVote();
                chosenPlayer = player.voice;
            }
        }
        return chosenPlayer;
	}

    public Player GetWolvesVote()
	{
        Player chosenPlayer = null;
        List<Player> wolves = new List<Player>();
        bool samePlayer = false;
        foreach (Player player in Players)
		{
            if(player.isAlive && player.Role == "wolf")
			{
                wolves.Add(player);
			}
		}
        //Si un seul loup, choix individuel
        if(wolves.Count == 1)
		{
            chosenPlayer = GetIndividualVote(wolves[0]);
		}
        //Si deux loups le joueur choisi est validé seulement si les 2 ont voté pour le meme joueur sinon le vote est réactivé
        else if(wolves.Count == 2)
		{
            if(wolves[0].hasVoted && wolves[1].hasVoted)
			{
                if(wolves[0].voice == wolves[1].voice)
				{
                    samePlayer = true;
                    chosenPlayer = wolves[0].voice;
                }
				else
				{
                    ResetVotes();
				}
			}
            foreach(Player wolf in wolves)
			{
                if(!wolf.hasVoted)
				{
                    wolf.ActivateVote();
                }
                else if (wolf.hasVoted)
				{
                    wolf.DeactivateVote();
                }
            }
		}
		else
		{
            Debug.Log("Bad number of wolves");
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
                StartCoroutine("VotingTimer", 20f);
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

    public void TellerTurn()
    {
        Debug.Log("tour de la Voyante");

        RaycastHit hit;

        if (Input.GetMouseButtonDown(0) && !tellerTimer)
        {
            //Debug.Log("Raycast !");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                playerTellerClicked = hit.transform.GetComponent<Player>();
                //Debug.Log("You selected the " + hit.transform.name + ", his role is " + playerTellerClicked.Role);
                playerTellerClicked.SetRoleUI();
                tellerTimer = true;
                StartCoroutine("TellerTimer", 5f);
            }
        }

        if (!tellerTimer && playerTellerClicked != null)
        { 
            //Debug.Log("Fin tour de la Voyante");
            playerTellerClicked.SetUI("citizen :)");
            tellerTurnOngoing = false;
            wolvesTurnOngoing = true;
        }    
    }

    public void WolvesTurn()
    {
        Debug.Log("tour des Loups Garous");

        StartCoroutine("WolvesTimer", 10f);

        RaycastHit hit;

        if (Input.GetMouseButtonDown(0) && playersKilledThisTurn.Count == 0)
        {
            Debug.Log("Raycast !");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                playerWolvesChose = hit.transform.GetComponent<Player>();
                if (playerWolvesChose.Role != "wolf")
                {
                    Debug.Log("You selected the " + hit.transform.name + ", he will be eaten by the werewolves tonight");
                    for (int i = 0; i < Players.Count; i++)
                    {
                        if (playerWolvesChose == Players[i])
                        {
                            playersKilledThisTurn.Add(Players[i]);
                        }
                    }
                }
                else
                {
                    Debug.Log("You can't eat another wolf !");
                }
            }
        }

        if (!wolvesTimer || playersKilledThisTurn.Count == 1)
        {
            if (playersKilledThisTurn.Count == 0)
            {
                Debug.Log("Nobody was eaten tonight");
            }
            Debug.Log("Fin tour des Loups Garous");
            wolvesTurnOngoing = false;
            if (witch) witchTurnOngoing = true;
            else killTurn = true;
            wolvesTimer = true;
        }
    }

    public void WitchTurn()
    {
        witchTurnOngoing = true;

        //DO something

        Debug.Log("tour de la Sorciere");

        witchTurnOngoing = false;
    }

    public void HunterTurn()
    {
        hunterTurnOngoing = true;

        //DO something

        Debug.Log("tour du Chasseur");

        hunterTurnOngoing = false;
    }

    public void NewMayor()
    {
        newMayorOngoing = true;

        //DO something

        Debug.Log("Choix d'un nouveau maire");

        newMayorOngoing = false;
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

    IEnumerator TellerTimer(float time)
    {
        //Debug.Log("TellerTimer started");
        yield return new WaitForSeconds(time);
        tellerTimer = false;
        //Debug.Log("TellerTimer finished");
    }

    IEnumerator WolvesTimer(float time)
    {
        Debug.Log("Wolves Timer started");
        yield return new WaitForSeconds(time);
        //wolvesTimerFinished = true;
        wolvesTimer = false;
        Debug.Log("Wolves Timer finished");
    }
    #endregion
}

