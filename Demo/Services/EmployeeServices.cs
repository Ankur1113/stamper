
using Demo.Exceptions;
using Demo.Interfaces;
using Demo.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using static Demo.Common.CommonEnum;
using Image = iTextSharp.text.Image;
using Demo.Common;
using Microsoft.AspNetCore.Hosting;
using iTextSharp.text.pdf.parser;
using System.Collections.Generic;
using Path = System.IO.Path;

namespace Demo.Services
{
    public class EmployeeServices : IEmployeeService
    {

        private readonly IDocuments _document;
        private readonly IWebHostEnvironment _appEnvironment;


        public EmployeeServices(IDocuments document, IWebHostEnvironment appEnvironment)
        {
            _document = document;
            _appEnvironment = appEnvironment;
        }
        public class RectAndText
        {
            public iTextSharp.text.Rectangle Rect;
            public String Text;
            public int pos;
            public RectAndText(iTextSharp.text.Rectangle rect, String text, int pos)
            {
                this.Rect = rect;
                this.Text = text;
                this.pos = pos;

            }
        }
        public class MyLocationTextExtractionStrategy : SimpleTextExtractionStrategy
        {
            //Hold each coordinate
            public List<RectAndText> myPoints = new List<RectAndText>();

            //The string that we're searching for
            public String TextToSearchFor { get; set; }

            //How to compare strings
            public System.Globalization.CompareOptions CompareOptions { get; set; }

