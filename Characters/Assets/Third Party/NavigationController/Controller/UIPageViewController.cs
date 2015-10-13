using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPageViewController : UIViewController 
{
	#region MemVars & Props

	public int activePageIndex = 0;
	public List<UIViewController> pages;
	
	#endregion


	#region Mono & UIViewController's Methods

	public override void Start()
	{
		base.Start();
		if (pages == null)
		{
			pages = new List<UIViewController>(); 
		}
		
	}

	public override void OnAppear()
	{
		base.OnAppear();

		for (int i = 0; i < pages.Count; i++)
		{
			ActivatePage(i, false);
		}
		ActivatePage(activePageIndex, true);
	}

	#endregion


	#region Internal Methods

	protected void ActivatePage(int pageIndex, bool state)
	{
		if (pageIndex < 0 || pageIndex > pages.Count - 1)
		{
			return;
		}

		var activePage = pages[pageIndex];
		if (activePage == null)
		{
			Debug.Log("Page Index: " + pageIndex + " is null");
			return;
		}
		activePage.gameObject.SetActive(state);
		if (state)
		{
			activePage.controllerParameters = new Dictionary<string, string>(this.controllerParameters);
			activePage.OnAppear();
			activePage.OnAppeared();
		}
	}

	public void ShowPage(int pageIndex, string parameter)
	{
		if (pageIndex < 0 || pageIndex > pages.Count - 1)
		{
			Debug.LogWarning("UIPageViewController: Invalid page index: " + pageIndex);
			return;
		}
		if (activePageIndex == pageIndex)
		{
			return;
		}

		var currentPage = pages[activePageIndex];
		var nextPage = pages[pageIndex];

		var defaultShowForwardAnimation = UINavigationController.navigationController != null ? UINavigationController.navigationController.showForwardAnimation : null;
		var defaultHideForwardAnimation = UINavigationController.navigationController != null ? UINavigationController.navigationController.hideForwardAnimation : null;
		var defaultShowBackAnimation = UINavigationController.navigationController != null ? UINavigationController.navigationController.showBackAnimation : null;
		var defaultHideBackAnimation = UINavigationController.navigationController != null ? UINavigationController.navigationController.hideBackAnimation : null;

		AnimationClip showAnimClip, hideAnimClip;

		if (activePageIndex < pageIndex)
		{
			showAnimClip = this.showForwardAnimation != null ? this.showForwardAnimation : defaultShowForwardAnimation;
			hideAnimClip = this.hideForwardAnimation != null ? this.hideForwardAnimation : defaultHideForwardAnimation;
		}
		else
		{
			showAnimClip = this.showBackAnimation != null ? this.showBackAnimation : defaultShowBackAnimation;
			hideAnimClip = this.hideBackAnimation != null ? this.hideBackAnimation : defaultHideBackAnimation;
		}

		Animation currentTarget = currentPage.GetComponent<Animation>();
		if (currentTarget != null && hideAnimClip != null)
		{
			var anim = ActiveAnimation.Play(currentTarget, hideAnimClip.name, AnimationOrTween.Direction.Forward, AnimationOrTween.EnableCondition.DoNothing, AnimationOrTween.DisableCondition.DisableAfterForward);
			currentPage.OnDissapear();
			
			anim.onFinished.Add(new EventDelegate(() =>
			                                      {
				currentPage.OnDisappeared();
			}));
		}

		Animation nextTarget = nextPage.GetComponent<Animation>();
		if (nextTarget != null && showAnimClip != null)
		{
			ActiveAnimation animation = ActiveAnimation.Play(nextTarget, showAnimClip.name, AnimationOrTween.Direction.Forward, AnimationOrTween.EnableCondition.EnableThenPlay, AnimationOrTween.DisableCondition.DoNotDisable);

			nextPage.SetParameter(parameter);
			nextPage.OnAppear();
			
			animation.onFinished.Add(new EventDelegate(() =>			                                           {
				nextPage.OnAppeared();
			}));
		}

		activePageIndex = pageIndex;
	}

	#endregion


	#region Public Methods

	#endregion
}
