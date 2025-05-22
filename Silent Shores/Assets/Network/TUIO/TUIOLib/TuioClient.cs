/*
	TUIO C# Library - part of the reacTIVision project
	http://reactivision.sourceforge.net/

	Copyright (c) 2005-2009 Martin Kaltenbrunner <mkalten@iua.upf.edu>

	This program is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
*/

using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OSC.NET;

namespace TUIO
{
	/**
	 * The TuioClient class is the central TUIO protocol decoder component. It provides a simple callback infrastructure using the {@link TuioListener} interface.
	 * In order to receive and decode TUIO messages an instance of TuioClient needs to be created. The TuioClient instance then generates TUIO events
	 * which are broadcasted to all registered classes that implement the {@link TuioListener} interface.<P>
	 * <code>
	 * TuioClient client = new TuioClient();<br/>
	 * client.addTuioListener(myTuioListener);<br/>
	 * client.start();<br/>
	 * </code>
	 *
	 * @author Martin Kaltenbrunner
	 * @version 1.4
	 */
	public class TuioClient
	{
		// 2D objects
		private object objectSync = new object();
		private Dictionary<long,TuioObject> objectList = new Dictionary<long,TuioObject>(32);
		private List<long> aliveObjectList = new List<long>(32);
		private List<long> newObjectList = new List<long>(32);
		private List<TuioObject> frameObjects = new List<TuioObject>(32);

		// 3D objects
		private object object3DSync = new object();
		private Dictionary<long,Tuio3DObject> object3DList = new Dictionary<long,Tuio3DObject>(32);
		private List<long> alive3DObjectList = new List<long>(32);
		private List<long> new3DObjectList = new List<long>(32);
		private List<Tuio3DObject> frame3DObjects = new List<Tuio3DObject>(32);
		
		// 2D cursors
		private object cursorSync = new object();
		private Dictionary<long,TuioCursor> cursorList = new Dictionary<long,TuioCursor>(32);
		private List<long> aliveCursorList = new List<long>(32);
		private List<long> newCursorList = new List<long>(32);
		private List<TuioCursor> frameCursors = new List<TuioCursor>(32);
		private List<TuioCursor> freeCursorList = new List<TuioCursor>();
		private int maxCursorID = -1;

		// 3D cursors
		private object cursor3DSync = new object();
		private Dictionary<long,Tuio3DCursor> cursor3DList = new Dictionary<long,Tuio3DCursor>(32);
		private List<long> alive3DCursorList = new List<long>(32);
		private List<long> new3DCursorList = new List<long>(32);
		private List<Tuio3DCursor> frame3DCursors = new List<Tuio3DCursor>(32);
		private List<Tuio3DCursor> free3DCursorList = new List<Tuio3DCursor>();
		private int max3DCursorID = -1;

		//2D Blobs
		private object blobSync = new object();
		private Dictionary<long,TuioBlob> blobList = new Dictionary<long,TuioBlob>(32);
		private List<long> aliveBlobList = new List<long>(32);
		private List<long> newBlobList = new List<long>(32);
		private List<TuioBlob> frameBlobs = new List<TuioBlob>(32);

		private int currentFrame = 0;
		private TuioTime currentTime;

		private List<TuioListener> listenerList = new List<TuioListener>();

		/**
		 * The default constructor creates a client that listens to the default TUIO port 3333
		 */
		public TuioClient() {}

