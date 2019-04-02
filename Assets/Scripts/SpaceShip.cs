using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceShip : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    public Bullet bulletPrefab;
    public GameObject shield;
    public GameObject laserBeam;
    public ParticleSystem particles;

    public float bulletSpawnDelay = 1f;
    private float bulletSpawnTime;
    private bool bulletWaveActive = false;
    private int bulletWaveCount;
    private float bulletWaveBreak = 0.5f;
    private float lastWaveTime;

    private bool isInvincible = false;
    public float invincibleTime = 3f;
    private float invincibleTimeStart;


	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Bounds bounds = UI.instance.OrthographicBounds(Camera.main);

        mousePos = new Vector3(
            Mathf.Clamp(mousePos.x, bounds.min.x, bounds.max.x),
            Mathf.Clamp(mousePos.y, bounds.min.y, bounds.max.y),
            0
            );
        transform.position = mousePos;



        //SHIELD
        if (Input.GetMouseButton(1))
        {
            if (UI.instance.energyBarFill.fillAmount > 0)
            {
                shield.SetActive(true);
                UI.instance.energyBarFill.fillAmount -= 0.5f * Time.deltaTime;
            }
            if (UI.instance.energyBarFill.fillAmount == 0)
            {
                shield.SetActive(false);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            shield.SetActive(false);
        }

        //LASERBEAM
        if (Input.GetMouseButton(0))
        {
            if (UI.instance.energyBarFill.fillAmount > 0)
            {
                laserBeam.transform.SetPositionAndRotation(new Vector3(transform.position.x, (bounds.max.y + this.transform.position.y) / 2, transform.position.z), Quaternion.identity);
                laserBeam.transform.localScale = new Vector3(4.5f, (bounds.max.y - this.transform.position.y) * 2.5f, 0);
                laserBeam.SetActive(true);

                UI.instance.energyBarFill.fillAmount -= 0.5f * Time.deltaTime;
            }
            if (UI.instance.energyBarFill.fillAmount == 0)
            {
                laserBeam.SetActive(false);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            laserBeam.SetActive(false);
        }

        

        if (!shield.activeSelf && !laserBeam.activeSelf && !UI.instance.restarting)
        {
            if (UI.instance.energyBarFill.fillAmount < 1)
            {
                UI.instance.energyBarFill.fillAmount += 0.03f * Time.deltaTime;
            }
        }

        //BULLET SPAWNING
        if (Time.time > bulletSpawnTime && !UI.instance.restarting && !laserBeam.activeSelf && !bulletWaveActive) {

            GameObject.Instantiate<Bullet>(bulletPrefab, transform.position, Quaternion.identity);
            bulletSpawnTime = Time.time + bulletSpawnDelay;
        }

        // RESTARTING
        if (UI.instance.lives <= 0)
        {   
            Instantiate(particles, transform.position, transform.rotation);
            this.gameObject.SetActive(false);
        }

        // INVINCIBLE
        if (invincibleTimeStart + invincibleTime < Time.time)
        {
            isInvincible = false;
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }

        //BULLETWAVE
        if (bulletWaveActive &&  bulletWaveCount > 0 && lastWaveTime + bulletWaveBreak < Time.time)
        {
            for (int i = -6; i <= 6; i++)
            {
                var bullet = GameObject.Instantiate<Bullet>(bulletPrefab, transform.position, Quaternion.identity);
                bullet.transform.SetPositionAndRotation(new Vector3(i, transform.position.y, transform.position.z), Quaternion.identity);
            }
            if (bulletWaveCount == 1)
            {
                bulletWaveActive = false;
            }
            lastWaveTime = Time.time;
            bulletWaveCount--;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Enemy") && !isInvincible && !UI.instance.restarting)
        {
            if (!shield.activeSelf)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, .5f);
                isInvincible = true;
                invincibleTimeStart = Time.time;
                UI.instance.LoseLife(1);
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.name == "EnemyBullet(Clone)" && !isInvincible && !UI.instance.restarting)
        {
            if (!shield.activeSelf)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, .5f);
                isInvincible = true;
                invincibleTimeStart = Time.time;
                UI.instance.LoseLife(1);
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.name == "EnemyBomb(Clone)" && !isInvincible && !UI.instance.restarting)
        {
            if (!shield.activeSelf)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, .5f);
                isInvincible = true;
                invincibleTimeStart = Time.time;
                UI.instance.LoseLife(3);
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.name.Contains("PowUpFastBullets") && !UI.instance.restarting)
        {
            Destroy(collision.gameObject);
            bulletSpawnDelay = bulletSpawnDelay / 2;
        }

        if (collision.gameObject.name.Contains("PowUpEnergyFull") && !UI.instance.restarting)
        {
            Destroy(collision.gameObject);
            UI.instance.energyBarFill.fillAmount = 1;
        }

        if (collision.gameObject.name.Contains("PowUpBulletWave") && !UI.instance.restarting)
        {
            Destroy(collision.gameObject);
            bulletWaveCount = 5;
            lastWaveTime = 0;
            bulletWaveActive = true;
        }
    }



}
