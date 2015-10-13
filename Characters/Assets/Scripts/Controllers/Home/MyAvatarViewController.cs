using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyAvatarViewController : UIViewController 
{
	#region MemVars & Props

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

	private string userid = "";
	
	#endregion


	#region Mono Methods

	public override void OnAppear()
	{
		base.OnAppear();

		var data = GameData.Load();
		userid = data.profile._id;
		
		var customUserId = GetParameter("userid");
		
		ShowButtons(true);
		
		bool showFriendMenu = false;
		
		if (customUserId.Trim() != "")
		{
			if (userid != customUserId)
			{
				ShowButtons(false);
				showFriendMenu = true;
			}
			
			userid = customUserId;
			Debug.LogWarning("App trying to open MyAvatar with custom UserId: " + userid);
		}

		if (fullNameLabel != null)
		{
			fullNameLabel.text = data.profile.fullname;
		}
		if (avatarNameLabel != null)
		{
			avatarNameLabel.text = data.profile.avatarname;
		}

		profileTexture.mainTexture = ResourceManager.Instance.LoadTexture2D("GUI/nopic");

		NetworkController.DownloadFromUrl(UserProfile.GetProfileByIdRequest(userid), (www) => {
			UserProfile profile = UserProfile.CreateObject(www.text); 
			Debug.LogWarning("View Profile: " + www.text);

			data.profile = profile.data;
			GameData.Save();

			if (fullNameLabel != null)
			{
				fullNameLabel.text = profile.data.fullname;
			}
			if (avatarNameLabel != null)
			{
				avatarNameLabel.text = profile.data.avatarname;
			}
		});

		if (showFriendMenu)
		{
			FetchFollowerCount();
			CheckFollowOrNot();
			CheckLovePage();
		}
	}

	public override void OnAppeared()
	{
		base.OnAppeared();

		var data = GameData.Load();

		Debug.Log("Opening avatar: " + userid);
		DressRoom.ShowCharacter(userid);
		DressRoom.CameraDolly(CameraShifter.ZoomTargetArea.Default);
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

	#endregion


	#region Internal Methods

	#endregion


	#region Public Methods

	public void OnCreateMix()
	{

	}

	#endregion
}
