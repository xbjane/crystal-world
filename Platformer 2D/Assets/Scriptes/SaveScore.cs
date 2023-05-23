using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class SaveScore : MonoBehaviour
{
    public static int scoreToSave;
    public Text scoreText;
    public static SaveScore Instance;
    private void Start()
    {
        Instance = this; 
    }
    public static void SaveGameScore()
    {
        BinaryFormatter bF = new BinaryFormatter();//объект необходимый дл€ сериализации/десериализации, отвечает за перевод инфы в поток бинарных данных
        FileStream file = File.Create(Application.persistentDataPath + "/MySavedData.dat");//созлаЄм бинарный файл по кончтантному адресу дл€ пользовательских данных
        SavedData data = new SavedData();//создаЄм экземпл€р класса 
        data.savedScore = scoreToSave;//записываем в него данные, необходимые к сохранению
        bF.Serialize(file, data);//сериализуем данные экземпл€ра и отправл€ем в файл
        file.Close();
        Debug.Log("Data Saved!!");
    }
   private void LoadGameScore()
    {
        if(File.Exists(Application.persistentDataPath+"/MySavedData.dat"))
        {
            BinaryFormatter bF = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MySavedData.dat", FileMode.Open);
            SavedData data = (SavedData)bF.Deserialize(file);
            file.Close();
            scoreToSave = data.savedScore;
            Debug.Log("Game Data Loaded!!");
        }
        else
        {
            Debug.LogError("No Saved Data!!");
        }
    }
   public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/MySavedData.dat"))
        {
            BinaryFormatter bF = new BinaryFormatter();
          File.Delete(Application.persistentDataPath + "/MySavedData.dat");
            scoreToSave = 0;
            scoreText.text = scoreToSave.ToString();
            Debug.Log("Data Reset Complete!!");
        }
        else
        {
            Debug.LogError("NoSavedDataToDelete");
        }
    }
    public void Print()
    {
        LoadGameScore();
        scoreText.text = scoreToSave.ToString();
    }
}
