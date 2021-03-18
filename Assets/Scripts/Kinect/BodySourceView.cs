using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour 
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;

    //public PartyManager myManager;

    public GameObject jointPrefab;
    public GameObject jointPrefabHead;
    public GameObject jointPrefabHand;
    private Color boneColor = new Color(1f, 0.51f, 0f, 1f);

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
    
    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };

    private List<Kinect.JointType> _joints = new List<Kinect.JointType>{ Kinect.JointType.HandLeft, Kinect.JointType.HandRight, Kinect.JointType.Head};
    
    void Update () 
    {
        if (BodySourceManager == null)
        {
            return;
        }
        
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
              }
                
            if(body.IsTracked)
            {
                trackedIds.Add (body.TrackingId);
            }
        }

        #region Delete Kinect bodies
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
		// First delete untracked bodies
		foreach (ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }
		#endregion

		#region Create Kinect bodies
		foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }
            
            if(body.IsTracked)
            {
                if(!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }
                
                RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }
        }
		#endregion
	}

	private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        GameObject jointObj;

        foreach (Kinect.JointType joint in _joints)
		{
            if(joint == Kinect.JointType.Head)
            {
                jointObj = Instantiate(jointPrefabHead);
            }
            else
			{
                jointObj = Instantiate(jointPrefabHand);
            }
            jointObj.name = joint.ToString();
            jointObj.transform.parent = body.transform;
        }

		#region Track Full body
		/*for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            
            //GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (jt == Kinect.JointType.Head)
            {
                jointObj = Instantiate(jointPrefabHead);
                //myManager.AddPlayer(jointObj.GetComponent<Player>());
            }
            else if (jt == Kinect.JointType.HandLeft || jt == Kinect.JointType.HandRight)
			{
                jointObj = Instantiate(jointPrefabHand);
            }
            else
            {
                jointObj = Instantiate(jointPrefab);
            }




            //LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            //lr.SetVertexCount(2);
            //lr.material = BoneMaterial;
            //lr.SetWidth(0.2f, 0.2f);
            //lr.startColor = boneColor;
            //lr.endColor = boneColor;
            
            //jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }*/
		#endregion

		return body;
    }
    
    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {

        foreach (Kinect.JointType _joint in _joints)
        {
            Kinect.Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            targetPosition.z = 0;

            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
            jointObject.position = targetPosition;
		}

		#region Refresh full body

		/*for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            //Kinect.JointType.
            
            if(_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }
            
            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);*/

		/*
        LineRenderer lr = jointObj.GetComponent<LineRenderer>();
        if(targetJoint.HasValue)
        {
            lr.SetPosition(0, jointObj.localPosition);
            lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
            //lr.SetColors(GetColorForState (sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            lr.startColor = boneColor;
            lr.endColor = boneColor;
        }
        else
        {
            lr.enabled = false;
        }
        */
		//}
		#endregion
	}

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
        case Kinect.TrackingState.Tracked:
            return Color.green;

        case Kinect.TrackingState.Inferred:
            return Color.red;

        default:
            return Color.black;
        }
    }
    
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

   
}
