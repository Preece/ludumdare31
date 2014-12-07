using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Extractor : ResourceNode {
	
	//how this structure will move its jank each tick
	public override void Process() {
		double outputAmt = output;

		List<ResourceNode> activeFeedees = new List<ResourceNode>();

		for(int i = 0; i < feedees.Count; i++) {
			if(!feedees[i].AtCapacity()) {
				activeFeedees.Add(feedees[i]);
			}
		}

		if(activeFeedees.Count == 0) return;

		if(output > raw) {
			outputAmt = raw;
			raw = 0;
		} else {
			raw -= output;
		}


		for(int i = 0; i < activeFeedees.Count; i++) {
			double amttt = outputAmt;
			amttt /= (double)activeFeedees.Count;
			activeFeedees[i].AddRaw(amttt);
		}
	}
}