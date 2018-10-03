﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : ActivationCriteria {
	public bool flipped = false;
	public bool active = true;
	public string flagName;
	public bool flipToInactive = false;
	bool satisfied = false;

	public override bool CheckSatisfied() {
		Animator anim;
		if ((anim = GetComponent<Animator>()) != null) {
			anim.SetBool("Active", active);
		}
		bool preSatisfied = this.satisfied;
		this.satisfied = false;
		return preSatisfied;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (active && other.CompareTag(Tags.PlayerHitbox)) {
			PlayerAttack a = other.GetComponent<PlayerAttack>();
			if (a != null) {
				CameraShaker.Shake(a.cameraShakeIntensity, a.cameraShakeTime);
				//instantiate the hitmarker
				if (a.hitmarker != null) {
					Instantiate(a.hitmarker, this.transform.position, Quaternion.identity);
				}
			}

			flipped = !flipped;
			if (flipToInactive) active = false;

			Animator anim;
			if ((anim = GetComponent<Animator>()) != null) {
				print("PINGAS");
				anim.SetBool("Flipped", flipped);
				if (flipToInactive) {
					anim.SetBool("Active", false);
				}
			}

			//then at the end of the flip, reset
			satisfied = true;
		}
	}
}
