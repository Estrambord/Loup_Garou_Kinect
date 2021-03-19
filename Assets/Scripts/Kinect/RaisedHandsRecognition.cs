using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisedHandsRecognition : MonoBehaviour
{
    private LoupGarouGestureListener gestureListener;

    [SerializeField] private bool trackOnlySpecificUser = true;
    [SerializeField][Range(0, 5)] private int allowedUserIndex = 0;

    private new Renderer renderer;

    [SerializeField] private Material m_red;
    [SerializeField] private Material m_green;
    [SerializeField] private Material m_flesh;

    [SerializeField] private AvatarController avatar0;
    [SerializeField] private AvatarController avatar1;
    [SerializeField] private AvatarController avatar2;
    [SerializeField] private AvatarController avatar3;
    [SerializeField] private AvatarController avatar4;
    [SerializeField] private AvatarController avatar5;
    private List<AvatarController> avatarList;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        gestureListener = LoupGarouGestureListener.Instance;

        gestureListener.trackOnlySpecificUser = trackOnlySpecificUser;

        if (trackOnlySpecificUser)
        {
            gestureListener.allowedUserIndex = allowedUserIndex;
        }

        renderer.material = m_red;

        avatarList = new List<AvatarController>();
        avatarList.Add(avatar0); avatarList.Add(avatar1); avatarList.Add(avatar2); avatarList.Add(avatar3); avatarList.Add(avatar4); avatarList.Add(avatar5);
    }

    void Update()
    {
        /*
        if (!gestureListener)
        {
            Debug.Log("no gesture listener");
            return;
        }

        if (gestureListener.IsLeftHandRaised())
        {
            renderer.material = m_red;
            //Debug.Log("LEFT hand up");
        }
        if (gestureListener.IsRightHandRaised())
        {
            renderer.material = m_green;
            //Debug.Log("RIGHT hand up");
        }
        if (gestureListener.AreBothHandsRaised())
        {
            renderer.material = m_flesh;
            //Debug.Log("BOTH hands up");
        }
        */

        if (avatar0.AreBothHandsUp && avatar1.AreBothHandsUp)
        {
            renderer.material = m_green;
        }

    }
}
