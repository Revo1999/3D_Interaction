using TUIO;

public class TUIOEvent {
	public enum Type { ADD_OBJECT, ADD_3DOBJECT, ADD_CURSOR, ADD_3DCURSOR, UPDATE_OBJECT, UPDATE_3DOBJECT, UPDATE_CURSOR, UPDATE_3DCURSOR, REMOVE_OBJECT, REMOVE_3DOBJECT, REMOVE_CURSOR, REMOVE_3DCURSOR, ADD_BLOB, UPDATE_BLOB, REMOVE_BLOB};
	private Type type;
	private Tuio3DPoint eventObject;
	
	public TUIOEvent(Type type, Tuio3DPoint eventObject){
		this.type = type;
		this.eventObject = eventObject;
	}
	
	public Tuio3DPoint getEventObject(){
		return eventObject;
	}
	
	public Type getType(){
		return type;
	}
}
