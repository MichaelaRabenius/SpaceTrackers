using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeMaterial : MonoBehaviour {

	public TextAsset imageAsset;
	// Use this for initialization
	void Start () {
		Texture2D tex = new Texture2D (2, 2);
		tex.LoadImage (imageAsset.bytes);
		GetComponent<Renderer> ().material.mainTexture = tex;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
