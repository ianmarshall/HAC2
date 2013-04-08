using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EntityBase
/// </summary>
public class PGEntityBase
{
    public T ReadFromXmlCache<T>(string filename)
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException("File not found", filename);

        using (System.IO.StreamReader reader = new System.IO.StreamReader(filename))
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            return (T)s.Deserialize(reader);
        }
    }


    public void WriteToXmlCache<T>(string filename)
    {
        using (System.IO.StreamWriter output = new System.IO.StreamWriter(filename))
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            s.Serialize(output, this);
        }
    }


    public string ToXml<T>()
    {
        using (System.IO.StringWriter output = new System.IO.StringWriter(new System.Text.StringBuilder()))
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            s.Serialize(output, this);
            return output.ToString();
        }
    }



    
    public PGEntityBase()
	{

	}
}
