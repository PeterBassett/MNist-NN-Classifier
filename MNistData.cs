using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MNistClassifier
{
    public class MNistDataLoader
    {
        public class MNistData
        {
            public byte[][] trainingImages;
            public byte[] trainingLabels;

            public byte[][] testingImages;
            public byte[] testingLabels;
        }

        public static bool CheckFilesExist()
        {
            if(!File.Exists(@"data\train-images-idx3-ubyte") ||
                !File.Exists(@"data\train-labels-idx1-ubyte") ||
                !File.Exists(@"data\t10k-images-idx3-ubyte") ||
                !File.Exists(@"data\t10k-labels-idx1-ubyte"))
            {
                // we are missing at least one requied data file.
                MessageBox.Show("This app requires the MNIST database of handwritten digits available here :\n\nhttp://yann.lecun.com/exdb/mnist/\n\nPlease download the four files, and decompress them into the data folder next to the executable.", "mnist data files not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public static MNistData LoadMNISTData()
        {
            return new MNistData()
            {
                trainingImages = LoadImageFile(@"data\train-images-idx3-ubyte"),
                trainingLabels = LoadLabelFile(@"data\train-labels-idx1-ubyte"),
                testingImages = LoadImageFile(@"data\t10k-images-idx3-ubyte"),
                testingLabels = LoadLabelFile(@"data\t10k-labels-idx1-ubyte"),
            };
        }

        public static int ToInt32R(byte [] data, int offset)
        {
            var bytes = new byte[4];
            bytes[0] = data[offset + 3];
            bytes[1] = data[offset + 2];
            bytes[2] = data[offset + 1];
            bytes[3] = data[offset + 0];

            return BitConverter.ToInt32(bytes, 0);
        }

        public static byte[][] LoadImageFile(string filePath)
        {
            var data = File.ReadAllBytes(filePath);

            var fileType = ToInt32R(data, 0);
            if (fileType != 2051)
                throw new ArgumentOutOfRangeException();

            int itemCount = ToInt32R(data, 4);
            int dataLength = ToInt32R(data, 8) * ToInt32R(data, 12);

            var imageData = new byte[itemCount][];
            for (var i = 0; i < itemCount; i++)
            {
                imageData[i] = new byte[dataLength];
                Array.Copy(data, 16 + (i * dataLength), imageData[i], 0, dataLength);
            }

            return imageData;            
        }

        public static byte[] LoadLabelFile(string filePath)
        {
            var data = File.ReadAllBytes(filePath);
            
            var fileType = ToInt32R(data, 0);
            if (fileType != 2049)
                throw new ArgumentOutOfRangeException();

            int itemCount = ToInt32R(data, 4);            

            var labelData = new byte[itemCount];
            Array.Copy(data, 8, labelData, 0, itemCount);
            return labelData;            
        }
    }
}
