using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Player> Players;
    private float timer;
    private Canvas timer_UI;
    //public GameObject Loup_Garou1;
    //public LoupGarou loup2;
    //public AvatarController[] avatars;
    #region Variables
    


    //PUBLIC
    public GameObject Loup_Garou1;
    public LoupGarou loup2;
    public List<AvatarController> playersList;
    public KinectManager[] kinectManagers;
    public AvatarController playerKilled;
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
        


    //PRIVATE
    private bool beforeGameStart = true;
    private bool jour = true;
    private bool rolesSet = false;
    private bool rolesDiscovered = false;
    private bool mayorElected = false;
    private int nbPlayersAlive;
    private int nbLoupGarouAlive;
    private List<Player> Players;
    private float timer;
    private Canvas timer_UI;
    private bool capitaineElected = false;

    [System.NonSerialized] public List<AvatarController> playersKilledThisTurn;
        
    private bool gameOver = false;
    private bool skip;

    #endregion

    #region Unity Base Methods
    void Start()
    {
        //playersList = new List<AvatarController>() { player0, player1, player2, player3, player4, player5 };
        playersKilledThisTurn = new List<AvatarController>();
    }

    void Update()
    {
<<<<<<< HEAD
        Player eliminatedPlayer = Vote_village();
        eliminatedPlayer.enabled = false;
=======
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
            if (!mayorElected)
            {
                //Son election du maire
                ElectionMayor();
            }

        }

        /*
        else if (tourDeChauffe)
        {
            EST CE QU'ON FAIT UN TOUR POUR RIEN ? SYLVAIN AVAIT PROPOSE MAIS JE TROUVE QUE CA FERAIT PERDRE TROP DE TEMPS AUX JOUEURS
            NON JE PENSE PAS IL SUFFIT DE DONNER DES EXPLICATIONS AU DEBUT
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
>>>>>>> ffa1ce936b6c0f17e429fb3f7870c948a7276bfa
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
/*
        //Détermination du nombre de loups (1 ou 2)
        Random nbWolvesSeed = new Random();
        int nbWolves = nbWolvesSeed.Next(1,2);
        
        Lit<string> roles = new List<string>(){ "witch", "hunter", "citizen", "teller", "wolf" };
        if(nbWolves == 1)
        {
            roles.Add("citizen");
        }
        else if(nbWolves == 2)
        {
            roles.Add("wolf");
        }
        var randomizedRoles = roles.OrderBy(item => rnd.Next());

        //Affecter les roles aux joueurs
        for(int i = 0; i < Players.Count; i++)
        {
            Players[i].Role() = randomizedRoles[i];
        }
*/
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
    //public void DiscoverOwnRole(AvatarController player)
    public void DiscoverOwnRole(Player player)
    {
        //Son qui demande à un joueur specifique d'enlever son masque

        //Afficher le rôle du joueur
        //Son qui lui demande d'interagir avec son role pour passer à la suite
        if (!skip) //Mouvement de la main pour skip ?
        {
            player.role.enabled = true;
            //Afficher UI
        }
        else
        {
            player.role.enabled = false;
        }
        skip = false;
        //Son qui lui demande de remettre son masque
    }

    /// <summary>
    /// Lance l'election du maire
    /// </summary>
    public void Electionmayor()
    {
        if ( ! mayorElected ){
            Player newMayor = VoteVillage();
            mayorElected = true;
        }
    }

    /// <summary>
    /// Déclenche un vote d'elimination
    /// </summary>
    public Player VoteVillage()
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
<<<<<<< HEAD
    public Player Vote_village() 
    {
        List <Player> votedPlayers = new List<Player>();

        Player chosenPlayer = null;
        timer = 60f;
        int nbVoters = 0;
        int nbVotes = 0;
        int maxVotes = 0;

        for (int i = 0; i < Players.Count; i++)
		{
			if (Players[i].isAlive)
			{
                //Players[i].handClick.enabled = true;
                nbVoters++;
			}
		}

        while (nbVotes < 2)
        //while (nbVotes < nbVoters)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if(Players[i].isAlive && !Players[i].hasVoted)
				{
                    if(Players[i].voice != null)
					{
                        nbVotes++;
                        Players[i].hasVoted = true;
                    }
				}
            }
        }
        foreach(Player player in Players)
		{
			if(player.nbVote > maxVotes)
			{
                chosenPlayer = player;
                maxVotes = player.nbVote;
			}
            else if(player.nbVote == maxVotes)
			{
                chosenPlayer = null;
                Debug.Log("Deux joueurs ont le même nombre de voix");
			}
		}
        foreach (Player player in Players)
        {
            player.hasVoted = false;
            player.nbVote = 0;
        }
            return chosenPlayer;
=======

    /// <summary>
    /// Lance le tour des Loups Garous
    /// </summary>
    public void TourLoupGarou()
    {

>>>>>>> ffa1ce936b6c0f17e429fb3f7870c948a7276bfa
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

    public void NewMayor()
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
}
=======
    #endregion
}
