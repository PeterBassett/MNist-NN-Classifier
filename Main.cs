using MNistClassifier.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MNistClassifier
{
    public partial class Main : Form
    {
        private MNistDataLoader.MNistData mnist;
        private INeuralNetwork network;

        private int trainingImageIndex = 0;
        private int testingImageIndex = 0;
        private int trainedAgainstImageCount = 0;
        private int totalTestsPerformed = 0;
        private int totalTestsCorrect = 0;

        private Bitmap currentTrainingImage;
        private Bitmap currentUserImage;
        private Bitmap currentStandardisedImage;

        private bool userHasDrawingToTest = false;
        private const int inputCount = 784;
        private const int hiddenCount = 64;
        private const int outputCount = 10;
        private const int TrainingImageWidthHeight = 28;
        private bool currentlyDrawing = false;
        private readonly Pen userDrawingPen = new Pen(Color.White, 12);
        private readonly List<Point> segments = new List<Point>();

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (!MNistDataLoader.CheckFilesExist())
            {
                this.Close();
                return;
            }

            network = CreateNetwork("NeuralNetwork");

            currentTrainingImage = new Bitmap(TrainingImageWidthHeight, TrainingImageWidthHeight);
            currentUserImage = new Bitmap(200, 200);

            mnist = MNistDataLoader.LoadMNISTData();

            trainingImageDisplay.Image = currentTrainingImage;

            using (var g = Graphics.FromImage(currentUserImage))
                g.Clear(Color.Black);

            userImageCanvas.Image = currentUserImage;
            currentStandardisedImage = StandardiseImage(currentUserImage);
            standardisedImage.Image = currentStandardisedImage;
            tmrIteration.Enabled = true;
        }

        private INeuralNetwork CreateNetwork(string type)
        {
            return CreateNetwork(type, null);
        }

        private INeuralNetwork CreateNetwork(string type, XDocument document)
        {
            if (!string.IsNullOrEmpty(type) && document != null)
            {
                switch (type)
                {
                    case "NeuralNetwork":
                        return NeuralNetwork.FromXml(document);
                    case "MultiLayerNeuralNetwork":
                        return MultiLayerNeuralNetwork.FromXml(document);
                    default:
                        throw new ArgumentNullException("Must provide Value Network Type"); ;
                }
            }
            else
            {
                switch (type)
                {
                    case "NeuralNetwork":
                        return new NeuralNetwork(inputCount, hiddenCount, outputCount);
                    case "MultiLayerNeuralNetwork":
                        {
                            var layerCounts = ParseLayerCounts();
                            return new MultiLayerNeuralNetwork(layerCounts);
                        }
                    default:
                        throw new ArgumentNullException("Must provide Value Network Type"); ;
                }
            }
        }

        private int [] ParseLayerCounts()
        {
            var layerText = txtLayerCounts.Text;

            var layerTextItems = layerText.Split(',');

            if(layerTextItems.Length <= 1)
            {
                layerTextItems = new [] { inputCount.ToString(), hiddenCount.ToString(), outputCount.ToString() };    
            }

            var layers = layerTextItems.Select(i => int.Parse(i)).ToList();

            while (layers.Count < 3)
                layers.Add(64);

            if (layers[0] != inputCount)
                layers[0] = inputCount;

            if (layers.Last() != outputCount)
                layers[layers.Count-1] = outputCount;

            txtLayerCounts.Text = string.Join(", ", layers);

            return layers.ToArray();            
        }

        public void Reset()
        {
            trainingImageIndex = 0;
            testingImageIndex = 0;
            trainedAgainstImageCount = 0;
            totalTestsPerformed = 0;
            totalTestsCorrect = 0;
            segments.Clear();
        }

        public void TrainNetwork(bool display)
        {
            double[] inputs = new double[inputCount];

            for (var i = 0; i < inputs.Length; i++)
            {
                var bright = mnist.trainingImages[trainingImageIndex][i];
                inputs[i] = bright / 255.0;

                if (display)
                {
                    var index = i * 4;
                    currentTrainingImage.SetPixel(i % TrainingImageWidthHeight, i / TrainingImageWidthHeight, Color.FromArgb(bright, bright, bright));
                }
            }

            if (display)
            {
                using (var g = trainingImageDisplay.CreateGraphics())
                {
                    g.DrawImage(currentTrainingImage, 0, 0, 200, 200);
                }
            }

            var label = mnist.trainingLabels[trainingImageIndex];
            var targets = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            targets[(int)label] = 1;
            var prediction = network.Predict(inputs);
            var guess = FindMax(prediction);

            network.Train(inputs, targets);
            trainedAgainstImageCount++;

            if (display)
            {
                lblTrainingImagesExamined.Text = trainedAgainstImageCount.ToString();
            }

            trainingImageIndex = (trainingImageIndex + 1) % mnist.trainingLabels.Length;
        }

        void TestNetwork()
        {
            var inputs = new double[inputCount];
            for (var i = 0; i < inputs.Length; i++)
            {
                var bright = mnist.testingImages[testingImageIndex][i];
                inputs[i] = bright / 255.0;
            }

            var label = mnist.testingLabels[testingImageIndex];

            var prediction = network.Predict(inputs);
            var guess = FindMax(prediction);

            totalTestsPerformed++;
            if (guess == label) // did we get the right answer?
                totalTestsCorrect++;

            var percent = 100 * ((double)totalTestsCorrect / (double)totalTestsPerformed);
            lblPredictionPercent.Text = percent.ToString("n") + "%";

            testingImageIndex++;
            if (testingImageIndex == mnist.testingLabels.Length)
            {
                testingImageIndex = 0;
                totalTestsPerformed = 0;
                totalTestsCorrect = 0;
            }
        }

        void GuessUserDrawDigit()
        {
            if (!userHasDrawingToTest)
            {
                userPrediction.Text = "_";
                return;
            }

            var thumbNail = new Bitmap(TrainingImageWidthHeight, TrainingImageWidthHeight);

            using (var g = Graphics.FromImage(thumbNail))
                g.DrawImage(currentStandardisedImage, 0, 0, TrainingImageWidthHeight, TrainingImageWidthHeight);

            var inputs = new double[TrainingImageWidthHeight * TrainingImageWidthHeight];
            for (var i = 0; i < inputs.Length; i++)
            {
                inputs[i] = thumbNail.GetPixel(i % TrainingImageWidthHeight,
                                          (int)(i / TrainingImageWidthHeight)).GetBrightness();
            }

            var prediction = network.Predict(inputs);
            var guess = FindMax(prediction);
            userPrediction.Text = guess.ToString() + " with a " + (prediction[guess] * 100).ToString("n") + "% confidence";
        }

        void PerformTrainingAndTesting()
        {
            GuessUserDrawDigit();

            var trainingRuns = 5;
            for (var i = 0; i < trainingRuns - 1; i++)
                TrainNetwork(false);

            TrainNetwork(true);

            var testingRuns = 25;
            for (var i = 0; i < testingRuns; i++)
                TestNetwork();
        }

        int FindMax(double[] arr)
        {
            return FindMax(arr, (a, b) => a > b);
        }

        int FindMax<T>(T[] arr, Func<T, T, bool> greater)
        {
            T max = default(T);
            var index = 0;

            for (var i = 0; i < arr.Length; i++)
            {
                if (greater(arr[i], max))
                {
                    max = arr[i];
                    index = i;
                }
            }
            return index;

        }

        private void tmrIteration_Tick(object sender, EventArgs e)
        {
            PerformTrainingAndTesting();
        }

        private void userImageCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            userHasDrawingToTest = true;
            currentlyDrawing = true;
            segments.Clear();
        }

        private void userImageCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!currentlyDrawing)
                return;

            segments.Add(e.Location);

            while (segments.Count > 5)
                segments.RemoveAt(0);

            if (segments.Count > 2)
            {
                using (var g = Graphics.FromImage(currentUserImage))
                {
                    g.DrawLines(userDrawingPen, segments.ToArray());
                }

                userImageCanvas.Image = currentUserImage;
            }

            currentStandardisedImage = StandardiseImage(currentUserImage);
            standardisedImage.Image = currentStandardisedImage;
        }

        private unsafe Bitmap StandardiseImage(Bitmap image)
        {
            var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            var scan0 = (byte*)data.Scan0.ToPointer();

            var aabb = GetImageConectedAABB(image, data, scan0);
            image.UnlockBits(data);

            if (aabb.Width <= 0 || aabb.Height <= 0)
                return image;

            var content = new Bitmap(aabb.Width, aabb.Height);

            using (var g = Graphics.FromImage(content))
                g.DrawImage(image, new Rectangle(0, 0, content.Width, content.Height), aabb, GraphicsUnit.Pixel);

            var output = new Bitmap(image.Width, image.Height);

            using (var g = Graphics.FromImage(output))
            {
                g.Clear(Color.Black);
                g.DrawImage(content,
                    new Rectangle((int)(image.Width / 2f - content.Width / 2f),
                                  (int)(image.Height / 2f - content.Height / 2f),
                                  content.Width,
                                  content.Height),
                    new Rectangle(0,
                                  0,
                                  content.Width,
                                  content.Height), GraphicsUnit.Pixel
                    );
            }

            return output;
        }

        private static unsafe Rectangle GetImageConectedAABB(Bitmap image, System.Drawing.Imaging.BitmapData data, byte* scan0)
        {
            int minX = image.Width;
            int maxX = 0;
            int minY = image.Height;
            int maxY = 0;

            for (int y = 0; y < data.Height; ++y)
            {
                for (int x = 0; x < data.Width; ++x)
                {
                    var bits = scan0 + y * data.Stride + x * 32 / 8;

                    if (bits[0] != 0 ||
                        bits[1] != 0 ||
                        bits[2] != 0)
                    {
                        if (x < minX)
                            minX = x;

                        if (x > maxX)
                            maxX = x;

                        if (y < minY)
                            minY = y;

                        if (y > maxY)
                            maxY = y;
                    }
                }
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        private void userImageCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            currentlyDrawing = false;
            segments.Clear();
        }

        private void btnSaveState_Click(object sender, EventArgs e)
        {
            tmrIteration.Enabled = false;
            var xml = network.ToXml().ToString();

            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDlg.FileName, xml);
            }
            
            tmrIteration.Enabled = true;
        }

        private void btnLoadState_Click(object sender, EventArgs e)
        {
            userHasDrawingToTest = false;
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                var xmlString = File.ReadAllText(openFileDlg.FileName);
                Reset();

                var xml = XDocument.Parse(xmlString);
                var type = xml.Root.Element("Type");
                network = CreateNetwork(type.Value, xml);
            }
            userHasDrawingToTest = true;
        }

        private void bntClear_Click(object sender, EventArgs e)
        {
            userHasDrawingToTest = false;

            using (var g = Graphics.FromImage(currentUserImage))
            {
                g.Clear(Color.Black);
            }

            userImageCanvas.Image = currentUserImage;
        }

        private void rdoSimple_CheckedChanged(object sender, EventArgs e)
        {
            network = CreateNetwork(rdoSimple.Checked ? "NeuralNetwork" : "MultiLayerNeuralNetwork");
            Reset();
        }        
    }
}