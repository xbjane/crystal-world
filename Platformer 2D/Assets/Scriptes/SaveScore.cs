using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class SaveScore : MonoBehaviour
{
    public static int score;
    private static int loadedScore;
    private static int[] scores;
    private bool isEmpty;
    public Text scoreText;
    public static SaveScore Instance;
    private void Start()
    {
        Instance = this;
        scores = new int[5];
    }
    public void CheckScore() 
    {
        bool isTheSameScore = false;
        LoadGameScore();
        if (isEmpty)
            for (int i = 0; i < scores.Length; i++)
                scores[i] = 0;
        loadedScore = scores[4];
        Debug.Log("score " + score);
        if (score > loadedScore)
        {
            Debug.Log("score > loadedScore");
            int j = 4;
            int m = j;
            while (j > 0)
            {
                if (score > scores[j - 1])
                    m = j-1;
                else if(score == scores[j - 1])
                {
                    isTheSameScore = true;
                    break;
                }
                else break;
                j--;
                Debug.Log("j=" + j);
            }
            if (!isTheSameScore)
            {
                scores[4] = score;
                int i = 4;
                while (i > m)
                {
                        Swap(ref scores[i], ref scores[i - 1]);
                    i--;
                }
            }
            SaveGameScore();
        }
    }
    private void Swap(ref int a, ref int b)
    {
        int n = a;
        a = b;
        b = n;
    }
    private void SaveGameScore()
    {
        BinaryFormatter bF = new BinaryFormatter();//объект необходимый дл€ сериализации/десериализации, отвечает за перевод инфы в поток бинарных данных
        FileStream file = File.Create(Application.persistentDataPath + "/MySavedData.dat");//созлаЄм бинарный файл по кончтантному адресу дл€ пользовательских данных
        SavedData data = new SavedData();//создаЄм экземпл€р класса 
        data.savedScore = scores;//записываем в него данные, необходимые к сохранению
        bF.Serialize(file, data);//сериализуем данные экземпл€ра и отправл€ем в файл
        file.Close();
    }
   private void LoadGameScore()
    {
        if(File.Exists(Application.persistentDataPath+"/MySavedData.dat"))
        {
            BinaryFormatter bF = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MySavedData.dat", FileMode.Open);
            SavedData data = (SavedData)bF.Deserialize(file);
            file.Close();
            scores = data.savedScore;
            isEmpty = false;
        }
        else
        {
            isEmpty = true;
        }
    }
   public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/MySavedData.dat"))
        {
            BinaryFormatter bF = new BinaryFormatter();
            File.Delete(Application.persistentDataPath + "/MySavedData.dat");
            Array.Clear(scores, 0, 5);
            scoreText.text = "Lets play to have records!";
        }
    }
    public void Print()
    {
        LoadGameScore();
        int i = 1;
        if(scores[0]==0)
            scoreText.text = "Lets play to have records!";
        else
        foreach (int s in scores)
        {
                if (s == 0)
                    break;
            scoreText.text +=i.ToString()+ ". "+ s.ToString()+ "\n";
                i++;
        }
    }
}
