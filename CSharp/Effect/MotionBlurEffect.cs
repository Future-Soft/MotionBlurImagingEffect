using Microsoft.Xna.Framework;
using Nokia.Graphics.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
    public class MotionBlurEffect : CustomEffectBase
    {
        private float angle = 0;
        private float distance = 3000.0f;
        private float zoom = 1.0f;
        private float rotation = (float)(30.0f * Math.PI / 180f);
        private bool wrapEdges = false;

        public MotionBlurEffect(IImageProvider source):
            base(source, false)
        {
        }
        public MotionBlurEffect(double f = 2.0) :
            base(null, false)
        {
        }

        int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        protected override void OnProcess(PixelRegion sourcePixelRegion, PixelRegion targetPixelRegion)
        {
            float centreX = 0.5f;
            float centreY = 0.5f;
            float cx = (float)sourcePixelRegion.Bounds.Width * centreX;
            float cy = (float)sourcePixelRegion.Bounds.Height * centreY;
            float imageRadius = (float)Math.Sqrt(cx * cx + cy * cy);
            
            int height = (int)sourcePixelRegion.Bounds.Height;

            sourcePixelRegion.ForEachRow((index, width, pos) =>
            {
                for (int x = 0; x < width; ++x, ++index)
                {
                    float translateX = (float)(distance * Math.Cos(angle));
                    float translateY = (float)(distance * -Math.Sin(angle));
                    float scale = zoom;
                    float rotate = rotation;
                    float maxDistance = distance + Math.Abs(rotation * imageRadius) + zoom * imageRadius;
                    int steps = (int)Math.Log(maxDistance, 2);

                    translateX /= maxDistance;
                    translateY /= maxDistance;
                    scale /= maxDistance;
                    rotate /= maxDistance;
                    int y = index / width;



                    Matrix t;
                    Vector3 p = new Vector3();
                    uint temptemp = 0;
                    uint a = 0, r = 0, g = 0, b = 0;
                    int count = 0;
                    for (int i = 0; i < steps; i++)
                    {
                        int newX = x, newY = y;
                        float f = (float)i / steps;
                        p.X = x;
                        p.Y = y;
                        p.Z = 0;
                        t = Matrix.Identity;
                        t = Matrix.Multiply(t, Matrix.CreateTranslation(- cx + f * translateX, - cy + f * translateY, 0));

                        float s = scale * f;
                        t = Matrix.Multiply(t, Matrix.CreateScale(s,s,0));
                        if (rotate != 0)
                        {
                            t = Matrix.Multiply(t, Matrix.CreateRotationZ(-rotate * f));
                        }
                        t = Matrix.Multiply(t, Matrix.CreateTranslation(cx, cy, 0));
                        p = Vector3.Transform(p, t);
                        newX = (int)p.X;
                        newY = (int)p.Y;

                        if (newX < 0 || newX >= width)
                        {
                            if (wrapEdges)
                                newX = mod(newX, width);
                            else
                                break;
                        }
                        if (newY < 0 || newY >= height)
                        {
                            if (wrapEdges)
                                newY = mod(newY, height);
                            else
                                break;
                        }

                        count++;
                        uint rgb = temptemp = sourcePixelRegion.ImagePixels[newY * width + newX];
                        a += (rgb >> 24) & 0xff;
                        r += (rgb >> 16) & 0xff;
                        g += (rgb >> 8) & 0xff;
                        b += rgb & 0xff;
                        translateX *= 2;
                        translateY *= 2;
                        scale *= 2;
                        rotate *= 2;
                    }
                    if (count == 0)
                    {
                        targetPixelRegion.ImagePixels[index] = sourcePixelRegion.ImagePixels[index];
                    }
                    else
                    {
                        a = (uint)Math.Max(0, Math.Min(255, ((int)(a / (float)count))));
                        r = (uint)Math.Max(0, Math.Min(255, ((int)(r / (float)count))));
                        g = (uint)Math.Max(0, Math.Min(255, ((int)(g / (float)count))));
                        b = (uint)Math.Max(0, Math.Min(255, ((int)(b / (float)count))));
                        uint temp = (a << 24) | (r << 16) | (g << 8) | b;
                        targetPixelRegion.ImagePixels[index] = temp;
                    }
                }
            });
        }



    }
}
