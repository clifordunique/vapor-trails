﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCConversations : MonoBehaviour {

	public List<Conversation> conversations;

	public List<Conversation> auxConversations = new List<Conversation>();

	public int Count() {
		return conversations.Count;
	}

	public Conversation this[int i] {
		get {
			if (i < conversations.Count) { 
				return conversations[i]; 
			} else {
				int offset = conversations.Count;
				if (i - offset < auxConversations.Count) {
					return auxConversations[i - offset];
				} else {
					return auxConversations.Last();
				}
			}
		}
	}

	public void AddConversations(List<Conversation> c) {
		auxConversations.AddRange(c);
	}

	public void RemoveConversations(List<Conversation> c) {
		foreach (Conversation subc in c) {
			if (auxConversations.Contains(subc)) {
				auxConversations.Remove(subc);
			}
		}
		if (GetComponent<NPC>() != null) {
			GetComponent<NPC>().currentConversation = conversations.Count - 1;
		}
	}

}
