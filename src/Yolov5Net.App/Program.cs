using System;
using System.Collections.Generic;
using System.Drawing;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;

namespace Yolov5Net.App
{
    class Program
    {
        static void Main(string[] args)
        {
            using var image = Image.FromFile("C:/Coding/201_SeamsModel/images/detect/ZH026_2080_049610_TX000079.png");

            using var scorer = new YoloScorer<YoloSeams5s>("C:/Coding/201_SeamsModel/runs/train/exp2/weights/best.onnx");

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

            image.Save("Assets/result1.jpg");
        }
    }
}
