using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HatSelector : MonoBehaviour
{
    [Header("Hats")]
    public List<GameObject> hats; // ѕеретащи сюда 5 шл€п
    private int currentIndex = 0;

    private void Start()
    {
        // «агружаем выбранную шл€пу из PlayerPrefs
        currentIndex = PlayerPrefs.GetInt("SelectedHatID", 0);
        UpdateHats();
    }

    //  нопка "Next"
    public void NextHat()
    {
        currentIndex++;
        if (currentIndex >= hats.Count)
            currentIndex = 0;

        UpdateHats();
    }

    //  нопка "Previous"
    public void PreviousHat()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = hats.Count - 1;

        UpdateHats();
    }

    //  нопка "Select" Ч сохран€ем и переходим на Level1
    public void SelectHat()
    {
        PlayerPrefs.SetInt("SelectedHatID", currentIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Level1");
    }

    // ¬ключаем текущую шл€пу, остальные выключаем
    private void UpdateHats()
    {
        for (int i = 0; i < hats.Count; i++)
        {
            hats[i].SetActive(i == currentIndex);
        }
    }
}
