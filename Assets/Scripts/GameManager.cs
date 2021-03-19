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
    public KinectManager[] kinectManagers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player eliminatedPlayer = Vote_village();
        eliminatedPlayer.enabled = false;
    }

    public void Get_Ready()
    { 
    // script qui indique que le jeu commence lorsque tous les joueurs sont prêts et détecter par le Kinect
    }

    public void Discover_roles()
    {
        // script qui permet aux joueurs chacun leur tour de découvrir leur rôle
    }

    public void Elect_Mayor()
    {
        // script qui permet de faire un premier vote pour élire le maire
    }

    public void Tour_Voyante()
    {
        // script qui gère le tour de la voyante
    }

    public void Tour_Loup_Garou()
    {
        // script qui gère le tour des Loups-Garous
    }
    public void Tour_Sorciere() 
    {
        // script qui gère le tour de la sorcière
    }
    public void Tour_Chasseur()
    {
        // script qui gère le tour du chasseur
    }
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
    }
    public void Nuit() 
    {
        // script qui lance la nuit
    }
    public void jour() 
    {
        // script qui lance la journée
    }
    public void Display_UI() { }
    //public Player Get_Vote_Result(int Nb_Vote) { 
    // script qui gère le résultat du vote quotidien
    //}
    public void Execution() 
    {
    // script qui gère la mort d'un player
    }
    public void Set_Players_Roles() 
    {
        // script qui attribue les rôles aux players
    }
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
