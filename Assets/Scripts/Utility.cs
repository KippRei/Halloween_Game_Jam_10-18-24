using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveTowards(Vector2 moveTo, MonoBehaviour objToMove)
    {
        float playerX = moveTo.x;
        float playerY = moveTo.y;
        float nailX = objToMove.transform.position.x;
        float nailY = objToMove.transform.position.y;

        float angle = Mathf.Atan2((playerX - nailX), (playerY - nailY));
        float xMov = 0.1f * Mathf.Sin(angle);
        float yMov = 0.1f * Mathf.Cos(angle);
        objToMove.transform.position = new Vector3(objToMove.transform.position.x + xMov, objToMove.transform.position.y + yMov, objToMove.transform.position.z);
    }
}
