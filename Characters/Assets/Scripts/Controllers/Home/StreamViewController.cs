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

	private Stream streamData;

	public override void OnAppear ()
	{
		base.OnAppear ();
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

		var streamApi = Stream.GetStreamAPI(userid, 5);

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

		if (itemsCountLabel != null)
		{
			itemsCountLabel.text = string.Format("{0}", stream.count_avataritem);
		}

		if (notificationslabel != null)
		{
			notificationslabel.text = string.Format("{0}", stream.count_notification);
		}
	}

	#endregion


	#region Public Methods

	#endregion
}
