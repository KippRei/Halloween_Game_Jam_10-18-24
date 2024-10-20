using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject potion;
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
    public float batXP = 1;
    SpriteRenderer sr;
    private float animTime = 0;
    private Sprite currSprite;

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
        if (!attacking)
        {
            attacking = true;
            active = StartCoroutine(AttackPlayer());
        }
        if (health <= 0)
        {
            gameLoop.currNumOfBats--;
            tooth.xp += batXP;
            int dropPotion = Random.Range(0, 11);
            if (dropPotion <= 2)
            {
                Instantiate(potion, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
            }
            Destroy(gameObject);
        }
        animTime += Time.deltaTime;
        currSprite = sr.sprite;
        if (animTime >= 0.3)
        {
            
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
            enemyHit = true;
            health -= nail.GetAtkPower();
        }
    }

    private IEnumerator AttackPlayer()
    {

        GetComponent<Collider2D>().enabled = true;
        while (Mathf.Abs(transform.position.x - tooth.transform.position.x) >= 0.05 || Mathf.Abs(transform.position.y - tooth.transform.position.y) >= 0.05)
        {
            if (enemyHit)
            {
                yield return new WaitForSeconds(0.2f);
                enemyHit = false;
            }
            MoveTowards(tooth.transform.position, this);
            yield return new WaitForSeconds(speed);
        }
        Vector3 retreatTo = transform.position;
        int randX = (int)Mathf.Floor(Random.value * 100);
        int randY = (int)Mathf.Floor(Random.value * 100);
        retreatTo.x = retreatTo.x + (retreatDist * Mathf.Pow(-1, randX));
        retreatTo.y = retreatTo.y + (retreatDist * Mathf.Pow(-1, randY));
        StartCoroutine(Retreat(retreatTo));
        
    }

    private IEnumerator Retreat(Vector2 retreatTo)
    {

        GetComponent<Collider2D>().enabled = false;
        while ((Mathf.Abs(transform.position.x - retreatTo.x) >= 0.05 || Mathf.Abs(transform.position.y - retreatTo.y) >= 0.05))
        {
            if (enemyHit)
            {
                yield return new WaitForSeconds(0.2f);
                enemyHit = false;
            }
            MoveTowards(retreatTo, this);
            yield return new WaitForSeconds(speed / 2);
        }
        attacking = false;
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
