﻿using UnityEngine;
using System.Collections;

public class StructureController : MonoBehaviour {

	public CommandCenter commandCenter;
	public Extractor extractor;
	public Refinery refinery;
	public Fabricator fabricator;
	public Pipe pipe;
	public RawMaterials rawMaterials;

	public LayerMask obstructions;
	public LayerMask rawResources;
	public LayerMask ground;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
