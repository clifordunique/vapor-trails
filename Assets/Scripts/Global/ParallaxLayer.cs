﻿using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour {
 
    public Vector2 speed;
    public bool moveInOppositeDirection;
 
    private ParallaxOption options;
 
    private void OnEnable()
    {
        options = Camera.main.GetComponentInParent<ParallaxOption>();
    }
 
    void Update ()
    { 
        if (!Application.isPlaying && !options.moveParallax)
        {
            return;
        }
 
        Vector3 distance = Camera.main.transform.position;
        float direction = (moveInOppositeDirection) ? -1f : 1f;
        transform.position = Vector3.Scale(distance, new Vector3(speed.x, speed.y)) * direction;
    }
}