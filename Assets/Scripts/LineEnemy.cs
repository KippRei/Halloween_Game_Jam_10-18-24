using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class LineEnemy : MonoBehaviour
{
    public Projectile upProjectile, downProjectile, leftProjectile, rightProjectile;
    [SerializeField]
    int health = 6, atkPower = 1;
    [SerializeField]
    float speed;
    [SerializeField]
    Nail nail;
    [SerializeField]
    Tooth tooth;
    Coroutine active;
    private bool attacking = false;
    [SerializeField]
    float retreatDist = 10f;
    [SerializeField]
    float animateSpeed = 0.2f;
    [SerializeField]
    Sprite one, two;
    bool enemyHit = false;
    [SerializeField]
    GameLoop gameLoop;
    [SerializeField]
    GameObject despawnPoint;
    public float lineXP = 6;
    SpriteRenderer sr;
    private float animTime = 0;
    private float timeElapsed = 0;
    public float attackFreq = 2f;
    private float hitTime = 0;
    private bool hit = false;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tooth = FindAnyObjectByType<Tooth>();
        nail = FindAnyObjectByType<Nail>();
        gameLoop = FindAnyObjectByType<GameLoop>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > attackFreq)
        {
            Debug.Log(gameObject.name);
            timeElapsed = 0;
            if (gameObject.name == "LineEnemy(Clone)")
            {
                Instantiate(upProjectile, transform.position, Quaternion.identity);
                Instantiate(downProjectile, transform.position, Quaternion.identity);
            }
            else if (gameObject.name == "LineEnemyVert(Clone)")
            {
                Instantiate(leftProjectile, transform.position, Quaternion.identity);
                Instantiate(rightProjectile, transform.position, Quaternion.identity);
            }

        }

        if (hitTime != 0)
        {
            hitTime -= Time.deltaTime;
        }
        if (hitTime <= 0)
        {
            hit = false;
        }

        if (transform.position.x < despawnPoint.transform.position.x && !hit)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + speed * Time.deltaTime, gameObject.transform.position.y , gameObject.transform.position.z);
        }
        if (transform.position.y > despawnPoint.transform.position.y && !hit)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x , gameObject.transform.position.y - speed * Time.deltaTime, gameObject.transform.position.z);
        }
        if (health <= 0)
        {
            gameLoop.currNumOfBats--;
            tooth.xp += lineXP;
            Destroy(gameObject);
        }

        animTime += Time.deltaTime;
        if (animTime >= 0.15)
        {
            Sprite currSprite = sr.sprite;
            animTime = 0;
            if (currSprite == one)
            {
                sr.sprite = two;
            }
            else
            {
                sr.sprite = one;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Nail")
        {
            hit = true;
            hitTime = 0.3f;
            enemyHit = true;
            health -= nail.GetAtkPower();
        }
    }

    private IEnumerator AttackPlayer()
    {

        GetComponent<Collider2D>().enabled = true;
        while (transform.position.x < despawnPoint.transform.position.x)
        {
            if (enemyHit)
            {
                yield return new WaitForSeconds(0.2f);
                enemyHit = false;
            }
            MoveTowards(new Vector2(despawnPoint.transform.position.x, despawnPoint.transform.position.y), this);
            yield return new WaitForSeconds(speed);
        }
        Destroy(gameObject);
    }


    void MoveTowards(Vector2 moveTo, MonoBehaviour objToMove)
    {
        float playerX = moveTo.x;
        float playerY = moveTo.y;
        float nailX = objToMove.transform.position.x;
        float nailY = objToMove.transform.position.y;

        float angle = Mathf.Atan2((playerX - nailX), (playerY - nailY));
        float xMov = 0.05f * Mathf.Sin(angle);
        float yMov = 0.05f * Mathf.Cos(angle);
        objToMove.transform.position = new Vector3(objToMove.transform.position.x + xMov, objToMove.transform.position.y + yMov, objToMove.transform.position.z);
    }

    public int GetAtkPower()
    {
        return atkPower;
    }
}
