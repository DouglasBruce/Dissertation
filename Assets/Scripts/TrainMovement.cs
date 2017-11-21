/*
* Copyright (c) Douglas Bruce
*
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TrainMovement : MonoBehaviour {

	#region Variables
	public GameManager gameManager;
	public PauseMenu pauseMenu;
	public Timer timer;

	public Rigidbody rb;
	public Animator anim;
	public float forwardForce = 160.0f;

	public Text trainForceForward;
	public Text trainForceBackward;
	public Text trainForceForwardTop;

	[HideInInspector]
	public Vector3 startPos;
	[HideInInspector]
	public Quaternion startRot;
	[HideInInspector]
	public GameObject[] particles;
	[HideInInspector]
	public List<GameObject> particle;
	#endregion

	#region Unity Methods

	// Use this for initialization
	void Start()
	{
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		pauseMenu = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PauseMenu>();
		timer = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Timer>();

		trainForceForward = GameObject.FindGameObjectWithTag("TrainForceForward").GetComponent<Text>();
		trainForceBackward = GameObject.FindGameObjectWithTag("TrainForceBackward").GetComponent<Text>();
		trainForceForwardTop = GameObject.FindGameObjectWithTag("TrainForceRightTop").GetComponent<Text>();

		startPos = transform.GetChild(0).transform.position;
		startRot = transform.GetChild(0).transform.rotation;
		
		anim = GetComponent<Animator>();
		particles = GameObject.FindGameObjectsWithTag("Smoke");

		if(particles.Length > 1)
		{
			particle.Insert(0, particles[1]);
		}
		else
		{
			particle.Insert(0, particles[0]);
		}

		var emission = particle[0].gameObject.GetComponent<ParticleSystem>().emission;
		emission.enabled = false;
	}

	void FixedUpdate()
	{
		if (!pauseMenu.gamePaused)
		{ 
			if (GameManager.GameInPlay && !PlayerStats.IsDead)
			{
				gameManager.StartGame();

				if (timer.countdown <= 0)
				{
					gameManager.DisableButtons();
				}
				
				trainForceForward.text = forwardForce.ToString() + " N";
				trainForceForwardTop.text = forwardForce.ToString() + " N";

				rb.useGravity = true;
				rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;

				anim.SetTrigger("Moving");
				var emission = particle[0].gameObject.GetComponent<ParticleSystem>().emission;
				emission.enabled = true;
				rb.AddForce(forwardForce * Time.deltaTime, 0, 0);
			}
			else if (GameManager.GameIsOver && !GameManager.GameInPlay)
			{
				trainForceForward.text = "0 N";
				trainForceForwardTop.text = "0 N";

				rb.useGravity = false;
				rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
				rb.velocity = new Vector3(0f, 0f, 0f);
				anim.SetTrigger("Idle");
				var emission = particle[0].gameObject.GetComponent<ParticleSystem>().emission;
				emission.enabled = false;
			}
			else if (GameManager.GameIsOver && GameManager.GameInPlay)
			{
				trainForceForward.text = "0 N";
				trainForceForwardTop.text = "0 N";

				anim.SetTrigger("Idle");
				var emission = particle[0].gameObject.GetComponent<ParticleSystem>().emission;
				emission.enabled = false;
			}
			else if (!GameManager.GameIsOver && !PlayerStats.IsDead)
			{
				trainForceForward.text = "0 N";
				trainForceForwardTop.text = "0 N";

				rb.useGravity = false;
				rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

				transform.GetChild(0).transform.position = startPos;
				transform.GetChild(0).transform.rotation = startRot;

				rb.velocity = new Vector3(0f, 0f, 0f);
				anim.SetTrigger("Idle");
				var emission = particle[0].gameObject.GetComponent<ParticleSystem>().emission;
				emission.enabled = false;
			}
		}
		else
		{
			rb.useGravity = false;
			rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
			rb.velocity = new Vector3(0f, 0f, 0f);
			anim.SetTrigger("Idle");
			var emission = particle[0].gameObject.GetComponent<ParticleSystem>().emission;
			emission.enabled = false;
		}
	}
	#endregion
}
