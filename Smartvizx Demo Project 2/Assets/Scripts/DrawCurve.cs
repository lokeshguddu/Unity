﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawCurve : MonoBehaviour {
	private LineRenderer line;
	private bool isMousePressed;
	private List<Vector3> pointsList;
	private Vector3 mousePos;

	// Structure for line points
	struct myLine {
		public Vector3 StartPoint;
		public Vector3 EndPoint;
	};
	//	-----------------------------------	
	void Awake() {
		// Create line renderer component and set its property
		line = gameObject.AddComponent<LineRenderer>();
		line.material =  new Material(Shader.Find("Particles/Additive"));
		line.SetVertexCount(0);
		line.SetWidth(0.1f,0.1f);
		line.SetColors(Color.green, Color.green);
		line.useWorldSpace = true;	
		isMousePressed = false;
		pointsList = new List<Vector3>();
		//		renderer.material.SetTextureOffset(
	}
	//	-----------------------------------	
	void Update () {
		// If mouse button down, remove old line and set its color to green
		if(Input.GetMouseButtonDown(0))
		{
			isMousePressed = true;
			line.SetVertexCount(0);
			pointsList.RemoveRange(0,pointsList.Count);
			line.SetColors(Color.green, Color.green);
		}
		else if(Input.GetMouseButtonUp(0))
		{
			isMousePressed = false;
		}
		// Drawing line when mouse is moving(presses)
		if(isMousePressed)
		{
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z=0;
			if (!pointsList.Contains (mousePos)) 			{
				pointsList.Add (mousePos);
				line.SetVertexCount (pointsList.Count);
				line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);
			}
		}
	}
}