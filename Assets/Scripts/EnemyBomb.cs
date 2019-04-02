using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour {

    public float speed = 2f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
