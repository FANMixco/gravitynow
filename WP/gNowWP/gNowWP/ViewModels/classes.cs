using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gNowWP.ViewModels
{
    public class cleanString
    {
        private const string oldString = "<!-- www.1freehosting.com Analytics Code -->\r\n<noscript><a title=\"Pagerank SEO analytic\" href=\"http://www.1pagerank.com\">Pagerank SEO analytic</a></noscript>\r\n<script type=\"text/javascript\">\r\n\r\n  var _gaq = _gaq || [];\r\n  _gaq.push(['_setAccount', 'UA-21588661-2']);\r\n  _gaq.push(['_setDomainName', window.location.host]);\r\n  _gaq.push(['_setAllowLinker', true]);\r\n  _gaq.push(['_trackPageview']);\r\n\r\n  (function() {\r\n    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;\r\n    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';\r\n    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);\r\n\r\n    var fga = document.createElement('script'); fga.type = 'text/javascript'; fga.async = true;\r\n    fga.src = ('https:' == document.location.protocol ? 'https://www' : 'http://www') + '.1freehosting.com/cdn/ga.js';\r\n    var fs = document.getElementsByTagName('script')[0]; fs.parentNode.insertBefore(fga, fs);\r\n\r\n  })();\r\n</script>\r\n<!-- End Of Analytics Code -->";

        public cleanString()
        {

        }

        public string clean(string value)
        {
            return value.Replace(oldString, "");
        }
    }

    class gravityInfo
    {
        public int idgravity { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int altitude { get; set; }
        public double gravity { get; set; }
    }

    public class result
    {
        public string total { get; set; }
    }

    public class resultG
    {
        public int total { get; set; }
    }


    public class UserInfo
    {
        string iuser;
        bool metres;
        bool sync;
        int ndecimal;
        int cdecimal;
        public bool Metres
        {
            get { return metres; }
            set { metres = value; }
        }
        
        public bool Autosync
        {
            get { return sync; }
            set { sync = value; }
        }

        public string Iduser
        {
            get { return iuser; }
            set { iuser = value; }
        }

        public int Ndecimal
        {
            get { return ndecimal; }
            set { ndecimal = value; }
        }

        public int Cdecimal
        {
            get { return cdecimal; }
            set { cdecimal = value; }
        }
    }
}
