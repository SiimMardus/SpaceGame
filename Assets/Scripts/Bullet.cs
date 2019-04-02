using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 20f;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
    

}
