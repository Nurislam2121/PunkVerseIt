using System.Collections.Generic;
using UnityEngine;

public class HatLoader : MonoBehaviour
{
    [Header("Hats")]
    public List<GameObject> hats; // «аполни через инспектор теми же шл€пами, что и в селекторе

    private void Start()
    {
        // ѕолучаем сохранЄнный айди шл€пы, если нет Ч 0
        int selectedHatID = PlayerPrefs.GetInt("SelectedHatID", 0);
        selectedHatID = Mathf.Clamp(selectedHatID, 0, hats.Count - 1);

        // јктивируем выбранную шл€пу, остальные отключаем
        for (int i = 0; i < hats.Count; i++)
        {
            hats[i].SetActive(i == selectedHatID);
        }
    }
}
