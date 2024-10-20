using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Nail : MonoBehaviour
{
    public GameLoop gl;
    [SerializeField]
    Tooth tooth;
    [SerializeField]
    float distToTooth, startingNailSpeed = .005f, warpSpeed = 0.001f, maxDistFromTooth, startingMaxDist = 2, nailRPM = 5;
    private float nailSpeed;
    [SerializeField]
    int atkPower = 1;
    [SerializeField]
    Camera cam;

    float xMov, yMov;
    private float x, y, currDistFromTooth = 0;
    private bool toothAtMaxDist = false, fired = false, warping = false, leftClickDown = false, rightClickDown = false, traversableTerrain = false;
    private Coroutine firedCoroutine;
    public float jumpEnergy = 1f;
    public float jumpRechargeSpeed = 0.1f;
    private bool timeToReturn = false;
    private Vector2 warpTo;
    private bool warpNail = false;
    // Start is called before the first frame update
    void Start()
    {
        toothAtMaxDist = false;
        fired = false; warping = false; leftClickDown = false; rightClickDown = false; traversableTerrain = false;
        warpNail = false;
        timeToReturn = false;
        x = distToTooth * Mathf.Sin((tooth.transform.rotation.eulerAngles.z % 360) * (Mathf.PI / 180));
        y = distToTooth * Mathf.Cos((tooth.transform.rotation.eulerAngles.z % 360) * (Mathf.PI / 180));
        transform.position = new Vector3(tooth.transform.position.x + x, tooth.transform.position.y + y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gl.isPaused) {
        if (tooth.currLevel < 10)
        {
            nailSpeed = startingNailSpeed + (tooth.currLevel * 4f);
            maxDistFromTooth = startingMaxDist + tooth.currLevel;
        }
        else if (tooth.currLevel >= 10)
        {
            nailSpeed = startingNailSpeed + (9 * 4f);
            maxDistFromTooth = startingMaxDist + 9;
        }

        if (jumpEnergy < 1)
        {
            jumpEnergy += jumpRechargeSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && !fired && !rightClickDown)
        {
            leftClickDown = true;
            Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);

            float angle = Mathf.Atan2((point.x - transform.position.x), (point.y - transform.position.y));

            xMov = nailSpeed * Mathf.Sin(angle);
            yMov = nailSpeed * Mathf.Cos(angle);

            fired = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            leftClickDown = false;
        }

        // Turn on attack trigger only when nail is fired
        if (fired)
        {
            Fire(xMov, yMov);
            GetComponent<Collider2D>().enabled = true;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + nailRPM);
        }

        // Turn off attack trigger when not firing
        if (!fired && !warping)
        {
            GetComponent<Collider2D>().enabled = false;
            transform.position = new Vector3(tooth.transform.position.x, tooth.transform.position.y, transform.position.z);
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        if (Input.GetMouseButtonDown(1))
        {
            rightClickDown = true;
        }

        // When right click is released, check that mouse position is a valid warp location then warp if possible
        if (Input.GetMouseButtonUp(1) && transform.position.x == tooth.transform.position.x && transform.position.y == tooth.transform.position.y && !warping)
        {

            traversableTerrain = false;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.rigidbody.CompareTag("Traversable") && jumpEnergy > 0.95)
                {
                    jumpEnergy = 0;
                    traversableTerrain = true;
                }
            }

            if (traversableTerrain)
            {
                warpTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                warping = true;
            }
            else
            {
                rightClickDown = false;
            }
        }

        if (warping)
        {
            WarpTo(warpTo);
        }
    }
    }

    // Returns nail attack power
    public int GetAtkPower()
    {
        return atkPower;
    }

    // Right click warps player to mouse location (if valid)
    private void WarpTo(Vector2 warpTo)
    {
        tooth.GetComponent<Collider2D>().enabled = false;
        warping = true;
        if (Mathf.Abs(transform.position.x - warpTo.x) <= 0.5 && Mathf.Abs(transform.position.y - warpTo.y) <= 0.5)
        {
            transform.position = warpTo;
            warpNail = true;
        }
        if (!warpNail)
        {
            MoveTowards(warpTo, this, warpSpeed);
        }
        else if ((Mathf.Abs(tooth.transform.position.x - warpTo.x) >= 0.5 || Mathf.Abs(tooth.transform.position.y - warpTo.y) >= 0.5))
        {
            MoveTowards(warpTo, tooth, warpSpeed);
        }
        else
        {
            transform.position = tooth.transform.position;
            tooth.GetComponent<Collider2D>().enabled = true;
            rightClickDown = false;
            warping = false;
            warpNail = false;
        }
     }


    // Left click fire nail attack
    private void Fire(float xMov, float yMov)
    {
        if (Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x - tooth.transform.position.x), 2) + Mathf.Pow(Mathf.Abs(transform.position.y - tooth.transform.position.y), 2)) >= maxDistFromTooth && !timeToReturn)
        {
            timeToReturn = true;
        }
        if (!timeToReturn)
        {
            transform.position = new Vector3(transform.position.x + xMov * Time.deltaTime, transform.position.y + yMov * Time.deltaTime, transform.position.z);
        }
        else if (Mathf.Abs(transform.position.x - tooth.transform.position.x) >= 0.5 || Mathf.Abs(transform.position.y - tooth.transform.position.y) >= 0.5)
        {
            MoveTowards(new Vector2(tooth.transform.position.x, tooth.transform.position.y), this, nailSpeed);
        }
        else
        {
            timeToReturn = false;
            transform.position = tooth.transform.position;
            fired = false;
        }
    } 

    // Helper function to move nail toward player when Returning
    private void MoveTowardsPlayer()
    {
        float playerX = tooth.transform.position.x;
        float playerY = tooth.transform.position.y;
        float nailX = transform.position.x;
        float nailY = transform.position.y;

        float angle = Mathf.Atan2((playerX - nailX), (playerY - nailY));
        float xMov = nailSpeed * Time.deltaTime * Mathf.Sin(angle);
        float yMov = nailSpeed * Time.deltaTime * Mathf.Cos(angle);
        transform.position = new Vector3(transform.position.x + xMov, transform.position.y + yMov, transform.position.z);
    }

    // Utility function to move one object towards moveTo coords
    private void MoveTowards(Vector2 moveTo, MonoBehaviour objToMove, float speed)
    {
        float playerX = moveTo.x;
        float playerY = moveTo.y;
        float nailX = objToMove.transform.position.x;
        float nailY = objToMove.transform.position.y;

        float angle = Mathf.Atan2((playerX - nailX), (playerY - nailY));
        float xMov = speed * Time.deltaTime * Mathf.Sin(angle);
        float yMov = speed * Time.deltaTime * Mathf.Cos(angle);
        objToMove.transform.position = new Vector3(objToMove.transform.position.x + xMov, objToMove.transform.position.y + yMov, objToMove.transform.position.z);
    }

    // Returns true if player is currently warping
    public bool GetWarping()
    {
        return warping;
    }
}
