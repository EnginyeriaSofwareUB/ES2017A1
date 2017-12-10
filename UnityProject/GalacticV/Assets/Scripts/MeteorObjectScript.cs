using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorObjectScript : MonoBehaviour {

    public Vector3 targetPosition;
    public bool finished = false;
    public bool started = false;
    public float speed = 6f;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (!started || finished) return;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        if (transform.position == targetPosition)
            finished = true;

    }
}
