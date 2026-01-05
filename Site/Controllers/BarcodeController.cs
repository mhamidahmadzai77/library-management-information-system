using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.QrCode;

namespace Site.Controllers
{
    public class BarcodeController : Controller
    {
        // GET: Barcode
        public ActionResult Index(string id)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 50, // Adjust the height of the barcode image
                    Width = 150   // Adjust the width of the barcode image
                }
            };

            var barcodeBitmap = barcodeWriter.Write(id);

            using (MemoryStream stream = new MemoryStream())
            {
                barcodeBitmap.Save(stream, ImageFormat.Png);
                byte[] byteImage = stream.ToArray();

                return File(byteImage, "image/png");
            }
        }
    }
}