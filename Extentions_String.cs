using System;

namespace Richard.String {

	public static class ExtensionsString {
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

		public class ReturnedTextWithStripedLeader{
			internal ReturnedTextWithStripedLeader(string text, bool hasLeader){Text=text; HadLeader=hasLeader;}
			public string Text;
			public bool HadLeader;
		}
		public static ReturnedTextWithStripedLeader WithStripedLeaderOf(this string text, string leader) {
			var startIndex=text.StartsWith(leader)?leader.Length:0;
			var name=text.Substring(startIndex);
			var hasLeader=startIndex>0;
			return new ReturnedTextWithStripedLeader(name, hasLeader);
		}
	}

}
