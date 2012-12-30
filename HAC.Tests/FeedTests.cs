using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace HAC.Tests
{
    public class FeedTests
    {
        [TestFixture]
        public class Tests
        {
            private string feedUrl = "http://newsrss.bbc.co.uk/";
            private string resource = "rss/sportonline_uk_edition/athletics/rss.xml";
            private string user = "";
            private string password = "";


            //[Test]
            //public void CanGetFeed()
            //{
            //    var getFeed = GetRssFeed.GetRssFeed.Get(feedUrl, resource);

            //    Assert.NotNull(getFeed);
            //    Assert.Greater(getFeed.Channels.Count, 0);
            //}

        }
    }
}
