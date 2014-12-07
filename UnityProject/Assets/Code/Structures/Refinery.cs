using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Refinery : ResourceNode {

	public double rawToFuel = 3.0;
	
	//how this structure will move its jank each tick
	public override void Process() {
		//the refinery will make raw shit into fuel, and then export only
		//fuel to its recipients. if it gets parts, it will just destroy them.

		//convert raw stuff into fuel
		if(raw >= rawToFuel) {
			raw -= rawToFuel;
			fuel++;
		}

		if(fuel <= 0) return;
		
		parts = 0;

		double outputAmt = output;

		if(fuel < outputAmt) outputAmt = fuel;

		List<ResourceNode> activeFeedees = new List<ResourceNode>();

		for(int i = 0; i < feedees.Count; i++) {
			if(!feedees[i].AtCapacity()) {
				activeFeedees.Add(feedees[i]);
			}
		}

		if(activeFeedees.Count == 0) return;

		for(int i = 0; i < activeFeedees.Count; i++) {
			double amttt = outputAmt;
			amttt /= (double)activeFeedees.Count;
			activeFeedees[i].AddFuel(amttt);
			fuel -= amttt;
		}

	}
}
