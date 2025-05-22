using UnityEngine;
using OSC.NET;
using TUIO;
using System.Collections.Generic;

public class JSProtocol : NetworkProtocol
{

    private List<NetworkListener> listeners;
    private TUIOControl tuioController;

    private List<OSCMessage> queuedMessages;

    public JSProtocol() {
        listeners = new List<NetworkListener>();
        queuedMessages = new List<OSCMessage>();
    }

    public void OnDisable() {
        listeners.Clear();
    }

    public override void addListener(NetworkListener listener)
    {
        listeners.Add(listener);
    }

    public override void removeAllListeners(){
        listeners.Clear();
    }

    public override void removeListener(NetworkListener listener)
    {
        listeners.Remove(listener);
    }

    public override void setTuioControl(TUIOControl tuio)
    {
        tuioController = tuio;
    }

    public void injectOSC(OSCPacket packet) {
        Debug.Log("Injecting OSC: "+packet.Address);
        if(packet.IsBundle()) {
            handleBundle((OSCBundle) packet);
        } else {
            handleMessage((OSCMessage) packet);
        }
    }

    private void handleBundle(OSCBundle bundle)
    {
        foreach (OSCPacket packet in bundle.Values)
        {
            if (packet.IsBundle())
            {
                handleBundle((OSCBundle)packet);
            }
            else
            {
                handleMessage((OSCMessage)packet);
            }
        }
    }

    private void handleMessage(OSCMessage message)
    {
        string address = message.Address;

        if(tuioController != null && address.StartsWith("/tuio")) {
            tuioController.processMessage(message);
        } else
        {
            lock (queuedMessages)
            {
                queuedMessages.Add(message);
            }
        }
    }

    public void Update() {
        runQueue();
    }

    public void runQueue()
    {
        lock(queuedMessages)
        {
            foreach (OSCMessage message in queuedMessages)
            {
                string address = message.Address;
                List<NetworkController.OSCValue> values = new List<NetworkController.OSCValue>();

                for (int i = 0; i < message.Values.Count; i++)
                {
                    values.Add(new NetworkController.OSCValue(message.Values[i]));
                }

                Debug.Log("Sending to listeners: "+address+" - "+values);

                Debug.Log("Listeners: "+listeners);

                lock (listeners)
                {
                    foreach (NetworkListener listener in listeners)
                    {
                        listener.messageArrived(address, values);
                    }
                }
            }

            queuedMessages.Clear();
        }
    }
}