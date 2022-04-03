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
using CikguHub.Document;
using CikguHub.Data;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using System.Linq;

namespace CikguHub.Console
{
    public static class DocumentTest
    {
        public static void Run()
        {
            //WebClient client = new WebClient();
            //client.DownloadFile(@"https://fastlaw.blob.core.windows.net/templates/rentaldeposit-letterofdemand.docx", "template.docx");

            var source = Package.Open("templates/certificate2.docx");
            var document = WordprocessingDocument.Open(source);

            var content = document.MainDocumentPart.GetXDocument().Descendants(W.p);
            var regex = new Regex(@"\[FullName\]", RegexOptions.IgnoreCase);
            int count = OpenXmlRegex.Replace(content, regex, "Andrew Yong Gin Whai", null);

            document.MainDocumentPart.PutXDocument();

            //int imageCounter = 0;
            //string imageDirectoryName = "img";
            //WmlToHtmlConverterSettings settings = new WmlToHtmlConverterSettings()
            //{
            //    FabricateCssClasses = true,
            //    CssClassPrefix = "cls-",
            //    RestrictToSupportedLanguages = false,
            //    RestrictToSupportedNumberingFormats = false,
            //    ImageHandler = imageInfo =>
            //    {
            //        DirectoryInfo localDirInfo = new DirectoryInfo(imageDirectoryName);
            //        if (!localDirInfo.Exists)
            //        {
            //            localDirInfo.Create();
            //        }

            //        ++imageCounter;
            //        string extension = imageInfo.ContentType.Split('/')[1].ToLower();
            //        ImageFormat imageFormat = null;
            //        if (extension == "png")
            //        {
            //            extension = "jpeg";
            //            imageFormat = ImageFormat.Jpeg;
            //        }
            //        else if (extension == "bmp")
            //        {
            //            imageFormat = ImageFormat.Bmp;
            //        }
            //        else if (extension == "jpeg")
            //        {
            //            imageFormat = ImageFormat.Jpeg;
            //        }
            //        else if (extension == "tiff")
            //        {
            //            imageFormat = ImageFormat.Tiff;
            //        }

            //        // If the image format is not one that you expect, ignore it,
            //        // and do not return markup for the link.
            //        if (imageFormat == null)
            //        {
            //            return null;
            //        }

            //        string imageFileName = imageDirectoryName + "/image" + imageCounter.ToString() + "." + extension;

            //        try
            //        {
            //            imageInfo.Bitmap.Save(imageFileName, imageFormat);
            //        }
            //        catch (System.Runtime.InteropServices.ExternalException)
            //        {
            //            return null;
            //        }

            //        XElement img = new XElement(Xhtml.img, new XAttribute(NoNamespace.src, imageFileName), imageInfo.ImgStyleAttribute, imageInfo.AltText != null ? new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
            //        return img;
            //    }
            //};

            HtmlConverterSettings settings = new HtmlConverterSettings();
            XElement html = HtmlConverter.ConvertToHtml(document, settings);

            //XElement logo = new XElement("img");
            //logo.SetAttributeValue("src", "https://app.fastlaw.my/assets/media/logos/logo-letter-13.png");
            //logo.SetAttributeValue("style", "margin-top:-50%;width:100%");
            //html.Add(logo);

            //html.Element.body;
            //var body = html.Elements("body");

            var body = html.FirstNode.ElementsAfterSelf().First();
            body.SetAttributeValue("style", "background-image: url(https://cikguhub.blob.core.windows.net/templates/certificate.png)");

            System.Console.WriteLine(html.ToString());
            var writer = File.CreateText("test.html");
            writer.WriteLine(html.ToString());
            writer.Dispose();
            //System.Console.ReadLine();

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings() { Top = 10 }
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = File.ReadAllText("test.html"),
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                        FooterSettings = { FontSize = 9, Right = "Page [page] of [toPage]" }
                    }
                }
            };

            var converter = new SynchronizedConverter(new PdfTools());
            byte[] pdf = converter.Convert(doc);

            File.WriteAllBytes("output.pdf", pdf);
        }

        public static void Run1()
        {
            var converter = new SynchronizedConverter(new PdfTools());
            DocumentService documentService = new DocumentService(converter);

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Data Source=fastlaw.database.windows.net;Initial Catalog=fastlaw-prod;User ID=fastlaw;Password=HappyClient5;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

            ApplicationDbContext context = new ApplicationDbContext(optionsBuilder.Options);

            //byte[] pdf = documentService.GeneratePdfFromModel(new CaseRenterDeposit());
            //File.WriteAllBytes("docservice.pdf", pdf);
        }
    }
}
