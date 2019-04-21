using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TheCrawler
{
    public enum Algos
    {
        OiganBet,
        CristiBet
    }

    public partial class Ops
    {
        public List<League> lglist;
        public List<League> playedmatches;
        public ChromeDriver browser;
        public ChromeOptions options;
        public StreamWriter writerToFile;
        public XmlSerializer serializer;
        public int windowsWait;
        public int triesNumber;
        public string filename;

        public Ops()
        {
            lglist = new List<League>();
            playedmatches = new List<League>();
            serializer = new XmlSerializer(typeof(List<League>));
            options = new ChromeOptions();
            //options.AddArgument("headless");
            windowsWait = 5000;
            triesNumber = 10;
        }
    }
}
