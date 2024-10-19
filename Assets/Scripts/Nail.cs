using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Nail : MonoBehaviour
{
    [SerializeField]
    Tooth tooth;
    [SerializeField]
    float distToTooth, nailSpeed, warpSpeed = 0.001f, maxDistFromTooth = 3;
    [SerializeField]
    Camera cam;

    private float x, y, currDistFromTooth = 0;
    private bool toothAtMaxDist = false, fired = false, warping = false;
    private Coroutine firedCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        x = distToTooth * Mathf.Sin((tooth.transform.rotation.eulerAngles.z % 360) * (Mathf.PI / 180));
        y = distToTooth * Mathf.Cos((tooth.transform.rotation.eulerAngles.z % 360) * (Mathf.PI / 180));
        transform.position = new Vector2 (tooth.transform.position.x + x, tooth.transform.position.y + y);
    }

    // Update is called once per frame
    void Update()
    {
/*        float currDistFromTooth = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x - tooth.transform.position.x), 2) + Mathf.Pow(Mathf.Abs(transform.position.y - tooth.transform.position.y), 2));
        if (currDistFromTooth >= maxDistFromTooth && fired)
        {
            StopCoroutine(firedCoroutine);
            StartCoroutine(Returning());
        }*/

        if (Input.GetMouseButton(0) && !fired)
        {
            Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);

            float angle = Mathf.Atan2((point.x - transform.position.x), (point.y - transform.position.y));

                
            Debug.Log(angle);
            float xMov = 0.1f * Mathf.Sin(angle);
            float yMov = 0.1f * Mathf.Cos(angle);

            firedCoroutine = StartCoroutine(Fire(xMov, yMov));
        }
        if (!fired && !warping)
        {
            transform.position = tooth.transform.position;
        }
        if (Input.GetMouseButton(1))
        {
            // TODO: slowmo
            // TODO: right now there is no collision on the nail, this means there will be issues when warping to areas that have collision with tooth (PC)
        }
        if (Input.GetMouseButtonUp(1))
        {
            Vector2 warpTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(WarpTo(warpTo));
        }
    }

    private IEnumerator WarpTo(Vector2 warpTo)
    {
        warping = true;
        while (Mathf.Abs(transform.position.x - warpTo.x) >= 0.05 || Mathf.Abs(transform.position.y - warpTo.y) >= 0.05)
        {
            MoveTowards(warpTo, this);
            yield return new WaitForSeconds(warpSpeed);
        }
        while (Mathf.Abs(tooth.transform.position.x - warpTo.x) >= 0.05 || Mathf.Abs(tooth.transform.position.y - warpTo.y) >= 0.05)
        {
            MoveTowards(warpTo, tooth);
            yield return new WaitForSeconds(warpSpeed);
        }
        Debug.Log("asdfasdfasdf");
        warping = false;
    }

    private IEnumerator Returning()
    {
        MoveTowardsPlayer();
        yield return new WaitForSeconds(nailSpeed);
    }

    private IEnumerator Fire(float xMov, float yMov)
    {
        fired = true;
        while (Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x - tooth.transform.position.x), 2) + Mathf.Pow(Mathf.Abs(transform.position.y - tooth.transform.position.y), 2)) <= maxDistFromTooth)
        {
            transform.position = new Vector2(transform.position.x + xMov, transform.position.y + yMov);
            yield return new WaitForSeconds(nailSpeed);
        }
        while (Mathf.Abs(transform.position.x - tooth.transform.position.x) >= 0.05 || Mathf.Abs(transform.position.y - tooth.transform.position.y) >= 0.05)
        {
            MoveTowardsPlayer();
            yield return new WaitForSeconds(nailSpeed);
        }
        fired = false;
    } 

    private void MoveTowardsPlayer()
    {
        float playerX = tooth.transform.position.x;
        float playerY = tooth.transform.position.y;
        float nailX = transform.position.x;
        float nailY = transform.position.y;

        float angle = Mathf.Atan2((playerX - nailX), (playerY - nailY));
        float xMov = 0.1f * Mathf.Sin(angle);
        float yMov = 0.1f * Mathf.Cos(angle);
        transform.position = new Vector2(transform.position.x + xMov, transform.position.y + yMov);
    }

    private void MoveTowards(Vector2 moveTo, MonoBehaviour objToMove)
    {
        float playerX = moveTo.x;
        float playerY = moveTo.y;
        float nailX = objToMove.transform.position.x;
        float nailY = objToMove.transform.position.y;

        float angle = Mathf.Atan2((playerX - nailX), (playerY - nailY));
        float xMov = 0.1f * Mathf.Sin(angle);
        float yMov = 0.1f * Mathf.Cos(angle);
        objToMove.transform.position = new Vector2(objToMove.transform.position.x + xMov, objToMove.transform.position.y + yMov);
    }
}
