using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pipe : ResourceNode {

	public MeshFilter mesh;
	
	//how this structure will move its jank each tick
	public override void Process() {

		int resTypeCount = 0;
		if(raw > 0) resTypeCount++;
		if(fuel > 0) resTypeCount++;
		if(parts > 0) resTypeCount++;

		if(resTypeCount == 0) return;

		double outputAmt = output / resTypeCount;

		List<ResourceNode> activeFeedees = new List<ResourceNode>();

		for(int i = 0; i < feedees.Count; i++) {
			if(!feedees[i].AtCapacity()) {
				activeFeedees.Add(feedees[i]);
			}
		}

		if(activeFeedees.Count == 0) return;

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


		if(raw > 0) {
			for(int i = 0; i < activeFeedees.Count; i++) {
				double amttt = outputAmt;
				amttt /= (double)activeFeedees.Count;
				amttt /= (double)resTypeCount;
				activeFeedees[i].AddRaw(amttt);
			}
		}

		if(fuel > 0) {
			for(int i = 0; i < activeFeedees.Count; i++) {
				double amttt = outputAmt;
				amttt /= (double)activeFeedees.Count;
				amttt /= (double)resTypeCount;
				activeFeedees[i].AddFuel(amttt);
			}
		}

		if(parts > 0) {
			for(int i = 0; i < activeFeedees.Count; i++) {
				double amttt = outputAmt;
				amttt /= (double)activeFeedees.Count;
				amttt /= (double)resTypeCount;
				activeFeedees[i].AddParts(amttt);
			}
		}
	}
}
