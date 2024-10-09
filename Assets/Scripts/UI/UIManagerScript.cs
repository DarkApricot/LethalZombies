using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] menus;
    [SerializeField] private GameObject mainStats;

    public void ChangeUIScreen(string _whichScreenOn)
    {
        // Checks if monster is being placed
        if (!FindObjectOfType<PlaceMonsterScript>().enabled)
        {
            // Gets all menus and cross checks names
            for (int i = 0; i < menus.Length; i++)
            {
                if (menus[i].name == _whichScreenOn)
                {
                    if (menus[i].activeInHierarchy == false)
                    {
                        PauseGameCheck(_whichScreenOn, "StoreUI", 0);
                        PauseGameCheck(_whichScreenOn, "PlayUI", 1);
                        TurnOffAllMenus();
                        menus[i].SetActive(true);
                        break;
                    }
                    else
                    {
                        if (_whichScreenOn != "DeathUI")
                        {
                            TurnOffAllMenus();
                            break;
                        }
                    }
                }
            }
        }
    }

    private void TurnOffAllMenus()
    {
        foreach (GameObject _screen in menus)
        {
            _screen.SetActive(false);
        }
    }

    /// <summary>
    /// if _screenA == _screenB, change timescale to _timeScale
    /// </summary>
    private void PauseGameCheck(string _screenA, string _screenB, int _timeScale)
    {
        if (_screenA == _screenB)
        {
            Time.timeScale = _timeScale;
        }
    }

    public void YouDied()
    {
        PauseGameCheck("DeathUI", "DeathUI", 0);
        StartCoroutine(ChangeUIScreenDeathUI());
    }

    private IEnumerator ChangeUIScreenDeathUI()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        ChangeUIScreen("DeathUI");
        mainStats.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
