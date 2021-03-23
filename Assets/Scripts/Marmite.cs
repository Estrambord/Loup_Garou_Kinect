using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marmite : MonoBehaviour
{
    [System.NonSerialized] public bool isChosen = false;
    [System.NonSerialized] public string chosen;
    [SerializeField] private GameManager gameManager;

    public List<string> remainingPotions;
    // Start is called before the first frame update


    void Start()
    {
        remainingPotions = new List<string> { "life", "dead" };
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("PotionLife"))
        {
            if (remainingPotions.Contains("life"))
            {
                remainingPotions.Remove("life");
                chosen = "life";
                isChosen = true;
                col.gameObject.SetActive(false);
                Debug.Log("La potion est choisie : " + col.gameObject.tag);
            }
        }
        else if (col.gameObject.CompareTag("PotionDead"))
        {
            if (remainingPotions.Contains("dead"))
            {
                remainingPotions.Remove("dead");
                chosen = "dead";
                isChosen = true;
                col.gameObject.SetActive(false);
                Debug.Log("La potion est choisie : " + col.gameObject.tag);
            }

        }
    }
}
