using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceNode : MonoBehaviour {

	//the types of materials
	protected double raw;
	protected double fuel;
	protected double parts;

	//the total amount of resources that can be houses here at one time
	public int resourceCapacity = 100;
	public bool outputRaw = true;
	public bool outputFuel = true;
	public bool outputParts = true;

	public int health = 100;
	public int rawToFuel = 1;
	public int rawToParts = 1;

	//a multiplier for a default rate (200ms). 1.0 = 1x
	public double processingRate = 1.0;
	private double processingTimer = 0.0;

	//what feeds this node and what it flows to
	List<ResourceNode> feeders = new List<ResourceNode>();
	List<ResourceNode> feedees = new List<ResourceNode>();
	
	void Start () {

	}

	void Update () {
		//ask the node to do its process every tick
		if(Time.time > processingTimer) {
			this.Process();
			processingTimer = Time.time + (0.2 / processingRate);
		}
	}

	public virtual void Process () {

	}

	public void AddFeeder(ResourceNode node) { feeders.Add(node); }
	public void AddFeedee(ResourceNode node) { feedees.Add(node); }

	public double GetRaw() { return raw; }
	public double GetFuel() { return fuel; }
	public double GetParts() { return parts; }

	public void AddRaw(int amt) {
		raw += amt;
	}

	public void AddFuel(int amt) {
		fuel += amt;
	}

	public void AddParts(int amt) {
		parts += amt;
	}

	//each node should check that its feedee is not at capacity before transferring resources
	public bool AtCapacity() {
		if(raw + fuel + parts >= resourceCapacity)
			return true;
		else
			return false;
	}
}
