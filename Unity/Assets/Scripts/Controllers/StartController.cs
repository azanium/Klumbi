using UnityEngine;
using System.Collections;

public class StartController : UIViewController 
{
	public UIViewController homePageController;

	public override bool StackPushable {
		get {
			return false;
		}
	}

	public void StartKlumbi()
	{
		Debug.Log("Starting Klumbi");
		//UINavigationController.PushController(homePageController);
		//UINavigationController.PushController("/HomePage?userid=1234&page=2");

		// Open Home page = 1 (MyAvatar)
		//UINavigationController.PushController("/HomePage?page=1&userid=53cf76bf09bf103c1c000000");
		UINavigationController.PushController("/HomePage?page=1");
	}
}
