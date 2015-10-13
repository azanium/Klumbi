using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowViewController : UIViewController 
{
	#region MemVars & Props

	public UITable tableView;
	public GameObject cellPrefab;
	public UILabel headerLabel;
	public UILabel titleLabel;

	private string userid = "";

	private List<GameObject> cells = new List<GameObject>();

	#endregion


	#region Mono Methods

	public override void OnAppear ()
	{
		base.OnAppear (); 

		var header = GetParameter("header");
		var title = GetParameter("title");
		var followType = GetParameter("type");
		var iduser = GetParameter("userid");

		var data = GameData.Load();
		this.userid = data.profile._id;

		if (iduser != "")
		{
			this.userid = iduser;
		}

		if (header != "" && headerLabel != null)
		{
			headerLabel.text = header;
		}

		if (title != "" && titleLabel != null)
		{
			titleLabel.text = title;
		}

		if (followType == "following")
		{
			FetchListFollowing(this.userid);
		}
		else
		{
			FetchListFollowers(this.userid);
		}

		UINavigationBar.ShowMenus(false);
	}

	public override void OnAppeared ()
	{
		base.OnAppeared ();
	}

	public override void OnDisappeared ()
	{
		base.OnDisappeared ();

		foreach (GameObject go in cells)
		{
			Destroy(go);
		}
		cells.Clear();
	}

	#endregion


	#region Internal Methods

	private void FetchListFollowing(string userid)
	{
		var url = FollowList.ListFollowingsApi(userid);
		NetworkController.DownloadFromUrl(url, (www) => {

			if (www.error != null)
			{
				Debug.Log("FetchListFollowing Error: " + www.error);
				return;
			}

			Debug.Log("FetchListFollow: " + www.text);

			FollowList list = FollowList.CreateObject(www.text);
			if (list != null)
			{
				Debug.LogWarning("Populating Following List: " + list.count);
				PopulateFollowList(list);
			}
		});
	}

	private void FetchListFollowers(string userid)
	{
		var url = FollowList.ListFollowersApi(userid);
		NetworkController.DownloadFromUrl(url, (www) => {
			if (www.error != null)
			{
				Debug.Log("FetchListFollowers Error: " + www.error);
				return;
			}

			Debug.Log("FetchListFollow: " + www.text);

			FollowList list = FollowList.CreateObject(www.text);
			if (list != null)
			{
				Debug.Log("Populating Followers List: " + list.count);
				PopulateFollowList(list);
			}
		});
	}

	private void PopulateFollowList(FollowList list)
	{
		if (cellPrefab == null || tableView == null)
		{
			return;
		}

		foreach (var item in list.data)
		{
			var go = (GameObject)Instantiate(cellPrefab);
			go.transform.parent = tableView.transform;
			go.transform.localScale = Vector3.one;
			go.transform.localRotation = Quaternion.identity;
			go.transform.localPosition = Vector3.zero;

			var info = go.GetComponent<FollowCell>();

			if (info != null)
			{
				info.SetLabel(item.fullname);
				info.FetchPicture(item.picture);
				info.EnableIcon(false);
				info.userid = item.user_id;
				Debug.Log("userid: " + info.userid);
				info.OnClicked = OnFollowCellClicked;
				cells.Add(info.gameObject);
			}
		}

		if (tableView != null)
		{
			tableView.repositionNow = true;
		}
	}

	private void OnFollowCellClicked(string id)
	{
		Debug.Log("ID: " + id);
		if (id != "")
		{
			UINavigationController.PushController("/HomePage?page=1&userid="+id);
		}
	}

	#endregion


	#region Public Methods

	#endregion
}
