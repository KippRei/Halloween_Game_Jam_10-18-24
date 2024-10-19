using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooth : MonoBehaviour
{
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


    // Start is called before the first frame update
    void Start()
    {
        keys_down.Add(KeyCode.A);
        keys_down.Add(KeyCode.S);
        keys_down.Add(KeyCode.D);
        keys_down.Add(KeyCode.W);
    }

    // Update is called once per frame
    void Update()
    {
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

        if (number_of_keys_down == 0 && !idling && !nail.GetWarping())
        {
            idling = true;
            animateCoroutine = StartCoroutine(Animate(transform.position.y));
        }
        if (number_of_keys_down > 0 || nail.GetWarping())
        {
            idling = false;
            StopCoroutine(animateCoroutine);
        }

        if (true)
        {
            if (number_of_keys_down > 1)
            {
                player_speed = Mathf.Sin(Mathf.Sqrt(2) / 2) * move_speed * 1.1f * Time.deltaTime;
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

    private IEnumerator Animate(float startPos)
    {
        while (idling)
        {
            if (transform.position.y >= startPos + 0.1f)
            {
                animateMovingUp = false;
            }
            if (transform.position.y <= startPos - 0.1f)
            {
                animateMovingUp = true;
            }

            if (animateMovingUp)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + .025f, transform.position.z);
            }
            if (!animateMovingUp)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - .025f, transform.position.z);
            }

            yield return new WaitForSeconds(animateSpeed);
        }
    }
}
