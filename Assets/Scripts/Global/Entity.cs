﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool movingRight = false;
    public bool frozen = false;
    public bool lockedInSpace = false;
	public bool inHitstop = false;

    public bool stunned = false;
    public bool staggerable = false;
    public Coroutine unStunRoutine;

    public bool invincible = false;
    public bool envDmgSusceptible = true;

    public void Flip() 
	{
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        //flip by scaling -1
    }

    public void Destroy() {
        Destroy(this.gameObject);
    }

    public void CheckFlip() {
        if (frozen || lockedInSpace) {
            return;
        }
        Rigidbody2D rb2d;
        if ((rb2d = GetComponent<Rigidbody2D>()) != null) {
            if (!facingRight && rb2d.velocity.x > 0 && movingRight)
            {
                Flip();
            }
            else if (facingRight && rb2d.velocity.x < 0 && !movingRight)
            {
                Flip();
            }
        }
    }

    public void LockInSpace() {
        Rigidbody2D rb2d;
        if ((rb2d = GetComponent<Rigidbody2D>()) != null) {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            this.lockedInSpace = true;
        }
    }

    public virtual void UnLockInSpace() {
        Rigidbody2D rb2d;
        if ((rb2d = GetComponent<Rigidbody2D>()) != null) {
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            this.lockedInSpace = false;
        }
    }

    //returns the x-direction the entity is facing
    public int GetForwardScalar() {
        return facingRight ? 1 : -1;
    }

    public void StunFor(float seconds) {
		if (staggerable) {
			//if the enemy is already stunned, then resstart the stun period
			if (stunned) {
				StopCoroutine(unStunRoutine);
				unStunRoutine = StartCoroutine(WaitAndUnStun(seconds));
			} else {
				stunned = true;
				unStunRoutine = StartCoroutine(WaitAndUnStun(seconds));
			}
		}
	}

	public void KnockBack(Vector2 kv) {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
		if (staggerable && rb2d != null) {
			rb2d.velocity = kv;
		}
	}

	IEnumerator WaitAndUnStun(float seconds) {
		yield return new WaitForSeconds(seconds);
		stunned = false;
        if (this.GetComponent<Animator>() != null) {
            Animator anim = GetComponent<Animator>();
            anim.logWarnings = false;
		    anim.SetBool("Stunned", false);
        }
	}

    public virtual void OnHit(Attack a) {

    }

    public virtual void OnGroundHit() {

    }

    public virtual void OnGroundLeave() {
        
    }

    public void Hide() {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (spr != null) {
            spr.enabled = false;
        }
    }

    public void Show() {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (spr != null) {
            spr.enabled = true;
        }
    }

    public bool IsLookingAt(GameObject o) {
        float sign = o.transform.position.x - this.transform.position.x;
        return ((facingRight && sign>0) || (!facingRight && sign<0));
    }
}