using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDController : MonoBehaviour
{
    [Header("Star HUD")]
    [SerializeField] private TMP_Text starText;

    [Header("Health HUD")]
    [SerializeField] private Image[] carrotIcons;

    public void UpdateStarCount(int count)
    {
        starText.text = "x " + count.ToString();
    }

    public void UpdateHealth(int health)
    {
        for (int i = 0; i < carrotIcons.Length; i++)
            carrotIcons[i].enabled = i < health;    //Enable img[i] khi mà i < health
    }
}
