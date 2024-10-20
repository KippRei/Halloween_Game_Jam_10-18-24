using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    private float counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.unscaledDeltaTime;
        if (Input.GetKeyDown(KeyCode.Escape) && counter > 1)
        {
            SceneManager.LoadScene("Title", LoadSceneMode.Single);
        }
    }
}
