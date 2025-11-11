using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : Singleton<MainMenuUI>
{
    [Header("Panels (assign in Inspector)")]
    public GameObject panelMainMenu;   // Panel_MainMenu (active lúc start)
    public GameObject panelSettings;   // Panel_Settings (inactive)
    public GameObject panelHelp;       // Panel_Help (inactive)
    public GameObject panelWinScreen;
    public GameObject panelLoseScreen;

    [Header("Button in game")]
    public GameObject btnBackFromGame; 

    [Header("Gameplay prefab (optional)")]
    public GameObject gameplayRootPrefab; // Prefab chứa level + player
    private GameObject gameplayInstance;

    private void Start()
    {
        // đảm bảo trạng thái ban đầu
        ShowMainMenu();
    }

    // ---------- BUTTON CALLBACKS (bấm trong UI) ----------
    public void OnStartButton()
    {
        StartGame();
    }

    public void OnSettingsButton()
    {
        panelMainMenu.SetActive(false);
        panelSettings.SetActive(true);
    }

    public void OnHelpButton()
    {
        panelMainMenu.SetActive(false);
        panelHelp.SetActive(true);
    }

    public void OnExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void BackFromSettings()
    {
        panelSettings.SetActive(false);
        btnBackFromGame.SetActive(false);
        panelMainMenu.SetActive(true);
    }

    public void BackFromHelp()
    {
        panelHelp.SetActive(false);
        btnBackFromGame.SetActive(false);
        panelMainMenu.SetActive(true);
    }

    // ---------- START / SHOW ----------
    public void StartGame()
    {
        panelMainMenu.SetActive(false);

        if (gameplayRootPrefab == null)
        {
            Debug.LogWarning("Gameplay prefab not assigned in MainMenuUI.");
            return;
        }

        if (gameplayInstance == null)
        {
            gameplayInstance = Instantiate(gameplayRootPrefab, Vector3.zero, Quaternion.identity);
            btnBackFromGame.SetActive(true);
        }
        else
        {
            gameplayInstance.SetActive(true);
            btnBackFromGame.SetActive(true);
        }
    }

    public void ShowMainMenu()
    {
        // ẩn gameplay nếu có
        if (gameplayInstance != null)
        {
            Destroy(gameplayInstance);
            Destroy(GameObject.Find("Player"));
            gameplayInstance = null;
        }

        panelWinScreen.SetActive(false);
        panelLoseScreen.SetActive(false);
        panelSettings.SetActive(false);
        panelHelp.SetActive(false);
        btnBackFromGame.SetActive(false);
        panelMainMenu.SetActive(true);
    }

    public void ShowWinScreen()
    {
        panelWinScreen.SetActive(true);
        panelLoseScreen.SetActive(false);
        panelSettings.SetActive(false);
        panelHelp.SetActive(false);
        panelMainMenu.SetActive(false);
        btnBackFromGame.SetActive(false);
    }
    
    public void ShowLoseScreen()
    {
        panelWinScreen.SetActive(false);
        panelLoseScreen.SetActive(true);
        panelSettings.SetActive(false);
        panelHelp.SetActive(false);
        panelMainMenu.SetActive(false);
        btnBackFromGame.SetActive(false);
    }
}
