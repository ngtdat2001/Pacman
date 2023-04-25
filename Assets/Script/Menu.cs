using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void NewGame()
    {
        DataPersistenceManager.instance.NewGame();
    }

    public void SaveGame()
    {
        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadGame()
    {
        DataPersistenceManager.instance.LoadGame();
    }
}
