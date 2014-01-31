using System;

namespace Richard {
	static partial class ExtensionsString {
		public static string AsString<T>(this System.Collections.Generic.IEnumerable<T> e) {
			if (e==null) {
				return null;
			}
			string Result="";
			foreach (var v in e) {
				Result=((Result=="")?"":Result+", ")+v;
			}
			return Result;
		}

		public static Tuple<bool,string> WithStripedLeaderOf(this string text, string leader) {
			var startIndex=text.StartsWith(leader)?leader.Length-1:0;
			var name=text.Substring(startIndex);
			var hasLeader=startIndex>0;
			return new Tuple<bool,string>(hasLeader, name);
		}
	}
}
