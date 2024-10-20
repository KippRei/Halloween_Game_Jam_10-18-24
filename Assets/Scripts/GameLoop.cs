using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    public GameObject witchPrefab;

    [SerializeField]
    GameObject batSpawnPoints;
    public GameObject batPrefab;
    public int currNumOfLine = 0;
    private float timeSinceLastLine = 0f;
    public float lineSpawnFreq = 10;
    [SerializeField]
    GameObject lineSpawnPoint, lineVertSpawnPoint;
    public GameObject linePrefab, lineVertPrefab;
    [SerializeField]
    Tooth tooth;
    [SerializeField]
    Nail nail;
    [SerializeField]
    int numOfBats = 3;
    public int currNumOfBats = 0;
    List<Transform> batSpawnArr = new List<Transform>();

    [SerializeField]
    Image healthFill, xpFill, jumpFill;
    [SerializeField]
    TMP_Text levelNumText;
    [SerializeField]
    GameObject pauseScreen;
    private float fillZeroPos = -50f;
    public bool isPaused = false;
    private bool witchSpawned = false;
    public GameObject gameOverScreen;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
        healthFill.transform.localPosition = new UnityEngine.Vector3(fillZeroPos, healthFill.transform.localPosition.y, healthFill.transform.localPosition.z);
        xpFill.transform.localPosition = new UnityEngine.Vector3(fillZeroPos, xpFill.transform.localPosition.y, xpFill.transform.localPosition.z);
        jumpFill.transform.localPosition = new UnityEngine.Vector3(fillZeroPos, jumpFill.transform.localPosition.y, jumpFill.transform.localPosition.z);

        foreach (var batSpawnPt in batSpawnPoints.GetComponentsInChildren<Transform>())
        {
            batSpawnArr.Add(batSpawnPt);
        }
        for (int i = 0; i < numOfBats; i++)
        {
            int randSpawnPt = (int)Mathf.Floor(Random.value * 5f);
            Instantiate(batPrefab, batSpawnArr[randSpawnPt].position, UnityEngine.Quaternion.identity);
            currNumOfBats++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tooth.health <= 0)
        {
             SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
        UpdateHealthBar();
        UpdateJumpBar();
        UpdateXPBar();
        if (currNumOfBats < numOfBats + tooth.currLevel)
        {
            int randSpawnPt = (int)Mathf.Floor(Random.value * 5f);
            Instantiate(batPrefab, batSpawnArr[randSpawnPt].position, UnityEngine.Quaternion.identity);
            currNumOfBats++;
        }
        timeSinceLastLine += Time.deltaTime;
        if (tooth.currLevel > 1 && timeSinceLastLine > lineSpawnFreq)
        {
            int rand = Random.Range(0, 2);
            timeSinceLastLine = 0;
            if (rand == 0)
            {
                Instantiate(linePrefab, lineSpawnPoint.transform.position, UnityEngine.Quaternion.identity);
            }
            else
            {
                Instantiate(lineVertPrefab, lineVertSpawnPoint.transform.position, UnityEngine.Quaternion.identity);
            }
        }
        if (tooth.currLevel > 5 && !witchSpawned)
        {
            witchSpawned = true;
            Instantiate(witchPrefab, new UnityEngine.Vector3(0,0,0), UnityEngine.Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            isPaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            isPaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }

        }

        void UpdateHealthBar()
    {
        float healthLevel = (float)tooth.health / (float)tooth.maxHealth;
        if (healthLevel <= 0)
        {
            healthFill.transform.localScale = new UnityEngine.Vector3(0, healthFill.transform.localScale.y, healthFill.transform.localScale.z);
        }
        else
        {
            float healthBarPos = healthLevel * (5 / .1f) + fillZeroPos;
            healthFill.transform.localPosition = new UnityEngine.Vector3(healthBarPos, healthFill.transform.localPosition.y, healthFill.transform.localPosition.z);
            healthFill.transform.localScale = new UnityEngine.Vector3(healthLevel, healthFill.transform.localScale.y, healthFill.transform.localScale.z);
        }
    }

    void UpdateJumpBar()
    {
        float jumpLevel = (float)nail.jumpEnergy;
        float jumpBarPos = jumpLevel * (5 / .1f) + fillZeroPos;
        jumpFill.transform.localPosition = new UnityEngine.Vector3(jumpBarPos, jumpFill.transform.localPosition.y, jumpFill.transform.localPosition.z);
        jumpFill.transform.localScale = new UnityEngine.Vector3(jumpLevel, jumpFill.transform.localScale.y, jumpFill.transform.localScale.z);
    }

    void UpdateXPBar()
    {
        levelNumText.text = tooth.currLevel.ToString();
        float xpLevel = (float)tooth.xp / tooth.nextLevelXP;
        float xpBarPos = xpLevel * (5 / .1f) + fillZeroPos;
        xpFill.transform.localPosition = new UnityEngine.Vector3(xpBarPos, healthFill.transform.localPosition.y, healthFill.transform.localPosition.z);
        xpFill.transform.localScale = new UnityEngine.Vector3(xpLevel, healthFill.transform.localScale.y, healthFill.transform.localScale.z);
    }
}
