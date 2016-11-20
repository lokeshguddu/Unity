﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawCurve : MonoBehaviour {
	private LineRenderer line;
	private bool isMousePressed;
	private List<Vector3> pointsList;
	private Vector3 mousePos;

	//Store points on the Catmull curve so we can visualize them
	List<Vector3> newPoints = new List<Vector3>();

	//set from 0-1
	public float alpha = 0.5f;

	//	-----------------------------------	
	void Awake() {
		// Create line renderer component and set its property
		line = gameObject.AddComponent<LineRenderer>();
		line.material =  new Material(Shader.Find("Particles/Additive"));
		line.SetVertexCount(0);
		line.SetWidth(.2f,0.2f);
		line.SetColors(Color.red, Color.red);
		line.useWorldSpace = true;	
		isMousePressed = false;
		pointsList = new List<Vector3>();
		//		renderer.material.SetTextureOffset(
	}
	//	-----------------------------------	
	void Update () {
		// If mouse button down, remove old line and set its color to green
		if(Input.GetMouseButtonDown(0)) {
			isMousePressed = true;
			line.SetVertexCount(0);
			pointsList.RemoveRange(0,pointsList.Count);
			line.SetColors(Color.red, Color.red);
		}
		else if(Input.GetMouseButtonUp(0)) {
			isMousePressed = false;
			for(int i = 0; i < pointsList.Count; i++)
			{
				Debug.Log("pointsList: " + pointsList[i]);
			}
			CatmulRom();
		}
		// Drawing line when mouse is moving(presses)
		if(isMousePressed) {
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z=0;
			if (!pointsList.Contains (mousePos)) {
				pointsList.Add (mousePos);
				line.SetVertexCount (pointsList.Count);
				line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);
			}
		}
	}

	void CatmulRom()
	{
		newPoints.Clear();

		Vector3 p0 = new Vector3(pointsList[0].x, pointsList[0].y, 0.0f);
		Vector3 p1 = new Vector3(pointsList[1].x, pointsList[1].y, 0.0f);
		Vector3 p2 = new Vector3(pointsList[2].x, pointsList[2].y, 0.0f);
		Vector3 p3 = new Vector3(pointsList[3].x, pointsList[3].y, 0.0f);

		float t0 = 0.0f;
		float t1 = GetT(t0, p0, p1);
		float t2 = GetT(t1, p1, p2);
		float t3 = GetT(t2, p2, p3);

		float amountOfPoints = pointsList.Count;

		for(float t=t1; t<t2; t+=((t2-t1)/amountOfPoints))
		{
			Vector3 A1 = (t1-t)/(t1-t0)*p0 + (t-t0)/(t1-t0)*p1;
			Vector3 A2 = (t2-t)/(t2-t1)*p1 + (t-t1)/(t2-t1)*p2;
			Vector3 A3 = (t3-t)/(t3-t2)*p2 + (t-t2)/(t3-t2)*p3;

			Vector3 B1 = (t2-t)/(t2-t0)*A1 + (t-t0)/(t2-t0)*A2;
			Vector3 B2 = (t3-t)/(t3-t1)*A2 + (t-t1)/(t3-t1)*A3;

			Vector3 C = (t2-t)/(t2-t1)*B1 + (t-t1)/(t2-t1)*B2;

			newPoints.Add(C);
		}
	}

	float GetT(float t, Vector3 p0, Vector3 p1)
	{
		float a = Mathf.Pow((p1.x-p0.x), 2.0f) + Mathf.Pow((p1.y-p0.y), 2.0f) + Mathf.Pow((p1.z-p0.z), 2.0f);
		float b = Mathf.Pow(a, 0.5f);
		float c = Mathf.Pow(b, alpha);

		return (c + t);
	}

	//Visualize the points
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		foreach(Vector3 temp in newPoints)
		{
			Vector3 pos = new Vector3(temp.x, temp.y, 0);
			Gizmos.DrawSphere(pos, 0.3f);
		}
	}
}