using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using esquire.Helpers;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace esquire {
    class EsquireService {
        
        static public List<string> GetUrls(string url) {
            var request = WebRequest.Create(url);
            string html = string.Empty;
            using (WebResponse response = request.GetResponse()) {
                if (response != null)
                    using (var sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8)) {
                        html = sr.ReadToEnd();
                    }
            }
            var urls = new List<string>();
            if (!String.IsNullOrEmpty(html)) {
                var matchLinks = Regex.Matches(html, @"(?<=<a href="").+(?=""><h2>)", RegexOptions.IgnoreCase);
                foreach (Match m in matchLinks) {
                    urls.Add(m.Value);
                }
            }
            return urls.Take(10).ToList();
        }
        public static Rule CreateRule(string url) {
            var ruleItem = new Rule();
            var html = string.Empty;
            if (string.IsNullOrEmpty(url)) return null;

            try {
                var request = WebRequest.Create(url);
                using (WebResponse response = request.GetResponse()) {
                    if (response != null)
                        using (var sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8)) {
                            html = sr.ReadToEnd();
                        }
                }

                if (!string.IsNullOrEmpty(html)) {
                    var matches = Regex.Matches(html, @"(?<=<td><a href="")[\S]+(?="">&nbsp;</a></td>)", RegexOptions.IgnoreCase);
                    foreach (Match matchItem in matches) {
                        ruleItem.ImgUrls.Add(matchItem.Value);
                        ruleItem.Imgs.Add(WebRequest.Create(matchItem.Value).GetResponse().GetResponseStream().ToMemoryStream().GetBuffer());
                        
                    }

                    var match = Regex.Match(html, @"(?<=<div class=""short-info""><h1>).+(?=</h1><p>)", RegexOptions.IgnoreCase);
                    if (!string.IsNullOrEmpty(match.Value))
                        ruleItem.Author = match.Value;

                    match = Regex.Match(html, @"(?<=<div class=""short-info""><h1>.+<p>).+(?=</p><img src="")", RegexOptions.IgnoreCase);
                    if (!string.IsNullOrEmpty(match.Value))
                        ruleItem.Signature = match.Value;

                    ruleItem.Url = url;

                    match = Regex.Match(html, @"(?<=<div class=""bText""[^>]*>)([\s\S]*?)(?=</div>)", RegexOptions.IgnoreCase);
                    if (!string.IsNullOrEmpty(match.Value))
                        ruleItem.Text = match.Value;

                    ruleItem.Id = url.Split('/').Last();
                }
            }
            catch { }


            return ruleItem;
        }

        static public IEnumerable<Rule> GetRules(string url) {

            var urls = GetUrls(url);
            var rules = new BlockingCollection<Rule>();
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 10 };
            
            Parallel.ForEach(urls, parallelOptions, urlItem =>
             {
                 Console.WriteLine("get rule {0}", urlItem);
                 var rule = CreateRule(urlItem);
                 if (rule != null) rules.Add(rule);
             });

            return rules;

        }
    }
}
