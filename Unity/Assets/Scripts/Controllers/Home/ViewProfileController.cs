using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewProfileController : UIViewController 
{
	#region MemVars & Props

	public GameObject bottomMenuPanel;
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

	private string userid = "";

	private int countFollowers = 0;
	private int countFollowing = 0;

	#endregion


	#region Mono Methods

	public override void OnAppear ()
	{
		base.OnAppear ();

		UINavigationBar.ShowMenus(false); 

		this.countFollowers = 0;
		this.countFollowing = 0;

		var data = GameData.Load();
		this.userid = data.profile._id;

		var iduser = GetParameter("userid");
		if (iduser.Trim() != "")
		{
			if (iduser != this.userid)
			{
				if (bottomMenuPanel != null)
				{
					bottomMenuPanel.gameObject.SetActive(false);
				}
			}
			else
			{
				if (bottomMenuPanel != null)
				{
					bottomMenuPanel.gameObject.SetActive(true);
				}
			}
			this.userid = iduser;
		}

		fullName.text = data.profile.fullname;
		avatarName.text = "@"+data.profile.avatarname;
		stateOfMind.text = data.profile.state_of_mind;
		gender.text = data.profile.avatargender;
		birthday.text = data.profile.birthday;
		about.text = data.profile.about;
		previewIcon.mainTexture = ResourceManager.Instance.LoadTexture2D("GUI/nopic");

		NetworkController.DownloadFromUrl(UserProfile.GetProfileByIdRequest(this.userid), onProfileDownloaded);

		FetchFollowersCount(this.userid);
		FetchFollowingCount(this.userid);
	}

	#endregion


	#region Internal Methods

	private void FetchFollowersCount(string userid)
	{
		string url = FollowCount.CountFollowerApi(userid);

		NetworkController.DownloadFromUrl(url, (www) => {
			if (www.error != null)
			{
				Debug.LogWarning("ViewProfile Followers Count error: " + www.error);
			}

			FollowCount followCount = FollowCount.CreateObject(www.text);
			Debug.LogWarning("FollowersCount: " + www.text);
			if (followCount != null && followerCount)
			{
				followerCount.text = string.Format("{0}", followCount.count);
				this.countFollowers = followCount.count;
			}
		});
	}

	private void FetchFollowingCount(string userid)
	{
		string url = FollowCount.CountFollowingApi(userid);
		
		NetworkController.DownloadFromUrl(url, (www) => {
			if (www.error != null)
			{
				Debug.LogWarning("ViewProfile Following Count error: " + www.error);
			}
			
			FollowCount followCount = FollowCount.CreateObject(www.text);
			Debug.LogWarning("FollowingCount: " + www.text);
			if (followCount != null && followingCount != null)
			{
				followingCount.text = string.Format("{0}", followCount.count);
				this.countFollowing = followCount.count;
			}
		});
	}

	private void onProfileDownloaded(WWW www)
	{
		UserProfile userProfile = UserProfile.CreateObject(www.text); 

		Debug.LogWarning("View Profile: " + www.text+ ", Error: " + www.error);

		if (userProfile != null)
		{
			var profile = userProfile.data;

			fullName.text = profile.fullname;
			avatarName.text = "@"+profile.avatarname;
			stateOfMind.text = profile.state_of_mind;
			gender.text = profile.avatargender;
			birthday.text = profile.birthday;
			about.text = profile.about;
		}
	}

	#endregion


	#region Public Methods

	public void OnShowAllFollowers()
	{
		if (this.countFollowers > 0)
		{
			UINavigationController.PushController("/Follow?header=Followers&title=Who's following You&type=follower&userid="+this.userid);
		}
	}

	public void OnShowAllFollowings()
	{
		if (this.countFollowing > 0)
		{
			UINavigationController.PushController("/Follow?header=Following&title=Who's You are following&type=following&userid="+this.userid);
		}
	}

	public void OnEditProfile()
	{
		UINavigationController.PushController("/EditProfile?userid="+this.userid);
	}

	#endregion
}