		/**
		 * The TuioClient stops listening to TUIO messages on the configured UDP port
		 */
		public void clean() {
            try
            {
                lock (this)
                {
                    aliveObjectList.Clear();
                    alive3DObjectList.Clear();
                    aliveCursorList.Clear();
                    aliveBlobList.Clear();

                    objectList.Clear();
                    object3DList.Clear();
                    cursorList.Clear();
                    blobList.Clear();

                    frameObjects.Clear();
                    frame3DObjects.Clear();
                    frameCursors.Clear();
                    frameBlobs.Clear();

                    freeCursorList.Clear();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error disconnecting: "+e);
            }
		}

		/**
		 * The OSC callback method where all TUIO messages are received and decoded
		 * and where the TUIO event callbacks are dispatched
		 *
		 * @param message	the received OSC message
		 */
		public void processMessage(OSCMessage message) {
			string address = message.Address;
			ArrayList args = message.Values;
			string command = (string)args[0];

			if (address == "/tuio/2Dobj") {
								if (command == "set") {

										long s_id = (int)args [1];
										int f_id = (int)args [2];
										float xpos = (float)args [3];
										float ypos = (float)args [4];
										float angle = (float)args [5];
										float xspeed = (float)args [6];
										float yspeed = (float)args [7];
										float rspeed = (float)args [8];
										float maccel = (float)args [9];
										float raccel = (float)args [10];

										lock (objectSync) {
												if (!objectList.ContainsKey (s_id)) {
														TuioObject addObject = new TuioObject (s_id, f_id, xpos, ypos, angle);
														frameObjects.Add (addObject);
												} else {
														TuioObject tobj = objectList [s_id];
														if (tobj == null)
																return;
														if ((tobj.getX () != xpos) || (tobj.getY () != ypos) || (tobj.getAngle () != angle) || (tobj.getXSpeed () != xspeed) || (tobj.getYSpeed () != yspeed) || (tobj.getRotationSpeed () != rspeed) || (tobj.getMotionAccel () != maccel) || (tobj.getRotationAccel () != raccel)) {
								
																TuioObject updateObject = new TuioObject (s_id, f_id, xpos, ypos, angle);
																updateObject.update (xpos, ypos, angle, xspeed, yspeed, rspeed, maccel, raccel);
																frameObjects.Add (updateObject);
														}
												}
										}

								} else if (command == "alive") {

										newObjectList.Clear ();
										for (int i=1; i<args.Count; i++) {
												// get the message content
												long s_id = (int)args [i];
												newObjectList.Add (s_id);
												// reduce the object list to the lost objects
												if (aliveObjectList.Contains (s_id))
														aliveObjectList.Remove (s_id);
										}

										// remove the remaining objects
										lock (objectSync) {
												for (int i=0; i<aliveObjectList.Count; i++) {
														long s_id = aliveObjectList [i];
														TuioObject removeObject = objectList [s_id];
														removeObject.remove (currentTime);
														frameObjects.Add (removeObject);
												}
										}

								} else if (command == "fseq") {
										int fseq = (int)args [1];
										bool lateFrame = false;

										if (fseq > 0) {
												if (fseq > currentFrame)
														currentTime = TuioTime.getSessionTime ();
												if ((fseq >= currentFrame) || ((currentFrame - fseq) > 100))
														currentFrame = fseq;
												else
														lateFrame = true;
										} else if ((TuioTime.getSessionTime ().getTotalMilliseconds () - currentTime.getTotalMilliseconds ()) > 100) {
												currentTime = TuioTime.getSessionTime ();
										}

										if (!lateFrame) {

												IEnumerator<TuioObject> frameEnum = frameObjects.GetEnumerator ();
												while (frameEnum.MoveNext()) {
														TuioObject tobj = frameEnum.Current;

														switch (tobj.getTuioState ()) {
														case TuioObject.TUIO_REMOVED:
																TuioObject removeObject = tobj;
																removeObject.remove (currentTime);

                                                                foreach (TuioListener listener in listenerList)
                                                                {
																    listener.removeTuioObject (removeObject);
                                                                }
																lock (objectSync) {
																		objectList.Remove (removeObject.getSessionID ());
																}
																break;
														case TuioObject.TUIO_ADDED:
																TuioObject addObject = new TuioObject (currentTime, tobj.getSessionID (), tobj.getSymbolID (), tobj.getX (), tobj.getY (), tobj.getAngle ());
																lock (objectSync) {
																		objectList[addObject.getSessionID ()] = addObject;
																}
                                                                foreach (TuioListener listener in listenerList)
                                                                {
                                                                    listener.addTuioObject(addObject);
																}
																break;
														default:
																TuioObject updateObject = getTuioObject (tobj.getSessionID ());
																if ((tobj.getX () != updateObject.getX () && tobj.getXSpeed () == 0) || (tobj.getY () != updateObject.getY () && tobj.getYSpeed () == 0))
																		updateObject.update (currentTime, tobj.getX (), tobj.getY (), tobj.getAngle ());
																else
																		updateObject.update (currentTime, tobj.getX (), tobj.getY (), tobj.getAngle (), tobj.getXSpeed (), tobj.getYSpeed (), tobj.getRotationSpeed (), tobj.getMotionAccel (), tobj.getRotationAccel ());

                                                                foreach (TuioListener listener in listenerList)
                                                                {
                                                                    listener.updateTuioObject(updateObject);
																}
																break;
														}
												}

                                                foreach (TuioListener listener in listenerList)
                                                {
                                                    listener.refresh(new TuioTime(currentTime));
												}

												List<long> buffer = aliveObjectList;
												aliveObjectList = newObjectList;
												// recycling the List
												newObjectList = buffer;
										}
										frameObjects.Clear ();
								}
			} else if (address == "/tuio/2Dblb") {
				if (command == "set") {
					long s_id = (int)args[1];
					float xpos = (float)args[2];
					float ypos = (float)args[3];
					float angle = (float)args[4];
					float width = (float)args[5];
					float height = (float)args[6];
					float area = (float)args[7];
					float xspeed = (float)args[8];
					float yspeed = (float)args[9];
					float rspeed = (float)args[10];
					float maccel = (float)args[11];
					float raccel = (float)args[12];

					lock(blobSync) {
						if (!blobList.ContainsKey(s_id)) {
							TuioBlob addBlob = new TuioBlob(s_id,xpos,ypos,angle,width,height,area);
							frameBlobs.Add(addBlob);
						} else {
							TuioBlob tblb = blobList[s_id];
							if (tblb==null) return;
							if((tblb.getX()!=xpos) || (tblb.getY()!=ypos) || (tblb.getAngle()!=angle) || (tblb.getXSpeed()!=xspeed) || (tblb.getYSpeed()!=yspeed) || (tblb.getRotationSpeed()!=rspeed) || (tblb.getMotionAccel()!=maccel) || (tblb.getRotationAccel()!=raccel) || (tblb.getWidth() != width) || (tblb.getHeight() != height) || (tblb.getArea() != area)) {
								
								TuioBlob updateBlob = new TuioBlob(s_id,xpos,ypos,angle,width,height,area);
								updateBlob.update(xpos,ypos,angle,width,height,area,xspeed,yspeed,rspeed,maccel,raccel);
								frameBlobs.Add(updateBlob);
							}
						}
					}
					
				} else if (command == "alive") {

					newBlobList.Clear();
					for (int i=1;i<args.Count;i++) {
						// get the message content
						long s_id = (int)args[i];
						newBlobList.Add(s_id);
						// reduce the blob list to the lost objects
						if (aliveBlobList.Contains(s_id))
							aliveBlobList.Remove(s_id);
					}
					
					// remove the remaining objects
					lock(blobSync) {
						for (int i=0;i<aliveBlobList.Count;i++) {
							long s_id = aliveBlobList[i];
							TuioBlob removeBlob = blobList[s_id];
							removeBlob.remove(currentTime);
							frameBlobs.Add(removeBlob);
						}
					}
					
				} else if (command=="fseq") {
					int fseq = (int)args[1];
					bool lateFrame = false;
					
					if (fseq>0) {
						if (fseq>currentFrame) currentTime = TuioTime.getSessionTime();
						if ((fseq>=currentFrame) || ((currentFrame-fseq)>100)) currentFrame = fseq;
						else lateFrame = true;
					} else if ((TuioTime.getSessionTime().getTotalMilliseconds()-currentTime.getTotalMilliseconds())>100) {
						currentTime = TuioTime.getSessionTime();
					}

					if (!lateFrame) {
						IEnumerator<TuioBlob> frameEnum = frameBlobs.GetEnumerator();
						while(frameEnum.MoveNext()) {
							TuioBlob tblb = frameEnum.Current;
							
							switch (tblb.getTuioState()) {
								case TuioBlob.TUIO_REMOVED:
									TuioBlob removeBlob = tblb;
									removeBlob.remove(currentTime);

                                    foreach (TuioListener listener in listenerList)
                                    {
                                        listener.removeTuioBlob(removeBlob);
									}
									lock(blobSync) {
										blobList.Remove(removeBlob.getSessionID());
									}
									break;
								case TuioBlob.TUIO_ADDED:
									TuioBlob addBlob = new TuioBlob(currentTime,tblb.getSessionID(),tblb.getX(),tblb.getY(),tblb.getAngle(), tblb.getWidth(), tblb.getHeight(), tblb.getArea());
									lock(blobSync) {
										blobList[addBlob.getSessionID()] = addBlob;
									}
                                    foreach (TuioListener listener in listenerList)
                                    {
                                        listener.addTuioBlob(addBlob);
									}
									break;
								default:
									TuioBlob updateBlob = getTuioBlob(tblb.getSessionID());
									if ( (tblb.getX()!=updateBlob.getX() && tblb.getXSpeed()==0) || (tblb.getY()!=updateBlob.getY() && tblb.getYSpeed()==0) ) {
										updateBlob.update(currentTime,tblb.getX(),tblb.getY(),tblb.getAngle(),tblb.getWidth (), tblb.getHeight (), tblb.getArea ());
									} else {
										updateBlob.update(currentTime,tblb.getX(),tblb.getY(),tblb.getAngle(),tblb.getWidth (), tblb.getHeight (), tblb.getArea (),tblb.getXSpeed(),tblb.getYSpeed(),tblb.getRotationSpeed(),tblb.getMotionAccel(),tblb.getRotationAccel());
									}

                                    foreach (TuioListener listener in listenerList)
                                    {
                                        listener.updateTuioBlob(updateBlob);
									}
									break;
							}
						}

                        foreach (TuioListener listener in listenerList)
                        {
                            listener.refresh(new TuioTime(currentTime));
						}
						
						List<long> buffer = aliveBlobList;
						aliveBlobList = newBlobList;
						// recycling the List
						newBlobList = buffer;
					}
					frameBlobs.Clear();
				}


			} else if (address == "/tuio/3Dobj") {
				if (command == "set") {

					long s_id = (int)args[1];
					int f_id = (int)args[2];
					float xpos = (float)args[3];
					float ypos = (float)args[4];
					float zpos = (float)args[5];
					
					float angleX = (float)args[6];
					float angleY = (float)args[7];
					float angleZ = (float)args[8];					
					
					float xspeed = (float)args[9];
					float yspeed = (float)args[10];
					float zspeed = (float)args[11];
					
					float rspeedX = (float)args[12];
					float rspeedY = (float)args[13];
					float rspeedZ = (float)args[14];
					
					float maccel = (float)args[15];
					float raccel = (float)args[16];

					lock(objectSync) {
						if (!object3DList.ContainsKey(s_id)) {
							Tuio3DObject addObject = new Tuio3DObject(s_id,f_id,xpos,ypos,zpos,angleX,angleY,angleZ);
							frame3DObjects.Add(addObject);
						} else {
							Tuio3DObject tobj = object3DList[s_id];
							if (tobj==null) return;
							if((tobj.getX()!=xpos) || (tobj.getY()!=ypos) || (tobj.getZ()!=zpos) || (tobj.getAngleX()!=angleX) || (tobj.getAngleY()!=angleY) || (tobj.getAngleZ()!=angleZ) || (tobj.getXSpeed()!=xspeed) || (tobj.getYSpeed()!=yspeed) || (tobj.getZSpeed()!=zspeed) || (tobj.getRotationSpeedX()!=rspeedX) || (tobj.getRotationSpeedY()!=rspeedY)  || (tobj.getRotationSpeedZ()!=rspeedZ) || (tobj.getMotionAccel()!=maccel) || (tobj.getRotationAccel()!=raccel)) {
								Tuio3DObject updateObject = new Tuio3DObject(s_id,f_id,xpos,ypos,zpos,angleX,angleY,angleZ);
								updateObject.update(xpos,ypos,zpos,angleX,angleY,angleZ,xspeed,yspeed,zspeed,rspeedX,rspeedY,rspeedZ,maccel,raccel);
								frame3DObjects.Add(updateObject);
							}
						}
					}
				} else if (command == "alive") {
					new3DObjectList.Clear();
					for (int i=1;i<args.Count;i++) {
						// get the message content
						long s_id = (int)args[i];
						new3DObjectList.Add(s_id);
						// reduce the object list to the lost objects
						if (alive3DObjectList.Contains(s_id))
							 alive3DObjectList.Remove(s_id);
					}

					// remove the remaining objects
					lock(object3DSync) {
						for (int i=0;i<alive3DObjectList.Count;i++) {
							long s_id = alive3DObjectList[i];
							Tuio3DObject removeObject = object3DList[s_id];
							removeObject.remove(currentTime);
							frame3DObjects.Add(removeObject);
						}
					}
				} else if (command=="fseq") {
					int fseq = (int)args[1];
					bool lateFrame = false;

					if (fseq>0) {
						if (fseq>currentFrame) currentTime = TuioTime.getSessionTime();
						if ((fseq>=currentFrame) || ((currentFrame-fseq)>100)) currentFrame = fseq;
						else lateFrame = true;
					} else if ((TuioTime.getSessionTime().getTotalMilliseconds()-currentTime.getTotalMilliseconds())>100) {
						currentTime = TuioTime.getSessionTime();
					}

					if (!lateFrame) {
						IEnumerator<Tuio3DObject> frameEnum = frame3DObjects.GetEnumerator();
						while(frameEnum.MoveNext()) {
							Tuio3DObject tobj = frameEnum.Current;

							switch (tobj.getTuioState()) {
								case Tuio3DObject.TUIO_REMOVED:
									Tuio3DObject removeObject = tobj;
									removeObject.remove(currentTime);

                                    foreach (TuioListener listener in listenerList)
                                    {
                                        listener.removeTuio3DObject(removeObject);
									}
									lock(object3DSync) {
										object3DList.Remove(removeObject.getSessionID());
									}
									break;
								case Tuio3DObject.TUIO_ADDED:
									Tuio3DObject addObject = new Tuio3DObject(currentTime,tobj.getSessionID(),tobj.getSymbolID(),tobj.getX(),tobj.getY(),tobj.getZ(),tobj.getAngleX(),tobj.getAngleY(),tobj.getAngleZ());
									lock(object3DSync) {
										object3DList[addObject.getSessionID()] = addObject;
									}
                                    foreach (TuioListener listener in listenerList)
                                    {
                                        listener.addTuio3DObject(addObject);
									}
									break;
								default:
									Tuio3DObject updateObject = getTuio3DObject(tobj.getSessionID());
									if ( (tobj.getX()!=updateObject.getX() && tobj.getXSpeed()==0) || (tobj.getY()!=updateObject.getY() && tobj.getYSpeed()==0) || (tobj.getZ()!=updateObject.getZ() && tobj.getZSpeed()==0) )
										updateObject.update(currentTime,tobj.getX(),tobj.getY(),tobj.getZ(),tobj.getAngleX(),tobj.getAngleY(),tobj.getAngleZ());
									else
										updateObject.update(currentTime,tobj.getX(),tobj.getY(),tobj.getZ(),tobj.getAngleX(),tobj.getAngleY(),tobj.getAngleZ(),tobj.getXSpeed(),tobj.getYSpeed(),tobj.getZSpeed(),tobj.getRotationSpeedX(),tobj.getRotationSpeedY(),tobj.getRotationSpeedZ(),tobj.getMotionAccel(),tobj.getRotationAccel());

                                    foreach (TuioListener listener in listenerList)
                                    {
                                        listener.updateTuio3DObject(updateObject);
									}
									break;
							}
						}

                        foreach (TuioListener listener in listenerList)
                        {
                            listener.refresh(new TuioTime(currentTime));
						}

						List<long> buffer = alive3DObjectList;
						alive3DObjectList = new3DObjectList;
						// recycling the List
						new3DObjectList = buffer;
					}
					frame3DObjects.Clear();
				}
			} else if (address == "/tuio/2Dcur") {
				if (command == "set") {

					long s_id = (int)args[1];
					float xpos = (float)args[2];
					float ypos = (float)args[3];
					float xspeed = (float)args[4];
					float yspeed = (float)args[5];
					float maccel = (float)args[6];

					lock(cursorList) {
						if (!cursorList.ContainsKey(s_id)) {

							TuioCursor addCursor = new TuioCursor(s_id,-1,xpos,ypos);
							frameCursors.Add(addCursor);

						} else {
							TuioCursor tcur = (TuioCursor)cursorList[s_id];
							if (tcur==null) return;
							if ((tcur.getX()!=xpos) || (tcur.getY()!=ypos) || (tcur.getXSpeed()!=xspeed) || (tcur.getYSpeed()!=yspeed) || (tcur.getMotionAccel()!=maccel)) {
								TuioCursor updateCursor = new TuioCursor(s_id,tcur.getCursorID(),xpos,ypos);
								updateCursor.update(xpos,ypos,xspeed,yspeed,maccel);
								frameCursors.Add(updateCursor);
							}
						}
					}

				} else if (command == "alive") {

					newCursorList.Clear();
					for (int i=1;i<args.Count;i++) {
						// get the message content
						long s_id = (int)args[i];
						newCursorList.Add(s_id);
						// reduce the cursor list to the lost cursors
						if (aliveCursorList.Contains(s_id))
							aliveCursorList.Remove(s_id);
					}

					// remove the remaining cursors
					lock(cursorSync) {
						for (int i=0;i<aliveCursorList.Count;i++) {
							long s_id = aliveCursorList[i];
							if (!cursorList.ContainsKey(s_id)) continue;
							TuioCursor removeCursor = cursorList[s_id];
 							removeCursor.remove(currentTime);
							frameCursors.Add(removeCursor);
						}
					}

				} else if (command=="fseq") {
					int fseq = (int)args[1];
					bool lateFrame = false;

					if (fseq>0) {
						if (fseq>currentFrame) currentTime = TuioTime.getSessionTime();
						if ((fseq>=currentFrame) || ((currentFrame-fseq)>100)) currentFrame = fseq;
						else lateFrame = true;
					} else if ((TuioTime.getSessionTime().getTotalMilliseconds()-currentTime.getTotalMilliseconds())>100) {
						currentTime = TuioTime.getSessionTime();
					}

					if (!lateFrame) {

						IEnumerator<TuioCursor> frameEnum = frameCursors.GetEnumerator();
						while(frameEnum.MoveNext()) {
							TuioCursor tcur = frameEnum.Current;
							switch (tcur.getTuioState()) {
								case TuioCursor.TUIO_REMOVED:
									TuioCursor removeCursor = tcur;
									removeCursor.remove(currentTime);

                                    foreach (TuioListener listener in listenerList)
                                    {
                                        listener.removeTuioCursor(removeCursor);
									}
									lock(cursorSync) {
										cursorList.Remove(removeCursor.getSessionID());

										if (removeCursor.getCursorID() == maxCursorID) {
											maxCursorID = -1;

											if (cursorList.Count > 0) {

												IEnumerator<KeyValuePair<long, TuioCursor>> clist = cursorList.GetEnumerator();
												while (clist.MoveNext()) {
													int f_id = clist.Current.Value.getCursorID();
													if (f_id > maxCursorID) maxCursorID = f_id;
												}

							 					List<TuioCursor> freeCursorBuffer = new List<TuioCursor>();
							 					IEnumerator<TuioCursor> flist = freeCursorList.GetEnumerator();
												while (flist.MoveNext()) {
								 					TuioCursor testCursor = flist.Current;
													if (testCursor.getCursorID() < maxCursorID) freeCursorBuffer.Add(testCursor);
												}
												freeCursorList = freeCursorBuffer;
											} else freeCursorList.Clear();
										} else if (removeCursor.getCursorID() < maxCursorID) freeCursorList.Add(removeCursor);
									}
									break;

							case TuioCursor.TUIO_ADDED:
								TuioCursor addCursor;
								lock(cursorSync) {
									int c_id = cursorList.Count;
									if ((cursorList.Count<=maxCursorID) && (freeCursorList.Count>0)) {
										TuioCursor closestCursor = freeCursorList[0];
										IEnumerator<TuioCursor> testList = freeCursorList.GetEnumerator();
										while(testList.MoveNext()) {
											TuioCursor testCursor = testList.Current;
											if (testCursor.getDistance(tcur)<closestCursor.getDistance(tcur)) closestCursor = testCursor;
										}
										c_id = closestCursor.getCursorID();
										freeCursorList.Remove(closestCursor);
									} else maxCursorID = c_id;

									addCursor = new TuioCursor(currentTime,tcur.getSessionID(),c_id,tcur.getX(),tcur.getY());
									cursorList[addCursor.getSessionID()] = addCursor;
								}

                                foreach (TuioListener listener in listenerList)
                                {
                                    listener.addTuioCursor(addCursor);
								}
								break;

							default:
								TuioCursor updateCursor = getTuioCursor(tcur.getSessionID());
								if ( (tcur.getX()!=updateCursor.getX() && tcur.getXSpeed()==0) || (tcur.getY()!=updateCursor.getY() && tcur.getYSpeed()==0) )
									updateCursor.update(currentTime,tcur.getX(),tcur.getY());
								else
									updateCursor.update(currentTime,tcur.getX(),tcur.getY(),tcur.getXSpeed(),tcur.getYSpeed(),tcur.getMotionAccel());

                                foreach (TuioListener listener in listenerList)
                                {
                                    listener.updateTuioCursor(updateCursor);
								}
								break;
							}
						}

                        foreach (TuioListener listener in listenerList)
                        {
                            listener.refresh(new TuioTime(currentTime));
						}

						List<long> buffer = aliveCursorList;
						aliveCursorList = newCursorList;
						// recycling the List
						newCursorList = buffer;
					}
					frameCursors.Clear();
					// vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
				}
			} else if (address == "/tuio/3Dcur") {
				if (command == "set") {
					long s_id = (int)args[1];
					float xpos = (float)args[2];
					float ypos = (float)args[3];
					float zpos = (float)args[4];
					float xspeed = (float)args[5];
					float yspeed = (float)args[6];
					float zspeed = (float)args[7];
					float maccel = 0f;//(float)args[8];

					lock(cursor3DList) {
						if (!cursor3DList.ContainsKey(s_id)) {
							Tuio3DCursor addCursor = new Tuio3DCursor(s_id,-1,xpos,ypos,zpos);
							frame3DCursors.Add(addCursor);
						} else {
							Tuio3DCursor tcur = (Tuio3DCursor)cursor3DList[s_id];
							if (tcur==null) return;
							if ((tcur.getX()!=xpos) || (tcur.getY()!=ypos) || (tcur.getZ()!=zpos) || (tcur.getXSpeed()!=xspeed) || (tcur.getYSpeed()!=yspeed) || (tcur.getZSpeed()!=zspeed) || (tcur.getMotionAccel()!=maccel)) {
								Tuio3DCursor updateCursor = new Tuio3DCursor(s_id,tcur.getCursorID(),xpos,ypos,zpos);
								updateCursor.update(xpos,ypos,zpos,xspeed,yspeed,zspeed,maccel);
								frame3DCursors.Add(updateCursor);
							}
						}
					}
				} else if (command == "alive") {
					new3DCursorList.Clear();
					for (int i=1;i<args.Count;i++) {
						// get the message content
						long s_id = (int)args[i];
						new3DCursorList.Add(s_id);
						// reduce the cursor list to the lost cursors
						if (alive3DCursorList.Contains(s_id))
							alive3DCursorList.Remove(s_id);
					}

					// remove the remaining cursors
					lock(cursor3DSync) {
						for (int i=0;i<alive3DCursorList.Count;i++) {
							long s_id = alive3DCursorList[i];
							if (!cursor3DList.ContainsKey(s_id)) continue;
							Tuio3DCursor removeCursor = cursor3DList[s_id];
 							removeCursor.remove(currentTime);
							frame3DCursors.Add(removeCursor);
						}
					}
				} else if (command=="fseq") {
					int fseq = (int)args[1];
					bool lateFrame = false;

					if (fseq>0) {
						if (fseq>currentFrame) currentTime = TuioTime.getSessionTime();
						if ((fseq>=currentFrame) || ((currentFrame-fseq)>100)) currentFrame = fseq;
						else lateFrame = true;
					} else if ((TuioTime.getSessionTime().getTotalMilliseconds()-currentTime.getTotalMilliseconds())>100) {
						currentTime = TuioTime.getSessionTime();
					}

					if (!lateFrame) {
						IEnumerator<Tuio3DCursor> frameEnum = frame3DCursors.GetEnumerator();
						while(frameEnum.MoveNext()) {
							Tuio3DCursor tcur = frameEnum.Current;
							switch (tcur.getTuioState()) {
								case Tuio3DCursor.TUIO_REMOVED:
									Tuio3DCursor removeCursor = tcur;
									removeCursor.remove(currentTime);

                                    foreach (TuioListener listener in listenerList)
                                    {
                                        listener.removeTuio3DCursor(removeCursor);
									}
									lock(cursor3DSync) {
										cursor3DList.Remove(removeCursor.getSessionID());

										if (removeCursor.getCursorID() == max3DCursorID) {
											max3DCursorID = -1;

											if (cursor3DList.Count > 0) {

												IEnumerator<KeyValuePair<long, Tuio3DCursor>> clist = cursor3DList.GetEnumerator();
												while (clist.MoveNext()) {
													int f_id = clist.Current.Value.getCursorID();
													if (f_id > max3DCursorID) max3DCursorID = f_id;
												}

							 					List<Tuio3DCursor> freeCursorBuffer = new List<Tuio3DCursor>();
							 					IEnumerator<Tuio3DCursor> flist = free3DCursorList.GetEnumerator();
												while (flist.MoveNext()) {
								 					Tuio3DCursor testCursor = flist.Current;
													if (testCursor.getCursorID() < max3DCursorID) freeCursorBuffer.Add(testCursor);
												}
												free3DCursorList = freeCursorBuffer;
											} else free3DCursorList.Clear();
										} else if (removeCursor.getCursorID() < max3DCursorID) free3DCursorList.Add(removeCursor);
									}
									break;

							case Tuio3DCursor.TUIO_ADDED:
								Tuio3DCursor addCursor;
								lock(cursor3DSync) {
									int c_id = cursor3DList.Count;
									if ((cursor3DList.Count<=max3DCursorID) && (free3DCursorList.Count>0)) {
										Tuio3DCursor closestCursor = free3DCursorList[0];
										IEnumerator<Tuio3DCursor> testList = free3DCursorList.GetEnumerator();
										while(testList.MoveNext()) {
											Tuio3DCursor testCursor = testList.Current;
											if (testCursor.getDistance(tcur)<closestCursor.getDistance(tcur)) closestCursor = testCursor;
										}
										c_id = closestCursor.getCursorID();
										free3DCursorList.Remove(closestCursor);
									} else max3DCursorID = c_id;

									addCursor = new Tuio3DCursor(currentTime,tcur.getSessionID(),c_id,tcur.getX(),tcur.getY(),tcur.getZ());
									cursor3DList[addCursor.getSessionID()] = addCursor;
								}

                                foreach (TuioListener listener in listenerList)
                                {
                                    listener.addTuio3DCursor(addCursor);
								}
								break;

							default:
								Tuio3DCursor updateCursor = getTuio3DCursor(tcur.getSessionID());
								if ( (tcur.getX()!=updateCursor.getX() && tcur.getXSpeed()==0) || (tcur.getY()!=updateCursor.getY() && tcur.getYSpeed()==0) || (tcur.getZ()!=updateCursor.getZ() && tcur.getZSpeed()==0) )
									updateCursor.update(currentTime,tcur.getX(),tcur.getY(),tcur.getZ());
								else
									updateCursor.update(currentTime,tcur.getX(),tcur.getY(),tcur.getZ(),tcur.getXSpeed(),tcur.getYSpeed(),tcur.getZSpeed(),tcur.getMotionAccel());

                                foreach (TuioListener listener in listenerList)
                                {
                                    listener.updateTuio3DCursor(updateCursor);
								}
								break;
							}
						}

                        foreach (TuioListener listener in listenerList)
                        {
                            listener.refresh(new TuioTime(currentTime));
						}

						List<long> buffer = alive3DCursorList;
						alive3DCursorList = new3DCursorList;
						// recycling the List
						new3DCursorList = buffer;
					}
					frame3DCursors.Clear();
				}
			// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
			}
		}

		/**
		 * Adds the provided TuioListener to the list of registered TUIO event listeners
		 *
		 * @param listener the TuioListener to add
		 */
		public void addTuioListener(TuioListener listener) {
			listenerList.Add(listener);
		}

		/**
		 * Removes the provided TuioListener from the list of registered TUIO event listeners
		 *
		 * @param listener the TuioListener to remove
		 */
		public void removeTuioListener(TuioListener listener) {
			listenerList.Remove(listener);
		}

		/**
		 * Removes all TuioListener from the list of registered TUIO event listeners
		 */
		public void removeAllTuioListeners() {
			listenerList.Clear();
		}

		/**
		 * Returns a Vector of all currently active TuioObjects
		 *
		 * @return a Vector of all currently active TuioObjects
		 */
		public List<TuioObject> getTuioObjects() {
			List<TuioObject> listBuffer;
			lock(objectSync) {
				listBuffer = new List<TuioObject>(objectList.Values);
			}
			return listBuffer;
		}

		/**
		 * Returns a Vector of all currently active Tuio3DObjects
		 */
		public List<Tuio3DObject> getTuio3DObjects() {
			List<Tuio3DObject> listBuffer;
			lock(object3DSync) {
				listBuffer = new List<Tuio3DObject>(object3DList.Values);
			}
			return listBuffer;
		}		
		
		/**
		 * Returns a Vector of all currently active TuioCursors
		 *
		 * @return a Vector of all currently active TuioCursors
		 */
		public List<TuioCursor> getTuioCursors() {
			List<TuioCursor> listBuffer;
			lock(cursorSync) {
				listBuffer = new List<TuioCursor>(cursorList.Values);
			}
			return listBuffer;
		}

		/**
		 * Returns a Vector of all currently active Tuio3DCursors
		 *
		 * @return a Vector of all currently active Tuio3DCursors
		 */
		public List<Tuio3DCursor> getTuio3DCursors() {
			List<Tuio3DCursor> listBuffer;
			lock(cursor3DSync) {
				listBuffer = new List<Tuio3DCursor>(cursor3DList.Values);
			}
			return listBuffer;
		}

		/**
		 * Returns the TuioObject corresponding to the provided Session ID
		 * or NULL if the Session ID does not refer to an active TuioObject
		 *
		 * @return an active TuioObject corresponding to the provided Session ID or NULL
		 */
		public TuioObject getTuioObject(long s_id) {
			TuioObject tobject = null;
			lock(objectSync) {
				objectList.TryGetValue(s_id,out tobject);
			}
			return tobject;
		}
		
		public TuioBlob getTuioBlob(long s_id) {
			TuioBlob tblb = null;
			lock(blobSync) {
				blobList.TryGetValue(s_id,out tblb);
			}
			return tblb;
		}


		/**
		 * Returns the Tuio3DObject corresponding to the provided Session ID
		 * or NULL if the Session ID does not refer to an active Tuio3DObject
		 *
		 * @return an active Tuio3DObject corresponding to the provided Session ID or NULL
		 */
		public Tuio3DObject getTuio3DObject(long s_id) {
			Tuio3DObject tobject = null;
			lock(object3DSync) {
				object3DList.TryGetValue(s_id,out tobject);
			}
			return tobject;
		}		

		/**
		 * Returns the TuioCursor corresponding to the provided Session ID
		 * or NULL if the Session ID does not refer to an active TuioCursor
		 *
		 * @return an active TuioCursor corresponding to the provided Session ID or NULL
		 */
		public TuioCursor getTuioCursor(long s_id) {
			TuioCursor tcursor = null;
			lock(cursorSync) {
				cursorList.TryGetValue(s_id, out tcursor);
			}
			return tcursor;
		}
		
		/**
		 * Returns the Tuio3DCursor corresponding to the provided Session ID
		 * or NULL if the Session ID does not refer to an active Tuio3DCursor
		 *
		 * @return an active Tuio3DCursor corresponding to the provided Session ID or NULL
		 */
		public Tuio3DCursor getTuio3DCursor(long s_id) {
			Tuio3DCursor tcursor = null;
			lock(cursor3DSync) {
				cursor3DList.TryGetValue(s_id, out tcursor);
			}
			return tcursor;
		}
    }
}