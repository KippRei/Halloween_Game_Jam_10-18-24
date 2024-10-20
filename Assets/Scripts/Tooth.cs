using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooth : MonoBehaviour
{
    public GameLoop gl;
    [SerializeField]
    float move_speed = 1.5f;
    [SerializeField]
    float player_speed = 0f;
    [SerializeField]
    Nail nail;

    private HashSet<KeyCode> keys_down = new HashSet<KeyCode>();
    [SerializeField]
    private int number_of_keys_down = 0;
    private Vector2 mouseLoc;
    private Vector3 lookLeft = new Vector3(0, 180, 0);
    private Vector3 lookRight = new Vector3(0, 0, 0);
    private Coroutine animateCoroutine;
    private bool animateMovingUp;
    [SerializeField]
    private float animateSpeed = 0.3f; // idol animation speed
    private bool idling = false;
    public int health = 10;
    public int maxHealth = 10;
    public float xp = 0;
    public float nextLevelXP;
    public int currLevel = 1;
    private float startPos; // for animation
    private float animTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        nextLevelXP = 8 * currLevel * 0.9f;
        keys_down.Add(KeyCode.A);
        keys_down.Add(KeyCode.S);
        keys_down.Add(KeyCode.D);
        keys_down.Add(KeyCode.W);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gl.isPaused) { 
        if (xp >= nextLevelXP)
        {
            currLevel++;
            xp = 0;
            nextLevelXP = 8 * currLevel * 0.9f;
        }
        mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouseLoc.x > transform.position.x)
        {
            transform.eulerAngles = lookRight;
        }
        else
        {
            transform.eulerAngles = lookLeft;
        }

        foreach (var key in keys_down)
        {
            if (Input.GetKeyDown(key))
            {
                number_of_keys_down++;
            }
            if (Input.GetKeyUp(key))
            {
                number_of_keys_down--;
            }
        }
        if (idling)
        {
            animTime += Time.deltaTime;
            if (startPos == Mathf.Infinity)
            {
                startPos = transform.position.y;
            }
            if (transform.position.y >= startPos + 0.1f)
            {
                animateMovingUp = false;
            }
            if (transform.position.y <= startPos - 0.1f)
            {
                animateMovingUp = true;
            }

            if (animateMovingUp && animTime > 0.05f)
            {
                animTime = 0;
                transform.position = new Vector3(transform.position.x, transform.position.y + .025f, transform.position.z);
            }
            if (!animateMovingUp && animTime > 0.05f)
            {
                animTime = 0;
                transform.position = new Vector3(transform.position.x, transform.position.y - .025f, transform.position.z);
            }
        }

        if (number_of_keys_down == 0 && !idling && !nail.GetWarping())
        {
            idling = true;
        }
        if (number_of_keys_down > 0 || nail.GetWarping())
        {
            startPos = Mathf.Infinity;
            idling = false;
        }

            if (true)
            {
                if (number_of_keys_down > 1)
                {
                    player_speed = Mathf.Sin(Mathf.Sqrt(2) / 2) * move_speed * 1.2f * Time.deltaTime;
                }
                else
                {
                    player_speed = move_speed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + player_speed, gameObject.transform.position.z);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + player_speed, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - player_speed, gameObject.transform.position.z);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - player_speed, gameObject.transform.position.y, gameObject.transform.position.z);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            health -= collision.GetComponent<Enemy>().GetAtkPower();
        }
        if (collision.tag == "LineEnemy")
        {
            health -= collision.GetComponent<LineEnemy>().GetAtkPower();
        }
        if (collision.tag == "Projectile")
        {
            health -= collision .GetComponent<Projectile>().GetAtkPower();
        }
        if (collision.tag == "Witch")
        {
            health -= 2;
        }
        if (collision.tag == "Potion")
        {
            if (health < maxHealth)
            {
                health += 1;
            }
            Destroy(collision.gameObject);
        }
    }
}
