using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool left = false, right = false, up = false, down = false;
    public int damage = 1;
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (left)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - (speed * Time.deltaTime), gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (right)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + (speed * Time.deltaTime), gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (up)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (speed * Time.deltaTime), gameObject.transform.position.z);
        }
        if (down)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (speed * Time.deltaTime), gameObject.transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Walls" || collision.tag == "Tooth")
        Destroy(gameObject);
    }

    public int GetAtkPower()
    {
        return damage;
    }
}
