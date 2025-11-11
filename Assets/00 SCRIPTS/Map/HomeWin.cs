using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeWin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collison)
    {
        if (collison.CompareTag("Player"))
        {
            MainMenuUI.Instant.ShowWinScreen();
            collison.gameObject.SetActive(false);
        }
    }
}
