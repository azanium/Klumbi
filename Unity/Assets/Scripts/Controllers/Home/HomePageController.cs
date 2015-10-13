using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomePageController : UIPageViewController 
{
	#region MemVars & Props

	private string userid = "";

	#endregion


	#region Mono Methods

	public override void OnAppear()
	{
		var page = GetParameter("page");
		int currentPage = page.Trim() != "" ? int.Parse(page) : 1;
		activePageIndex = currentPage;

		base.OnAppear();

		UINavigationBar.Show(true);
		UINavigationBar.ShowMenus(true);

		userid = GetParameter("userid");
	
		//Debug.Log("Userid ===============> " + userid + ", page: " + page);
	}

	public override void OnDissapear()
	{
		base.OnDissapear();

		DressRoom.HideCharacter();
	}

	#endregion


	#region Internal Methods

	#endregion


	#region Public Methods

	public void OpenHome()
	{
		ShowPage(0, "");
	}

	public void OpenMyAvatar()
	{
		ShowPage(1, "?userid="+userid);
	}

	public void OpenStream()
	{
		ShowPage(2, "");
	}

	public void OpenMoreMenu()
	{
		ShowPage(3, "");
	}

	public void OnStyle() 
	{
		ShowPage(4, "");
	}

	#endregion
}
