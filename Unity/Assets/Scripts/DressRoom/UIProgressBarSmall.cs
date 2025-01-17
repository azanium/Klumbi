using UnityEngine;
using System.Collections;

public class UIProgressBarSmall
{

    public static readonly UIProgressBarSmall Instance = new UIProgressBarSmall();
    private GUIStyle _styleLoadingBar;
    private GUIStyle _styleLoadingFrame;
    private GUIStyle _styleLoadingText;
    private GUIStyle _styleLoadingBarError;
	private GUIStyle _styleLoadingBarInfinity;


    public UIProgressBarSmall()
    {
        _styleLoadingBar = new GUIStyle();
        _styleLoadingBar.normal.background = ResourceManager.Instance.LoadTexture2D("GUI/HUD/loading_fill");

		_styleLoadingBarInfinity = new GUIStyle();
		_styleLoadingBarInfinity.normal.background = ResourceManager.Instance.LoadTexture2D("GUI/HUD/loading_fill_blue");

        _styleLoadingBarError = new GUIStyle();
        _styleLoadingBarError.normal.background = ResourceManager.Instance.LoadTexture2D("GUI/HUD/loading_fill_error");

        _styleLoadingFrame = new GUIStyle();
        _styleLoadingFrame.normal.background = ResourceManager.Instance.LoadTexture2D("GUI/HUD/loading_bg");

        _styleLoadingText = new GUIStyle();
        _styleLoadingText.font = ResourceManager.Instance.LoadFont("GUI/Fonts/DroidSans");
        _styleLoadingText.normal.textColor = Color.white;
    }

    // Update is called once per frame
    public void Update(float x, float y, float width, float height, string title, int progress, int fontSize)
    {
        DrawProgress(x, y, width, height, title, progress, fontSize);
    }

    void DrawProgress(float x, float y, float width, float height, string title, int progress, int fontSize)
    {
        Rect rect = new Rect(x, y, width, height);

        _styleLoadingText.fontSize = fontSize;

        // Draw the Title
        //GUI.Label(new Rect(rect.x, rect.y, rect.width, 25), string.Format("{0} {1}%", title, progress), _styleLoadingText);

        //Vector2 offset = _styleLoadingText.CalcSize(new GUIContent(title));

        // Draw the Loading Frame
        //GUI.Box(new Rect(rect.x, rect.y + offset.y + 2, rect.width, rect.height), "", _styleLoadingFrame);

        // Draw the Loading Bar
        GUI.BeginGroup(new Rect(rect.x, rect.y, ((float)progress / 100) * rect.width, rect.height));

        GUI.DrawTexture(new Rect(0, 0, rect.width, rect.height), _styleLoadingBar.normal.background);

        GUI.EndGroup();
    }

    public void UpdateError(float x, float y, float width, float height, int progress)
    {
        Rect rect = new Rect(x, y, width, height);
        
        // Draw the Loading Bar
        GUI.BeginGroup(new Rect(rect.x, rect.y, ((float)progress / 100) * rect.width, rect.height));

        GUI.DrawTexture(new Rect(0, 0, rect.width, rect.height), _styleLoadingBarError.normal.background);

        GUI.EndGroup();
    }

	float position = 0;
	bool moveRight = true;
	public void UpdateInfinite(float x, float y, float width, float height)
	{
		if (position + x >= Screen.width - width -5 && moveRight)
		{
			moveRight = false;
		}
		else if (position + x <= 5 && !moveRight)
		{
			moveRight = true;
		}
		if (moveRight) 
		{
			position = Mathf.Lerp(position, Screen.width - width, Time.deltaTime * 2);
		}
		else 
		{	
			position = Mathf.Lerp(position, 0, Time.deltaTime * 2);
		}
		Rect rect = new Rect(x + position, y, width, height);

		GUI.BeginGroup(rect);
		
		GUI.DrawTexture(new Rect(0, 0, rect.width, rect.height), _styleLoadingBarInfinity.normal.background);
		
		GUI.EndGroup();

	}
    
}
