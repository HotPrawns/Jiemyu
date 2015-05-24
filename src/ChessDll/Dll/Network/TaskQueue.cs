using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace ChessDll.Network
{
    public class EventArgsWrapper<T> : EventArgs
    {
        public EventArgsWrapper(T data)
        {
            this.Data = data;
        }

        public T Data { get; set; }
    }

    public class TaskQueue<T> : IDisposable where T : class
    {
        private object _lock = new object();
        private Thread[] _workers;
        Queue<T> _taskQ = new Queue<T>();

        Stopwatch _stopWatch = new Stopwatch();

        /// <summary>
        /// Minimum time for worker callback to keep the thread from
        /// completely hogging up resources in a loop
        /// </summary>
        private static long MinimumEllapsedMilliseconds = 100;

        /// <summary>
        /// Callback used for when work is available 
        /// </summary>
        /// <param name="task"></param>
        public event EventHandler<EventArgsWrapper<T>> WorkAvailable;

        /// <summary>
        /// Default constructor that sets amount of threads
        /// </summary>
        public TaskQueue() : this(2)
        {
        }

        /// <summary>
        /// Constructor that sets amount of worker threads to passed in value
        /// </summary>
        /// <param name="workers"></param>
        public TaskQueue(int workers)
        {
            _workers = new Thread[workers];

            // Each thread works as a consumer
            for (var i = 0; i < workers; i++)
            {
                _workers[i] = new Thread(Consume);
                _workers[i].Start();
            }
        }

        /// <summary>
        /// Puts a task up to be consumed. Null task represents end
        /// </summary>
        /// <param name="task"></param>
        public void EnqueTask(T task)
        {
            lock (_lock)
            {
                _taskQ.Enqueue(task);
                Monitor.PulseAll(_lock);
            }
        }

        private void Consume()
        {
            while (true)
            {
                T task;
                lock (_lock)
                {
                    // Wait until there is work to do
                    while (_taskQ.Count == 0) Monitor.Wait(_lock);

                    task = _taskQ.Dequeue();
                }

                if (task == null) return;

                _stopWatch.Start();
                var handler = WorkAvailable;
                EventArgsWrapper<T> data = new EventArgsWrapper<T>(task);

                if (handler == null)
                {
                    // Requeue the work since it wasn't handled
                    EnqueTask(task);
                    Debug.Print("Re-qing task");
                }
                else
                {
                    handler(this, data);
                }
                _stopWatch.Stop();

                var elapsedTime = _stopWatch.ElapsedMilliseconds;
                if (elapsedTime < MinimumEllapsedMilliseconds)
                {
                    long remainingTime = MinimumEllapsedMilliseconds - elapsedTime;
                    Thread.Sleep((int) remainingTime);
                }
            }
        }

        /// <summary>
        /// Cleans up the threads for this queue
        /// </summary>
        public void Dispose()
        {
            // Make sure each worker has a null task to join to
            foreach (Thread worker in _workers) EnqueTask(null);
            foreach (Thread worker in _workers) worker.Join();
        }
    }
}
