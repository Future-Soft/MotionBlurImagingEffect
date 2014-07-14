using Nokia.Graphics.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
    public class MyEffect : CustomEffectBase
    {
        private float borderWidth = 0.4f;
        private int borderColor = 0x000000;

        double factor;
        public MyEffect(IImageProvider source, double f = 2.0):
            base(source, false)
        {
            factor = f;
        }
        public MyEffect( double f = 2.0) :
            base(null, false)
        {
            factor = f;
        }

        public uint getPixel(int x, int y, uint[] inPixels, int width, int height)
        {
            /*float nx = m00 * x + m01 * y;
            float ny = m10 * x + m11 * y;
            nx /= scale;
            ny /= scale * stretch;
            nx += 1000;
            ny += 1000;	// Reduce artifacts around 0,0
            float f = evaluate(nx, ny);

            float f1 = results[0].distance;
            float f2 = results[1].distance;
            int srcx = ImageMath.clamp((int)((results[0].x - 1000) * scale), 0, width - 1);
            int srcy = ImageMath.clamp((int)((results[0].y - 1000) * scale), 0, height - 1);
            int v = inPixels[srcy * width + srcx];
            f = (f2 - f1) / edgeThickness;
            f = ImageMath.smoothStep(0, edgeThickness, f);
            if (fadeEdges)
            {
                srcx = ImageMath.clamp((int)((results[1].x - 1000) * scale), 0, width - 1);
                srcy = ImageMath.clamp((int)((results[1].y - 1000) * scale), 0, height - 1);
                int v2 = inPixels[srcy * width + srcx];
                v2 = ImageMath.mixColors(0.5f, v2, v);
                v = ImageMath.mixColors(f, v2, v);
            }
            else
                v = ImageMath.mixColors(f, edgeColor, v);
            return v;*/
            return 0;
        }


        protected override void OnProcess(PixelRegion sourcePixelRegion, PixelRegion targetPixelRegion)
        {
            sourcePixelRegion.ForEachRow((index, width, pos) =>
            {
                for (int x = 0; x < width; ++x, ++index)
                {
                    /*uint color = sourcePixelRegion.ImagePixels[index];

                    // Extract color channel values 
                    var a = (byte)((color >> 24) & 255);
                    var r = (byte)((color >> 16) & 255);
                    var g = (byte)((color >> 8) & 255);
                    var b = (byte)((color) & 255);

                    r = (byte)Math.Min(255, r * factor);
                    g = (byte)Math.Min(255, g * factor);
                    b = (byte)Math.Min(255, b * factor);

                    // Combine modified color channels 
                    var newColor = (uint)(b | (g << 8) | (r << 16) | (a << 24));
                    
                    targetPixelRegion.ImagePixels[index] = newColor;
                    */
                    targetPixelRegion.ImagePixels[index] = getPixel(0, 0, sourcePixelRegion.ImagePixels, (int)(sourcePixelRegion.Bounds.Width), (int)(sourcePixelRegion.Bounds.Height));
                }
            }); 
        }
    }
}
