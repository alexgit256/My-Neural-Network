using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public interface IActivationFunction
    {
        double GetValue(double input); 
    }

    public interface IErrorCountingMethod
    {
        double GetErrorValue();
    }

    public interface INeuron<AF> where AF : IActivationFunction
    {
        void FillInput(double A);   //Loads signal into Neuron's internal storage
        double GetOutput();
    }

    public interface INeuronLayer
    {
        void ProcessInfo();
        List<double> GetOutput();
    }

    public interface INeuronNetwork
    {
        List<double> GetOutput();
    }

    public abstract class NeuronAbstractClass<AF>: INeuron<AF> where AF : IActivationFunction
    {
        private AF ActivationFunction;
        private double Input;
        public void FillInput(double A)
        {
            Input = A;
        }


        public double GetOutput()
        {
            return ActivationFunction.GetValue(Input);
        }
    }

    public abstract class NeuronLayerAbstractClass: INeuronLayer
    {
        private List<INeuron<IActivationFunction>> NeuronSet;
        private List<Double> Input;

        public NeuronLayerAbstractClass(List<INeuron<IActivationFunction>> NS, List<Double> Inp)
        {
            this.NeuronSet = NS;
            this.Input = Inp;
        }

        public void ProcessInfo()   //Loads signal into EACH Neuron's internal storage
        {
            foreach (var N in NeuronSet)
            {
                N.FillInput(FormatInputAvg());
            }
        }

        public List<double> GetOutput() //gets output data
        {
            List<Double> output = new List<Double>();
            foreach (var N in NeuronSet)
            {
                output.Add(N.GetOutput());
            }
            return output;
        }

        private double FormatInputAvg()    //function that converts previous layer's data to input value. 
        {
            double X = 0;
            foreach (double d in Input)
            {
                X += d;
            }
            return X/Input.Count;
        }
    }

    public abstract class NeuronNetworkAbstractClass<T>: INeuronNetwork
    {
        private List<INeuronLayer> Layers;
        private List<T> Input;
        public abstract List<double> FormatInput(); //Converts data to neuron signal

        public List<double> GetOutput()
        {
            return null;
        }
    }
}
