using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareTwoPictures
{
    public class ImageComporator
    {
        public Bitmap Img1 { get; set; }
        public Bitmap Img2 { get; set; }
        public List<Rectangle> Areas { get; set; }


        private List<Point> _exceptedPoints;
        private Queue<Point> _checkingQueue;
        private List<List<Point>> _areas;

        public ImageComporator(Bitmap img1, Bitmap img2)
        {
            this.Img1 = img1;
            this.Img2 = img2;
        }
        public void Compare()
        {
            _exceptedPoints = new List<Point>();
            double acceptableDelta = 255 * 3 * 0.1;

            int width = Math.Min(Img1.Width, Img2.Width);
            int height = Math.Min(Img1.Height, Img2.Height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var color1 = Img1.GetPixel(i, j);
                    var color2 = Img2.GetPixel(i, j);

                    if (Math.Abs(GetSum(color1) - GetSum(color2)) > acceptableDelta)
                    {
                        _exceptedPoints.Add(new Point(i, j));
                    }

                }
            }

            FindAreas();
        }

        private void FindAreas()
        {
            int currentArea = -1;
            _checkingQueue = new Queue<Point>();
            _areas = new List<List<Point>>();

            while (_exceptedPoints.Count != 0 || _checkingQueue.Count != 0)
            {

                if (_checkingQueue.Count == 0)
                {
                    var currentPoint = _exceptedPoints[0];
                    _exceptedPoints.Remove(currentPoint);
                    _checkingQueue.Enqueue(currentPoint);


                    currentArea++;

                    _areas.Add(new List<Point>());
                }
                else
                {
                    var currentPoint = _checkingQueue.Dequeue();


                    _areas[currentArea].Add(currentPoint);


                    var nearPoints = _exceptedPoints.Where(x => GetDistance(currentPoint, x) < 10).ToArray();
                    foreach (var i in nearPoints)
                    {
                        _checkingQueue.Enqueue(i);
                        _exceptedPoints.Remove(i);
                    }
                }

            }
            Rect();
        }

        private void Rect()
        {
            Areas = new List<Rectangle>();
            foreach (var i in _areas)
            {
                var minX = i.Min(x => x.X);
                var minY = i.Min(x => x.Y);


                var maxX = i.Max(x => x.X);
                var maxY = i.Max(x => x.Y);
                Areas.Add(new Rectangle(minX - 5, minY - 5, maxX - minX + 5, maxY - minY + 5));
            }
        }


        private double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }


        private int GetSum(Color color)
        {
            return color.R + color.G + color.B;
        }


    }
}
