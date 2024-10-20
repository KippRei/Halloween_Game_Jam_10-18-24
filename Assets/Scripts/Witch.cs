using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Witch : MonoBehaviour
{
    public GameObject tl, tr, tm, mm, bl, br, bm;
    public Projectile up, left, down, right, upL, upR, downL, downR;
    public float atkFreq = 0.3f;
    public float howLongToWarp = 3.5f;
    private float timeSinceLastAtk = 0;
    private float timeSinceLastWarp = 0;
    private bool lastAtkCross = false;
    private List<GameObject> locs = new List<GameObject>();
    private int health = 35;
    SpriteRenderer sr;
    public Sprite one, two;
    private bool hit = false;
    private float counter = 0;
    private float hitTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        locs.Add(tl);
        locs.Add(tr);
        locs.Add(tm);
        locs.Add(mm);
        locs.Add(bl);
        locs.Add(br);
        locs.Add(bm);

    }

    // Update is called once per frame
    void Update()
    {
        if (health > 17)
        {
            sr.sprite = one;
        }
        if (health <= 17)
        {
            sr.sprite = two;
        }
        if (hit)
        {
            hitTimer += Time.deltaTime;
        }
        if (hitTimer > 0.3)
        {
            hit = false;
        }
        timeSinceLastAtk += Time.deltaTime;
        timeSinceLastWarp += Time.deltaTime;

        if (timeSinceLastAtk > atkFreq && !hit)
        {
            timeSinceLastAtk = 0;
            if (!lastAtkCross)
            {
                lastAtkCross = true;
                Instantiate(up, transform.position, Quaternion.identity);
                Instantiate(down, transform.position, Quaternion.identity);
                Instantiate(right, transform.position, Quaternion.identity);
                Instantiate(left, transform.position, Quaternion.identity);
            }
            else
            {
                lastAtkCross = false;
                Instantiate(upL, transform.position, Quaternion.identity);
                Instantiate(upR, transform.position, Quaternion.identity);
                Instantiate(downL, transform.position, Quaternion.identity);
                Instantiate(downR, transform.position, Quaternion.identity);
            }
        }
        if (timeSinceLastWarp > howLongToWarp)
        {
            timeSinceLastWarp = 0;
            int rand = Random.Range(0, 7);
            transform.position = locs[rand].transform.position;
        }
        if (health < 0)
        {
            Time.timeScale = 0.3f;
            counter += Time.unscaledDeltaTime;
            if (counter > 2.5)
            {
                SceneManager.LoadScene("Ending", LoadSceneMode.Single);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Nail")
        {
            hit = true;
            health -= 1;
        }
    }
}
