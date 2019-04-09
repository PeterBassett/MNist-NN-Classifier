using System.Xml.Linq;

namespace MNistClassifier.Math
{
    public interface INeuralNetwork
    {
        double[] Predict(double[] inputVector);
        XElement ToXml();
        void Train(double[] inputValues, double[] targetValues);
    }
}