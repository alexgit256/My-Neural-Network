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

    public class Synapse
    {
        double WeightForward;
        public double GetWeightForward { get { return WeightForward; } }
        double WeightBackward;
        public double GetWeightBackward { get { return WeightBackward; } }

        NeuronAbstractClass<IActivationFunction> ForwardLink;
        public NeuronAbstractClass<IActivationFunction> GetForwardLink { get { return ForwardLink; } }
        NeuronAbstractClass<IActivationFunction> BackwardLink;
        public NeuronAbstractClass<IActivationFunction> GetBackwardLink { get { return BackwardLink; } }
    }

    public abstract class NeuronAbstractClass<AF> : INeuron<AF> where AF : IActivationFunction
    {
        private AF ActivationFunction;
        private List<Synapse> SynapseSet;
        private double Input;
        public NeuronAbstractClass(AF AFu, List<Synapse> SS)
        {
            this.ActivationFunction = AFu;
            this.SynapseSet = SS;
        }

        public void FillInput(double A)
        {
            Input = FormatInputAvg();
        }

        public void FillInputFromPrevLayer()
        {
            Input = FormatInputAvg();
        }

        private double FormatInputAvg()    //function that converts previous layer's data to input value. Doesn't work with empty synapse set.
        {
            double output = 0;
            int counter = 0;
            foreach (Synapse S in SynapseSet)
            {
                counter++;
                output += S.GetBackwardLink.GetOutput();
            }
            if (counter > 0)
                return output / counter;
            else
                return double.MinValue; 
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

        public void FillInput(List<Double> A)
        {
            Input = A;
        }

        public void ProcessInfo()   //Loads signal into EACH Neuron's internal storage
        {
            foreach (var N in NeuronSet)
            {
                N.FillInput(1f);
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
    }

    public abstract class NeuronNetworkAbstractClass<T>: INeuronNetwork
    {
        private NeuronLayerAbstractClass InputLayer;
        private List<INeuronLayer> InternalLayers;
        private NeuronLayerAbstractClass OutputLayer;
        private List<T> Input;
        public abstract List<double> FormatInput(); //Converts data to neuron signal. Needs override.

        public List<double> GetOutput()
        {
            return OutputLayer.GetOutput();
        }
    }
}
