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
    public bool isAlive = true;
    public int nbVote = 0;
    [System.NonSerialized] public bool hasVoted = false;
    protected bool isAwake = true;
    [SerializeField] private string role = "citizen";
    public bool voteActivated = false;
    public string Role
    {
        get { return role; }
        set { role = value; }
    }
	public bool isMayor = false;

	public bool IsMayor
	{
		get { return isMayor; }
		set { isMayor = value; }
	}

    protected bool canDie;
    protected bool roleVisible;
    protected float votingCountdown;
    public List<GameObject> remainingPotions;
    public List<string> remainingPotionsString;

    public Player voice = null;
    public HandClickScript handClick;
    public InteractionManager interactionManager;
    public GrabDropScript grabDrop;
    #endregion

    #region Assets variables
    public Marmite marmite;
    public GameObject capHunter;
    public GameObject capWitch;
    public GameObject capTeller;
    public GameObject maskWolf;
    public GameObject starMayor;
    #endregion

    #region Prototype Variables
    public TMPro.TMP_Text roleText;

    public bool RoleDiscovered { get; set; } = false;
    public bool RoleDisplayed { get; set; } = false;

    private Renderer r { get; set; }
    public Material m_dead;
    public Material m_flesh;
    public GameObject mesh;
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
        //remainingPotionsString.Add("life");
        //remainingPotionsString.Add("dead");
        r = mesh.GetComponent<Renderer>();
    }
    
    #endregion

    /*public void Sleep()
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
    }*/

    public void Die()
    {
        r.material = m_dead;
        isAlive = false;
        //mesh.GetComponent<Renderer>().material = m_dead;
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


    public void ActivateVote()
    {
        //Activer le vote à la main
        handClick.enabled = true;
        interactionManager.enabled = true;
        voteActivated = true;
    }

    public void DeactivateVote()
    {
        handClick.enabled = false;
        interactionManager.enabled = false;
        voteActivated = false;
    }

    public void ActivateGrabDrop()
    {
        interactionManager.enabled = true;
        grabDrop.enabled = true;
    }

    public void DeactivateGrabDrop()
    {
        //interactionManager.enabled = false;
        grabDrop.enabled = false;
    }

    #region Prototype methods
    public void SetRoleUI()
    {
        Debug.Log("Setting role UI");
        roleText.gameObject.SetActive(true);
        ActivateRole();
		switch (Role)
		{
            case "teller":
                roleText.text = "Voyante";
                break;
            case "wolf":
                roleText.text = "Loup-garou";
                break;
            case "hunter":
                roleText.text = "Chasseur";
                break;
            case "citizen":
                roleText.text = "Villageois";
                break;
            case "witch":
                roleText.text = "Sorciere";
                break;
		}
        //roleText.text = Role;

        //AFFICHER UI SELON LE PERSONNAGE
        /*string nomSphere = "Sphere (" + gameObject.GetComponent<AvatarController>().playerIndex.ToString() + ")";
        transform.Find(nomSphere).gameObject.SetActive(true);*/
    }

    public void ActivateRole()
	{
        switch (Role)
		{
            case "teller":
                capTeller.SetActive(true);
                break;
            case "wolf":
                maskWolf.SetActive(true);
                break;
            case "witch":
                capWitch.SetActive(true);
                break;
            case "hunter":
                capHunter.SetActive(true);
                break;
        }
	}

    public void DeactivateRole()
    {
        switch (Role)
        {
            case "teller":
                capTeller.SetActive(false);
                break;
            case "wolf":
                maskWolf.SetActive(false);
                break;
            case "witch":
                capWitch.SetActive(false);
                break;
            case "hunter":
                capHunter.SetActive(false);
                break;
        }
    }

    public void ActivateMayor()
	{
        if (IsMayor)
		{
            starMayor.SetActive(true);
		}
	}

    public void DeactivateMayor()
    {
        if (IsMayor)
        {
            starMayor.SetActive(false);
        }
    }

    public void SetUI(string s)
    {
        roleText.text = s;
    }

    public void MakeFlesh()
    {
        r.material = m_flesh;
    }

    public void MakeDead()
	{
        r.material = m_dead;
	}
    #endregion
}