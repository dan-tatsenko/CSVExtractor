using System;
using System.Text;
using System.Xml;
using System.IO;

namespace AgilityCIS.Data.CSVExtractor
{
    public class IntervalDataExtractor
    {
        XmlDocument _xmlDocument;
        public string SourceFile { get; set; }
        public string OutputDirectory { get; set; }

        public IntervalDataExtractor(string sourceFile, string outputDirectory)
        {
            SourceFile = sourceFile;
            OutputDirectory = outputDirectory;
            _xmlDocument = new XmlDocument();
        }

        public int SplitIntoFiles()
        {
            try
            {
                _xmlDocument.Load(SourceFile);
                
                //Creates output directory, if it does not exist
                if (!Directory.Exists(OutputDirectory))
                {
                    Directory.CreateDirectory(OutputDirectory);
                }
                
                XmlNode transactions = _xmlDocument.DocumentElement.FirstChild.NextSibling;
                int i = 0;
                foreach (XmlNode t in transactions)
                {
                    string out_filename = t.Attributes.GetNamedItem("transactionID").Value + ".csv";
                    FileStream f = File.Create(OutputDirectory + out_filename);
                    f.Close();
                    StreamWriter sw = new StreamWriter(OutputDirectory + out_filename, false, Encoding.Default);
                    sw.WriteLine(t.FirstChild.ChildNodes[0].InnerText);
                    sw.Dispose();
                    i++;
                }
                return i;
            }
            catch (FileNotFoundException)
            {
                //If source file does not exist, then return error message
                return -1;
            }
        }
    }
}
