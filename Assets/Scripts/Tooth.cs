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
}
