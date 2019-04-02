using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private bool isQuitting;
    public int health = 1;
    public ParticleSystem particles;

    public float speed = 5f;
    public float delta = 1.5f;  // Amount to move left and right from the start point
    private Vector3 startPos;

    public EnemyBullet enemyBulletPrefab;
    public EnemyBomb enemyBombPrefab;
    public float bulletSpawnTime;
    public float bulletSpawnDelay = 2f;
    public float bombSpawnTime;
    public float bombSpawnDelay = 5f;


    // Use this for initialization
    void Start(){
        startPos = transform.position;
       
    }

	// Update is called once per frame
	void Update () {

        Bounds bounds = UI.instance.OrthographicBounds(Camera.main);

        if (this.health <= 0)
        {
            Destroy(this.gameObject);
        }


        //BLUE ENEMY
        if (this.gameObject.tag == "EnemyBlue")
        {
            if (transform.position.y < bounds.max.y + 1)
            {
                Vector3 v = startPos;
                v.x += delta * Mathf.Sin(Time.time * speed);
                transform.position = v;
                transform.rotation = Quaternion.Euler(0f, 0f, 45f * Mathf.Cos(Time.time * speed));

            }

        }

        //GREEN ENEMY
        if (this.gameObject.tag == "EnemyGreen")
        {
            if (transform.position.y < bounds.max.y + 1)
            {
                Vector3 v = startPos;
                v.x += delta * Mathf.Sin(Time.time * speed * 1.5f);
                transform.position = v;
                transform.rotation = Quaternion.Euler(0f, 0f, 45f * Mathf.Cos(Time.time * speed * 1.5f));

                if (Time.time > bulletSpawnTime && !UI.instance.restarting)
                {
                    GameObject.Instantiate<EnemyBullet>(enemyBulletPrefab, transform.position, Quaternion.identity);
                    bulletSpawnTime = Time.time + bulletSpawnDelay;
                }
            }
        }

        //RED ENEMY
        if (this.gameObject.tag == "EnemyRed")
        {
            if (transform.position.y < bounds.max.y + 1)
            {
                Vector3 v = startPos;
                v.x += delta * 3 * Mathf.Sin(Time.time * speed);
                transform.position = v;
                transform.rotation = Quaternion.Euler(0f, 0f, 45f * Mathf.Cos(Time.time * speed));

                if (Time.time > bulletSpawnTime && !UI.instance.restarting)
                {
                    GameObject.Instantiate<EnemyBomb>(enemyBombPrefab, transform.position, Quaternion.identity);
                    bulletSpawnTime = Time.time + bulletSpawnDelay;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            Destroy(collision.gameObject);
            this.health -= 1;
        }

        if (collision.gameObject.name == "LaserBeam")
        {
            this.health -= 5;
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (!isQuitting)
        {
            Instantiate(particles, transform.position, transform.rotation);
            UI.instance.AddScore(10);
        }
        
    }


}
