using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Camp : MonoBehaviour
{
    public GameObject BG;  // BG
    public GameObject Panel;
    public UnityEngine.UI.Button[] HeroButton;
    void Update()
    {
        if (BG.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            BG.SetActive(false);
            Panel.SetActive(false);
        }
        else if (BG.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

    }
    public void OnClickStart()
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickHeroButton(int n)
    {
        Color savedColor = HeroButton[0].GetComponent<Graphic>().color;
        Color savedColor1 = HeroButton[1].GetComponent<Graphic>().color;
        switch (n)
        {
            case 0:
                HeroButton[0].GetComponent<Graphic>().color = Color.red;
                HeroButton[1].GetComponent<Graphic>().color = savedColor;
                break;
            case 1:
                HeroButton[1].GetComponent<Graphic>().color = Color.red;
                HeroButton[0].GetComponent<Graphic>().color = savedColor1;
                break;

        }

        }

    public void OnClickChange()
    {
        BG.SetActive(true);
        Panel.SetActive(true);
    }


    public void OnClickExit()
    {
        SceneManager.LoadScene(0);
    }



    public void HeroChanger(int hero)
    {
        PlayerPrefs.SetInt("h", hero);
        PlayerPrefs.Save();
        switch (hero)
        {
            case 0:

                break;

            case 1:

                break;

        }


    }
}