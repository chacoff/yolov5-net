using System.Collections.Generic;
using System.Drawing;
using Yolov5Net.Scorer.Models.Abstract;

namespace Yolov5Net.Scorer.Models
{
    public class YoloSeams5s : YoloModel
    {
        public override int Width { get; set; } = 768;
        public override int Height { get; set; } = 768;
        public override int Depth { get; set; } = 3;

        public override int Dimensions { get; set; } = 7; // number of classes + 5

        public override int[] Strides { get; set; } = new int[] { 8, 16, 32 }; // P3-P5

        public override int[][][] Anchors { get; set; } = new int[][][]
        {
            new int[][] { new int[] { 006, 056 }, new int[] { 008, 106 }, new int[] { 009, 181 } },
            new int[][] { new int[] { 010, 226 }, new int[] { 011, 299 }, new int[] { 013, 413 } },
            new int[][] { new int[] { 018, 464 }, new int[] { 413, 561 }, new int[] { 513, 560 } }
        };

        public override int[] Shapes { get; set; } = new int[] { 96, 48, 24 }; // input size divided by the strides

        public override float Confidence { get; set; } = 0.15f;
        public override float MulConfidence { get; set; } = 0.15f;
        public override float Overlap { get; set; } = 0.10f;

        public override string[] Outputs { get; set; } = new[] { "output" };

        public override List<YoloLabel> Labels { get; set; } = new List<YoloLabel>()
        {
            new YoloLabel { Id = 1, Name = "Seams", Color = Color.DeepPink},
            new YoloLabel { Id = 2, Name = "Beam", Color = Color.Yellow}
        };

        public override bool UseDetect { get; set; } = true;

        public YoloSeams5s()
        {

        }
    }
}
