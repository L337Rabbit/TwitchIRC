using okitoki.twitch.irc.messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.client
{
    public class MessageQueue
    {
        public int MaxMessages { get; set; } = 1000;

        public long MaxHoldingTime { get; set; } = 3600000;

        public List<IRCMessage> Queue { get; private set; } = new List<IRCMessage>();

        private Thread monitorThread;

        public bool IsActive { get; private set; } = false;

        public void Start()
        {
            //Create a thread to monitor the message queue and remove old messages and clear out space when max capacity is reached.
            monitorThread = new Thread(() =>
            {
                while (true)
                {
                    long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    Queue.TakeWhile((message) => time - message.CreationTime > MaxHoldingTime);
                    Thread.Sleep(5000);
                }
            });

            monitorThread.Name = "Twitch_MessageQueue";
            monitorThread.Start();
            IsActive = true;
        }

        public void Stop()
        {
            IsActive = false;
            monitorThread.Interrupt();
        }

        public void QueueMessage(IRCMessage message)
        {
            if (IsActive)
            {
                if (Queue.Count >= MaxMessages)
                {
                    Queue.RemoveAt(0);
                }

                Queue.Add(message);
            }
        }

        public void ClearQueue()
        {
            Queue.Clear();
        }
    }
}
