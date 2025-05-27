using System.Diagnostics;
using MQDmxController.Abstractions;

namespace MQDmxController
{
    public class DmxEngine : IDisposable
    {
        private const int REFRESH_RATE = 20; // in Hz

        private readonly IDmxOutput _output;
        private readonly byte[] _dmxData = new byte[512];
        private readonly object _lock = new();
        private bool _running;
        private Thread _thread;

        public DmxEngine(IDmxOutput output, DmxOutputOptions options)
        {
            _output = output;
            _output.Initialize(options);

            Thread.Sleep(5000); // Allow time for the device to initialize

            _running = true;
            _thread = new Thread(Run)
            {
                IsBackground = true
            };
            _thread.Start();
        }

        public void AnimateChannel(int channel, int startValue, int targetValue, int duration, int cycles)
        {
            if (channel < 1 || channel > 512)
                throw new ArgumentException("Channel must be between 1 and 512");

            if (startValue < 0 || startValue > 255 || targetValue < 0 || targetValue > 255)
                throw new ArgumentException("Value must be between 0 and 255");

            if (duration <= 0)
                throw new ArgumentException("Duration must be greater than zero");

            Task.Run(() =>
            {
                for (int cycle = 0; cycle < cycles; cycle++)
                {
                    AnimateStep(channel, startValue, targetValue, duration);
                    AnimateStep(channel, targetValue, startValue, duration);
                }
            });
        }

        public void FadeChannel(int channel, int targetValue, int duration)
        {
            if (channel < 1 || channel > 512)
                throw new ArgumentException("Channel must be between 1 and 512");

            if (targetValue < 0 || targetValue > 255)
                throw new ArgumentException("Value must be between 0 and 255");

            if (duration < 0)
                throw new ArgumentException("Duration must be non-negative");

            int startValue;
            lock (_lock)
            {
                startValue = _dmxData[channel - 1];
            }

            Task.Run(() => AnimateStep(channel, startValue, targetValue, duration));
        }

        private void AnimateStep(int channel, int from, int to, int duration)
        {
            int frameTimeMs = 1000 / REFRESH_RATE;
            int steps = Math.Max(duration / frameTimeMs, 1);
            float stepValue = (to - from) / (float)steps;

            var stopwatch = Stopwatch.StartNew();

            for (int step = 1; step <= steps; step++)
            {
                int intermediateValue = (int)Math.Round(from + stepValue * step);
                intermediateValue = Math.Clamp(intermediateValue, 0, 255);
                SetChannel(channel, intermediateValue);

                long targetElapsed = step * frameTimeMs;

                // Wait until we've reached the target elapsed time
                while (stopwatch.ElapsedMilliseconds < targetElapsed)
                {
                    Thread.SpinWait(100); // Light CPU yield to avoid busy waiting
                }
            }

            stopwatch.Stop();
        }

        public void SetChannel(int channel, int value)
        {
            if (channel < 1 || channel > 512)
                throw new ArgumentException("Channel must be between 1 and 512 (both included)");

            if (value < 0 || value > 255)
                throw new ArgumentException("Value must be between 0 and 255 (both included)");

            lock (_lock)
            {
                _dmxData[channel - 1] = (byte)value;
            }
        }

        private void Run()
        {
            int frameIntervalMs = 1000 / REFRESH_RATE;
            Stopwatch stopwatch = new();

            while (_running)
            {
                stopwatch.Restart();

                byte[] frame;
                lock (_lock)
                {
                    frame = (byte[])_dmxData.Clone();
                }
                _output.SendFrame(frame);

                // Busy wait until the frame interval is complete
                while (stopwatch.ElapsedMilliseconds < frameIntervalMs)
                {
                    Thread.SpinWait(100);
                }
            }
        }

        public void Dispose()
        {
            _running = false;
            _thread.Join();
            _output.Shutdown();
        }
    }
}
