using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
	/*[SerializeField] private GameObject _buttonChoose;
	[SerializeField] private GameObject _buttonLeft;
	[SerializeField] private GameObject _buttonRight;*/


	private List<GameObject> buttons;
    // Start is called before the first frame update
    void Start()
    {
		buttons = new List<GameObject>();
		buttons.Add(transform.parent.gameObject.transform.Find("Head").gameObject.transform.Find("ButtonChoose").gameObject);
		buttons.Add(transform.parent.gameObject.transform.Find("Head").gameObject.transform.Find("ButtonChoose").gameObject.transform.Find("ButtonLeft").gameObject);
		buttons.Add(transform.parent.gameObject.transform.Find("Head").gameObject.transform.Find("ButtonChoose").gameObject.transform.Find("ButtonRight").gameObject);

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.gameObject.CompareTag("Button"))
		{
            return;
		}
        this.gameObject.SetActive(false);
		collision.gameObject.GetComponent<Image>().color = Color.red;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Hand"))
		{
			collision.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
			GameObject[] allButtons = GameObject.FindGameObjectsWithTag("Button");
			foreach (GameObject button in buttons)
			{
				button.SetActive(true);
			}
		}
		
		if (!collision.gameObject.CompareTag("Button"))// || (collision.gameObject.CompareTag("Button") && !buttons.Contains(collision.gameObject)))
		{
			return;
		}
		//this.gameObject.SetActive(false);
		collision.gameObject.SetActive(false);
	}

}
