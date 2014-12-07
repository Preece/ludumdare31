using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pipe : ResourceNode {

	public MeshFilter mesh;
	
	//how this structure will move its jank each tick
	public override void Process() {

		//how many types of resources are in this pipe
		int resTypeCount = 0;
		if(raw > 0) resTypeCount++;
		if(fuel > 0) resTypeCount++;
		if(parts > 0) resTypeCount++;

		//if there are no types, then there is nothing to process
		if(resTypeCount == 0) return;

		//the output amount is the output capacity divided by the number of res types
		double outputAmt = output / resTypeCount;

		//get any feedees that have capacity
		List<ResourceNode> activeFeedees = new List<ResourceNode>();

		for(int i = 0; i < feedees.Count; i++) {
			if(!feedees[i].AtCapacity()) {
				activeFeedees.Add(feedees[i]);
			}
		}

		//if there are none with capacity, bail
		if(activeFeedees.Count == 0) return;

		//distribute eah of the three resources to each of the feedees
		if(raw > 0) {
			for(int i = 0; i < activeFeedees.Count; i++) {
				double amttt = outputAmt;
				amttt /= (double)activeFeedees.Count;
				activeFeedees[i].AddRaw(amttt);
			}
		}

		if(fuel > 0) {
			for(int i = 0; i < activeFeedees.Count; i++) {
				double amttt = outputAmt;
				amttt /= (double)activeFeedees.Count;
				activeFeedees[i].AddFuel(amttt);
			}
		}

		if(parts > 0) {
			for(int i = 0; i < activeFeedees.Count; i++) {
				double amttt = outputAmt;
				amttt /= (double)activeFeedees.Count;
				activeFeedees[i].AddParts(amttt);
			}
		}

		//deplete the resources
		if(outputAmt > raw) {
			raw = 0;
		} else {
			raw -= outputAmt;
		}

		if(outputAmt > fuel) {
			fuel = 0;
		} else {
			fuel -= outputAmt;
		}

		if(outputAmt > parts) {
			parts = 0;
		} else {
			parts -= outputAmt;
		}

	}
}
