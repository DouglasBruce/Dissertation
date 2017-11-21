/*
* Copyright (c) Douglas Bruce
*
*/

using UnityEngine;

public class NodeCollision : MonoBehaviour {

	#region Variables

	public Color hoverColor;

	private Color startColor;
	private Renderer rend;
	private GameObject connectingObject;
	private new HingeJoint hingeJoint;
	private float hingeJointX;

	private float breakForce;
	private float breakTorque;

	#endregion

	#region Unity Methods

	// Use this for initialization
	void Start () 
	{
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;
		connectingObject = null;
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(1))
		{
			if (connectingObject != null)
			{
				if (connectingObject.tag == "Track2" || connectingObject.tag == "SupportPlatform")
				{
					connectingObject.transform.parent = this.transform.parent;
				}

				if (connectingObject.transform.GetChild(0).gameObject.GetComponent<HingeJoint>() != null)
				{ 
					if (connectingObject.transform.GetChild(0).gameObject.GetComponent<HingeJoint>().connectedBody == null)
					{
						connectingObject.transform.parent.parent = this.transform.parent.transform;
						if (connectingObject.transform.parent.GetComponent<Renderer>() != null)
						{
							connectingObject.transform.parent.GetComponent<Renderer>().material.color = startColor;
						}
						hingeJoint = connectingObject.transform.GetChild(0).gameObject.GetComponent<HingeJoint>();
					}
					else
					{
						if (this.transform.parent.name == "WoodenSupport(Clone)" || this.transform.parent.name == "SteelSupport(Clone)")
						{
							hingeJoint = this.transform.parent.GetChild(0).gameObject.AddComponent<HingeJoint>();
						}
						else
						{
							hingeJoint = connectingObject.transform.GetChild(0).gameObject.AddComponent<HingeJoint>();
						}
					}
				}
				else
				{
					if (this.transform.parent.name == "WoodenSupport(Clone)" || this.transform.parent.name == "SteelSupport(Clone)")
					{
						hingeJoint = this.transform.parent.GetChild(0).gameObject.AddComponent<HingeJoint>();
					}
					else
					{
						hingeJoint = connectingObject.transform.GetChild(0).gameObject.AddComponent<HingeJoint>();
					}
				}

				if (this.transform.parent.name == "WoodenSupport(Clone)" || this.transform.parent.name == "SteelSupport(Clone)")
				{
					if(connectingObject.GetInstanceID() == this.transform.parent.parent.GetInstanceID())
					{
						hingeJoint.connectedBody = connectingObject.transform.GetChild(0).gameObject.GetComponentInChildren<Rigidbody>();
					}
					else if (connectingObject.tag == "SupportPlatform" || connectingObject.tag == "Track2")
					{
						hingeJoint.connectedBody = connectingObject.transform.GetChild(0).gameObject.GetComponentInChildren<Rigidbody>();
					}
					else if (connectingObject.name == "WoodenBridge(Clone)" || connectingObject.name == "UpgradedWoodenBridge(Clone)" || connectingObject.name == "SteelBridge(Clone)" || connectingObject.name == "UpgradedSteelBridge(Clone)")
					{
						hingeJoint.connectedBody = connectingObject.transform.GetChild(0).gameObject.GetComponentInChildren<Rigidbody>();
					}
					else
					{
						hingeJoint.connectedBody = this.transform.parent.parent.GetComponentInChildren<Rigidbody>();
					}
				}
				else
				{
					hingeJoint.connectedBody = this.transform.parent.parent.GetComponentInChildren<Rigidbody>();
				}

				if (this.transform.parent.name == "WoodenBridge(Clone)")
				{
					breakForce = 4000f;
					breakTorque = 60f;
				}
				else if (this.transform.parent.name == "UpgradedWoodenBridge(Clone)")
				{
					breakForce = 5000f;
					breakTorque = 60f;
				}
				else if (this.transform.parent.name == "WoodenSupport(Clone)")
				{
					breakForce = 5000f;
					breakTorque = 5000f;
				}
				else if (this.transform.parent.name == "SteelBridge(Clone)")
				{
					breakForce = 6000f;
					breakTorque = 60f;
				}
				else if (this.transform.parent.name == "UpgradedSteelBridge(Clone)")
				{
					breakForce = 7000f;
					breakTorque = 60f;
				}
				else if (this.transform.parent.name == "SteelSupport(Clone)")
				{
					breakForce = 7000f;
					breakTorque = 7000f;
				}

				if (connectingObject.transform.name == "Track(4)" || connectingObject.transform.name == "Track(2)")
				{
					hingeJoint.anchor = new Vector3(0f, -0.038f, 0f);
					hingeJoint.axis = new Vector3(-1, 0, 0);
					breakForce = Mathf.Infinity;
					breakTorque = 7000f;
				}
				else
				{
					hingeJoint.anchor = new Vector3(hingeJointX, 0f, 0f);
					hingeJoint.axis = new Vector3(0, 0, 1);
				}

				if (this.transform.parent.name == "WoodenSupport(Clone)" && (connectingObject.name == "WoodenBridge(Clone)" || connectingObject.name == "UpgradedWoodenBridge(Clone)" || connectingObject.name == "SteelBridge(Clone)" || connectingObject.name == "UpgradedSteelBridge(Clone)"))
				{
					HingeJoint[] joints = connectingObject.GetComponentsInChildren<HingeJoint>();
					foreach (HingeJoint joint in joints)
					{
						joint.breakTorque = 5000f;
					}
				}
				else if (this.transform.parent.name == "SteelSupport(Clone)" && (connectingObject.name == "WoodenBridge(Clone)" || connectingObject.name == "UpgradedWoodenBridge(Clone)" || connectingObject.name == "SteelBridge(Clone)" || connectingObject.name == "UpgradedSteelBridge(Clone)"))
				{
					HingeJoint[] joints = connectingObject.GetComponentsInChildren<HingeJoint>();
					foreach (HingeJoint joint in joints)
					{
						joint.breakTorque = 7000f;
					}
				}

				hingeJoint.breakForce = breakForce;
				hingeJoint.breakTorque = breakTorque;

				if (connectingObject.transform.GetChild(1).GetComponent<Renderer>() != null)
				{
					connectingObject.transform.GetChild(1).GetComponent<Renderer>().material.color = startColor;
				}
				
				rend.material.color = startColor;

				Destroy(this.gameObject);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent.parent.transform.childCount >= 2 && other.transform.parent.parent.transform != this.transform.parent.transform)
		{
			if (other.transform.parent.parent.parent.parent == null && other.transform.parent.parent.parent.tag == "Node")
			{
				if (other.transform.parent.parent.name == "SteelBridge(Clone)" || other.transform.parent.parent.name == "UpgradedSteelBridge(Clone)" || other.transform.parent.parent.name == "SteelSupport(Clone)")
				{
					hingeJointX = -0.029f;
				}
				else
				{
					hingeJointX = -0.075f;
				}
				connectingObject = other.transform.parent.parent.gameObject;
				other.transform.parent.parent.parent.GetComponent<Renderer>().material.color = hoverColor;
				rend.material.color = hoverColor;
			}
			else if (other.transform.parent.parent.GetChild(1).tag == "Node")
			{
				if (other.transform.parent.parent.name == "SteelBridge(Clone)" || other.transform.parent.parent.name == "UpgradedSteelBridge(Clone)" || other.transform.parent.parent.name == "SteelSupport(Clone)")
				{
					hingeJointX = 0.006f;
				}
				else if ((other.transform.parent.parent.name == "SupportPlatform (1)" || other.transform.parent.parent.name == "SupportPlatform") && this.transform.parent.name == "SteelSupport(Clone)")
				{
					hingeJointX = 0.006f;
				}
				else
				{
					hingeJointX = 0.016f;
				}
				connectingObject = other.transform.parent.parent.gameObject;
				other.transform.parent.parent.GetChild(1).GetComponent<Renderer>().material.color = hoverColor;
				rend.material.color = hoverColor;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.transform.parent.parent.transform.childCount >= 2 && other.transform.parent.parent.transform != this.transform.parent.transform)
		{
			if (other.transform.parent.parent.parent == null && other.transform.parent.parent.parent.tag == "Node")
			{
				connectingObject = null;
				if (other.transform.parent.parent.parent.GetComponent<Renderer>() != null)
				{
					other.transform.parent.parent.parent.GetComponent<Renderer>().material.color = startColor;
				}
				other.transform.parent.parent.GetChild(1).GetComponent<Renderer>().material.color = startColor;
				rend.material.color = startColor;
			}
			else if (other.transform.parent.parent.GetChild(1).tag == "Node")
			{
				connectingObject = null;
				if (other.transform.parent.parent.parent.GetComponent<Renderer>() != null)
				{
					other.transform.parent.parent.parent.GetComponent<Renderer>().material.color = startColor;
				}
				other.transform.parent.parent.GetChild(1).GetComponent<Renderer>().material.color = startColor;
				rend.material.color = startColor;
			}
		}
	}

	#endregion
}
