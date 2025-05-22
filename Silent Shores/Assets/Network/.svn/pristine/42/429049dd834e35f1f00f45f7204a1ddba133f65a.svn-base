// Janus Kristensen / cavi.dk
// Rolf Bagge / cavi.dk
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TUIO;
using OSC.NET;

[System.Serializable]
public class ObjectPrefabMapping {
	public int objectID;
	public ObjectActionListener prefab;
	public GameObject optionalParent;
}
[System.Serializable]
public class Object3DPrefabMapping {
	public int objectID;
	public Object3DActionListener prefab;
	public GameObject optionalParent;
}
[System.Serializable]
public class CursorPrefabMapping {
	public CursorActionListener prefab;
	public GameObject optionalParent;
}
[System.Serializable]
public class Cursor3DPrefabMapping {
	public Cursor3DActionListener prefab;
	public GameObject optionalParent;
}
[System.Serializable]
public class BlobPrefabMapping {
	public BlobActionListener prefab;
	public GameObject optionalParent;
}

public class TUIOControl : MonoBehaviour, TuioListener {
	public ObjectPrefabMapping[] tuioObjectPrefabs;
	public Object3DPrefabMapping[] tuio3DObjectPrefabs;
	public CursorPrefabMapping tuioCursorPrefab;
	public Cursor3DPrefabMapping tuio3DCursorPrefab;
	public BlobPrefabMapping tuioBlobPrefab;

	private TuioClient client;
	private Dictionary<long, BlobActionListener> blobInstances;
	private Dictionary<long, CursorActionListener> cursorInstances;
	private Dictionary<long, Cursor3DActionListener> cursor3DInstances;
	private Dictionary<int, ObjectActionListener> objectInstances;
	private Dictionary<int, Object3DActionListener> object3DInstances;
	
	private Queue<TUIOEvent> events;

	/**
		* These chars was sponsered by Rolf
		* ; < > " ' ' ` : ? } { [ ]
		**/
	void Start () {
        //Called after OnEnable, so dont use for anything that is connected with stuff in OnEnable
    }
	
	void OnEnable(){
        //Setup in enable instead of Start
        blobInstances = new Dictionary<long, BlobActionListener>();
        cursorInstances = new Dictionary<long, CursorActionListener>();
        cursor3DInstances = new Dictionary<long, Cursor3DActionListener>();
        objectInstances = new Dictionary<int, ObjectActionListener>();
        object3DInstances = new Dictionary<int, Object3DActionListener>();
        events = new Queue<TUIOEvent>();

		Debug.Log("Creating new TUIO client");		
		client = new TuioClient();
		client.addTuioListener(this);
        Debug.Log("CAVI Uber TUIO v4 enabled for TUIO");
	}
	
	void OnDisable(){
        if(client != null) {
			Debug.Log("Unregistering TUIO listener");
            client.removeTuioListener(this);
		}	
	}
	
    public void processMessage(OSCMessage message)
    {
        client.processMessage(message);
    }


    public int getNumOfBlobs(){
		return blobInstances.Count;
	}
	public int getNumOfObjects(){
		return objectInstances.Count;
	}
	public int getNumOf3DObjects(){
		return object3DInstances.Count;
	}
	public int getNumOfCursors(){
		return cursorInstances.Count;
	}
	public int getNumOf3DCursors(){
		return cursor3DInstances.Count;
	}
	
