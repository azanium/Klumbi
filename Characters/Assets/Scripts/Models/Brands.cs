using PB.Client;
using System.Collections;
using System.Collections.Generic;

public class Brands : JsonModel<Brands>
{
    public class Brand
    {
        public string id { get; set; }
		public string brand_id { get; set; }
        public string title { get; set; }
        public string website { get; set; }
		public bool backgroundcolor { get; set; }
		public bool textcolor { get; set; }
		public string description { get; set; }
        public string picture { get; set; }
		public string banner { get; set; }
        public string action { get; set; }
        public string value { get; set; }
    }

    public bool success { get; set; }
	public bool logged_in { get; set; }
    public List<Brand> data { get; set; }

    public static string GetBrandListApi(int start, int limit)
    {
        return string.Format("{0}api/mobile/store/brandlist/{1}/{2}", PopBloopSettings.WebServerUrl, start, limit);
    }

	public static string GetBrandList(int start, int limit)
	{
		return string.Format("{0}store/list_data", Api.getApiUrl());
	}

	public static string GetBrandItemIndex(string email, string func, string brand_id, int start, int limit, string sort = "asc")
	{
		Dictionary<string, string> param = new Dictionary<string, string>() 
		{
			{ "email", email },
			{ "func", func },
			{ "brand_id", brand_id },
			{ "sort", sort }
		};

		return string.Format("{0}store?{1}", Api.getApiUrl(), Api.GenerateApiParams(param));
	}
}
