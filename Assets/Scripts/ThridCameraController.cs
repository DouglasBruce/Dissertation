/*
* Copyright (c) Douglas Bruce
*
*/

using UnityEngine;

public class ThridCameraController : MonoBehaviour {

	#region Variables

	public float scrollSpeed = 15f;
	public float scrollEdge = 0.02f;
	public float panSpeed = 10f;
	
	public float currentZoom = 0f;

	public float minX;
	public float maxX;
	public float minZ;
	public float maxZ;

	private Vector3 initialPosition;

	public bool doMovement = true;

	#endregion

	#region Unity Methods

	// Use this for initialization
	void Start () 
	{
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float scrollX = Input.GetAxis("Horizontal");

		if (GameManager.GameIsOver)
		{
			this.enabled = false;
			return;
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			doMovement = !doMovement;
		}

		if (!doMovement)
		{
			return;
		}

		if (Input.GetMouseButton(0))
		{
			transform.Translate(Vector3.right * Time.deltaTime * panSpeed * (Input.mousePosition.x - Screen.width * 0.5f) / (Screen.width * 0.5f), Space.World);
			transform.Translate(Vector3.forward * Time.deltaTime * panSpeed * (Input.mousePosition.y - Screen.height * 0.5f) / (Screen.height * 0.5f), Space.World);
		}
		else
		{
			if (scrollX > 0 || Input.mousePosition.x >= Screen.width * (1 - scrollEdge))
			{
				transform.Translate(Vector3.right * Time.deltaTime * panSpeed, Space.World);
			}
			else if (scrollX < 0 || Input.mousePosition.x <= Screen.width * scrollEdge)
			{
				transform.Translate(Vector3.left * Time.deltaTime * panSpeed, Space.World);
			}
		}

		Vector3 tempPos = transform.position;
		tempPos.x = Mathf.Clamp(transform.position.x, minX, maxX);
		tempPos.z = Mathf.Clamp(transform.position.z, minZ, maxZ);
		transform.position = tempPos;

		transform.position = new Vector3(transform.position.x, transform.position.y - (transform.position.y - (initialPosition.y + currentZoom)) * 0.1f, transform.position.z);
	}
	
	#endregion
}
