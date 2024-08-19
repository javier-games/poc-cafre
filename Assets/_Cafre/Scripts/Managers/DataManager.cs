using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour{
    
	public static DataManager instance;

	[SerializeField]
	private string playerDataFile;

    private StreamReader sr;
    private StreamWriter sw;
    private string contentToReadWrite;

	private PlayerData data;
	public PlayerData temp;


    void Awake(){
        instance = this;
		LoadData(playerDataFile);
    }

	void Start (){
		GameManager.instance.ChangeStateEvent += GameStateChange;	
	}

	void GameStateChange(){
		switch (GameManager.instance.currentState){
		case GameState.GAME_OVER:
			SaveData(playerDataFile);
			
			break;
		case GameState.WIN:
			SaveData(playerDataFile);
			break;
		}
	}

    public void SaveData(string path){
        sw = new StreamWriter(Application.persistentDataPath+"/"+path, false);
        contentToReadWrite = "";
		contentToReadWrite = JsonUtility.ToJson(temp);
        sw.Write(contentToReadWrite);
        sw.Close();
		Debug.Log("Path: " + Application.persistentDataPath + "/" + path);
    }

    public void LoadData(string path){
		data = new PlayerData ();
        
		if (File.Exists(Application.persistentDataPath + "/" + path)){
            sr = new StreamReader(Application.persistentDataPath + "/" + path);
            contentToReadWrite = "";
            contentToReadWrite = sr.ReadToEnd();
			data = JsonUtility.FromJson<PlayerData>(contentToReadWrite);
            sr.Close();
        }
        else{
			data.money = 0;
			data.level = 0;
        }
		temp = data;
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			SaveData(playerDataFile);
	}
}

[System.Serializable]
public class PlayerData{
	public int money;
	public int level;
	public List<ItemData> items;
	public List<TrophyData> trophies;
	public List<UpdateData> updates;

	public PlayerData(){
		items = new List<ItemData> ();
		trophies = new List<TrophyData> ();
		updates = new List<UpdateData> ();
	}
}
	
[System.Serializable]
public class ItemData{
	public string iD;
	public float price;
	public int amount;
}

[System.Serializable]
public class UpdateData{
	public string iD;
	public float price;
	public int amount;
}

[System.Serializable]
public class TrophyData{
	public string iD;
	public int amount;
}
