using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisedHandsRecognition : MonoBehaviour
{
    private LoupGarouGestureListener gestureListener;

    private new Renderer renderer;

    [SerializeField] private Material m_red;
    [SerializeField] private Material m_green;
    [SerializeField] private Material m_flesh;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        gestureListener = LoupGarouGestureListener.Instance;
    }

    void Update()
    {
        if (!gestureListener)
        {
            Debug.Log("no gesture listener");
            return;
        }

        if (gestureListener.IsLeftHandRaised())
        {
            renderer.material = m_red;
            Debug.Log("LEFT hand up");
        }
        if (gestureListener.IsRightHandRaised())
        {
            renderer.material = m_green;
            Debug.Log("RIGHT hand up");
        }
        if (gestureListener.AreBothHandsRaised())
        {
            renderer.material = m_flesh;
            Debug.Log("BOTH hands up");
        }
    }
}
