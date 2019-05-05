using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CodeMonkey.Utils;
public class Camp : MonoBehaviour
{
    public GameObject BGTavern;  // BG tavern
    public GameObject BGMarketPlace;  // BG market
    public GameObject TavernPanel;
    public GameObject MarketPlacePanel;
    public UnityEngine.UI.Button[] HeroButton;
    public Text coinText;                          // Количество золота, имеющееся у игрока

    void Start()
    {
        if(!PlayerPrefs.HasKey("coin"))
        {
            PlayerPrefs.SetInt("coin", 0);
            PlayerPrefs.Save();
        }
    }
        void Update()
    {
        if ((BGTavern.activeSelf == true && Input.GetKeyDown(KeyCode.Escape)) || (BGMarketPlace.activeSelf == true && Input.GetKeyDown(KeyCode.Escape)))
        {
            BGTavern.SetActive(false);
            TavernPanel.SetActive(false);
            MarketPlacePanel.SetActive(false);
            BGMarketPlace.SetActive(false);
        }
        else if (BGTavern.activeSelf == false && BGMarketPlace.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        coinText.text = "Gold: " + PlayerPrefs.GetInt("coin").ToString();
    }
    public void OnClickStart()
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickTavern()
    {
        BGTavern.SetActive(true);
        TavernPanel.SetActive(true);
        MarketPlacePanel.SetActive(false);
        BGMarketPlace.SetActive(false);
    }
    public void OnClickMarketPlace()
    {
        BGMarketPlace.SetActive(true);
        MarketPlacePanel.SetActive(true);
        BGTavern.SetActive(false);
        TavernPanel.SetActive(false);
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