using UnityEngine;
using System.Collections;
using TUIO;

public abstract class CursorActionListener : MonoBehaviour {
	public abstract void updateTuioCursor(TuioCursor c);
}
