using System;
using System.Collections.Generic;

namespace MNistClassifier.Math
{
    public class ActivationFunction
    {
        public readonly Func<double, double> func;
        public readonly Func<double, double> dfunc;

        public ActivationFunction(Func<double, double> func, Func<double, double> dfunc)
        {
            this.func = func;
            this.dfunc = dfunc;
        }

        public static Dictionary<string, ActivationFunction> ActivationFunctions = new Dictionary<string, ActivationFunction> {
            {
                "sigmoid",
                new ActivationFunction(
                    x => 1.0 / (1.0 + System.Math.Exp(-x)),
                    y => y * (1.0 - y)
                )
            },
            {  "tanh",
                new ActivationFunction(
                    x => System.Math.Tanh(x),
                    y => 1.0 - (y * y)
                )
            }
        };
    }
}