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
	}
}

