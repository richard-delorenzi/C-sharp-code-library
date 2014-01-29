using System;
using Newtonsoft.Json.Linq;

namespace Richard.Json
{
	public static partial class JsonExtensions
	{
		public static string ToJsonString(this object o)
		{
			return o==null?null:JObject.FromObject(o).ToString();
		}

		public static string ToJsonString(this JObject o){
			return o==null?null:o.ToString();
		}

		public static string AsString(this JObject o, string key)
		{
			var v = o[key];
			return (v==null)?null:v.ToString();
		}
	}
}

