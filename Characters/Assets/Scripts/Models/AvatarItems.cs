using PB.Client;
using System.Collections;
using System.Collections.Generic;


public class AvatarItems : JsonModel<AvatarItems>
{
    public class AvatarItem
    {
        public string tipe { get; set; }
        public string title { get; set; }
        public string picture { get; set; }
        public string action { get; set; }
        public string value { get; set; }

    }

	public bool success { get; set; }
	public bool logged_in { get; set; }
    public int count { get; set; }
    public List<AvatarItem> data { get; set; }

    public static string GetFeaturesApi(string email, string part, int start, int limit)
    {
        return string.Format("{0}api/mobile/feature/list/{1}/{2}/{3}/{4}", PopBloopSettings.WebServerUrl, email, part, start, limit);
    }

	public static string GetFeaturesApiV2(string email, string part, int start, int limit)
	{
		Dictionary<string, string> param = new Dictionary<string, string>()
		{
			{ "func", part },
			{ "email", email },
			{ "start", string.Format("{0}", start) },
			{ "limit", string.Format("{0}", limit) }
		};
		return string.Format("{0}feature/index?{1}", Api.getApiUrl(), Api.GenerateApiParams(param));
	}

    public static string GetImagePath(string image)
    {
        return string.Format("{0}bundles/preview_images/{1}", PopBloopSettings.WebServerUrl, image);
    }

    public static string GetStylesApi(string email, string part, int start, int limit)
    {
        return string.Format("{0}api/mobile/style/list/{1}/{2}/{3}/{4}", PopBloopSettings.WebServerUrl, email, part, start, limit);
    }

	public static string GetStylesApiV2(string email, string part, string search_key, string sort, int start, int limit)
	{
		Dictionary<string, string> param = new Dictionary<string, string>()
		{
			{ "func", part },
			{ "email", email },
			{ "keysearch", search_key },
			{ "sort", sort },
			{ "start", string.Format("{0}", start) },
			{ "limit", string.Format("{0}", limit) }
		};
		return string.Format("{0}style/index?{1}", Api.getApiUrl(), Api.GenerateApiParams(param));
	}
}
