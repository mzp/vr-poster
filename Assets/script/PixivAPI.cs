using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;


public class PixivAPI {

    private readonly Encoding UTF8 = Encoding.GetEncoding("UTF-8");
    public IEnumerable<String> Search(string keyword, int page = 1){
        string url = String.Format("http://spapi.pixiv.net/iphone/search.php?&s_mode=s_tag&word={0}&order=date&PHPSESSID=0&p={1}",
                                   keyword, page);
        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
        HttpWebResponse res = (HttpWebResponse)req.GetResponse();
        Stream str = res.GetResponseStream();
        StreamReader strr = new StreamReader(str, UTF8);
        string text = Regex.Replace(strr.ReadToEnd(), "\" \"", ",");


        return text.Split('\n')
                   .Select(x => SplitCsvLine(x))
                   .Where( x => x.Length > 9)
                   .Select(x => x[9]);
    }

    private string[] SplitCsvLine(string line)
	{
		return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
		@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
		System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
		select m.Groups[1].Value).ToArray();
	}
}
