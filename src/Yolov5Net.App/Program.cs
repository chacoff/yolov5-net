using System;
using System.Collections.Generic;
using System.Drawing;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;
using System.Diagnostics;

namespace Yolov5Net.App
{
    class Program
    {
        public static string onnx = "C:/Coding/201_SeamsModel/runs/train/exp2/weights/best.onnx";
        public static string input_img = "C:/Coding/201_SeamsModel/images/detect/ZH026_2080_049610_TX000079.png";
        public static string output_img = "Assets/result1.jpg";
        static void Main(string[] args)
        {
            var timer = new Stopwatch();
            
            timer.Start();
            using var image = Image.FromFile(input_img);

            using var scorer = new YoloScorer<YoloSeams5s>(onnx);

            List<YoloPrediction> predictions = scorer.Predict(image);

            using var graphics = Graphics.FromImage(image);

            foreach (var prediction in predictions) // iterate predictions to draw results
            {
                double score = Math.Round(prediction.Score, 2);
                string _class = prediction.Label.Name.ToString();
                Console.WriteLine($"prediction: {_class} - {score}%");

                graphics.DrawRectangles(new Pen(prediction.Label.Color, 1),
                    new[] { prediction.Rectangle });

                var (x, y) = (prediction.Rectangle.X - 3, prediction.Rectangle.Y - 23);

                graphics.DrawString(
                    $"{prediction.Label.Name} ({score})",
                    new Font("Consolas", 26, GraphicsUnit.Pixel), new SolidBrush(prediction.Label.Color),
                    new PointF(x, y)
                    );
            }

            image.Save(output_img);
            timer.Stop();
            Console.WriteLine("Time taken: {0}ms ", timer.ElapsedMilliseconds.ToString());
        }
    }
}
