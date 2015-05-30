﻿using UnityEngine;
using System.Collections;
using System.Threading;

public class FloorCube : MonoBehaviour {

	public int isMoving = 0;
	public bool canMove = true;
	private RaycastHit hit;
	private bool isShut = true;
	private int timer = 0;
	private bool isChangeMaterail = false;
	private int changeMaterailCount = 0;

	public Material changeMaterail;
	public Material originalMaterail;

	private float xrayDistance = 1.0f;
	private int explosiveSpeed = 1;   //explosive speed related to time; only take 1, 2, 5
	// Use this for initialization
	void Start () {
		foreach(Transform child in transform)
		{
			child.gameObject.SetActive(false);
			child.gameObject.particleSystem.Stop();

			transform.localPosition = new Vector3(transform.localPosition.x,0,transform.localPosition.z);

			gameObject.GetComponent<BoxCollider>().center = new Vector3(0f,0f,0.05f);
			gameObject.GetComponent<BoxCollider>().size = new Vector3(0.1f,0.1f,0.1f);
			//transform.eulerAngles = new Vector3(270,0,0);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		if(isOnCube())
		{
			canMove = false;
		}
		else
		{
			canMove = true;
		}

		if (isMoving == 1 ) {
			gameObject.GetComponent<BoxCollider>().center = new Vector3(0f,0f,0.0749f);
			gameObject.GetComponent<BoxCollider>().size = new Vector3(0.1f,0.1f,0.15f);
			//transform.eulerAngles = new Vector3(270,0,0);
			renderer.material = originalMaterail;
			isChangeMaterail = false;
			int upTime =50/explosiveSpeed;
			if(timer==0) rigidbody.velocity = new Vector3(rigidbody.velocity.x,explosiveSpeed,rigidbody.velocity.z);
			else if(timer==upTime)
			{
				foreach(Transform child in transform)
				{
					child.gameObject.SetActive(true);
					child.gameObject.particleSystem.startLifetime = xrayDistance/10;      //xrayDistance related to start life time
					child.gameObject.particleSystem.startSize = 0.5f;

					//child.gameObject.particleSystem.maxParticles = 20;
					//child.gameObject.particleSystem.startSpeed = 15;
					child.gameObject.particleSystem.Play();
				}
				rigidbody.velocity = new Vector3(0,0,0);

			}
			else if(timer==upTime+100) 
			{
				foreach(Transform child in transform)
				{
					child.gameObject.SetActive(false);
					child.gameObject.particleSystem.Stop();
				}
				rigidbody.velocity = new Vector3(0,-explosiveSpeed,0);
			}
			else if(timer==2*upTime +100) 
			{
				rigidbody.velocity = new Vector3(0,0,0);
				transform.localPosition = new Vector3(transform.localPosition.x,0,transform.localPosition.z);
				isMoving = 0;
				gameObject.GetComponent<BoxCollider>().center = new Vector3(0f,0f,0.05f);
				gameObject.GetComponent<BoxCollider>().size = new Vector3(0.1f,0.1f,0.1f);
				//transform.eulerAngles = new Vector3(270,0,0);
				SendMessagetoCharactor();
			}
			timer++; 
			if(isMoving == 0){
				timer=0;
			}
		}
		if(isChangeMaterail)
		{
			changeMaterailCount++;
			if(changeMaterailCount==10) 
			{
				renderer.material = originalMaterail;
				changeMaterailCount=0;
			}
		}
		if (isMoving == 0) {
			transform.localPosition = new Vector3(transform.localPosition.x,0,transform.localPosition.z);
		}
	}

	public void moving(float xray, int explosive){
			isMoving = 1;
			xrayDistance = xray;
			explosiveSpeed = explosive;
	}

	public void ChangeMaterial(){
		renderer.material = changeMaterail;
		isChangeMaterail = true;
	}

	void SendMessagetoCharactor(){
		GameObject charactor = GameObject.Find ("First Person Controller");
		charactor.GetComponent<MouseLook> ().getMessage (gameObject);
	}

	bool isOnCube(){
		GameObject charactor = GameObject.Find ("First Person Controller");
		foreach(Transform j in charactor.GetComponent<SenceLoad>().CreatureList)
		{
			if(j.position.x<=gameObject.transform.position.x+0.7f&&j.position.x>=gameObject.transform.position.x-0.7f)
			{
				if(j.position.z<=gameObject.transform.position.z+0.7f&&j.position.z>=gameObject.transform.position.z-0.7f)
				{
					return true;
				}
			}
		}
		return false;
	}
}
