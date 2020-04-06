using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrainSignalsThreading
{
    public enum signalColour { red, green }; // for the signal states
    public delegate void lightChangeHandler(signalColour newColour); // Delegate used to handle signal change, takes argument of new signal colour, returns void.
    class Program
    {
        public class TrainSignal
        {

            public event lightChangeHandler onSignalChange;

            public signalColour signal { private set; get; } = signalColour.red;

            public void setSignal(signalColour newSignalColour)
            {
                if (newSignalColour != signal)
                {
                    signal = newSignalColour;

                    Console.WriteLine("Signal now changing to " + (signal == signalColour.red ? "Red" : "Green") + "...");


                    // trigger the event when it's not null
                    onSignalChange?.Invoke(signal);
                }
            }
        }

        public class Train // choo choo!
        {
            private void startMove() { Console.WriteLine("Choo Choo!!!"); }
            private void stopMove() { Console.WriteLine("Applying brakes..."); }

            public void registerSignal(TrainSignal signal)
            {
                signal.onSignalChange += new lightChangeHandler(respondToSignal);
            }

            private void respondToSignal(signalColour colour)
            {
                if(colour == signalColour.red)
                {
                    stopMove();
                }
                else
                {
                    startMove();
                }
            }
        }

        public class LevelCrossing
        {
            private void lowerBarriers() { Console.WriteLine("Barriers lowering..."); }
            private void raiseBarriers() { Console.WriteLine("Barriers raising..."); }

            public void registerToSignal(TrainSignal signal)
            {
                signal.onSignalChange += new lightChangeHandler(respondToSignal);
            }

            private void respondToSignal(signalColour colour)
            {
                if(colour == signalColour.red)
                {
                    raiseBarriers();
                }
                else
                {
                    lowerBarriers();
                }
            }
        }
        public class WarningSiren
        {
            public void registerToSignal(TrainSignal signal)
            {
                signal.onSignalChange += new lightChangeHandler(respondToSignal);
            }

            public void respondToSignal(signalColour colour)
            {
                if(colour == signalColour.red)
                {
                    Console.Beep(200, 5000);
                }
                else
                {
                    Console.Beep(2000, 5000);
                }
            }
        }

        static void Main()
        {
            TrainSignal signal = new TrainSignal();
            Train train = new Train();
            LevelCrossing crossing = new LevelCrossing();
            WarningSiren siren = new WarningSiren();

            crossing.registerToSignal(signal);
            train.registerSignal(signal);
            siren.registerToSignal(signal);

            signal.setSignal(signalColour.green);
            siren.respondToSignal(signalColour.green);
            signal.setSignal(signalColour.red);
            siren.respondToSignal(signalColour.red);
            signal.setSignal(signalColour.green);
            siren.respondToSignal(signalColour.green);
        }
    }
}

