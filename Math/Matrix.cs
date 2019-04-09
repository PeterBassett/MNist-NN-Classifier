using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MNistClassifier.Math
{
    public class Matrix
    {
        public readonly int rows;
        public readonly int cols;
        public readonly double[,] data;

        public Matrix(Matrix matrix) : this(matrix.data)
        {
        }

        public Matrix(int rows, int cols) : this(new double[rows, cols])
        {
        }

        public Matrix(double[] vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector));

            rows = vector.Length;
            cols = 1;
            data = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                data[i, 0] = vector[i];
            }
        }

        public Matrix(double[,] data)
        {
            rows = data.GetLength(0);
            cols = data.GetLength(1);
            this.data = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    this.data[i, j] = data[i, j];
                }
            }
        }

        public static Matrix Subtract(Matrix a, Matrix b)
        {
            if (a.rows != b.rows || a.cols != b.cols)
                throw new ArgumentOutOfRangeException();

            return new Matrix(a.rows, a.cols)
              .Map((_, i, j) => a.data[i, j] - b.data[i, j]);
        }

        public double[] ToArray()
        {
            var arr = new List<double>();
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    arr.Add(data[i, j]);
                }
            }

            return arr.ToArray();
        }

        public static Matrix Random(int rows, int cols)
        {
            Random rnd = new Random();
            return new Matrix(rows, cols).Map((e, x, j) => rnd.NextDouble() * 2 - 1);
        }

        public Matrix Add(Matrix n)
        {
            if (rows != n.rows || cols != n.cols)
                throw new ArgumentOutOfRangeException();

            return Map((e, x, y) => e + n.data[x, y]);
        }

        public static Matrix Transpose(Matrix matrix)
        {
            return new Matrix(matrix.cols, matrix.rows)
              .Map((_, x, y) => matrix.data[y, x]);
        }

        public static Matrix Multiply(Matrix a, Matrix b)
        {
            if (a.cols != b.rows)
                throw new ArgumentOutOfRangeException();

            return new Matrix(a.rows, b.cols).Map((e, x, y) =>
            {

                double sum = 0;
                for (var k = 0; k < a.cols; k++)
                {
                    sum += a.data[x, k] * b.data[k, y];
                }
                return sum;
            });
        }

        public Matrix Hadamard(Matrix n)
        {
            if (rows != n.rows || cols != n.cols)
                throw new ArgumentOutOfRangeException();
            return Map((e, x, y) => e * n.data[x, y]);
        }

        public Matrix Multiply(double n)
        {
            return Map((e, x, y) => e * n);
        }

        public Matrix Map(Func<double, int, int, double> transform)
        {
            for (var x = 0; x < rows; x++)
            {
                for (var y = 0; y < cols; y++)
                {
                    var val = data[x, y];
                    data[x, y] = transform(val, x, y);
                }
            }
            return this;
        }

        public Matrix Map(Func<double, double> transform)
        {
            return Map((e, x, y) => transform(e));
        }

        public static Matrix Map(Matrix matrix, Func<double, double> transform)
        {
            return new Matrix(matrix.rows, matrix.cols).Map((e, x, y) => transform(matrix.data[x, y]));
        }

        public static Matrix FromXElement(XElement xml)
        {
            var rows = int.Parse(xml.Element("Rows").Value);
            var cols = int.Parse(xml.Element("Cols").Value);
            var data1d = xml.Element("Data").Value.Split(',').Select(i => double.Parse(i)).ToArray();

            if (rows * cols != data1d.Length)
                throw new ArgumentOutOfRangeException("Rows * Cols must equal Data Element Count");

            var data = new double[rows, cols];

            for (var x = 0; x < rows; x++)
            {
                for (var y = 0; y < cols; y++)
                {
                    data[x, y] = data1d[x * cols + y];
                }
            }

            return new Matrix(data);
        }

        public XElement ToXml()
        {            
            var output = new List<double>();

            for (var x = 0; x < rows; x++)
            {
                for (var y = 0; y < cols; y++)
                {
                    output.Add(data[x, y]);
                }
            }

            return new XElement("Matrix",
                new XElement("Rows", rows),
                new XElement("Cols", cols),
                new XElement("Data", string.Join(",", output)));
        }
    }
}
