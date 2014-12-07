using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RawMaterials : ResourceNode {
	
	//how this structure will move its jank each tick
	public override void Process() {

		double outputAmt = output;

		if(output > raw) {
			outputAmt = raw;
			raw = 0;
		} else {
			raw -= output;
		}

		List<ResourceNode> activeFeedees = new List<ResourceNode>();

		for(int i = 0; i < feedees.Count; i++) {
			if(!feedees[i].AtCapacity()) {
				activeFeedees.Add(feedees[i]);
			}
		}

		for(int i = 0; i < activeFeedees.Count; i++) {
			double amttt = outputAmt;
			amttt /= (double)activeFeedees.Count;
			activeFeedees[i].AddRaw(amttt);
		}
	}
}