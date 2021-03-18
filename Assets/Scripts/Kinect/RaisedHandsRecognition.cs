using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisedHandsRecognition : MonoBehaviour
{
    private LoupGarouGestureListener gestureListener;

    //private new Renderer renderer;

    /*
    [SerializeField] private Material m_red;
    [SerializeField] private Material m_flesh;
    */

    // Start is called before the first frame update
    void Start()
    {
        //renderer = GetComponent<Renderer>();

        gestureListener = LoupGarouGestureListener.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gestureListener)
        {
            Debug.Log("no gesture listener");
            return;
        }

        if (gestureListener.IsLeftHandRaised())
        {
            Debug.Log("GOOD left hand up");
        }
        if (gestureListener.IsRightHandRaised())
        {
            Debug.Log("GOOD right hand up");
        }
    }
}
