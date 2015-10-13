using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData 
{
	static public GameData current = null;

	public string email { get; set; }
	public string password { get; set; }
	public UserProfile.Profile profile = new UserProfile.Profile();

	static public GameData Load()
	{
		string xml = PlayerPrefs.GetString("GameData");
		
		if (string.IsNullOrEmpty(xml) == false)
		{
			current = (GameData)XmlManager.DeserializeObject(typeof(GameData), xml);
		}
		
		if (current == null)
		{
			current = new GameData();
		}

		return current;
	}
	
	static public void Save()
	{
		if (current == null)
		{
			current = new GameData();
		}
		
		string xml = XmlManager.SerializeObject(typeof(GameData), current);
		Debug.Log("GameData.Save: " + xml);
		PlayerPrefs.SetString("GameData", xml);
		PlayerPrefs.Save();
	}
	
}