	void Update(){
		// Run all events that have arrived since last frame
		lock(events) {
			while (events.Count > 0){
                TUIOEvent thisEvent = events.Dequeue();
                switch (thisEvent.getType())
                {
                    case TUIOEvent.Type.ADD_BLOB:
                        addTuioBlobImpl((TuioBlob)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.UPDATE_BLOB:
                        updateTuioBlobImpl((TuioBlob)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.REMOVE_BLOB:
                        removeTuioBlobImpl((TuioBlob)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.ADD_OBJECT:
                        addTuioObjectImpl((TuioObject)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.UPDATE_OBJECT:
                        updateTuioObjectImpl((TuioObject)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.REMOVE_OBJECT:
                        removeTuioObjectImpl((TuioObject)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.ADD_3DOBJECT:
                        addTuio3DObjectImpl((Tuio3DObject)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.UPDATE_3DOBJECT:
                        updateTuio3DObjectImpl((Tuio3DObject)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.REMOVE_3DOBJECT:
                        removeTuio3DObjectImpl((Tuio3DObject)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.ADD_CURSOR:
                        addTuioCursorImpl((TuioCursor)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.UPDATE_CURSOR:
                        updateTuioCursorImpl((TuioCursor)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.REMOVE_CURSOR:
                        removeTuioCursorImpl((TuioCursor)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.ADD_3DCURSOR:
                        addTuio3DCursorImpl((Tuio3DCursor)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.UPDATE_3DCURSOR:
                        updateTuio3DCursorImpl((Tuio3DCursor)thisEvent.getEventObject());
                        break;
                    case TUIOEvent.Type.REMOVE_3DCURSOR:
                        removeTuio3DCursorImpl((Tuio3DCursor)thisEvent.getEventObject());
                        break;
                }
            }
		}
	}
	
	public void addTuioObjectImpl(TuioObject o) {		
		foreach(ObjectPrefabMapping mapping in tuioObjectPrefabs) {
			if(mapping.objectID == o.getSymbolID()) {
				ObjectActionListener listener = null;
				if (objectInstances.TryGetValue(o.getSymbolID(), out listener)){
					Destroy(listener.gameObject);
					objectInstances.Remove(o.getSymbolID());
				}
				if (mapping.prefab!=null){
					listener = (ObjectActionListener) Instantiate(mapping.prefab, mapping.prefab.transform.localPosition, mapping.prefab.transform.localRotation);
					objectInstances.Add(o.getSymbolID(), listener);		
					if (mapping.optionalParent!=null){
						listener.transform.parent = mapping.optionalParent.transform;
					}
					updateTuioObjectImpl(o);
				}
            }
		}		
	}
	public void addTuio3DObjectImpl(Tuio3DObject o) {		
		foreach(Object3DPrefabMapping mapping in tuio3DObjectPrefabs) {
			if(mapping.objectID == o.getSymbolID()) {
				Object3DActionListener listener = null;
				if (object3DInstances.TryGetValue(o.getSymbolID(), out listener)){
					Destroy(listener.gameObject);
					object3DInstances.Remove(o.getSymbolID());
				}
				if (mapping.prefab!=null){
					listener = (Object3DActionListener) Instantiate(mapping.prefab, mapping.prefab.transform.localPosition, mapping.prefab.transform.localRotation);
					object3DInstances.Add(o.getSymbolID(), listener);		
					if (mapping.optionalParent!=null){
						listener.transform.parent = mapping.optionalParent.transform;
					}
					updateTuio3DObjectImpl(o);
				}
			}
		}		
	}		
	public void updateTuioObjectImpl(TuioObject o) {
		if (objectInstances.ContainsKey(o.getSymbolID())){
			ObjectActionListener listener = null;
			if (objectInstances.TryGetValue(o.getSymbolID(), out listener)){
				listener.updateTuioObject(o);
			}
		}		
	}	
	public void updateTuio3DObjectImpl(Tuio3DObject o) {
		if (object3DInstances.ContainsKey(o.getSymbolID())){
			Object3DActionListener listener = null;
			if (object3DInstances.TryGetValue(o.getSymbolID(), out listener)){
				listener.updateTuioObject(o);
			}
		}		
	}	
	public void removeTuioObjectImpl(TuioObject o) {
		ObjectActionListener listener = null;
		if (objectInstances.TryGetValue(o.getSymbolID(), out listener)){
			Destroy(listener.gameObject);
			objectInstances.Remove(o.getSymbolID());
		} else {
			Debug.Log("Trying to remove object which wasnt there in the first place...");
		}
	}
	public void removeTuio3DObjectImpl(Tuio3DObject o) {
		Object3DActionListener listener = null;
		if (object3DInstances.TryGetValue(o.getSymbolID(), out listener)){
			Destroy(listener.gameObject);
			object3DInstances.Remove(o.getSymbolID());
		} else {
			Debug.Log("Trying to remove object which wasnt there in the first place...");
		}
	}
	
	public void addTuioCursorImpl(TuioCursor c) {
		CursorActionListener listener = null;
		if (cursorInstances.TryGetValue(c.getSessionID(), out listener)){
			Destroy(listener.gameObject);
			cursorInstances.Remove(c.getSessionID());
		}
		if (tuioCursorPrefab.prefab!=null){
			listener = (CursorActionListener) Instantiate(tuioCursorPrefab.prefab, tuioCursorPrefab.prefab.transform.localPosition, tuioCursorPrefab.prefab.transform.localRotation);
			cursorInstances.Add(c.getSessionID(), listener);		
			if (tuioCursorPrefab.optionalParent!=null){
				Debug.Log(listener);
				Debug.Log(listener.transform);
				Debug.Log(listener.transform.parent);
				listener.transform.parent = tuioCursorPrefab.optionalParent.transform;
			}
			updateTuioCursorImpl(c);			
		}		
	}
	public void addTuio3DCursorImpl(Tuio3DCursor c) {
		Cursor3DActionListener listener = null;
		if (cursor3DInstances.TryGetValue(c.getSessionID(), out listener)){
			Destroy(listener.gameObject);
			cursor3DInstances.Remove(c.getSessionID());
		}
		if (tuio3DCursorPrefab.prefab!=null){
			listener = (Cursor3DActionListener) Instantiate(tuio3DCursorPrefab.prefab, tuio3DCursorPrefab.prefab.transform.localPosition, tuio3DCursorPrefab.prefab.transform.localRotation);
			cursor3DInstances.Add(c.getSessionID(), listener);		
			if (tuio3DCursorPrefab.optionalParent!=null){
				Debug.Log(listener);
				Debug.Log(listener.transform);
				Debug.Log(listener.transform.parent);
				listener.transform.parent = tuio3DCursorPrefab.optionalParent.transform;
			}
			updateTuio3DCursorImpl(c);			
		}		
	}
	public void updateTuioCursorImpl(TuioCursor c) {
		if (cursorInstances.ContainsKey(c.getSessionID())){
			CursorActionListener listener = null;
			if (cursorInstances.TryGetValue(c.getSessionID(), out listener)){
				listener.updateTuioCursor(c);
			}
		}		
	}
	public void updateTuio3DCursorImpl(Tuio3DCursor c) {
		if (cursor3DInstances.ContainsKey(c.getSessionID())){
			Cursor3DActionListener listener = null;
			if (cursor3DInstances.TryGetValue(c.getSessionID(), out listener)){
				listener.updateTuioCursor(c);
			}
		}		
	}
	public void removeTuioCursorImpl(TuioCursor c) {
		CursorActionListener listener = null;
		if (cursorInstances.TryGetValue(c.getSessionID(), out listener)){
			Destroy(listener.gameObject);
			cursorInstances.Remove(c.getSessionID());
		} else {
			Debug.Log("Trying to remove cursor which wasnt there in the first place...");
		}
	}
	public void removeTuio3DCursorImpl(Tuio3DCursor c) {
		Cursor3DActionListener listener = null;
		if (cursor3DInstances.TryGetValue(c.getSessionID(), out listener)){
			Destroy(listener.gameObject);
			cursor3DInstances.Remove(c.getSessionID());
		} else {
			Debug.Log("Trying to remove cursor which wasnt there in the first place...");
		}
	}
	public void removeTuioBlobImpl(TuioBlob b) {
		BlobActionListener listener = null;
		if (blobInstances.TryGetValue(b.getSessionID(), out listener)){
			Destroy(listener.gameObject);
			blobInstances.Remove(b.getSessionID());
		} else {
			Debug.Log("Trying to remove blob which wasnt there in the first place...");
		}
	}
	public void updateTuioBlobImpl(TuioBlob b) {
		if (blobInstances.ContainsKey(b.getSessionID())){
			BlobActionListener listener = null;
			if (blobInstances.TryGetValue(b.getSessionID(), out listener)){
				listener.updateTuioBlob(b);
			}
		}		
	}
	public void addTuioBlobImpl(TuioBlob b) {
		BlobActionListener listener = null;
		if (blobInstances.TryGetValue(b.getSessionID(), out listener)){
			Destroy(listener.gameObject);
			blobInstances.Remove(b.getSessionID());
		}
		if (tuioBlobPrefab.prefab!=null){
			listener = (BlobActionListener) Instantiate(tuioBlobPrefab.prefab, tuioBlobPrefab.prefab.transform.localPosition, tuioBlobPrefab.prefab.transform.localRotation);
			blobInstances.Add(b.getSessionID(), listener);		
			if (tuioBlobPrefab.optionalParent!=null){
				Debug.Log(listener);
				Debug.Log(listener.transform);
				Debug.Log(listener.transform.parent);
				listener.transform.parent = tuioBlobPrefab.optionalParent.transform;
			}
			updateTuioBlobImpl(b);
		}		
	}


	public void refresh(TUIO.TuioTime timestamp) {}	
		
	// TUIO event queues
	public void addTuioBlob(TuioBlob b) {	
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.ADD_BLOB, b));
		}
	}
	public void removeTuioBlob(TuioBlob b) {	
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.REMOVE_BLOB, b));
		}
	}
	public void updateTuioBlob(TuioBlob b) {	
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.UPDATE_BLOB, b));
		}
	}

	public void addTuioObject(TuioObject o) {	
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.ADD_OBJECT, o));
		}
	}
	public void addTuio3DObject(Tuio3DObject o) {	
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.ADD_3DOBJECT, o));
		}
	}
	public void updateTuioObject(TuioObject o) {
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.UPDATE_OBJECT, o));
		}
	}
	public void updateTuio3DObject(Tuio3DObject o) {
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.UPDATE_3DOBJECT, o));
		}
	}
	public void removeTuioObject(TuioObject o) {
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.REMOVE_OBJECT, o));
		}
	}
	public void removeTuio3DObject(Tuio3DObject o) {
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.REMOVE_3DOBJECT, o));
		}
	}
	public void addTuioCursor(TuioCursor c) {
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.ADD_CURSOR, c));
		}
	}
	public void addTuio3DCursor(Tuio3DCursor c) {
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.ADD_3DCURSOR, c));
		}
	}
	public void updateTuioCursor(TuioCursor c) {
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.UPDATE_CURSOR, c));
		}
	}
	public void updateTuio3DCursor(Tuio3DCursor c) {
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.UPDATE_3DCURSOR, c));
		}
	}
	public void removeTuioCursor(TuioCursor c) {
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.REMOVE_CURSOR, c));
		}
	}
	public void removeTuio3DCursor(Tuio3DCursor c) {
		lock(events) {
			events.Enqueue(new TUIOEvent(TUIOEvent.Type.REMOVE_3DCURSOR, c));
		}
	}
}
