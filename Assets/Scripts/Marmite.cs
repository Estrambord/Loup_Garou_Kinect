using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marmite : MonoBehaviour
{
    [System.NonSerialized] public bool isChosen = false;
    [System.NonSerialized] public string chosen;
    [SerializeField] private GameManager gameManager;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("PotionLife"))
        {
            if (gameManager.remainingPotions.Contains(col.gameObject))
            {
                gameManager.remainingPotions.Remove(col.gameObject);
                chosen = "life";
                isChosen = true;
            }
            
        }
        else if (col.gameObject.CompareTag("PotionDead"))
        {
            if (gameManager.remainingPotions.Contains(col.gameObject))
            {
                gameManager.remainingPotions.Remove(col.gameObject);
                chosen = "dead";
                isChosen = true;
            }

        }
    }
}
