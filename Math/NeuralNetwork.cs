using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MNistClassifier.Math
{
    public class NeuralNetwork : INeuralNetwork
    {
        private int inputNodeCount;
        private int hiddenNodeCount;
        private int outputNodeCount;
        private Matrix weightsInputToHidden;
        private Matrix weightsHiddenToOutput;
        private Matrix biasHidden;
        private Matrix biasOutput;
        private double learningRate;
        private ActivationFunction activationFunction;

        public NeuralNetwork(int inputNodeCount, int hiddenNodeCount, int outputNodeCount)
        {
            this.inputNodeCount = inputNodeCount;
            this.hiddenNodeCount = hiddenNodeCount;
            this.outputNodeCount = outputNodeCount;

            weightsInputToHidden = Matrix.Random(this.hiddenNodeCount, this.inputNodeCount);
            weightsHiddenToOutput = Matrix.Random(this.outputNodeCount, this.hiddenNodeCount);

            biasHidden = Matrix.Random(this.hiddenNodeCount, 1);
            biasOutput = Matrix.Random(this.outputNodeCount, 1);

            learningRate = 0.1;
            activationFunction = ActivationFunction.ActivationFunctions["sigmoid"];
        }

        public NeuralNetwork(NeuralNetwork network)
        {
            inputNodeCount = network.inputNodeCount;
            hiddenNodeCount = network.hiddenNodeCount;
            outputNodeCount = network.outputNodeCount;
            learningRate = network.learningRate;
            activationFunction = network.activationFunction;

            weightsInputToHidden = new Matrix(network.weightsInputToHidden);
            weightsHiddenToOutput = new Matrix(network.weightsHiddenToOutput);
            biasHidden = new Matrix(network.biasHidden);
            biasOutput = new Matrix(network.biasOutput);
        }

        public double[] Predict(double[] inputVector)
        {
            // convert our 1d array into a 1xN Matrix
            //
            // [1,2,3,4,5]
            // ==
            // [
            //  [1],
            //  [2],
            //  [3],
            //  [4],
            //  [5],
            // ]
            //
            var inputs = new Matrix(inputVector);
            var layerMatrices = Predict(inputs);
            return layerMatrices.Last().ToArray();
        }

        public Matrix[] Predict(Matrix inputs)
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Use the input values and hidden layer weights and bias to generate the output from the hidden layer.
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            Matrix hidden = CalculateLayer(inputs, weightsInputToHidden, biasHidden);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Use the hidden layer output values and output layer weights and bias to generate the output from the output layer.
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Matrix output = CalculateLayer(hidden, weightsHiddenToOutput, biasOutput);

            // The output layer now has the result
            return new[] { hidden, output };
        }

        private Matrix CalculateLayer(Matrix input, Matrix weights, Matrix bias)
        {
            // normal matrix multiply between the weights of the layer and our input to the layer
            var layer = Matrix.Multiply(weights, input);

            // add in the bias values. The bias values can be negative.
            // simple element by element addition
            layer.Add(bias);

            // Now map the values in the layer to a value between 0 and 1 using our activation function.
            layer.Map(activationFunction.func);

            return layer;
        }

        public void Train(double[] inputValues, double[] targetValues)
        {
            /// This is our method to train the NN to poduce better results.
            /// It is one of the most simple example of "Supervised Learning"
            /// i.e. "Given this input we would like this output"
            /// The algorithm we use is called Back Proogation.
            /// We use this to tune the weights and bias values of each "Neuron"
            /// to minimise the error in our output
            /// That is all it does, just minimises an error value.
            /// I dont understand the maths as a whole, but it's reasonably easy to 
            /// implement as just a reciepe of Matrix transformations.

            ////// SETUP /////////
            // Convert array to matrix object
            var inputs = new Matrix(inputValues);
            var targets = new Matrix(targetValues);

            ////// RUN EXISTSING PREDICTION /////////
            // Run the NN to generate output which we will then compare against the targetValues and train against
            var layers = Predict(inputs);
            // get the layer values to calculate errors with
            var hidden = layers[0];
            var outputs = layers[1];

            ////// CALCULATE OUTPUT ERROR /////////            
            // This is just the difference between what we produced and what we SHOULD have produced.
            // ERROR = TARGETS - OUTPUTS
            var outputLayerError = Matrix.Subtract(targets, outputs);

            ////// Back Propagate Error To Output Layer Weight And Bias /////////
            CalculateLayerGradientAndApplyChange(weightsHiddenToOutput, biasOutput, hidden, outputs, outputLayerError);

            ////// CALCULATE HIDDEN LAYER ERROR /////////
            // A little more complex
            var transposedHiddenLayerWeights = Matrix.Transpose(weightsHiddenToOutput);
            var hiddenLayerError = Matrix.Multiply(transposedHiddenLayerWeights, outputLayerError);

            ////// Back Propagate Error To Hidden Layer Weight And Bias /////////
            CalculateLayerGradientAndApplyChange(weightsInputToHidden, biasHidden, inputs, hidden, hiddenLayerError);
        }

        private void CalculateLayerGradientAndApplyChange(Matrix layerWeights, Matrix layerBias, Matrix inputs, Matrix outputs, Matrix layerError)
        {
            Matrix gradients = Gradient(outputs, layerError);

            // Calculate deltas
            Matrix weightDeltas = Deltas(inputs, gradients);

            ApplyLayerChange(layerWeights, weightDeltas, layerBias, gradients);
        }

        private void ApplyLayerChange(Matrix weights, Matrix weightDeltas, Matrix bias, Matrix biasGradient)
        {
            // Adjust the weights by deltas
            weights.Add(weightDeltas);
            // Adjust the bias by its deltas (which is just the gradients)
            bias.Add(biasGradient);
        }

        private static Matrix Deltas(Matrix m, Matrix gradient)
        {
            var mTransposed = Matrix.Transpose(m);
            var deltas = Matrix.Multiply(gradient, mTransposed);
            return deltas;
        }

        private Matrix Gradient(Matrix m, Matrix mError)
        {
            // map over our input
            var gradients = Matrix.Map(m, activationFunction.dfunc);
            // element-by-element multiply by our error
            gradients.Hadamard(mError);
            // multiply eaah element by our learning rate. 
            // This just scales down the output by a constant factor
            // It slows learning but also stop us from jumping wildly back and forth
            // Eases us into our solution
            gradients.Multiply(learningRate);

            return gradients;
        }

        public XElement ToXml()
        {
            return new XElement("NeuralNetwork",
                new XElement("Type", this.GetType().Name),
                new XElement("InputNodeCount", inputNodeCount),
                new XElement("HiddenNodeCount", hiddenNodeCount),
                new XElement("OutputNodeCount", outputNodeCount),
                new XElement("LearningRate", learningRate),
                new XElement("ActivationFunction", ActivationFunction.ActivationFunctions.First(af => af.Value.Equals(activationFunction)).Key),
                new XElement("WeightsInputToHidden", weightsInputToHidden.ToXml()),
                new XElement("weightsHiddenToOutput", weightsHiddenToOutput.ToXml()),
                new XElement("BiasHidden", biasHidden.ToXml()),
                new XElement("BiasOutput", biasOutput.ToXml()));
        }

        public static NeuralNetwork FromXml(XDocument xml)
        {            
            var inputNodeCount = int.Parse(xml.Root.Element("InputNodeCount").Value);
            var hiddenNodeCount = int.Parse(xml.Root.Element("HiddenNodeCount").Value);
            var outputNodeCount = int.Parse(xml.Root.Element("OutputNodeCount").Value);

            var network = new NeuralNetwork(inputNodeCount, hiddenNodeCount, outputNodeCount);

            var funcName = xml.Root.Element("ActivationFunction").Value;

            network.activationFunction = ActivationFunction.ActivationFunctions.Values.First();
            if (ActivationFunction.ActivationFunctions.ContainsKey(funcName))
                network.activationFunction = ActivationFunction.ActivationFunctions[funcName];

            network.learningRate = double.Parse(xml.Root.Element("LearningRate").Value);
            network.weightsInputToHidden = Matrix.FromXElement(xml.Root.Element("WeightsInputToHidden").Element("Matrix"));
            network.weightsHiddenToOutput = Matrix.FromXElement(xml.Root.Element("weightsHiddenToOutput").Element("Matrix"));
            network.biasHidden = Matrix.FromXElement(xml.Root.Element("BiasHidden").Element("Matrix"));
            network.biasOutput = Matrix.FromXElement(xml.Root.Element("BiasOutput").Element("Matrix"));

            return network;
        }
    }
}