using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyAvatarViewController : UIViewController 
{
	#region MemVars & Props

	public UILabel headerLabel;
	public UILabel fullNameLabel;
	public UILabel avatarNameLabel;
	public UILabel starsLabel;
	public UILabel diamondsLabel;
	public UITexture profileTexture;

	public UIButton featuresButton;
	public UIButton styleButton;
	public UIButton storeButton; 

	public UIToggle loveButton;
	public UIToggle followButton;

	public GameObject diamondIcon;

	public GameObject actionMenuBar;
	public GameObject friendMenuBar;

	public UserInfo stateOfMindInfo;

	private string userid = "";
	private bool isMyAvatar = true;
	private string avatarStatusText = "";

	private float timer = 0;
	private float statusTimer = 5;

	#endregion


	#region Mono Methods

	public override void OnAppear()
	{
		base.OnAppear();

		ResetProfile();

		var data = GameData.Load();
		userid = data.profile._id;
		
		var customUserId = GetParameter("userid");
		
		ShowButtons(true);
		isMyAvatar = true;

		if (customUserId.Trim() != "")
		{
			if (userid != customUserId)
			{
				ShowButtons(false);
				isMyAvatar = false;
			}
			
			userid = customUserId;
			Debug.LogWarning("App trying to open MyAvatar with custom UserId: " + userid);
		}

		if (!isMyAvatar)
		{
			NetworkController.DownloadFromUrl(UserProfile.GetProfileByIdRequest(userid), (www) => {
				UserProfile profile = UserProfile.CreateObject(www.text); 
				Debug.LogWarning("View Profile: " + www.text);

				PopulateProfile("Avatar", profile.data);

				ShowCharacter(data.profile);
			});

			FetchFollowerCount();
			CheckFollowOrNot();
			CheckLovePage();
		}
		else
		{
			PopulateProfile("My Avatar", data.profile);
		}

		Debug.LogWarning("IsMyAvatar: " + isMyAvatar);
	}

	private void ResetProfile()
	{
		if (fullNameLabel != null)
		{
			fullNameLabel.text = "";
		}
		if (avatarNameLabel != null)
		{
			avatarNameLabel.text = "";
		}
		
		avatarStatusText = "";
		
		if (headerLabel != null)
		{
			headerLabel.text = "Avatar";
		}
		profileTexture.mainTexture = ResourceManager.Instance.LoadTexture2D("GUI/nopic");
		starsLabel.text = "0";
		diamondsLabel.text = "0";
		if (stateOfMindInfo != null)
		{
			stateOfMindInfo.gameObject.SetActive(false);
		}
		stateOfMindInfo.SetLabel("");
	}

	private void PopulateProfile(string title, UserProfile.Profile profile)
	{
		if (fullNameLabel != null)
		{
			fullNameLabel.text = profile.fullname;
		}
		if (avatarNameLabel != null)
		{
			avatarNameLabel.text = profile.avatarname;
		}

		avatarStatusText = profile.state_of_mind;

		if (headerLabel != null)
		{
			headerLabel.text = title;
		}

		FetchProfilePicture(profile.picture, profile._id);
		stateOfMindInfo.SetLabel(profile.state_of_mind);
	}

	private void FetchProfilePicture(string url, string id)
	{
		if (url.Trim() == "")
		{
			return;
		}

		NetworkController.DownloadFromUrl(url, (www) => {
			if (www.error != null)
			{
				return;
			}

			if (profileTexture != null && id == this.userid)
			{
				profileTexture.mainTexture = www.texture;
			}
		});
	}

	public override void OnAppeared()
	{
		base.OnAppeared();

		Debug.Log("Opening avatar: " + userid);
		if (isMyAvatar)
		{
			DressRoom.ShowCharacter(userid);
		}
		DressRoom.CameraDolly(CameraShifter.ZoomTargetArea.Default);
	}

	private void ShowCharacter(UserProfile.Profile profile)
	{
		var url = AvatarConfig.GetDefaultAvatarConfigApiV2(profile._id, profile.bodytype, profile.sex);
		NetworkController.DownloadFromUrl(url, (www) => {
			if (www.error != null)
			{
				Debug.LogWarning("ShowCaracter Error: " + www.error);
				return;
			}

			AvatarConfig config = AvatarConfig.CreateObject(www.text);
			Debug.LogWarning("ShowCharacter: " + www.text);
			if (config != null)
			{
				string configuration = config.configurations;

				DressRoom.ChangePlayerCharacter(configuration);
				DressRoom.CameraDolly(CameraShifter.ZoomTargetArea.Default);
			}
		});
	}

	private void FetchFollowerCount()
	{
		string countFollowerApi = FollowCount.CountFollowerApi(userid);

		Debug.Log("API: FetchFollowerCount: " + countFollowerApi);

		NetworkController.DownloadFromUrl(countFollowerApi, (www) =>  {
			if (www.error != null)
			{
				return;
			}
			
			FollowCount followCount = FollowCount.CreateObject(www.text);
			if (followCount != null)
			{
				UILabel followText = followButton.GetComponentInChildren<UILabel>();
				if (followText != null)
                {
                    followText.text = string.Format("{0}", followCount.count);
                }
            }
        });
    }

	private void CheckFollowOrNot()
	{
		var data = GameData.Load();
		var myId = data.profile._id;
		string checkFollowUser = Follow.CheckFollowUserApi(myId, userid);

		Debug.Log("API: CheckFollowOrNot: " + checkFollowUser);

		NetworkController.DownloadFromUrl(checkFollowUser, (www) => {
			if (www.error != null)
			{
				return;
			}

			Follow follow = Follow.CreateObject(www.text);
			if (follow != null)
			{
				if (followButton != null)
				{
					followButton.value = follow.follow;
				}
			}
		});
	}
    
	public void OnFollowFriend()
	{
		var data = GameData.Load();
		var myId = data.profile._id;

		string followUnfollowApi = Follow.FollowUnfollowUserApi(myId, userid);

		Debug.Log("API: FollowUnFollowUser: " + followUnfollowApi);

		NetworkController.DownloadFromUrl(followUnfollowApi, (www) => {
			if (www.error != null)
			{
				return;
			}

			Debug.Log("Follow Friend JSON: " + www.text);

			Follow followUnfollow = Follow.CreateObject(www.text);
			if (followUnfollow != null)
			{
				if (followButton != null)
				{
					followButton.value = followUnfollow.follow;
				}

				FetchFollowerCount();
			}
		});
	}

	private void CheckLovePage()
	{
		var data = GameData.Load();
		var myId = data.profile._id;

		string checkLoveUserPageApi = Like.CheckLikeUserPageApi(myId, userid);

		NetworkController.DownloadFromUrl(checkLoveUserPageApi, (www) => {
			if (www.error != null)
			{
				return;
			}

			Debug.Log("Check Love Page: " + www.text);

			LikeUserPage like = LikeUserPage.CreateObject(www.text);
			if (like != null)
			{
				if (loveButton != null)
				{
					loveButton.value = like.like;
					UILabel loveText = loveButton.GetComponentInChildren<UILabel>();
					if (loveText != null)
					{
						loveText.text = string.Format("{0}", like.count);
					}
				}
			}
		});
	}

	public void OnLoveUserPage()
	{
		var data = GameData.Load();
		var myId = data.profile._id;

		string loveUnloveUserPageApi = LikeUserPage.LikeUnlikeUserPageApi(myId, userid);

		NetworkController.DownloadFromUrl(loveUnloveUserPageApi, (www) => {
			if (www.error != null)
			{
				return;
			}

			Debug.Log("OnLoveUserPage: " + www.text);

			LikeUserPage likeUserPage = LikeUserPage.CreateObject(www.text);
			if (likeUserPage != null)
			{
				if (loveButton != null)
				{
					loveButton.value = likeUserPage.like;

					UILabel loveCountLabel = loveButton.GetComponentInChildren<UILabel>();
					if (loveCountLabel != null)
					{
						loveCountLabel.text = string.Format("{0}", likeUserPage.count);
					}
				}
			}
		});
	}

    public override void OnDissapear()
	{
		base.OnDissapear();
		DressRoom.HideCharacter();
	}


	private void ShowButtons(bool state)
	{
		featuresButton.gameObject.SetActive(state);
		styleButton.gameObject.SetActive(state);
		storeButton.gameObject.SetActive(state);
		actionMenuBar.SetActive(state);
		diamondIcon.SetActive(state);
		friendMenuBar.SetActive(!state);
	}

	public override void Update()
	{
		timer += Time.deltaTime;
		if (timer > statusTimer)
		{
			timer = 0;

			if (stateOfMindInfo != null)
			{
				if (stateOfMindInfo.GetLabel() != "")
				{
					if (stateOfMindInfo.gameObject.activeSelf)
					{
						statusTimer = 3;
					}
					else
					{
						statusTimer = 5;
					}
					stateOfMindInfo.gameObject.SetActive(!stateOfMindInfo.gameObject.activeSelf);
				}
				else
				{
					stateOfMindInfo.gameObject.SetActive(false);
				}
			}
		}
	}


	#endregion


	#region Internal Methods

	#endregion


	#region Public Methods

	public void OnCreateMix()
	{

	}

	public void OnViewOtherUserProfile()
	{
		UINavigationController.PushController("/ViewProfile?userid="+this.userid);
	}

	#endregion
}
