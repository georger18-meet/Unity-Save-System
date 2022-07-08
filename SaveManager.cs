using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerParams
{
    public Vector3 pos;
}

public class SaveManager : MonoBehaviour
{
    public GameObject Player;
    public KeyCode savingKey = KeyCode.Alpha1;
    public KeyCode loadingKey = KeyCode.Alpha2;
    public KeyCode toggleAutoKey = KeyCode.T;

    [SerializeField] private float AutoSaveTime = 5;
    [SerializeField] private bool AutoEnabled = true;
    private bool _alreadyAutoSaved;

    private static string saveFolder;

    private void Awake()
    {
        saveFolder = Application.dataPath + "/PlayerParams/";
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }
        LoadFromJson();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleAutoKey))
        {
            ToggleAutoSave();
        }

        if (AutoEnabled && !_alreadyAutoSaved)
        {
            StartCoroutine(AutoSave());
        }
        else if (!AutoEnabled)
        {
            if (Input.GetKeyDown(savingKey))
            {
                SaveToJson();
            }
        }

        if (Input.GetKeyDown(loadingKey))
        {
            LoadFromJson();
        }
    }

    IEnumerator AutoSave()
    {
        _alreadyAutoSaved = true;
        yield return new WaitForSeconds(AutoSaveTime);
        SaveToJson();
        _alreadyAutoSaved = false;
    }

    private void SaveToJson()
    {
        PlayerParams pp = new PlayerParams();
        pp.pos = Player.transform.position;

        string pp_json = JsonUtility.ToJson(pp);
        Debug.Log(pp_json);

        Debug.Log("Saved Position");


        File.WriteAllText(saveFolder + "/Player.json", pp_json);
    }

    private string LoadFromJson()
    {
        if (File.Exists(saveFolder + "/Player.json"))
        {
            Debug.Log("Loaded Position");
            var load = File.ReadAllText(saveFolder + "/Player.json");
            Debug.Log(load);
            PlayerParams pp = JsonUtility.FromJson<PlayerParams>(load);
            Player.transform.position = pp.pos;
            return load;
        }
        else
        {
            return null;
        }
    }

    public void ToggleAutoSave()
    {
        if (AutoEnabled == true)
        {
            AutoEnabled = false;
            Debug.Log("Auto Save: " + AutoEnabled);
        }
        else
        {
            AutoEnabled = true;
            Debug.Log("Auto Save: " + AutoEnabled);
        }
    }
}
