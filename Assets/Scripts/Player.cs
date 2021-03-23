using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    #region Variables

    #region Gesture Variables
    public bool IsLeftHandUp { get; set; } = false;

    public bool IsRightHandUp { get; set; } = false;

    public bool IsPlayerReady { get; set; } = false;
	#endregion


	#region Player variables
	[System.NonSerialized] public bool isAlive = true;
    [System.NonSerialized] public int nbVote = 0;
    protected bool isMayor = false;
    [System.NonSerialized] public bool hasVoted = false;
    protected bool isAwake = true;
    [SerializeField] private string role = "citizen";
    public string Role
    {
        get { return role; }
        set { role = value; }
    }
    public bool IsMayor { get; set; } = false;
    protected bool canDie;
    protected bool roleVisible;
    protected float votingCountdown;
    public List<GameObject> remainingPotions;
    public List<string> remainingPotionsString;

    public Player voice = null;
    public HandClickScript handClick;
    public InteractionManager interactionManager;
    public Marmite marmite;
	#endregion


	#region Prototype Variables
	public GameObject voteUI;
    public TMPro.TMP_Text roleText;
    public TMPro.TMP_Text player;

    public bool RoleDiscovered { get; set; } = false;

    public Material m_yellow;

    public GameObject mesh;

    private Renderer r;
    #endregion

    #endregion

    #region Unity Base Methods
    void Start()
    {
        //role.enabled = false;
        nbVote = 0;
        hasVoted = false;
        isAwake = false;
        roleVisible = false;
        canDie = false;
        isAlive = true;
        remainingPotionsString.Add("life");
        remainingPotionsString.Add("dead");
        r = mesh.GetComponent<Renderer>();
    }

    void Update()
    {
       
    }
    #endregion

    public void Sleep()
    {
        //endort un player
        roleText.enabled = false;
        player.enabled = true;
        isAwake = false;
    }

    public void WakeUp()
    {
        //réveille un player
        roleText.enabled = true;
        player.enabled = false;
        isAwake = true;
    }

    public void Die()
    {
        //tue possiblement un joueur sous réserve d'intervention de la sorcière
        isAlive = false;
        canDie = true;
        enabled = false;
    }

    public void Revive()
    {
        //réssuscite un joueur grâce à la sorcière
        canDie = false;
        isAlive = true;
    }

    public void StandardVote(Player player)
    {
		if (player.isAlive)
		{
            voice = player;
            Debug.Log(voice);
            player.nbVote++;
            Debug.Log("Le joueur " + this + " a voté contre le joueur " + player);
            Debug.Log("le " + player + " a " + player.nbVote + " votes contre lui");
        }
		else
		{
            Debug.Log("Le joueur " + player + " est mort, vous ne pouvez pas voter contre lui.");
        }
    }
    



    //Méthodeà déplacer dans le GameManager pour avoir accès à tous les joueurs
    public virtual void SpecialVote(Player player)
    {

        switch (this.Role)
        {
            case ("witch"):
                if (remainingPotionsString.Contains("life")){
                    //Activer les potions grabbable
                    remainingPotions[0].SetActive(true);
                }
                if (remainingPotionsString.Contains("dead"))
                {
                    //Activer les potions grabbable
                    remainingPotions[1].SetActive(true);
                }
                //Demander de choisir une potion (son)
                //Au trigger de la marmite faire
                if (marmite.isChosen)
                {
                    if(marmite.chosen == "life")
                    {
                        //foreach(Player player in Players){
                        //  if player.canDie == true{
                        //      player.canDie = false;
                        //  }
                        //}
                    }
                    else
                    {
                        ActivateVote();
                        //Tuer player sur qui est la voix
                        DeactivateVote();
                    }
                }

                //Si la potion est lâchée, réinitialiser la position (GrabScript)

                break;
            case ("hunter"):
                break;
            case ("teller"):
                break;
            case ("wolf"):
                break;
        }
        //le script de vote des personnages à rôles spéciaux

    }

    public void ActivateVote()
    {
        //Activer le vote à la main
        handClick.enabled = true;
        interactionManager.enabled = true;
    }

    public void DeactivateVote()
    {
        handClick.enabled = false;
        interactionManager.enabled = false;
    }

    #region Prototype methods
    public void SetRoleUI()
    {
        roleText.text = Role;
    }

    public void SetUI(string s)
    {
        roleText.text = s;
    }

    public void MayorYellow()
    {
        r.material = m_yellow;
    }

    #endregion

}