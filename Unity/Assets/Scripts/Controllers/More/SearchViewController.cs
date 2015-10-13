using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchViewController : UIViewController 
{
	#region MemVars & Props

	public UITable tableView;
	public GameObject cellPrefab;

	private List<UserInfo> cells = new List<UserInfo>();

	#endregion


	#region Mono Methods

	public override void OnAppear ()
	{
		Clear();

		base.OnAppear ();

		UINavigationBar.ShowMenus(false);

		FetchPeoples();
	}

	public override void OnAppeared ()
	{
		base.OnAppeared ();
	}

	#endregion

	
	#region Internal Methods

	private void FetchPeoples()
	{
		var url = People.GetPeoplesApi(0);
		NetworkController.DownloadFromUrl(url, (www) => {

			if (www.error != null)
			{
				Debug.LogWarning("FetchPeoples Error: " + www.error);
				return;
			}

			Debug.Log("FetchPeoples: " + www.text);

			var people = People.CreateObject(www.text);
			if (people != null)
			{
				PopulatePeopleList(people);
			}
		});

	}

	private void PopulatePeopleList(People peoples)
	{
		if (cellPrefab == null || tableView == null)
		{	
			return;
		}
		
		foreach (var item in peoples.data)
		{
			var go = (GameObject)Instantiate(cellPrefab);
			go.transform.parent = tableView.transform;
			go.transform.localScale = Vector3.one;
			go.transform.localRotation = Quaternion.identity;
			go.transform.localPosition = Vector3.zero;
			
			var info = go.GetComponent<UserInfo>();
			
			if (info != null)
			{
				info.SetLabel(item.fullname);
				info.FetchPicture(item.picture);
				info.SetUserObject(item);
				info.OnClicked = OnClicked;
				cells.Add(info);
			}
		}
		
		if (tableView != null)
		{
			tableView.repositionNow = true;
		}
	}

	private void Clear()
	{
		foreach (UserInfo go in cells)
		{
			Destroy(go.gameObject);
		}
		cells.Clear();
	}

	#endregion


	#region Public Methods

	public void OnClicked(UserInfo info)
	{
		People.UserData data = (People.UserData)info.userObject;
		if (data != null)
		{
			Debug.Log(string.Format("User Email: {0}, Id: {1}", data.email, data._id));
			UINavigationController.PushController("/HomePage?page=1&userid="+data._id);
		}
	}

	#endregion
}
