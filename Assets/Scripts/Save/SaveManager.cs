using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public SaveState state;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Load();
        }

        SceneManager.LoadScene("Pregame");
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", SerializeHelper.Serialize(state));
    }

    public void Load()
    {
        if(PlayerPrefs.HasKey("save"))
        {
            state = SerializeHelper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveState();
            Save();
        }
    }
}
