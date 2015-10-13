using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamViewController : UIViewController 
{
	#region MemVars & Props

	public UILabel followersCountLabel;
	public UILabel itemsCountLabel;
	public UILabel notificationslabel;

	public UserInfo followerUserInfo;
	public UserInfo[] avatarItemsInfo;
	public UILabel[] notificationCellLabels;

	public UIButton buttonNewFollowersShowAll;
	public UIButton buttonNewItemsShowAll;
	public UIButton buttonNotificationsShowAll;

	public int maxStreamDay = 3;

	private Stream streamData;

	public override void OnAppear ()
	{
		base.OnAppear ();

		if (avatarItemsInfo != null)
		{
			if (avatarItemsInfo.Length > 0)
			{
				foreach (UserInfo info in avatarItemsInfo)
				{
					info.gameObject.SetActive(false);
				}
			}
		}

		if (notificationCellLabels != null)
		{
			if (notificationCellLabels.Length > 0)
			{
				foreach (UILabel label in notificationCellLabels)
				{
					label.gameObject.SetActive(false);
				}
			}
		}
		
		if (followerUserInfo != null)
		{
			followerUserInfo.gameObject.SetActive(false);
		}

	}

	public override void OnAppeared ()
	{
		base.OnAppeared ();

		DressRoom.HideCharacter();

		FetchStreams();
	}

	#endregion


	#region Mono Methods



	#endregion


	#region Internal Methods

	public void FetchStreams()
	{
		var data = GameData.Load();
		var userid = data.profile._id;

		var streamApi = Stream.GetStreamAPI(userid, maxStreamDay);

		NetworkController.DownloadFromUrl(streamApi, (www) => {
			if (www.error != null)
			{
				return;
			}

			streamData = Stream.CreateObject(www.text);
			if (streamData != null)
			{
				PopulateStream(streamData);
			}
		});
	}

	private void PopulateStream(Stream stream)
	{
		Debug.Log("Populating Stream: " + stream.data_notification.ToString());

		if (followersCountLabel != null)
		{
			followersCountLabel.text = string.Format("{0}", stream.count_new_follower);

			if (followerUserInfo != null)
			{
				if (stream.count_new_follower > 0)
				{
					followerUserInfo.gameObject.SetActive(true);

					var follower = stream.data_new_follower[0];

					followerUserInfo.FetchPicture(follower.picture);
					followerUserInfo.SetLabel(follower.fullname);
				}
				else
				{
					followerUserInfo.gameObject.SetActive(false);
				}
			}

		}

		if (buttonNewFollowersShowAll != null)
		{
			buttonNewFollowersShowAll.gameObject.SetActive(stream.count_new_follower > 0);
		}

		if (itemsCountLabel != null)
		{
			itemsCountLabel.text = string.Format("{0}", stream.count_avataritem);

			if (avatarItemsInfo.Length == 3)
			{
				int count = stream.count_avataritem > 3 ? 3 : stream.count_avataritem;
				for (int i = 0; i < count; i++)
				{
					var avatarItem = stream.data_avataritem[i];
					var info = avatarItemsInfo[i];

					info.SetLabel(avatarItem.name);
					info.gameObject.SetActive(true);
				}
			}

		}

		if (buttonNewItemsShowAll != null)
		{
			buttonNewItemsShowAll.gameObject.SetActive(stream.count_avataritem > 0);
		}


		if (notificationslabel != null)
		{
			notificationslabel.text = string.Format("{0}", stream.count_notification);

			if (notificationCellLabels != null && stream.count_notification > 0)
			{
				int count = stream.count_notification > 3 ? 3 : stream.count_notification;

				for (int i = 0; i < count; i++)
				{
					var notif = stream.data_notification[i];

					string notification = string.Format("{0}\n{1}", notif.header, notif.detail);
					notificationCellLabels[i].text = notification;
					notificationCellLabels[i].gameObject.SetActive(true);
				}
			}
		}

		if (buttonNotificationsShowAll != null)
		{
			buttonNotificationsShowAll.gameObject.SetActive(stream.count_notification > 0);
		}

	}

	#endregion


	#region Public Methods

	public void ShowAllNewFollowers()
	{
		var data = GameData.Load();
		var userid = data.profile._id;

		UINavigationController.PushController("/Follow?header=Stream&title=New Followers&type=follower&userid="+userid);
	}

	#endregion
}
