using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleOptions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TMP_Text btn;
    bool mouseOver = false;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseOver)
        {
            btn.color = Color.red;
        }
        else
        {
            btn.color = Color.white;
        }
        if (mouseOver && Input.GetMouseButtonDown(0))
        {
            if (btn.name == "startgame")
            {
                SceneManager.LoadScene("Level1", LoadSceneMode.Single);
            }
            else if (btn.name == "quitgame")
            {
                Application.Quit();
            }
            else if (btn.name == "QuitText")
            {
                SceneManager.LoadScene("Title", LoadSceneMode.Single);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver= false;
    }
}
