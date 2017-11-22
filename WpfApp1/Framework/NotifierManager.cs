using System.Collections.Generic;
using System.Threading;

namespace Framework
{
    internal class NotifierManager
    {
        private object _syncObj = new object();
        private Dictionary<NotifiableObject, SortedSet<string>> _notifiers = new Dictionary<NotifiableObject, SortedSet<string>>();

        private Thread _taskThread = null;

        public NotifierManager()
        {
            StartListening();
        }

        private void StartListening()
        {
            _taskThread = new Thread(new ThreadStart(Notifier));
            _taskThread.Start();
        }

        private void Notifier()
        {
            while (true)
            {
                foreach (var item in _notifiers)
                {
                    lock (item.Key)
                    {
                        foreach (var property in item.Value)
                        {
                            item.Key.ApplyToUI(property);
                        }
                        item.Value.Clear();
                    }
                }
                Thread.Sleep(10);
            }
        }

        public void RegisterToNotify(NotifiableObject sender, string property)
        {
            if (_notifiers.ContainsKey(sender))
            {
                lock (sender)
                {
                    _notifiers[sender].Add(property);
                }
            }
            else
            {
                var properties = new SortedSet<string>
                {
                    property
                };
                _notifiers[sender] = properties;
            }

            CheckTask();
        }

        private void CheckTask()
        {
            if (_taskThread.ThreadState == ThreadState.Aborted || _taskThread.ThreadState == ThreadState.Stopped)
            {
                StartListening();
            }
        }
    }
}
