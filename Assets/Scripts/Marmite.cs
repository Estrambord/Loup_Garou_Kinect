using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marmite : MonoBehaviour
{
    [NonSerialized] public bool isChosen = false;
    [NonSerialized] public string chosen;
    [SerializedField] private GameManager gameManager;
    // Start is called before the first frame update
    private void OnCollisionEnter(collision col)
    {
        if (col.GameObject.CompareTag("PotionLife"))
        {
            if (gameManager.remainingPotions.Contains(col.GameObject))
            {
                gameManager.remainingPotions.Remove(col.GameObject);
                chosen = "life";
                isChosen = true;
            }
            
        }
        else if (col.GameObject.CompareTag("PotionDead"))
        {
            if (gameManager.remainingPotions.Contains(col.GameObject))
            {
                gameManager.remainingPotions.Remove(col.GameObject);
                chosen = "dead";
                isChosen = true;
            }

        }
    }
}
