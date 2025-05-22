using UnityEngine;
using OSC.NET;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using TUIO;

public class UDPProtocol : NetworkProtocol {

    public int listenPort = 3333;

    private bool connected;

    private TUIOControl tuioControl;

    private OSCReceiver receiver;
    private Thread ourThread;

    private Dictionary<string, OSCTransmitter> clients;
    private List<NetworkListener> listeners;

    private List<OSCMessage> queuedMessages;

    [System.Serializable]
    public class Client
    {
        public string name;
        public string ip;
        public int port = 3333;
    }

    public Client[] udpClients;

    public UDPProtocol()
    {
        this.connected = false;
        this.clients = new Dictionary<string, OSCTransmitter>();
        this.listeners = new List<NetworkListener>();
        this.queuedMessages = new List<OSCMessage>();
    }

    public void OnEnable() {
        connect();

        foreach (Client client in udpClients)
        {
            this.addClient(client.name, client.ip, client.port);
        }
    }

    public void OnDisable() {
        disconnect();
    }

    public void connect() {
#if UNITY_WEBGL && !UNITY_EDITOR
        Debug.Log("Skipping UDPProtocol because of WebGL");
#else
        if (!connected) {

            receiver = new OSCReceiver(listenPort);
            ourThread = new Thread(listen);
            ourThread.Start();
            connected = true;

            Debug.Log("UDP Network Connected");
        } else {
            Debug.Log("UDP Network already connected!");
        }
#endif        
    }

    override public void addListener(NetworkListener listener)
    {
        listeners.Add(listener);
    }

    override public void removeAllListeners(){
        listeners.Clear();
    }

    override public void removeListener(NetworkListener listener)
    {
        listeners.Remove(listener);
    }

    public void addClient(string name, string ip, int port)
    {
        if (clients.ContainsKey(name))
        {
            Debug.Log("Already added a client with name [" + name + "] this is probabely a bug!");
        } else
        {
            clients.Add(name, new OSCTransmitter(ip, port));
        }
    }

    public void sendOSCMessageToAll(string address, params object[] values)
    {
        sendMessageToAll(address, values);
    }

    public void sendOSCMessage(string receiver, string address, params object[] values)
    {
        sendMessage(receiver, address, values);
    }

    public void disconnect()
    {
        if (connected)
        {
            connected = false;

            if (ourThread.IsAlive) {
                ourThread.Abort();
                ourThread.Join();
            }

            receiver.Close();
            receiver = null;

            ourThread = null;

            foreach (OSCTransmitter client in clients.Values)
            {
                client.Close();
            }
            clients.Clear();

            lock (listeners)
            {
                listeners.Clear();
            }

            Debug.Log("Network disconnected");
        }
    }

    public void sendMessageToAll(string address, params object[] values)
    {
        foreach(string receiver in clients.Keys) {
            sendMessage(receiver, address, values);
        }
    }

    public void sendMessage(string receiver, string address, params object[] values)
    {
        OSCMessage packet = new OSCMessage(address);
        if (values != null && values.Length > 0) {
            for (int i = 0; i < values.Length; i++)
            {
                packet.Append(values[i]);
            }
        }
        OSCTransmitter client = null;
        if (clients.TryGetValue(receiver, out client))
        {
            client.Send(packet);
        }
    }

    private void listen()
    {
        while (connected)
        {
            try {
                OSCPacket packet = receiver.Receive();

                if (packet != null) {

                    if (packet.IsBundle()) {
                        handleBundle((OSCBundle)packet);
                    } else {
                        handleMessage((OSCMessage)packet);
                    }
                }
            } catch (ThreadAbortException e) {
                //Ignore
                Debug.Log("Thread aborted:" + e);
            } catch (System.Exception e) {
                Debug.Log("Exception in listen(): " + e);
            }
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

    override public void setTuioControl(TUIOControl control)
    {
        tuioControl = control;
    }

    private void handleMessage(OSCMessage message)
    {
        string address = message.Address;

        if(tuioControl != null && address.StartsWith("/tuio")) {
            tuioControl.processMessage(message);
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
