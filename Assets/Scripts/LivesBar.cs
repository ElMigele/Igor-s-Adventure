using UnityEngine;
using System.Collections;

public class LivesBar : MonoBehaviour
{
    private Transform[] HP = new Transform[5];

    private Player player;


    private void Awake()
    {
        player = FindObjectOfType<Player>();

        for (int i = 0; i < HP.Length; i++)
        {
            HP[i] = transform.GetChild(i);
            Debug.Log(HP[i]);
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < HP.Length; i++)
        {
            if (i < player.Lives) HP[i].gameObject.SetActive(true);
            else HP[i].gameObject.SetActive(false);
        }
    }
}