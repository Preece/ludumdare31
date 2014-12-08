using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fabricator : ResourceNode {
	
	public double rawPerPart = 3.0;
	public double fuelPerPart = 2.0;

	//how this structure will move its jank each tick
	public override void Process() {
		//convert raw stuff into fuel
		if(raw >= rawPerPart && fuel >= fuelPerPart) {
			raw -= rawPerPart;
			fuel -= fuelPerPart;
			parts++;
		}

		if(parts <= 0) return;

		double outputAmt = output;

		if(parts < outputAmt) outputAmt = parts;

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
			activeFeedees[i].AddParts(amttt);
			parts -= amttt;
		}
	}
}