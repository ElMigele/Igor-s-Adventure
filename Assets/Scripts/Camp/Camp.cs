using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Camp : MonoBehaviour {
    public GameObject BG;  // BG
    public GameObject Panel;
    void Update()
    {
        if( BG.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
            {
            BG.SetActive(false);
            Panel.SetActive(false);
            }
    }
    public void OnClickStart()
    {
        SceneManager.LoadScene(2);
    }
    public void OnClickChange()
    {
        BG.SetActive(true);
        Panel.SetActive(true);
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
    public void OnClickExit()
    {
        Application.Quit();
    }
}
