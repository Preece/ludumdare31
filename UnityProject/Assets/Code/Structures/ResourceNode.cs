using UnityEngine;
using System.Collections;

public class ResourceNode : MonoBehaviour {

	//the types of materials
	protected int raw;
	protected int fuel;
	protected int parts;

	//a multiplier for a default rate (200ms). 1.0 = 1x
	protected double processingRate = 1.0;
	private double processingTimer = 0.0;

	//what feeds this node and what it flows to
	ResourceNode feeder = null;
	ResourceNode feedee = null;
	
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

	public int GetRaw() { return raw; }
	public int GetFuel() { return fuel; }
	public int GetParts() { return parts; }

	public void AddRaw(int amt) {
		raw += amt;
	}

	public void AddFuel(int amt) {
		fuel += amt;
	}

	public void AddParts(int amt) {
		parts += amt;
	}
}
