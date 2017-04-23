using CafeT.SmartCrawler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace GenericSerialize
{

    public class SimpleClass
    {

        const string FileSavePath  = @"c:\data\SimpleClass.xml";

        public Guid ID { get; set; }
        public string CityName { get; set; }
        public int Rank { get; set; }
        public bool Active { get; set; }
        public List<string> RandomList { get; set; }

        public SimpleClass()
        {
            ID = Guid.NewGuid();
            RandomList = new List<string>();
        }

        public void BasicSave()
        {
            var xs = new XmlSerializer(typeof(SimpleClass));
            using (TextWriter sw = new StreamWriter(FileSavePath))
            {
                xs.Serialize(sw, this);
            }
        }

        public void BasicLoad()
        {
            var xs = new XmlSerializer(typeof(SimpleClass));
            using (var sr = new StreamReader(FileSavePath))
            {
                var tempObject = (SimpleClass)xs.Deserialize(sr);
                ID = tempObject.ID;
                CityName = tempObject.CityName;
                Rank = tempObject.Rank;
                Active = tempObject.Active;
            }
        }

        public bool SaveGeneric1(string fileName)
        {
            return GenericUtils.Save<SimpleClass>(fileName, this);
        }

        public void LoadGeneric1(string fileName)
        {
            var fileData = GenericUtils.Load<SimpleClass>(fileName);
            ID = fileData.ID;
            CityName = fileData.CityName;
            Rank = fileData.Rank;
            Active = fileData.Active;   
        }


        public bool SaveGeneric2(string fileName)
        {
            return GenericUtils.Save2(fileName, this);
        }

        public void LoadGeneric2(string fileName)
        {
            var obj = GenericUtils.Load2(fileName);
            // TODO .. show implement of reflection call to deep copy object onto self
        }

    }
}








