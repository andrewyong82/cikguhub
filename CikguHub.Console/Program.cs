using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using Azure.AI.FormRecognizer.Training;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using System.IO.Packaging;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System.Xml.Linq;
using DinkToPdf;
using System.Text.RegularExpressions;
using System.Net;

namespace CikguHub.Console
{
    class Program
    {

        static void Main(string[] args)
        {
            //ParserTest.Run();
            DocumentTest.Run();
        }

    }
}
