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

    [Header("Roles Settings")]
    public bool randomizeNbWolves = false;
    public int nbWolves = 2;
    public bool randomizeOtherRoles = false;
    private bool teller = false;
    private bool witch = false;
    private bool hunter = true;

    //PRIVATE
    public int nbPlayersAlive;
    public int nbWolvesAlive;

    #region UI
    public Text timerText;
    public Text NotReadyText;
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
    private Player eliminatedPlayer;

    #endregion

    #region GamePlay bools

    private bool beforeGameStart = true;
    private bool jour = false;

    private bool rolesSet = false;
    private bool roleTimer = false;

    private bool mayorElected = false;

    private bool tellerTurnOngoing = true;
    private bool tellerTimer = false;
    private bool tellerTimerFinished = false;
    private int tellerPlayerIndex;
    private Player playerTellerClicked = null;

    private bool wolvesTurnOngoing;
    List<Player> wolves; 
    private Player playerWolvesChose = null;

    private bool witchTurnOngoing = false;
    private bool witchTimer = true;
    private int witchPlayerIndex;
    private bool potionsItemShown = false;
    private bool witchTimerActivated = false;
    public Marmite marmite;
    private Player playerWitchChose = null;

    private bool hunterTurnOngoing = false;
    private Player playerHunterChose = null;
    private bool checkedHunterDead = false;

    private bool killTurn = false;

    private bool voteOngoing = false;

    private bool newMayorOngoing = false;
    private bool checkedMayorDead = false;
    private Player oldMayor = null;

    private bool gameOver = false;
    private bool endPlaying = false;
    #endregion
    #endregion

    #region Unity Base Methods
    void Start()
    {
        wolves = new List<Player>();
        playersKilledThisTurn = new List<Player>();

        if (!teller)
        {
            tellerTurnOngoing = false;
            wolvesTurnOngoing = true;
        }
    }



    void Update()
    {

        #region Vote du village

        /*for (int i = 3; i < Players.Count; i++)
        {
            Players[i].Die();
        }*/

        /*if (Input.GetKeyDown(KeyCode.F))
        {
            votingTime = true;
            electedPlayer = null;
            Debug.Log("It's Time to elect a new Mayor !");
        }
		if (votingTime)
		{
            VoteVillage("elimination");
            //VoteVillage("election");
        }*/

        #endregion



        if (beforeGameStart) //initialisation du jeu, avant la premiere nuit
        {
            dayText.text = "Initialisation";
            //Son d'introduction
            //Son qui dit aux joueurs de se mettre à leur place et de lever les bras pour Ready
            if (ArePlayersReady() == false)
            {
                //Afficher à l'écran que les joueurs ne sont pas prêts
                NotReadyText.enabled = true;
                Debug.Log("Players not ready");
            }

            if (rolesSet == false && ArePlayersReady())
            {
                NotReadyText.enabled = false;
                SetPlayersRoles();

                Debug.Log("Roles are set up");
                //Son qui lance la nuit pour les joueurs
            }

            
            //if (Players[Players.Count - 1].RoleDiscovered == false && rolesSet)
             if (Players[1].RoleDiscovered == false && rolesSet)
             {
                 turnText.enabled = true;
                 turnText.text = "Discovering roles one by one";
                 Debug.Log("Discovering roles one by one");
                 //for (int i = 0; i < Players.Count; i++)
                 for (int i = 0; i < 2; i++)
                 {
                     if (i == 0 && !Players[i].RoleDiscovered)
                     {
                         DiscoverOwnRole(Players[i], i);
                     }
                     else if ( i != 0 &&  Players[i - 1].RoleDiscovered && !Players[i].RoleDiscovered)
                     {
                         DiscoverOwnRole(Players[i], i);
                     }
                 }
                 //Son qui explique le but du jeu
                 //Son qui dit que tout le monde peut relever son masque
                 votingTime = true;
             }

             //if (Players[Players.Count - 1].RoleDiscovered && !mayorElected)// Le dernier player ? pq ?
             if (Players[1].RoleDiscovered && !mayorElected)
             {
                 //Debug.Log("Election du maire");
                 //Son election du mair
                 turnText.text = "Election du Maire";
                 ElectMayor();
             }

             if (mayorElected)
             {
                 beforeGameStart = false;

                 Debug.Log("Game launched");
             }
             

			if (rolesSet)
			{
                beforeGameStart = false;
            }
            
        }
        if (!beforeGameStart && !jour) //nuit
        {
            dayText.text = "Nuit";
            //Debug.Log("Nuit");

            //Son pour que tout le monde mette son masque sur ses yeux

            if (teller && tellerTurnOngoing && IsTellerAlive()) TellerTurn();

            else if (wolvesTurnOngoing) WolvesTurn();

            else if (witch && witchTurnOngoing && IsWitchAlive()) WitchTurn();

            else if (!wolvesTurnOngoing && killTurn)
            {
                for (int i = 0; i < playersKilledThisTurn.Count; i++)
                {
                    KillPlayer(playersKilledThisTurn[i]);
                    Debug.Log("Joueur " + playersKilledThisTurn[i] + " est mort; indice " + i);

                }

                Debug.Log("All players killed");

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
                    for (int i = 0; i < playersKilledThisTurn.Count; i++)
                    {
                        if (playersKilledThisTurn[i].Role == "hunter") hunterTurnOngoing = true;
                    }
                    checkedHunterDead = true;
                }

                if (hunterTurnOngoing) HunterTurn();

                if (!checkedMayorDead && !hunterTurnOngoing)
                {
                    Debug.Log("Checking if Mayor is Alive and well");
                    for (int i = 0; i < Players.Count; i++)
                    {
                        if (Players[i].IsMayor && !Players[i].isAlive)
                        {
                            oldMayor = Players[i];
                            newMayorOngoing = true;
                        }
                    }
                    checkedMayorDead = true;
                }

                if (newMayorOngoing)
                {
                    //Son ancien Maire doit en choisir un nouveau
                    NewMayor(oldMayor);
                }

                if (!hunterTurnOngoing && !newMayorOngoing) votingTime = true;

                if (nbPlayersAlive <= 2 * nbWolvesAlive - 1 || nbWolvesAlive == 0) gameOver = true;

                if (!gameOver && votingTime)
                {
                    Debug.Log("It's VOTING TIME");
                    //Son debut du vote
                    VoteVillage("elimination");
                    //Son fin du vote et elimination d'un joueur
                    afterVote = true;
                }
            }
            else if (afterVote)
            {
                if (!hunterTurnOngoing)
                {
                    for (int i = 0; i < playersKilledThisTurn.Count; i++)
                    {
                        if (playersKilledThisTurn[i].Role == "hunter") hunterTurnOngoing = true;
                    }
                    //killTurn = true;
                }

                if (hunterTurnOngoing) HunterTurn();

                if (!newMayorOngoing)
                {
                    for (int i = 0; i < playersKilledThisTurn.Count; i++)
                    {
                        if (playersKilledThisTurn[i].IsMayor)
                        {
                            oldMayor = Players[i];
                            newMayorOngoing = true;
                        }
                    }
                }
                if (newMayorOngoing) NewMayor(oldMayor);

                if(!hunterTurnOngoing && !newMayorOngoing)
				{
                    killTurn = true;
				}
                if (killTurn)
                {
                    Debug.Log("Kill Turn");
                    for (int i = 0; i < playersKilledThisTurn.Count; i++)
                    {
                        KillPlayer(playersKilledThisTurn[i]);
                    }
                    playersKilledThisTurn.Clear();
                    killTurn = false;
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


    //#######################################################################################################################################
    //#######################################################################################################################################
    //#######################################################################################################################################
    //#######################################################################################################################################
    //#######################################################################################################################################
    //#######################################################################################################################################
    //#######################################################################################################################################
    //#######################################################################################################################################
    //#######################################################################################################################################
    //#######################################################################################################################################

    #region Methodes generales

    public bool ArePlayersReady()
    {
        //for (int i = 0; i < Players.Count; i++)
        for (int i = 0; i < 2; i++)
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
        rolesList = new List<string> { "hunter", "wolf", "citizen", "citizen", "citizen", "citizen" };
        //Affecter les roles aux joueurs
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].Role = rolesList[i];
            if (rolesList[i] == "witch")
            {
                witchPlayerIndex = i;
            }
            else if(rolesList[i] == "teller")
			{
                tellerPlayerIndex = i;
			}
            else if (Players[i].Role == "wolf")
            {
                wolves.Add(Players[i]);
            }
        }


        rolesSet = true;
    }

    public void DiscoverOwnRole(Player player, int i)
    {
        Debug.Log(i.ToString());
		//Son qui demande à un joueur specifique d'enlever son masque

		//Afficher le rôle du joueur
		//Son qui lui demande d'interagir avec son role pour passer à la suite
		if (!roleTimer && player.RoleDisplayed == false)
		{
            roleTimer = true;
            //Demander de regarder l'écran au player
            StartCoroutine("RoleDiscovery", 10f);
            player.SetRoleUI();
            player.RoleDisplayed = true;
        }
        else if (player.RoleDisplayed && !roleTimer) //player.RoleDiscovered || 
        {
            Debug.Log("Hé ho je désactive l'UI");
            player.RoleDiscovered = true;
            player.roleText.enabled = false;
            string nomSphere = "Sphere (" + player.gameObject.GetComponent<AvatarController>().playerIndex.ToString() + ")";
            player.transform.Find(nomSphere).gameObject.SetActive(false);
            //Demander de remettre le masque
            roleTimer = true;
            StartCoroutine("RoleDiscovery", 5f);
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
            wolves.Remove(player);
        }
    }

    public void RestartVariablesNight()
    {
        afterVote = false;
        everybodyVoted = false;
        votingTime = false;
        votingTimer = false;
        jour = false;
        playerTellerClicked = null;
        tellerTimer = false;
        tellerTimerFinished = false;
        playerWolvesChose = null;
        witchTimer = true;
        potionsItemShown = false;
        witchTimerActivated = false;
        playerWitchChose = null;
        hunterTurnOngoing = false;
        playerHunterChose = null;
        checkedHunterDead = false;
        witchTurnOngoing = false;
        killTurn = false;
        voteOngoing = false;
        newMayorOngoing = false;
        checkedMayorDead = false;
        oldMayor = null;

		if (IsTellerAlive())
		{
            tellerTurnOngoing = true;
            wolvesTurnOngoing = false;
        }
		else
		{
            tellerTurnOngoing = false;
            wolvesTurnOngoing = true;
        }
    }
    #endregion

    #region Voting Methods
    public void ElectMayor()
    {
        Debug.Log("Election du Maire");
        VoteVillage("election");

        //Changer l'aspect esthétique du Maire ?
        if(votingTime == false)
		{
            mayorElected = true;
        }
    }

    public void NewMayor(Player oldMayor)
    {
        Debug.Log("Choix d'un nouveau Maire");

        Player newMayor = null;

        newMayor = IndividualVote(oldMayor);
        oldMayor.IsMayor = false;

        if (newMayor != null)
        {
            newMayor.IsMayor = true;

            //Changer aspect esthétique nouveau maire ?

            newMayorOngoing = false;
        }
    }

    public Player GetVoteResult()
    {
        Player chosenPlayer = null;
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
            StartCoroutine("VotingTimer", 40f);
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
                    playersKilledThisTurn.Clear();
                    playersKilledThisTurn.Add(eliminatedPlayer);
                }
                else if (voteType == "election")
				{
                    Debug.Log("Everybody voted : Player " + eliminatedPlayer + "Was elected as Mayor");
                    eliminatedPlayer.IsMayor = true;
                    string nomSphere = "Sphere (" + eliminatedPlayer.gameObject.GetComponent<AvatarController>().playerIndex.ToString() + ")";
                    eliminatedPlayer.transform.Find(nomSphere).gameObject.SetActive(true);
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
                                //Pas censé arriver
                                Debug.Log("Everybody voted : Player " + eliminatedPlayer + "Was elected as Mayor thanks to the Mayor's decision");
                                eliminatedPlayer.IsMayor = true;
                            }
                        }
					}
				}
                if (voteType == "election")
				{
                    int rand = Random.Range(0, 6);
                    Players[rand].IsMayor = true;
                    string nomSphere = "Sphere (" + Players[rand].gameObject.GetComponent<AvatarController>().playerIndex.ToString() + ")";
                    Players[rand].transform.Find(nomSphere).gameObject.SetActive(true);
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
            player.DeactivateVote();
        }
        /*if (player.isAlive && !player.hasVoted)
        {
            if (player.voice != null)
            {
                player.hasVoted = true;
                player.DeactivateVote();
                chosenPlayer = player.voice;
            }
        }*/
        return chosenPlayer;
    }

    public Player IndividualVote(Player player)
	{
        eliminatedPlayer = null;
        eliminatedPlayer = GetIndividualVoteResult(player);
        if (!voteOngoing)
        {
            ResetVotes();
            votingTimer = true;
            voteOngoing = true;
            timerText.gameObject.SetActive(true);
            //StartCoroutine("VotingTimer", 30f);
            Debug.Log("Voting Timer started");
        }
        if(player.hasVoted) //!votingTimer || )
		{
            //eliminatedPlayer = GetIndividualVoteResult(player);
            /*
            witchTurnOngoing = false;
            tellerTurnOngoing = false;
            hunterTurnOngoing = false;
            */
            voteOngoing = false;
            Debug.Log("Vote ended");
            timerText.gameObject.SetActive(false);
            //StopCoroutine("VotingTimer"); 
        }
        return eliminatedPlayer;
    }

    public Player GetWolvesVoteResult()
    {
        Player chosenPlayer = null;
        //Si un seul loup, choix individuel
        if (wolves.Count == 1)
        {
            Debug.Log("1 wolf is voting");
            chosenPlayer = GetIndividualVoteResult(wolves[0]);
        }
        //Si deux loups le joueur choisi est validé seulement si les 2 ont voté pour le meme joueur sinon le vote est réactivé
        else if (wolves.Count == 2)
        {
            Debug.Log("2 wolves are voting");
            if (wolves[0].hasVoted && wolves[1].hasVoted)
            {
                if (wolves[0].voice == wolves[1].voice)
                {
                    chosenPlayer = wolves[0].voice;
                    Debug.Log("The wolves seem to be in a peace agreement");
                }
                else
                {
                    ResetVotes();
                    Debug.Log("Both wolves didn't choose the same player");
                }
            }
            foreach (Player wolf in wolves)
            {
                if (!wolf.hasVoted && wolf.voice == null)
                {
                    wolf.ActivateVote();
                }
                else if (!wolf.hasVoted && wolf.voice != null && wolf.isAlive)
                {
                    wolf.DeactivateVote();
                    wolf.hasVoted = true;
                }
            }
        }
        else
        {
            Debug.Log("Bad number of wolves");
        }
        return chosenPlayer;
    }

    public Player WolvesVote()
	{
        eliminatedPlayer = null;
        if (!voteOngoing)
        {
            ResetVotes();
            votingTimer = true;
            voteOngoing = true;
            timerText.gameObject.SetActive(true);
            //StartCoroutine("VotingTimer", 30f);
            Debug.Log("Wolves are now voting (WolvesVote)");
        }
        eliminatedPlayer = GetWolvesVoteResult();
        if (!votingTimer || eliminatedPlayer != null)
        {
            wolvesTurnOngoing = false;
            voteOngoing = false;
            Debug.Log("Vote ended");
            timerText.gameObject.SetActive(false);
            //StopCoroutine("VotingTimer");
        }
        return eliminatedPlayer;
    }

    public void ResetVotes()
    {
        foreach (Player player in Players)
        {
            player.hasVoted = false;
            player.nbVote = 0;
            player.voice = null;
        }
        nbVotes = 0;
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
        if (playerTellerClicked == null)
        {
            playerTellerClicked = IndividualVote(Players[tellerPlayerIndex]);
        }
        else if (playerTellerClicked != null && !tellerTimer)
        {
            playerTellerClicked.SetRoleUI();
            Debug.Log("Teller chose to see a beautiful UI");
            tellerTimer = true;
            StartCoroutine("TellerTimer", 5f);
        }

        if (tellerTimerFinished && playerTellerClicked != null)
        {
            Debug.Log("Fin tour de la Voyante");
            //playerTellerClicked.SetUI("citizen :)");
            tellerTimer = false;
            tellerTurnOngoing = false;
            wolvesTurnOngoing = true;
        }
    }
    public void WolvesTurn()
    {
        turnText.text = "Tour des Loups Garous";
        Debug.Log("tour des Loups Garous");

        if (playerWolvesChose == null)
		{
            playerWolvesChose = WolvesVote();
		}
        if (playerWolvesChose != null)
        {
            Debug.Log("You selected the " + playerWolvesChose + ", he will be eaten by the werewolves tonight");
            for (int i = 0; i < Players.Count; i++)
            {
                if (playerWolvesChose == Players[i])
                {
                    playersKilledThisTurn.Add(Players[i]);
                    Players[i].MakeDead();
                }
            }
        }

        if (playersKilledThisTurn.Count == 1)
        {
            Debug.Log("Fin tour des Loups Garous");
            wolvesTurnOngoing = false;

            if (witch && IsWitchAlive()) witchTurnOngoing = true;
            else killTurn = true;
		}
    }
    public void WitchTurn()
    {
        string potionChosen = "";
		if (!potionsItemShown)
		{
            marmite.gameObject.SetActive(true);
            foreach (GameObject potion in potions)
			{
                if(potion.tag == "PotionLife" && marmite.remainingPotions.Contains("life"))
				{
                    //potion.SetActive(true);
                }
                
                else if(potion.tag == "PotionDead" && marmite.remainingPotions.Contains("dead"))
				{
                    //potion.SetActive(true);
                }
			}
            potionsItemShown = true;
		}
        turnText.text = "Tour de la Sorciere";
        Debug.Log("tour de la Sorciere");

        if (witchTimer && !witchTimerActivated)
        {
            StartCoroutine("WitchTimer", 60f);
            Players[witchPlayerIndex].ActivateGrabDrop(); ;
        }
        if (marmite.isChosen)
		{
            StopCoroutine("WitchTimer");
            Players[witchPlayerIndex].DeactivateGrabDrop();
            potionChosen = marmite.chosen;
			switch (potionChosen)
			{
                case "life":
                    //playersKilledThisTurn[0].Remettrematerial()
                    playersKilledThisTurn[0].MakeFlesh();
                    playersKilledThisTurn.Clear();
                    Debug.Log("The witch revived a dead guy");
                    Debug.Log("Il y a " + playersKilledThisTurn.Count + " qui sont morts cette nuit...");
                    witchTurnOngoing = false;
                    killTurn = true;
                    foreach (GameObject potion in potions)
                    {
                        potion.SetActive(false);
                    }
                    marmite.gameObject.SetActive(false);
                    potionsItemShown = false;
                    break;
                case "dead":
                    turnText.text = "Witch, choose a player to kill";
                    if (playerWitchChose == null)
                    {
                        playerWitchChose = IndividualVote(Players[witchPlayerIndex]);
                    }
                    if (playerWitchChose != null)
                    {
                        playersKilledThisTurn.Add(playerWitchChose);
                        Debug.Log("Witch, you chose to kill " + playerWitchChose);
                        //playerWitchChose.MakeDead();
                        witchTurnOngoing = false;
                        killTurn = true;
                        foreach (GameObject potion in potions)
                        {
                            potion.SetActive(false);
                        }
                        marmite.gameObject.SetActive(false);
                        potionsItemShown = false;
                    }
                    break;
                 default:
                    break;
			}
		}
        else if (! marmite.isChosen && !witchTimer)
		{
            Debug.Log("Aucune potion n'a été utilisée");
            witchTurnOngoing = false;
            killTurn = true;
            foreach (GameObject potion in potions)
            {
                potion.SetActive(false);
            }
            marmite.gameObject.SetActive(false);
            potionsItemShown = false;
        }
    }
    public void HunterTurn()
    {
        turnText.text = "Tour du chasseur";
        Debug.Log("tour du chasseur");
        if (playerHunterChose == null)
        {
            playerHunterChose = IndividualVote(Players[tellerPlayerIndex]);
        }
        if (playerHunterChose != null)
        {
            playersKilledThisTurn.Add(playerHunterChose);
            playerHunterChose.MakeDead();
            Debug.Log("Hunter chose to kill " + playerHunterChose);
            hunterTurnOngoing = false;
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
        Debug.Log("TellerTimer started");
        yield return new WaitForSeconds(time);
        tellerTimerFinished = true;
        Debug.Log("TellerTimer finished");
        Debug.Log("Teller chose " + playerTellerClicked);
    }

    IEnumerator WitchTimer(float time)
    {
        timerText.enabled = true;
        witchTimerActivated = true;
        Debug.Log("Witch Timer started");
        for (int i = (int)time; i >= 0; i--)
        {
            yield return new WaitForSeconds(1);
            timerText.text = i.ToString();
            Debug.Log("time remaining" + i);
        }
        Debug.Log("Witch timer finished");
        witchTimer = false;
    }

    IEnumerator RoleDiscovery(float time)
    {
        timerText.enabled = true;
        Debug.Log("Discovering roles");        
        for (int i = (int)time; i >= 0; i--)
        {
            yield return new WaitForSeconds(1);
            timerText.text = i.ToString();
            Debug.Log("time remaining" + i);
        }
        timerText.enabled = false;
        roleTimer = false;

    }
    #endregion;
}