            public MyLocationTextExtractionStrategy(String textToSearchFor, System.Globalization.CompareOptions compareOptions = System.Globalization.CompareOptions.IgnoreNonSpace)
            {
                this.TextToSearchFor = textToSearchFor;
                this.CompareOptions = compareOptions;
                this.iscomplete = false;
            }
            public int previouslength = 0;
            public bool iscomplete = false;
            //Automatically called for each chunk of text in the PDF
            public override void RenderText(TextRenderInfo renderInfo)
            {
                try
                {
                    base.RenderText(renderInfo);


                    //See if the current chunk contains the text

                    if (iscomplete == false && TextToSearchFor.Length >= renderInfo.GetText().Length)
                    {
                        var compar = this.TextToSearchFor.Substring(previouslength, renderInfo.GetText().Length);

                        var startPosition = System.Globalization.CultureInfo.CurrentCulture.CompareInfo.IndexOf(renderInfo.GetText(), compar, this.CompareOptions);

                        if (this.TextToSearchFor.Length > (previouslength + renderInfo.GetText().Length))
                        {
                            this.previouslength = this.previouslength + renderInfo.GetText().Length;
                        }
                        else
                        {
                            iscomplete = true;
                            this.previouslength = 0;
                        }

                        //If not found bail
                        if (startPosition < 0)
                        {
                            this.myPoints = new List<RectAndText>();
                            this.previouslength = 0;
                            return;
                        }


                        //Grab the individual characters
                        var chars = renderInfo.GetCharacterRenderInfos().Skip(startPosition).Take(compar.Length).ToList();

                        //Grab the first and last character
                        var firstChar = chars.First();
                        var lastChar = chars.Last();


                        //Get the bounding box for the chunk of text
                        var bottomLeft = firstChar.GetDescentLine().GetStartPoint();
                        var topRight = lastChar.GetAscentLine().GetEndPoint();

                        //Create a rectangle from it
                        var rect = new iTextSharp.text.Rectangle(
                                                                bottomLeft[Vector.I1],
                                                                bottomLeft[Vector.I2],
                                                                topRight[Vector.I1],
                                                                topRight[Vector.I2]
                                                                );

                        //Add this to our main collection
                        this.myPoints.Add(new RectAndText(rect, this.TextToSearchFor, previouslength));
                    }



                }
                catch (Exception ex)
                {


                }

            }
        }
        public async Task<string> pdffile(IFormFile pdffile)
        {
            string filepath = _document.UploadFile(pdffile, "uploads");

            try
            {
                string path = _document.GetPath(filepath);

                using (Stream inputPdfStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {

                    var reader = new PdfReader(inputPdfStream);

                    //  int counter = 1;

                    string vFileName = $"{pdffile.FileName.Substring(1, 5) + "_" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfffffff")}" + Path.GetExtension(pdffile.FileName);

                    string OutputFile = _document.GetPath("temp" + vFileName);

                    Stream outputPdfStream1 = new FileStream(OutputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    var stamper = new PdfStamper(reader, outputPdfStream1);




                    for (int pageNumber = 1; pageNumber <= reader.NumberOfPages; pageNumber++)
                    {
                        var pdfContentByte = stamper.GetOverContent(pageNumber);


                        var t = new MyLocationTextExtractionStrategy("Approved Stamp");
                        var ex = PdfTextExtractor.GetTextFromPage(reader, pageNumber, t);
                        float Rectleft = 0;
                        float Rectbutton = 0;

                        if (t.myPoints.Count() > 0)
                        {
                            var p = t.myPoints.FirstOrDefault();
                            Rectleft = p.Rect.Left;
                            foreach (var item in t.myPoints)
                            {
                                if (item.pos == 0)
                                {
                                    Rectbutton = item.Rect.Bottom;
                                    break;
                                }
                            }
                        }


                        var t1 = new MyLocationTextExtractionStrategy("Copy Stamp");
                        var ex1 = PdfTextExtractor.GetTextFromPage(reader, pageNumber, t1);

                        float RectleftCC = 0;
                        float RectbuttonCC = 0;

                        if (t1.myPoints.Count() > 0)
                        {
                            var p = t1.myPoints.FirstOrDefault();
                            RectleftCC = p.Rect.Left;
                            foreach (var item in t1.myPoints)
                            {
                                if (item.pos == 0)
                                {
                                    RectbuttonCC = item.Rect.Bottom;
                                    break;
                                }
                            }
                        }

                        var size = reader.GetPageSize(pageNumber);
                        string copyFile = _document.GetPath("img/copy.PNG");
                        string approveFile = _document.GetPath("img/approve.PNG");
                        using (FileStream fsCopy = File.Open(copyFile, FileMode.Open))
                        //using (FileStream fsCopy = File.Open(@"./wwwroot/img/copy.PNG", FileMode.Open))
                        using (FileStream fsApprove = File.Open(approveFile, FileMode.Open))
                        {
                            Image imageCopy = Image.GetInstance(fsCopy);
                            Image imageApprove = Image.GetInstance(fsApprove);

                            //var XIndentCopy = (size.Height - 211);
                            //var YIndentCopy = (size.Width - 120);

                            //var XIndentApprove = (size.Height - 516);
                            //var YIndentApprove = (size.Width - 120);

                            var XIndentCopy = (size.Height - 350);
                            var YIndentCopy = (size.Width + 100);

                            var XIndentApprove = (size.Height - 720);
                            var YIndentApprove = (size.Width + 100);

                            //imageCopy.SetAbsolutePosition(XIndentCopy, YIndentCopy);
                            //imageApprove.SetAbsolutePosition(XIndentApprove, YIndentApprove);

                            imageCopy.SetAbsolutePosition(RectleftCC, RectbuttonCC - 12);
                            imageApprove.SetAbsolutePosition(Rectleft, Rectbutton - 12);
                            if (Rectleft != 0 && Rectbutton != 0)
                            {
                                pdfContentByte.AddImage(imageApprove);
                            }
                            if (RectleftCC != 0 && RectbuttonCC != 0)
                            {
                                pdfContentByte.AddImage(imageCopy);
                            }



                        };


                    }
                    stamper.Close();
                    reader.Close();
                    outputPdfStream1.Close();
                    outputPdfStream1.Dispose();


                    var vMemoryStream = ConvertFileStream(OutputFile);
                    var bytes = vMemoryStream.ToArray();
                    string sBase64 = Convert.ToBase64String(bytes, 0, bytes.Length);
                    if (sBase64.Contains("77u/"))
                    {
                        sBase64 = sBase64.Replace("77u/", "");
                    }

                    _document.DeleteFile("Temp", vFileName);

                    return await Task.FromResult(sBase64);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private MemoryStream ConvertFileStream(string Filename)
        {
            var vMemoryStream = new MemoryStream();
            using (var vFileStream = new FileStream(Filename, FileMode.Open))
            {
                vFileStream.CopyTo(vMemoryStream);
            }
            vMemoryStream.Position = 0;

            return vMemoryStream;
        }

    }


}

