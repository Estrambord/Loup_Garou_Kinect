using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Variables

    //PUBLIC

    #region SOUNDMANAGER
	public SoundManager soundManager;

    private bool voyanteInit = false;
    private bool nightInit = false;
    private bool gameInit = false;
    private bool roleInit = false;
    private bool ruleInit = false;
    private bool mayorInit = false;
    private bool dayInit = false;
    private bool newMayorInit = false;
    private bool villageEliminateInit = false;
    private bool nightRecapInit = false;
    private bool hunterTurnInit = false;
    private bool wolvesTurnInit = false;
    #endregion


    //[SerializeField] private List<KinectManager> kinectManagers;
    [System.NonSerialized] public List<Player> playersKilledThisTurn;
    public List<Player> Players;

    [Header("Roles Settings")]
    public bool randomizeNbWolves = false;
    public int nbWolves = 2;
    public bool randomizeOtherRoles = false;
    public bool teller = true;
    public bool witch = false;
    public bool hunter = true;
    private int wait = 0;

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
    private bool jour = true;

    private bool rolesSet = false;
    private bool roleTimer = false;

    private bool mayorElected = false;

    private bool tellerTurnOngoing = true;
    private bool beforeTellerTimer = false;
    private bool beforeTellerTimerActivated = false;
    private bool tellerTimer = false;
    private bool tellerTimerFinished = false;
    private int tellerPlayerIndex;
    private Player playerTellerClicked = null;

    private bool wolvesTurnOngoing;
    List<Player> wolves; 
    private Player playerWolvesChose = null;
    private bool beforeWolvesTimer = false;
    private bool beforeWolvesTimerActivated = false;

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
    private int hunterPlayerIndex;

    private bool killTurn = false;

    private bool voteOngoing = false;

    private bool newMayorOngoing = false;
    private bool checkedMayorDead = false;
   [SerializeField] private Player oldMayor = null;

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
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);


		if (beforeGameStart) //initialisation du jeu, avant la premiere nuit
        {
            
			if (!gameInit) { 
                dayText.text = "Initialisation";
                soundManager.PlayNarration(0);
                soundManager.PlaySFX(0);
                soundManager.PlayInstruction(0); //Levez un bras au-dessus de votre tête pour indiquer que vous êtes prêts
                gameInit = true;
            }
            

            if (ArePlayersReady() == false)
            {
                //Afficher à l'écran que les joueurs ne sont pas prêts
                NotReadyText.enabled = true;
                Debug.Log("Players not ready");
            }

            if (rolesSet == false && ArePlayersReady())
            {
                jour = false;
                soundManager.SwitchAmbientTime(jour);
                NotReadyText.enabled = false;
                SetPlayersRoles();
                
                Debug.Log("Roles are set up");
				if (!roleInit) {
                    roleInit = false;
                }
                
            }

            if (Players[Players.Count - 1].RoleDiscovered == false && rolesSet)
            //if (Players[1].RoleDiscovered == false && rolesSet)
            {
                turnText.enabled = true;
                turnText.text = "Découvrez vos rôles un par un";
                Debug.Log("Discovering roles one by one");
                WaitSeconds(7);
                if(wait == 1)
				{
                    for (int i = 0; i < Players.Count; i++)
                    //for (int i = 0; i < 2; i++)
                    {
                        if (i == 0 && !Players[i].RoleDiscovered)
                        {
                            DiscoverOwnRole(Players[i], i);
                        }
                        else if (i != 0 && Players[i - 1].RoleDiscovered && !Players[i].RoleDiscovered)
                        {
                            DiscoverOwnRole(Players[i], i);
                        }
                    }
                }

				if (!ruleInit)
				{
                    //soundManager.PlayNarration(3); //Règle du jeu

                    ruleInit = true;
                }
                votingTime = true;
            }

             if (Players[Players.Count - 1].RoleDiscovered && !mayorElected)// Le dernier player ? pq ?
             //if (Players[1].RoleDiscovered && !mayorElected)
             {
				//Debug.Log("Election du maire");
				//Son election du mair
				if (!mayorInit)
				{
                    jour = true;
                    soundManager.SwitchAmbientTime(jour);
                    soundManager.PlayNarration(4); //A présent vous devez voter et élire votre capitaine.
                    //soundManager.PlayInstruction(5); //Le capitaine tranchera en cas d’égalité lors d’un vote. Choisissez-le bien ! Utilisez le système de vote pour élire votre capitaine.
                    turnText.text = "Election du Maire";
                    mayorInit = true;
				}
                ElectMayor();
             }

             if (mayorElected)
             {
                beforeGameStart = false;
                jour = false;
                soundManager.SwitchAmbientTime(jour);
                Debug.Log("Game launched");
             }
             
            /*
			if (rolesSet)
			{
                beforeGameStart = false;
            }
            */
            
        }
        if (!beforeGameStart && !jour) //nuit
        {
            
            if (!nightInit)
            {
                dayText.text = "Nuit";
                nightInit = true;
            }

            if (beforeTellerTimer == false && beforeTellerTimerActivated == false)
			{
                beforeTellerTimerActivated = true;
                StartCoroutine("BeforeTellerTimer", 7);
			}

            if (teller && tellerTurnOngoing && IsTellerAlive() && beforeTellerTimer == true)
            {
				if (!voyanteInit) { 
                    soundManager.WaitEndVocal(); //Attend la fin de l'instruction et met AudioFinished à false
                    voyanteInit = true;
                }
				else if(soundManager.AudioFinished)
				{
                    TellerTurn();
				}
            }

            if (beforeWolvesTimer == false && beforeWolvesTimerActivated== false)
            {
                beforeWolvesTimerActivated = true;
                StartCoroutine("BeforeWolvesTimer", 7);
            }

            if (wolvesTurnOngoing) WolvesTurn();

            if (witch && witchTurnOngoing && IsWitchAlive()) WitchTurn();

            if (!wolvesTurnOngoing && killTurn)
            {
                for (int i = 0; i < playersKilledThisTurn.Count; i++)
                {
                    KillPlayer(playersKilledThisTurn[i]);
                    playersKilledThisTurn[i].ActivateRole();
                    Debug.Log("Joueur " + playersKilledThisTurn[i] + " est mort; indice " + i);

                }

                Debug.Log("All players killed");

                killTurn = false;
                CalculatePlayersAlive();
                jour = true;
                soundManager.SwitchAmbientTime(jour);
                Debug.Log("It's DAYTIME !");
            }
        }
        else if (!beforeGameStart && jour) //jour
        {
            dayText.text = "Jour";
            Debug.Log("Jour");

			if (!dayInit)
			{
                dayInit = true;
			}

            //Son pour que tout le monde enleve son masque des yeux

            ////////////////////////////////
            ///ANNONCE DES MORTS DE LA NUIT
            ////////////////////////////////
            /*
            if (!nightRecapInit) {
                int deadPlayerRole = 0;
                for (int i = 0; i < playersKilledThisTurn.Count; i++)
                {
                    switch (playersKilledThisTurn[i].Role)
                    {
                        case "teller":
                            deadPlayerRole = 9;
                            break;
                        case "wolf":
                            deadPlayerRole = 12;
                            break;
                        case "witch":
                            deadPlayerRole = 10;
                            break;
                        case "hunter":
                            deadPlayerRole = 11;
                            break;
                    }
                    //Cette nuit un joueur est mort il s'agit de Joueur "i" qui était "Role"
                    //soundManager.PlayNightReport(playersKilledThisTurn[i].GetComponent<AvatarController>().playerIndex + 1, deadPlayerRole); 
                }
                nightRecapInit = true;
            }
            */
            if (!afterVote && !gameOver)
            {
                if (!checkedHunterDead)
                {
                    Debug.Log("Checking if Hunter is Alive and well");
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
                            //soundManager.PlayInstruction(14); //Le maire est mort
                            newMayorOngoing = true;
                        }
                    }
                    checkedMayorDead = true;
                }

                if (newMayorOngoing)
                {
					if (!newMayorInit) 
                    { 
                        soundManager.PlayNarration(4); //A présent vous devez voter et élire votre capitaine.
                        //soundManager.PlayInstruction(5); //Le capitaine tranchera en cas d’égalité lors d’un vote. Choisissez-le bien ! Utilisez le système de vote pour élire votre capitaine.
                        turnText.text = "Choissez un nouveau maire";
                        newMayorInit = true;
                    }
                    NewMayor(oldMayor);
                }

                if (!hunterTurnOngoing && !newMayorOngoing)
                {
                    killTurn = true;
                }
				if (killTurn)
				{

                    Debug.Log("Kill Turn");

                    for (int i = 0; i < playersKilledThisTurn.Count; i++)
                    {
                        KillPlayer(playersKilledThisTurn[i]);//mettre dans la fonction kill player
                        playersKilledThisTurn[i].ActivateRole();
                    }
                    playersKilledThisTurn.Clear();
                    killTurn = false;
                    CalculatePlayersAlive();
                    votingTime = true;
                }

                if (nbPlayersAlive <= 2 * nbWolvesAlive - 1 || nbWolvesAlive == 0) {
                    gameOver = true;
                }

                /////////////////////////////
                ///VOTE DU VILLAGE 
                ////////////////////////////
                if (!gameOver && votingTime)
                {
                    if (!villageEliminateInit)
                    {
                        //Son debut du vote
                        soundManager.PlaySFX(3); //Vote Start
                        soundManager.PlayInstruction(12); //Le village peut à présent voter
                        Debug.Log("It's VOTING TIME : Choose a player to eliminate");
                        turnText.text = "Vote des citoyens : éliminez un joueur";
                        villageEliminateInit = true;
                    }

                    VoteVillage("elimination"); //Son dans la fonction
                }
                if(!newMayorOngoing && !hunterTurnOngoing && votingTime == false)
				{
                    afterVote = true;
                }
            }
            ///////////////////////////////////////////
            ///VERIFICATION ET VALIDATION DU MORT ELU
            ///////////////////////////////////////////
            else if (afterVote)
            {
                if (!hunterTurnOngoing)
                {
                    for (int i = 0; i < playersKilledThisTurn.Count; i++)
                    {
                        if (playersKilledThisTurn[i].Role == "hunter")
                        {
                            hunterTurnInit = false;
                            hunterTurnOngoing = true;
                        }
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
                            oldMayor = playersKilledThisTurn[i];
                            //oldMayor = Players[i];
                            //soundManager.PlayInstruction(14);  //Le maire est mort
                            newMayorOngoing = true;
                            newMayorInit = false;
                        }
                    }
                }
                if (newMayorOngoing)
                {
                    if (!newMayorInit)
                    {
                        soundManager.PlayNarration(4); //A présent vous devez voter et élire votre capitaine.
                        turnText.text = "Choissez un nouveau maire";
                        newMayorInit = true;
                    }
                    NewMayor(oldMayor);
                }

                if(!hunterTurnOngoing && !newMayorOngoing && playersKilledThisTurn.Count != 0)
				{
                    killTurn = true;
				}
                if (killTurn)
                {
                    Debug.Log("Kill Turn");
                    for (int i = 0; i < playersKilledThisTurn.Count; i++)
                    {
                        KillPlayer(playersKilledThisTurn[i]);
                        playersKilledThisTurn[i].ActivateRole();
                    }
                    playersKilledThisTurn.Clear();
                    killTurn = false;
                    CalculatePlayersAlive();
                }

                if (nbPlayersAlive <= 2 * nbWolvesAlive || nbWolvesAlive == 0)
                {
                    gameOver = true;
                }
                if (!gameOver && !hunterTurnOngoing && !newMayorOngoing)
                {
                    Debug.Log("It's NIGHTTIME !");
                    jour = false;
                    soundManager.SwitchAmbientTime(jour);
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
                soundManager.PlaySFX(9);
                soundManager.PlayNarration(5);
                soundManager.PlayGameOver(true);
                //Son Bonne fin
            }
            else
            {
                turnText.text = "GAME OVER LOUPS GAROUS WIN";
                soundManager.PlaySFX(10);
                soundManager.PlayNarration(6);
                soundManager.PlayGameOver(false);
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

    public void WaitSeconds(int time)
	{
        if(wait == 0)
		{
            StartCoroutine("Wait", time);
        }
	}

    public bool ArePlayersReady()
    {
        for (int i = 0; i < Players.Count; i++)
        //for (int i = 0; i < 2; i++)
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
            witch = false;
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
        //rolesList = new List<string> { "hunter", "wolf", "citizen", "citizen", "citizen", "citizen" };
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
            else if (Players[i].Role == "hunter")
			{
                hunterPlayerIndex = i;
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
            soundManager.PlayerAndAction(i+1, 0);
            //soundManager.PlayInstruction(3);
            roleTimer = true;
            //Demander de regarder l'écran au player
            StartCoroutine("RoleDiscovery", 5f);
            player.SetRoleUI();
            player.RoleDisplayed = true;
        }
        else if (player.RoleDisplayed && !roleTimer) //player.RoleDiscovered || 
        {
            Debug.Log("Hé ho je désactive l'UI");
            player.RoleDiscovered = true;
            player.roleText.gameObject.SetActive(false);
            player.DeactivateRole();
            /*string nomSphere = "Sphere (" + player.gameObject.GetComponent<AvatarController>().playerIndex.ToString() + ")";
            player.transform.Find(nomSphere).gameObject.SetActive(false);*/
            //player.DeactivateMayor();
            //Demander de remettre le masque
            soundManager.PlayerAndAction(i+1, 1);
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
        if (player.Role == "wolf")
        {
            wolves.Remove(player);
        }
    }

    public void CalculatePlayersAlive()
	{
        nbPlayersAlive = 0;
        nbWolvesAlive = 0;
        foreach(Player player in Players)
		{
            if (player.isAlive)
			{
                nbPlayersAlive++;
                if(player.Role == "wolf")
				{
                    nbWolvesAlive++;
				}
			}
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
        beforeTellerTimer = false;
        beforeTellerTimerActivated = false;
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
        beforeWolvesTimer = false;
        beforeWolvesTimerActivated = false;

        voyanteInit = false;
        nightInit = false;
        gameInit = false;
        roleInit = false;
        ruleInit = false;
        mayorInit = false;
        dayInit = false;
        newMayorInit = false;
        villageEliminateInit = false;
        nightRecapInit = false;
        hunterTurnInit = false;
        wolvesTurnInit = false;

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

        /*string nomSphereOld = "Sphere (" + oldMayor.gameObject.GetComponent<AvatarController>().playerIndex.ToString() + ")";
        oldMayor.transform.Find(nomSphereOld).gameObject.SetActive(false);*/
        /*string nomSphereNew = "Sphere (" + newMayor.gameObject.GetComponent<AvatarController>().playerIndex.ToString() + ")";
        newMayor.transform.Find(nomSphereNew).gameObject.SetActive(true);*/


        /*newMayor.IsMayor = true;
        newMayor.ActivateMayor();*/
        





        if (newMayor != null)
        {
            oldMayor.DeactivateMayor();
            oldMayor.IsMayor = false;
            newMayor.IsMayor = true;
            newMayor.ActivateMayor();

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
				if (!Players[i].hasVoted && !Players[i].voteActivated)
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
            //timerText.gameObject.SetActive(true);
            //StartCoroutine("VotingTimer", 60f);
            Debug.Log("Voting Timer started");
        }

        //Get result of the vote
        //if (!votingTimer || everybodyVoted)
        if (everybodyVoted)
        {
            eliminatedPlayer = GetVoteResult();
            Debug.Log(eliminatedPlayer);
            if (eliminatedPlayer != null)
            {
                if(voteType == "elimination") //vote village elimination
				{
                    Debug.Log("Everybody voted : Player " + eliminatedPlayer + "Was eliminated");
                    playersKilledThisTurn.Clear();
                    playersKilledThisTurn.Add(eliminatedPlayer);
                    //Son fin du vote et elimination d'un joueur
                    soundManager.PlaySFX(4); //Vote Over
                    /*
                    int eliminatedPlayerRole = 0;
                    switch (eliminatedPlayer.Role)
                    {
                        case "teller":
                            eliminatedPlayerRole = 9;
                            break;
                        case "wolf":
                            eliminatedPlayerRole = 12;
                            break;
                        case "witch":
                            eliminatedPlayerRole = 10;
                            break;
                        case "hunter":
                            eliminatedPlayerRole = 11;
                            break;
                    */
                    //soundManager.PlayVillageVotedFor(eliminatedPlayer.GetComponent<AvatarController>().playerIndex, eliminatedPlayerRole); //Le village a voté pour le joueur "i" qui était "role"
                }
                else if (voteType == "election") //vote village election
				{
                    Debug.Log("Everybody voted : Player " + eliminatedPlayer + "Was elected as Mayor");
                    eliminatedPlayer.IsMayor = true;
                    /*string nomSphere = "Sphere (" + eliminatedPlayer.gameObject.GetComponent<AvatarController>().playerIndex.ToString() + ")";
                    eliminatedPlayer.transform.Find(nomSphere).gameObject.SetActive(true);*/
                    eliminatedPlayer.ActivateMayor();
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

                            
                            if (voteType == "elimination")
                            {
                                KillPlayer(eliminatedPlayer);
                                playersKilledThisTurn.Add(eliminatedPlayer);
                                Debug.Log("Everybody voted : Player " + eliminatedPlayer + "Was eliminated thanks to the Mayor's decision");
                                soundManager.PlaySFX(4); //Vote Over
                                /*
                                int eliminatedPlayerRole = 0;
                                switch (eliminatedPlayer.Role)
                                {
                                    case "teller":
                                        eliminatedPlayerRole = 9;
                                        break;
                                    case "wolf":
                                        eliminatedPlayerRole = 12;
                                        break;
                                    case "witch":
                                        eliminatedPlayerRole = 10;
                                        break;
                                    case "hunter":
                                        eliminatedPlayerRole = 11;
                                        break;
                                }
                                */
                                //soundManager.PlayVillageVotedFor(eliminatedPlayer.GetComponent<AvatarController>().playerIndex, eliminatedPlayerRole); //Le village a voté pour le joueur "i" qui était "role"
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
                    /*string nomSphere = "Sphere (" + Players[rand].gameObject.GetComponent<AvatarController>().playerIndex.ToString() + ")";
                    Players[rand].transform.Find(nomSphere).gameObject.SetActive(true);*/
                    Players[rand].ActivateMayor();
                }


            }
            votingTime = false;
            voteOngoing = false;
            Debug.Log("Vote ended");
            //timerText.gameObject.SetActive(false);
            //StopCoroutine("VotingTimer");
        }
		#region copié collé bizarre
		#endregion
		//Continue vote
		else if (!everybodyVoted)
        {
            eliminatedPlayer = GetVoteResult();
        }
    }


    public Player GetIndividualVoteResult(Player player)
    {
        Player chosenPlayer = null;
        if (!player.hasVoted && !player.voteActivated)
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
            //votingTimer = true;
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
                if (!wolf.hasVoted && wolf.voice == null && !wolf.voteActivated)
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
        everybodyVoted = false;
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
		
		if (!Players[tellerPlayerIndex].capTeller.activeSelf)
		{
            Players[tellerPlayerIndex].ActivateRole();
            soundManager.PlayerAndAction(9, 0); //La voyante - se réveille
            soundManager.PlayInstruction(6); //Instruction voyante
        }
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
            soundManager.PlaySFX(5);
            soundManager.PlayerAndAction(9, 1); //La voyante se rendort
            StartCoroutine("TellerTimer", 5f);
        }

        if (tellerTimerFinished && playerTellerClicked != null)
        {
            Debug.Log("Fin tour de la Voyante");
            playerTellerClicked.roleText.gameObject.SetActive(false);
            playerTellerClicked.DeactivateRole();
            Players[tellerPlayerIndex].DeactivateRole();
            /*soundManager.WaitEndVocal();
			if (soundManager.AudioFinished)
			{
                tellerTimer = false;
                tellerTurnOngoing = false;
                wolvesTurnOngoing = true;
            }*/
            tellerTimer = false;
            tellerTurnOngoing = false;
            wolvesTurnOngoing = true;
        }
    }
    public void WolvesTurn()
    {
        if (!wolvesTurnInit)
        {
            turnText.text = "Tour des Loups Garous";
            Debug.Log("tour des Loups Garous");
            soundManager.PlayerAndAction(8, 2); //LG se reveillent
            //soundManager.PlayInstruction(7);
            wolvesTurnInit = true;
        }

        foreach(Player wolf in wolves)
		{
            wolf.ActivateRole();
            wolf.SetRoleUI();
		}
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
                    soundManager.PlaySFX(6);
                    playersKilledThisTurn.Add(Players[i]);
                    Players[i].MakeDead();
                }
            }
        }

        if (playersKilledThisTurn.Count == 1)
        {
            Debug.Log("Fin tour des Loups Garous");
            soundManager.PlayerAndAction(8, 3); //LG se redorment
            wolvesTurnOngoing = false;
            foreach (Player wolf in wolves)
            {
                wolf.DeactivateRole();
                wolf.roleText.gameObject.SetActive(false);
            }

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
                    potion.SetActive(true);
                }
                
                else if(potion.tag == "PotionDead" && marmite.remainingPotions.Contains("dead"))
				{
                    potion.SetActive(true);
                }
			}
            potionsItemShown = true;
		}
        turnText.text = "Tour de la Sorciere";
        Debug.Log("tour de la Sorciere");
        Players[witchPlayerIndex].ActivateRole();
        Players[witchPlayerIndex].SetRoleUI(); 

        if (witchTimer && !witchTimerActivated)
        {
            StartCoroutine("WitchTimer", 30f);
            Players[witchPlayerIndex].ActivateGrabDrop();
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
                    if (playerWitchChose == null)
                    {
                        playerWitchChose = IndividualVote(Players[witchPlayerIndex]);
                    }
                    if (playerWitchChose != null)
                    {
                        playersKilledThisTurn.Add(playerWitchChose);
                        Debug.Log("Witch, you chose to kill " + playerWitchChose);
                        //playerWitchChose.MakeDead();
                        Players[witchPlayerIndex].DeactivateRole();
                        Players[witchPlayerIndex].roleText.gameObject.SetActive(false);
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
        if (!hunterTurnInit)
        {
            soundManager.PlayInstruction(14); //Le chasseur peut abbatre un fdp
            turnText.text = "Tour du chasseur";
            Debug.Log("tour du chasseur");
            hunterTurnInit = true;
        }
        Players[hunterPlayerIndex].ActivateRole();
        Players[hunterPlayerIndex].SetRoleUI();
        if (playerHunterChose == null)
        {
            playerHunterChose = IndividualVote(Players[hunterPlayerIndex]);
        }
        if (playerHunterChose != null)
        {
            soundManager.PlaySFX(8);
            playersKilledThisTurn.Add(playerHunterChose);
            playerHunterChose.MakeDead();
            Debug.Log("Hunter chose to kill " + playerHunterChose);
            hunterTurnOngoing = false;
            Players[hunterPlayerIndex].DeactivateRole();
            Players[hunterPlayerIndex].roleText.gameObject.SetActive(false);
        }
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
        Debug.Log("Witch Timer started");
        for (int i = (int)time; i >= 0; i--)
        {
            yield return new WaitForSeconds(1);
            timerText.text = i.ToString();
            timerText.enabled = true;
            //Debug.Log("time remaining" + i);
        }
        tellerTimerFinished = true;
        Debug.Log("TellerTimer finished");
        Debug.Log("Teller chose " + playerTellerClicked);
    }

    IEnumerator WitchTimer(float time)
    {
        
        witchTimerActivated = true;
        Debug.Log("Witch Timer started");
        for (int i = (int)time; i >= 0; i--)
        {
            yield return new WaitForSeconds(1);
            timerText.text = i.ToString();
            timerText.enabled = true;
            Debug.Log("time remaining" + i);
        }
        Debug.Log("Witch timer finished");
        witchTimer = false;
    }

    IEnumerator RoleDiscovery(float time)
    {
        timerText.gameObject.SetActive(true);
        Debug.Log("Discovering roles");
        for (int i = (int)time; i >= 0; i--)
        {
            yield return new WaitForSeconds(1);
            timerText.text = i.ToString();
            Debug.Log("time remaining" + i);
        }
        timerText.gameObject.SetActive(false);
        roleTimer = false;

    }

    IEnumerator Wait(float time)
    {
        timerText.gameObject.SetActive(true);
        Debug.Log("Waiting for preparation");
        timerText.text = "Mettez vos masques";
        for (int i = (int)time; i >= 0; i--)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("time remaining" + i);
        }
        timerText.gameObject.SetActive(false);
        wait = 1;
    }

    IEnumerator BeforeTellerTimer(float timer)
	{
        yield return new WaitForSeconds(timer);
        beforeTellerTimer = true;
	}

    IEnumerator BeforeWolvesTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        beforeWolvesTimer = true;
    }
    #endregion;
}