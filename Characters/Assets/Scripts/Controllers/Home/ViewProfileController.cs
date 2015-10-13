using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewProfileController : UIViewController 
{
	#region MemVars & Props

	public UILabel fullName;
	public UILabel avatarName;
	public UILabel mixCount;
	public UILabel friendCount;
	public UILabel followerCount;
	public UILabel followingCount;
	public UITexture previewIcon;
	public UILabel stateOfMind;
	public UILabel gender;
	public UILabel birthday;
	public UILabel location;
	public UILabel about;

	#endregion


	#region Mono Methods

	public override void OnAppear ()
	{
		base.OnAppear ();

		UINavigationBar.ShowMenus(false); 

		var data = GameData.Load();

		fullName.text = data.profile.fullname;
		avatarName.text = "@"+data.profile.avatarname;
		stateOfMind.text = data.profile.state_of_mind;
		gender.text = data.profile.avatargender;
		birthday.text = data.profile.birthday;
		about.text = data.profile.about;
		previewIcon.mainTexture = ResourceManager.Instance.LoadTexture2D("GUI/nopic");

		NetworkController.DownloadFromUrl(UserProfile.GetProfileRequest(data.email), onProfileDownloaded);
	}

	#endregion


	#region Internal Methods

	private void onProfileDownloaded(WWW www)
	{
		UserProfile profile = UserProfile.CreateObject(www.text); 
		Debug.LogWarning("View Profile: " + www.text);

		var data = GameData.Load();
		data.profile = profile.data;
		GameData.Save();

		fullName.text = data.profile.fullname;
		avatarName.text = "@"+data.profile.avatarname;
		stateOfMind.text = data.profile.state_of_mind;
		gender.text = data.profile.avatargender;
		birthday.text = data.profile.birthday;
		about.text = data.profile.about;
	}

	#endregion


	#region Public Methods

	#endregion
}
