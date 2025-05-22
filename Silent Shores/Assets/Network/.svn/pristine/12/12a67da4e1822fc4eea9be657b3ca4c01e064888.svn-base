using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkProtocol : MonoBehaviour {
    abstract public void setTuioControl(TUIOControl tuio);
    abstract public void addListener(NetworkListener listener);
    abstract public void removeAllListeners();
    abstract public void removeListener(NetworkListener listener);
}

public class NetworkController : MonoBehaviour {
    public TUIOControl tuioController;
    private HashSet<NetworkListener> listeners = new HashSet<NetworkListener>();
    private static NetworkController cachedController = null;

    public void OnEnable() {
	refreshMapping();
    }

    private NetworkProtocol[] getProtocols(){
	return GetComponents<NetworkProtocol>();
    }

    private void refreshMapping(){
        foreach (NetworkProtocol protocol in getProtocols()) {
	    protocol.removeAllListeners();
            foreach(NetworkListener listener in listeners) {
                protocol.addListener(listener);
            }
        }

        if(tuioController == null){
            //No TUIOControl added to script, check if one is on this gameobject
            tuioController = GetComponent<TUIOControl>();
            if (tuioController != null){
		Debug.Log("Found and registered TUIO: " + tuioController);
	    }
        }

        if(tuioController != null && tuioController.enabled){
            foreach(NetworkProtocol protocol in getProtocols()) {
                protocol.setTuioControl(tuioController);
            }
        }
    }

    public void addListener(NetworkListener listener){
	if (listener==null) {
	    Debug.Log("Cannot add null to list of network listeners, ignoring", this);
	    return;
	}
	listeners.Add(listener);
	refreshMapping();
    }

    public void removeListener(NetworkListener listener){
	listeners.Remove(listener);
	refreshMapping();
    }

    public HashSet<NetworkListener> getListeners()
    {
        return listeners;
    }
    private void OnDisable(){
        cachedController = null;
    }

    public static NetworkController getController(){
        if(cachedController == null){
            cachedController = Object.FindAnyObjectByType<NetworkController>();
        }

        return cachedController;
    }

    public class OSCValue {
        private object value;

        public OSCValue(object value){
            this.value = value;
        }

        public object getRaw(){
            return value;
        }

        public string getString(){
            return value.ToString();
        }

        public float getFloat() {
            return float.Parse(getString().Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
        }

        public int getInt() {
            return int.Parse(getString());
        }

        public long getLong() {
            return long.Parse(getString());
        }

        public double getDouble() {
            return double.Parse(getString().Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
