using UnityEngine;
//using Windows.Kinect;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class LoupGarouGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
	private bool leftHandUp;
	private bool rightHandUp;
	private bool bothHandsUp;

	[System.NonSerialized] public bool trackOnlySpecificUser = false;
	[System.NonSerialized] public int allowedUserIndex = 0;

	[SerializeField] private AvatarController avatar0;
	[SerializeField] private AvatarController avatar1;
	[SerializeField] private AvatarController avatar2;
	[SerializeField] private AvatarController avatar3;
	[SerializeField] private AvatarController avatar4;
	[SerializeField] private AvatarController avatar5;
	private List<AvatarController> avatarList; 

	// singleton instance of the class
	private static LoupGarouGestureListener instance = null;

	void Awake() { instance = this; }

    private void Start()
    {
		avatarList = new List<AvatarController>();
		avatarList.Add(avatar0); avatarList.Add(avatar1); avatarList.Add(avatar2); avatarList.Add(avatar3); avatarList.Add(avatar4); avatarList.Add(avatar5);
	}

    /// <summary>
    /// Gets the singleton CubeGestureListener instance.
    /// </summary>
    /// <value>The CubeGestureListener instance.</value>
    public static LoupGarouGestureListener Instance { get { return instance; } }

	public void UserDetected(long userId, int userIndex)
	{
		KinectManager manager = KinectManager.Instance;
		if (trackOnlySpecificUser)
		{
			if (!manager || (userId != manager.GetUserIdByIndex(allowedUserIndex)))
			{
				return;
			}
		}

		manager.DetectGesture(userId, KinectGestures.Gestures.RaiseLeftHand);
		manager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);
		manager.DetectGesture(userId, KinectGestures.Gestures.Psi);
	}

	public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
								  KinectInterop.JointType joint, Vector3 screenPos)
	{
		KinectManager manager = KinectManager.Instance;

        if (trackOnlySpecificUser)
        {
			if (!manager || (userId != manager.GetUserIdByIndex(allowedUserIndex)))
            {
				return false;
            }			
        }

		if (gesture == KinectGestures.Gestures.RaiseLeftHand)
		{
			leftHandUp = true;
			avatarList[manager.GetUserIndexById(userId)].IsLeftHandUp = leftHandUp;
		}
		if (gesture == KinectGestures.Gestures.RaiseRightHand)
		{
			rightHandUp = true;
			avatarList[manager.GetUserIndexById(userId)].IsRightHandUp = rightHandUp;
		}
		if (gesture == KinectGestures.Gestures.Psi)
		{
			bothHandsUp = true;
			avatarList[manager.GetUserIndexById(userId)].AreBothHandsUp = bothHandsUp;
		}

        return true;
	}

	/// <summary>
	/// Determines whether left hand is raised.
	/// </summary>
	/// <returns><c>true</c> if left hand is raised; otherwise, <c>false</c>.</returns>
	public bool IsLeftHandRaised()
	{
		if (leftHandUp)
		{
			leftHandUp = false;
			return true;
		}

		return false;
	}

	/// <summary>
	/// Determines whether right hand is raised.
	/// </summary>
	/// <returns><c>true</c> if right hand is raised; otherwise, <c>false</c>.</returns>
	public bool IsRightHandRaised()
	{
		if (rightHandUp)
		{
			rightHandUp = false;
			return true;
		}

		return false;
	}

	/// <summary>
	/// Determines whether both arms are raised.
	/// </summary>
	/// <returns><c>true</c> if both arms are raised; otherwise, <c>false</c>.</returns>
	public bool AreBothHandsRaised()
	{
		if (bothHandsUp)
		{
			bothHandsUp = false;
			return true;
		}

		return false;
	}

	#region Useless Methods needed for the Kinect Gestures library to work properly
	public void UserLost(long userId, int userIndex)
	{

	}

	public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
								  float progress, KinectInterop.JointType joint, Vector3 screenPos)
	{

	}

	public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
								  KinectInterop.JointType joint)
	{
		return true;
	}

	public void Update()
	{

	}
    #endregion
}