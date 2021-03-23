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
    [System.NonSerialized] public List<Player> playersKilledLastTurn;
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

    //PRIVATE
    public int nbPlayersAlive;
    public int nbWolvesAlive;
    private bool skip;

    #region UI
    public Text timerText;
    public Text NotReadyText;
    public Marmite chaudron;
    public List<GameObject> potions;

    public Text turnText;
    public Text dayText;
    #endregion


    #region Voting Variables
    private bool afterVote = false;
    private int nbVoters = 0;
    private int nbVotes = 0;
    private bool everybodyVoted = false;
    private bool votingTime = false;
    private bool votingTimer = false;
    private Player electedPlayer;
    private Player eliminatedPlayer;
    #endregion

    #region GamePlay bools

    private bool beforeGameStart = true;
    private bool jour = false;

    private bool rolesSet = false;

    private bool mayorElected = false;

    private bool tellerTurnOngoing = true;
    private bool tellerTimer = false;
    private Player playerTellerClicked = null;

    private bool wolvesTurnOngoing;
    private bool wolvesTimer = true;
    private Player playerWolvesChose = null;

    private bool witchTurnOngoing = false;
    private bool witchUItoggled = false;
    private bool witchUsed1Potion = false;
    private bool witchTimer = true;
    private int witchPlayerIndex;
    private Player playerWitchChose = null;

    private bool hunterTurnOngoing = false;
    private Player playerHunterChose = null;
    private bool checkedHunterDead = false;

    private bool killTurn = false;

    private bool voteOngoing = false;

    private bool newMayorOngoing = false;
    private bool checkedMayorDead = false;

    private bool gameOver = false;
    private bool endPlaying = false;
    #endregion

    #endregion


    #region Unity Base Methods
    void Start()
    {
        playersKilledThisTurn = new List<Player>();
        playersKilledLastTurn = new List<Player>();
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
        
        for (int i = 3; i < Players.Count; i++)
        {
            Players[i].Die();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            votingTime = true;
            electedPlayer = null;
            Debug.Log("It's Time to elect a new Mayor !");
        }
		if (votingTime)
		{
            VoteVillage("elimination");
            //VoteVillage("election");
        }
       
		#endregion
        */

        
		if (beforeGameStart) //initialisation du jeu, avant la premiere nuit
        {
            dayText.text = "Initialisation";
            //Son d'introduction
            //Son qui dit aux joueurs de se mettre à leur place et de lever les bras pour Ready
            if (ArePlayersReady() == false)
            {
                //Afficher à l'écran que les joueurs ne sont pas prêts
                turnText.text = "Players are not ready";
                Debug.Log("Players not ready");
            }

            if (Players[Players.Count - 1].RoleDiscovered == false && rolesSet)
            {
                SetPlayersRoles();

                Debug.Log("Roles are set up");
                //Son qui lance la nuit pour les joueurs
            }

            if (Players[Players.Count - 1].RoleDiscovered == false && rolesSet)
            {
                turnText.text = "Discovering roles one by one";
                Debug.Log("Discovering roles one by one");
                for (int i = 0; i < Players.Count; i++)
                {
                    if (i == 0)
                    {
                        DiscoverOwnRole(Players[i], i);
                    }
                    else if (Players[i - 1].RoleDiscovered)
                    {
                        DiscoverOwnRole(Players[i], i);
                    }
                }
                //Son qui explique le but du jeu
                //Son qui dit que tout le monde peut relever son masque
            }

            if (Players[Players.Count - 1].RoleDiscovered && !mayorElected)
            {

                //Debug.Log("Election du maire");
                ////Son election du maire
                //ElectMayor();
            }
            
            if (mayorElected)
            {
                beforeGameStart = false;

                Debug.Log("Game launched");
            }
        }
        else if (!beforeGameStart && !jour) //nuit
        {
            dayText.text = "Nuit";
            Debug.Log("Nuit");

            //Son pour que tout le monde mette son masque sur ses yeux

            if (teller && tellerTurnOngoing && IsTellerAlive()) TellerTurn();

            else if (wolvesTurnOngoing) WolvesTurn();

            else if (witch && witchTurnOngoing && IsWitchAlive()) WitchTurn();

            else if (!wolvesTurnOngoing && killTurn)
            {
                for (int i = 0; i < playersKilledThisTurn.Count; i++)
                {
                    KillPlayer(playersKilledThisTurn[i]);
                }

                Debug.Log("All players killed");

                playersKilledLastTurn.Clear();
                playersKilledLastTurn.AddRange(playersKilledThisTurn);

                playersKilledThisTurn.Clear();
                killTurn = false;
                jour = true;
                Debug.Log("It's DAYTIME !");
            }
        }
        else if (!beforeGameStart && jour) //jour
        {
            dayText.text = "Jour";
            Debug.Log("Jour");

            //Son pour que tout le monde enleve son masque des yeux

            //Annonce des morts

            if (!afterVote && !gameOver)
            {
                if (!checkedHunterDead)
                {
                    for (int i = 0; i < playersKilledLastTurn.Count; i++)
                    {
                        if (playersKilledLastTurn[i].Role == "hunter") hunterTurnOngoing = true;
                    }
                    checkedHunterDead = true;
                }
                if (hunterTurnOngoing) HunterTurn();
                if (!checkedMayorDead && !hunterTurnOngoing)
                {
                    Debug.Log("Checking if Mayor is Alive and well");
                    for (int i = 0; i < Players.Count; i++)
                    {
                        if (Players[i].IsMayor && !Players[i].isAlive) newMayorOngoing = true;
                    }
                    checkedMayorDead = true;
                }
                if (newMayorOngoing)
                {
                    Debug.Log("Electing NewMayor");
                    //NewMayor();
                    newMayorOngoing = false;
                }
                if (!hunterTurnOngoing && !newMayorOngoing) votingTime = true;
                if (nbPlayersAlive <= 2 * nbWolvesAlive - 1 || nbWolvesAlive == 0) gameOver = true;
                if (!gameOver && votingTime)
                {
                    Debug.Log("It's VOTING TIME");
                    //Son debut du vote
                    //VoteVillage();
                    //Son fin du vote et elimination d'un joueur
                    afterVote = true;
                }
            }
            else if (afterVote)
            {
                if (!hunterTurnOngoing && !newMayorOngoing)
                {
                    for (int i = 0; i < playersKilledThisTurn.Count; i++)
                    {
                        if (playersKilledThisTurn[i].Role == "hunter") hunterTurnOngoing = true;
                        else if (!hunterTurnOngoing && playersKilledThisTurn[i].IsMayor) newMayorOngoing = true;
                    }
                    killTurn = true;
                }
                if (hunterTurnOngoing) HunterTurn();

                if (newMayorOngoing)
                {
                    Debug.Log("Electing NewMayor");
                    //NewMayor();
                    newMayorOngoing = false;
                }
                if (killTurn)
                {
                    for (int i = 0; i < playersKilledThisTurn.Count; i++)
                    {
                        KillPlayer(playersKilledThisTurn[i]);
                    }
                    playersKilledThisTurn.Clear();
                }
                if (nbPlayersAlive <= 2 * nbWolvesAlive || nbWolvesAlive == 0) gameOver = true;
                if (!gameOver && !hunterTurnOngoing && !newMayorOngoing)
                {
                    Debug.Log("It's NIGHTTIME !");
                    jour = false;
                    RestartVariablesNight();
                }
            }
        }

        if (gameOver && !endPlaying)
        {
            Debug.Log("GAME OVER");
            endPlaying = true;
            if (nbWolvesAlive == 0)
            {
                turnText.text = "GAME OVER VILLAGEOIS WIN";
                //Son Bonne fin
            }
            else
            {
                turnText.text = "GAME OVER LOUPS GAROUS WIN";
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
        nbWolvesAlive = nbWolves;
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
            if (rolesList[i] == "witch")
            {
                witchPlayerIndex = i;
            }
        }

        rolesSet = true;
    }

    public void DiscoverOwnRole(Player player, int i)
    {
        //Son qui demande à un joueur specifique d'enlever son masque

        //Afficher le rôle du joueur
        //Son qui lui demande d'interagir avec son role pour passer à la suite

        player.SetRoleUI();
        if (player.RoleDiscovered)
        {
            player.SetUI("citizen :)");
        }

        /*
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
        */

        //Son qui lui demande de remettre son masque
    }

    public void KillPlayer(Player player)
    {
        player.Die();
        nbPlayersAlive--;
        if (player.Role == "wolf")
        {
            nbWolvesAlive--;
        }
    }

    public void RestartVariablesNight()
    {
        tellerTurnOngoing = true;
        playerTellerClicked = null;
        wolvesTimer = true;
        witchTimer = true;
    }
    #endregion

    #region Voting Methods
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

    public void NewMayor()
    {
        newMayorOngoing = true;

        //DO something

        Debug.Log("Choix d'un nouveau maire");

        newMayorOngoing = false;
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
                //Debug.Log("Checking player's vote");
                //Vérifie si un joueur vient de voter et désactive le vote pour ce joueur le cas échéant
                if (Players[i].isAlive && !Players[i].hasVoted)
                {
                    if (Players[i].voice != null)
                    {
                        Debug.Log(Players[i] + "voted against : " + Players[i].voice);
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
        //Trouve le joueur ayant le plus de voix contre lui si tout le monde a voté ou si le temps est écoulé
        if (everybodyVoted || !votingTimer)
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

    public void VoteVillage(string voteType) // "elimination" / "election"
	{
        eliminatedPlayer = null;
        if (!voteOngoing)
        {
            ResetVotes();
            votingTimer = true;
            voteOngoing = true;
            timerText.gameObject.SetActive(true);
            StartCoroutine("VotingTimer", 20f);
            Debug.Log("Voting Timer started");
        }

        //Get result of the vote
        if (!votingTimer || everybodyVoted)
        {
            eliminatedPlayer = GetVoteResult();
            Debug.Log(eliminatedPlayer);
            if (eliminatedPlayer != null)
            {
                if(voteType == "elimination")
				{
                    Debug.Log("Everybody voted : Player " + eliminatedPlayer + "Was elected as Mayor");
                    eliminatedPlayer.Die();
                }
                else if (voteType == "election")
				{
                    Debug.Log("Everybody voted : Player " + eliminatedPlayer + "Was elected as Mayor");
                    eliminatedPlayer.IsMayor = true;

                }
            }
            else
            {
                foreach (Player player in Players)
                {
					if (player.isAlive)
					{
						if (player.IsMayor && player.hasVoted)
						{
                            eliminatedPlayer = player.voice;

                            Debug.Log("Everybody voted : Player " + eliminatedPlayer + "Was eliminated thanks to the Mayor's decision");
                            if (voteType == "elimination")
                            {

                                KillPlayer(eliminatedPlayer);
                            }
                            else if (voteType == "election")
                            {
                                Debug.Log("Everybody voted : Player " + eliminatedPlayer + "Was elected as Mayor thanks to the Mayor's decision");
                                eliminatedPlayer.IsMayor = true;
                            }
                        }
					}
				}
            }
            votingTime = false;
            voteOngoing = false;
            Debug.Log("Vote ended");
            timerText.gameObject.SetActive(false);
            StopCoroutine("VotingTimer");
        }
        //Continue vote
        else if (!everybodyVoted)
        {
            eliminatedPlayer = GetVoteResult();
        }
    }


    public Player GetIndividualVoteResult(Player player)
    {
        Player chosenPlayer = null;
        if (!player.hasVoted)
        {
            player.ActivateVote();
        }
        if (!player.hasVoted && player.voice != null)
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

    public void InvividualVote(Player player)
	{
        eliminatedPlayer = null;
        if (!voteOngoing)
        {
            ResetVotes();
            votingTimer = true;
            voteOngoing = true;
            timerText.gameObject.SetActive(true);
            StartCoroutine("VotingTimer", 30f);
            Debug.Log("Voting Timer started");
        }
        if(!votingTimer || player.hasVoted)
		{
            eliminatedPlayer = GetIndividualVoteResult(player);
            if(player.Role == "teller")
			{
                eliminatedPlayer.roleText.enabled = true;
			}
			else
			{
                KillPlayer(eliminatedPlayer);
            }
            witchTurnOngoing = false;
            tellerTurnOngoing = false;
            hunterTurnOngoing = false;
            voteOngoing = false;
            Debug.Log("Vote ended");
            timerText.gameObject.SetActive(false);
            StopCoroutine("VotingTimer");
        }
    }

    public Player GetWolvesVoteResult()
    {
        Player chosenPlayer = null;
        List<Player> wolves = new List<Player>();
        foreach (Player player in Players)
        {
            if (player.isAlive && player.Role == "wolf")
            {
                wolves.Add(player);
            }
        }
        //Si un seul loup, choix individuel
        if (wolves.Count == 1)
        {
            chosenPlayer = GetIndividualVoteResult(wolves[0]);
        }
        //Si deux loups le joueur choisi est validé seulement si les 2 ont voté pour le meme joueur sinon le vote est réactivé
        else if (wolves.Count == 2)
        {
            if (wolves[0].hasVoted && wolves[1].hasVoted)
            {
                if (wolves[0].voice == wolves[1].voice)
                {
                    chosenPlayer = wolves[0].voice;
                }
                else
                {
                    ResetVotes();
                }
            }
            foreach (Player wolf in wolves)
            {
                if (!wolf.hasVoted)
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

    public void WolvesVote()
	{
        eliminatedPlayer = null;
        if (!voteOngoing)
        {
            ResetVotes();
            votingTimer = true;
            voteOngoing = true;
            timerText.gameObject.SetActive(true);
            StartCoroutine("VotingTimer", 30f);
            Debug.Log("Voting Timer started");
        }
        eliminatedPlayer = GetWolvesVoteResult();
        if (!votingTimer || eliminatedPlayer != null)
        {
            KillPlayer(eliminatedPlayer);
            wolvesTurnOngoing = false;
            voteOngoing = false;
            Debug.Log("Vote ended");
            timerText.gameObject.SetActive(false);
            StopCoroutine("VotingTimer");
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
    
    public bool IsTellerAlive()
    {
        foreach (Player player in Players)
        {
            if (player.Role == "teller" && player.isAlive)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsWitchAlive()
    {
        foreach (Player player in Players)
        {
            if (player.Role == "witch" && player.isAlive)
            {
                return true;
            }
        }
        return false;
    }

    public void TellerTurn()
    {
        turnText.text = "Tour de la Voyante";
        Debug.Log("tour de la Voyante");
        if (Input.GetMouseButtonDown(0) && !tellerTimer)
        {
            RaycastHit hit;
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
        turnText.text = "Tour des Loups Garous";
        Debug.Log("tour des Loups Garous");
        StartCoroutine("WolvesTimer", 10f);
        if (Input.GetMouseButtonDown(0) && playersKilledThisTurn.Count == 0)
        {
            RaycastHit hit;
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
        turnText.text = "Tour de la Sorciere";
        Debug.Log("tour de la Sorciere");
        if (!witchUItoggled)
        {
            Players[witchPlayerIndex].ToggleWitchUI(true, playersKilledThisTurn.Count == 1);
            witchUItoggled = true;
            witchUsed1Potion = false;
        }
        StartCoroutine("WitchTimer", 10f);
        if (playersKilledThisTurn.Count == 1)
        {
            playersKilledThisTurn[0].MakeRed();
        }
        if (Players[witchPlayerIndex].deathPotionUsedThisTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Debug.Log("Raycast !");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    playerWitchChose = hit.transform.GetComponent<Player>();
                    Debug.Log("You chose to poison the " + hit.transform.name + ", he will die in his sleep tonight");
                    for (int i = 0; i < Players.Count; i++)
                    {
                        if (playerWitchChose == Players[i])
                        {
                            playersKilledThisTurn.Add(Players[i]);
                            Players[witchPlayerIndex].deathPotionUsedThisTurn = false;
                            Players[witchPlayerIndex].DeathPotionUsed = true;
                            witchUsed1Potion = true;
                        }
                    }

                }
            }
        }
        else if (Players[witchPlayerIndex].lifePotionUsedThisTurn)
        {
            playersKilledThisTurn[0].MakeFlesh();
            playersKilledThisTurn.RemoveAt(0);
            Debug.Log("You healed the dead guy !");
            Players[witchPlayerIndex].lifePotionUsedThisTurn = false;
            Players[witchPlayerIndex].LifePotionUsed = true;
            witchUsed1Potion = true;
        }
        if (witchUsed1Potion || !witchTimer)
        {
            if (witchUsed1Potion)
            {
                Debug.Log("Fin tour de la Sorciere, 1 potion utilisée");
            }
            else if (!witchTimer)
            {
                Debug.Log("Fin tour de la Sorciere, plus de temps");
            }
            for (int i = 0; i < playersKilledThisTurn.Count; i++)
            {
                playersKilledThisTurn[i].MakeFlesh();
            }
            Players[witchPlayerIndex].ToggleWitchUI(false, true);
            witchUItoggled = false;
            witchTurnOngoing = false;
            killTurn = true;
        }
    }
    public void HunterTurn()
    {
        turnText.text = "Tour du Chasseur";
        Debug.Log("tour du Chasseur");
        if (Input.GetMouseButtonDown(0) && playerHunterChose == null)
        {
            RaycastHit hit;
            Debug.Log("Raycast !");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                playerHunterChose = hit.transform.GetComponent<Player>();
                Debug.Log("You chose to kill " + hit.transform.name + " right before dying. BANG !");
                for (int i = 0; i < Players.Count; i++)
                {
                    if (playerHunterChose == Players[i])
                    {
                        //playersKilledThisTurn.Add(Players[i]);
                        KillPlayer(playerHunterChose);
                    }
                }
                hunterTurnOngoing = false;
            }
        }


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
        votingTime = false;
        Debug.Log("Voting Timer OFF");
        timerText.enabled = false;
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
        wolvesTimer = false;
        if (wolvesTurnOngoing) Debug.Log("Wolves Timer finished");
    }
    IEnumerator WitchTimer(float time)
    {
        Debug.Log("Witch Timer started");
        yield return new WaitForSeconds(time);
        witchTimer = false;
        if (witchTurnOngoing) Debug.Log("Witch Timer finished");

    }
    #endregion;
}

