using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandClickScript : MonoBehaviour 
{
	[Tooltip("List of the objects that may be dragged and dropped.")]
	public GameObject[] clickableObjects;

	[Tooltip("Material used to outline the currently selected object.")]
	public Material selectedObjectMaterial;
	
	[Tooltip("Speed of dragging of the selected object.")]
	public float dragSpeed = 3.0f;

	// public options (used by the Options GUI)
	[Tooltip("Whether the objects obey gravity when released or not. Used by the Options GUI-window.")]
	public bool useGravity = true;
	[Tooltip("Whether the objects should be put in their original positions. Used by the Options GUI-window.")]
	public bool resetObjects = false;

	[Tooltip("GUI-Text used to display information messages.")]
	public Text infoGuiText;


	// interaction manager reference
	private InteractionManager manager;
	private bool isLeftHandDrag;

	// currently dragged object and its parameters
	private GameObject clickedObject;
	private float clickedObjectDepth;
	private Vector3 clickedObjectOffset;
	private Material clickedObjectMaterial;

	// initial objects' positions and rotations (used for resetting objects)
	private Vector3[] initialObjPos;
	private Quaternion[] initialObjRot;


	void Start()
	{
		// save the initial positions and rotations of the objects
		initialObjPos = new Vector3[clickableObjects.Length];
		initialObjRot = new Quaternion[clickableObjects.Length];

		for(int i = 0; i < clickableObjects.Length; i++)
		{
			initialObjPos[i] = clickableObjects[i].transform.position;
			initialObjRot[i] = clickableObjects[i].transform.rotation;
		}
	}

	void Update() 
	{
		if(resetObjects && clickedObject == null)
		{
			// reset the objects as needed
			resetObjects = false;
			ResetObjects ();
		}

		// get the interaction manager instance
		if(manager == null)
		{
			manager = InteractionManager.Instance;
		}

		if(manager != null && manager.IsInteractionInited())
		{
			Vector3 screenNormalPos = Vector3.zero;
			Vector3 screenPixelPos = Vector3.zero;
			
			if(clickedObject == null)
			{
				// if there is a hand grip, select the underlying object and start dragging it.
				if(manager.IsLeftHandPrimary())
				{
					// if the left hand is primary, check for left hand grip
					if(manager.GetLastLeftHandEvent() == InteractionManager.HandEventType.Grip)
					{
						isLeftHandDrag = true;
						screenNormalPos = manager.GetLeftHandScreenPos();
					}
				}
				if(manager.IsRightHandPrimary())
				{
					// if the right hand is primary, check for right hand grip
					if(manager.GetLastRightHandEvent() == InteractionManager.HandEventType.Grip)
					{
						isLeftHandDrag = false;
						screenNormalPos = manager.GetRightHandScreenPos();
					}
				}
				
				// check if there is an underlying object to be selected
				if(screenNormalPos != Vector3.zero)
				{
					// convert the normalized screen pos to pixel pos
					screenPixelPos.x = (int)(screenNormalPos.x * Camera.main.pixelWidth);
					screenPixelPos.y = (int)(screenNormalPos.y * Camera.main.pixelHeight);
					Ray ray = Camera.main.ScreenPointToRay(screenPixelPos);
					
					// check if there is an underlying objects
					RaycastHit hit;
					if(Physics.Raycast(ray, out hit))
					{
						foreach(GameObject obj in clickableObjects)
						{
							if(hit.collider.gameObject == obj)
							{
								// an object was hit by the ray. select it and start drgging
								clickedObject = obj;
								clickedObjectDepth = clickedObject.transform.position.z - Camera.main.transform.position.z;
								clickedObjectOffset = hit.point - clickedObject.transform.position;
								
								// set selection material
								clickedObjectMaterial = clickedObject.GetComponent<Renderer>().material;
								clickedObject.GetComponent<Renderer>().material = selectedObjectMaterial;

								// stop using gravity while dragging object
								clickedObject.GetComponent<Rigidbody>().useGravity = false;
								break;
							}
						}
					}
				}
				
			}
			else
			{
				/*
				// continue dragging the object
				screenNormalPos = isLeftHandDrag ? manager.GetLeftHandScreenPos() : manager.GetRightHandScreenPos();
				
				// convert the normalized screen pos to 3D-world pos
				screenPixelPos.x = (int)(screenNormalPos.x * Camera.main.pixelWidth);
				screenPixelPos.y = (int)(screenNormalPos.y * Camera.main.pixelHeight);
				screenPixelPos.z = screenNormalPos.z + clickedObjectDepth;
				
				Vector3 newObjectPos = Camera.main.ScreenToWorldPoint(screenPixelPos) - clickedObjectOffset;
				clickedObject.transform.position = Vector3.Lerp(clickedObject.transform.position, newObjectPos, dragSpeed * Time.deltaTime);
				*/
				
				// check if the object (hand grip) was released
				bool isReleased = isLeftHandDrag ? (manager.GetLastLeftHandEvent() == InteractionManager.HandEventType.Release) :
					(manager.GetLastRightHandEvent() == InteractionManager.HandEventType.Release);
				
				if(isReleased)
				{
					// restore the object's material and stop dragging the object
					clickedObject.GetComponent<Renderer>().material = clickedObjectMaterial;

					if(useGravity)
					{
						// add gravity to the object
						clickedObject.GetComponent<Rigidbody>().useGravity = true;
					}

					clickedObject = null;
				}
			}
		}
	}

	// reset positions and rotations of the objects
	private void ResetObjects()
	{
		for(int i = 0; i < clickableObjects.Length; i++)
		{
			clickableObjects[i].GetComponent<Rigidbody>().useGravity = false;
			clickableObjects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;

			clickableObjects[i].transform.position = initialObjPos[i];
			clickableObjects[i].transform.rotation = initialObjRot[i];
		}
	}
	
	void OnGUI()
	{
		if(infoGuiText != null && manager != null && manager.IsInteractionInited())
		{
			string sInfo = string.Empty;
			
			long userID = manager.GetUserID();
			if(userID != 0)
			{
				if(clickedObject != null)
					sInfo = "Dragging the " + clickedObject.name + " around.";
				else
					sInfo = "Please grab and drag an object around.";
			}
			else
			{
				KinectManager kinectManager = KinectManager.Instance;

				if(kinectManager && kinectManager.IsInitialized())
				{
					sInfo = "Waiting for Users...";
				}
				else
				{
					sInfo = "Kinect is not initialized. Check the log for details.";
				}
			}
			
			infoGuiText.GetComponent<Text>().text = sInfo;
		}
	}
	
}
