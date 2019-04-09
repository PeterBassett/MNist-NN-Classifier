using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MNistClassifier.Math
{
    public class MultiLayerNeuralNetwork : INeuralNetwork
    {
        public class Layer
        {
            public readonly Matrix weights;
            public readonly Matrix bias;

            public Layer(int outputNodeCount, int inputNodeCount)
            {
                weights = Matrix.Random(outputNodeCount, inputNodeCount);
                bias = Matrix.Random(outputNodeCount, 1);
            }

            public Layer(Layer other) : this(new Matrix(other.weights), new Matrix(other.bias))
            {
            }

            public Layer(Matrix weights, Matrix bias)
            {
                this.weights = weights;
                this.bias = bias;
            }
        }

        private int[] layerNodeCounts;
        private Layer[] layers;

        private double learningRate;
        private ActivationFunction activationFunction;

        public MultiLayerNeuralNetwork(int[] layerNodeCounts)
        {
            this.layerNodeCounts = layerNodeCounts;

            layers = new Layer[this.layerNodeCounts.Length - 1];

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new Layer(layerNodeCounts[i+1], layerNodeCounts[i]);
            }
            
            learningRate = 0.1;
            activationFunction = ActivationFunction.ActivationFunctions["sigmoid"];
        }

        public MultiLayerNeuralNetwork(MultiLayerNeuralNetwork network) : this(network.layerNodeCounts)
        {
            for (int i = 0; i < network.layers.Length; i++)
            {
                layers[i] = new Layer(network.layers[i]);
            }
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
            var layerResults = new List<Matrix>();

            layerResults.Add(inputs);

            Matrix layerInput = inputs;

            for (int i = 0; i < layers.Length; i++)
            {
                var layer = this.layers[i];

                Matrix layerOutput = CalculateLayer(layerInput, layer.weights, layer.bias);

                layerResults.Add(layerOutput);

                layerInput = layerOutput;
            }
            
            // The output layer now has the result
            return layerResults.ToArray();
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
            var layerResults = Predict(inputs);

            ////// CALCULATE OUTPUT ERROR /////////            
            // This is just the difference between what we produced and what we SHOULD have produced.
            // ERROR = TARGETS - OUTPUTS
            var outputs = layerResults.Last();
            var error = Matrix.Subtract(targets, outputs);

            // Iterate Backwards! Because it's Back Propogation!
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                var layer = layers[i];
                ////// Back Propagate Error To Output Layer Weight And Bias /////////
                CalculateLayerGradientAndApplyChange(layers[i].weights, layers[i].bias, layerResults[i], layerResults[i + 1], error);

                // calculate error for next iteration
                var transposedHiddenLayerWeights = Matrix.Transpose(layers[i].weights);
                error = Matrix.Multiply(transposedHiddenLayerWeights, error);
            }            
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
                new XElement("LayerCounts", 
                    layerNodeCounts.Select( c => new XElement("LayerNodeCount", c))
                ),               
                new XElement("LearningRate", learningRate),
                new XElement("ActivationFunction", ActivationFunction.ActivationFunctions.First(af => af.Value.Equals(activationFunction)).Key),

                new XElement("Layers",
                    layers.Select( l => {
                        return new XElement("Layer",
                            new XElement("Weights", l.weights.ToXml()),
                            new XElement("Bias", l.bias.ToXml())
                        );
                    })                        
                ));
        }

        public static MultiLayerNeuralNetwork FromXml(XDocument xml)
        {
            var root = xml.Root;

            // get the layer count array
            var layerCounts = root.Element("LayerCounts").Elements("LayerCount").Select(e => int.Parse(e.Value)).ToArray();

            var network = new MultiLayerNeuralNetwork(layerCounts);

            network.learningRate = double.Parse(xml.Root.Element("LearningRate").Value);

            var funcName = xml.Root.Element("ActivationFunction").Value;

            network.activationFunction = ActivationFunction.ActivationFunctions.Values.First();
            if (ActivationFunction.ActivationFunctions.ContainsKey(funcName))
                network.activationFunction = ActivationFunction.ActivationFunctions[funcName];

            var layerElements = root.Element("Layers").Elements("Layer").ToArray();
                
            for (int i = 0; i < layerElements.Length; i++)
            {
                var weights = Matrix.FromXElement(layerElements[i].Element("Weights"));
                var bias = Matrix.FromXElement(layerElements[i].Element("Bias"));
                network.layers[i] = new Layer(weights, bias);            
            }

            return network;
        }
    }
}