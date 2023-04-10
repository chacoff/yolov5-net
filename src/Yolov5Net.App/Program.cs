using System;
using System.Collections.Generic;
using System.Drawing;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;
using System.Diagnostics;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Yolov5Net.App
{
    class Program
    {
        public static string onnx = "D:/Projects/201_SeamsModel/runs/train/exp3/weights/best_768.onnx";
        //public static string onnx = "C:\\Users\\jaime\\source\\repos\\yolov5-net\\src\\Yolov5Net.App\\Assets\\Weights\\yolov5n.onnx";
        public static string input_img = "C:\\Users\\jaime\\source\\repos\\yolov5-net\\img\\ZH026_2080_049670_TX000077.png"; //"D:/Projects/201_SeamsModel/images/detect/ZH026_2080_049610_TX000079.png";
        public static string output_img = "C:\\Users\\jaime\\source\\repos\\yolov5-net\\img\\result3_768.jpg";
        
        static void Main(string[] args)
        {
            Predictions();
            Benchmarks();
        }

        private static void Predictions()
        {
            var timer = new Stopwatch();
            
            using var image = Image.FromFile(input_img);

            //using var scorer = new YoloScorer<YoloSeams5s>(onnx);
            using var scorer = new YoloScorer<YoloSeams5s_768>(onnx);

            timer.Start();
            List<YoloPrediction> predictions = scorer.Predict(image);
            timer.Stop();

            using var graphics = Graphics.FromImage(image);

            foreach (var prediction in predictions) // iterate predictions to draw results
            {
                double score = Math.Round(prediction.Score, 2);
                string _class = prediction.Label.Name.ToString();
                Console.WriteLine($"prediction: {_class} - {score}%");

                graphics.DrawRectangles(new Pen(prediction.Label.Color, 1), new[] { prediction.Rectangle });

                var (x, y) = (prediction.Rectangle.X - 3, prediction.Rectangle.Y - 23);

                graphics.DrawString($"{prediction.Label.Name} ({score})", 
                    new Font("Consolas", 26, GraphicsUnit.Pixel), new SolidBrush(prediction.Label.Color), 
                    new PointF(x, y));
            }

            image.Save(output_img);
            
            Console.WriteLine("Time taken: {0}ms ", timer.ElapsedMilliseconds.ToString());
        }

        private static void Benchmarks()
        {
            var sw = new Stopwatch();
            using (var image = Image.FromFile(input_img))
            using (var scorer = new YoloScorer<YoloSeams5s_768>(onnx))
            {
                List<long> stats = new List<long>();

                for (int i = 0; i < 100; i++)
                {
                    sw.Restart();
                    scorer.Predict(image);
                    long fps = 1000 / sw.ElapsedMilliseconds;
                    stats.Add(fps);
                    sw.Stop();
                }

                stats.Sort();
                Console.WriteLine($@"
                    Max FPS: {stats[stats.Count - 1]}
                    Avg FPS: {Avg(stats)}
                    Min FPS: {stats[0]}
                    Time ms: {sw.ElapsedMilliseconds.ToString()}
                ");
            }
        }

        private static int Avg(List<long> stats)
        {
            long sum = 0;
            foreach (long i in stats)
            {
                sum += i;
            }
            return (int)(sum / stats.Count);
        }
    }
}
