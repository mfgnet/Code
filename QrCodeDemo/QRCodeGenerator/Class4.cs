using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using ImageMagick;

namespace QRCodeGenerator
{
    public class GeneratorWithLogo
    {
        public void Generate(IEnumerable<CodeInfo> infos)
        {
            
                foreach (CodeInfo codeInfo in infos)
                {using (var ms = new MemoryStream())
            {
                    bool result = GetQRCode(codeInfo.Content, codeInfo.FileName, codeInfo.BackgroundPath, codeInfo.Size, codeInfo.X, codeInfo.Y, ms, codeInfo.CenterImagePath, codeInfo.CenterImgSize);


                }


            }


        }

        public bool GetQRCode(string strContent, string path, string backgroundPath, int size, int x, int y, MemoryStream ms, string userFace, int centerImgSize)
        {
            ErrorCorrectionLevel Ecl = ErrorCorrectionLevel.M; //误差校正水平   
            string Content = strContent;//待编码内容  
            QuietZoneModules QuietZones = QuietZoneModules.Two;  //空白区域   
            int ModuleSize = 12;//大小  
            ISizeCalculation calculation = new FixedCodeSize(size, QuietZoneModules.Two);
            var encoder = new QrEncoder(Ecl);
            QrCode qr;
            if (encoder.TryEncode(Content, out qr))//对内容进行编码，并保存生成的矩阵  
            {
                var render = new GraphicsRenderer(calculation);
                render.WriteToStream(qr.Matrix, ImageFormat.Png, ms);

              //  using (FileStream fs = new FileStream(backgroundPath, FileMode.Open))
                {
                    using (MagickImage image = new MagickImage(backgroundPath))
                    {
                        using (MagickImage image1 = new MagickImage(userFace))
                        {
                            Bitmap ercode = new Bitmap(ms);
                            Bitmap background = image.ToBitmap();
                            Bitmap logo = image1.ToBitmap();
                            Graphics graphics = Graphics.FromImage(ercode);
                            graphics.DrawImage(logo, new Rectangle((size - centerImgSize) / 2,
                                (size - centerImgSize) / 2, centerImgSize, centerImgSize));
                            Graphics graphics1 = Graphics.FromImage(background);
                           
                            graphics1.DrawImage(ercode,x,y);
                            background.Save(path);
                        }
                    }
                }


            }
            else
            {
                return false;
            }
            return true;
        }

    }
}
