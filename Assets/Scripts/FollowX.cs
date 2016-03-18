using UnityEngine;
using System.Collections;

public class FollowX : MonoBehaviour {

    public Transform target;
    public float offset;
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(target.position.x + offset, transform.position.y, transform.position.z);
	}
}
