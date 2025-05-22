/*
  	TUIO C# Library - part of the reacTIVision project
	http://reactivision.sourceforge.net/
	Copyright (c) 2005-2009 Martin Kaltenbrunner <mkalten@iua.upf.edu>

    TUIO C# library extensions for 3D 
    Copyright (c) 2013 Janus B. Kristensen, CAVI, Aarhus University
	
	This program is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

using System;
using System.Collections.Generic;

namespace TUIO {
	public class Tuio3DObject : Tuio3DContainer {
		protected int symbol_id;		// The individual symbol ID number that is assigned to each Tuio3DObject.
		protected float angleX;			// The rotation angle value (euler)
		protected float angleY;			// The rotation angle value (euler)
		protected float angleZ;			// The rotation angle value (euler)
		
		protected float rotation_speedX;
		protected float rotation_speedY;
		protected float rotation_speedZ;

		protected float rotation_accel;

		public static readonly int TUIO_ROTATING = 5; // Defines the ROTATING state.

		/**
		 * This constructor takes a TuioTime argument and assigns it along with the provided
		 * Session ID, Symbol ID, X, Y and Z coordinate and angles to the newly created Tuio3DObject.
		 *
		 * @param	ttime	the TuioTime to assign
		 * @param	si	the Session ID to assign
		 * @param	sym	the Symbol ID to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Z coordinate to assign
		 * @param	a	the X-angle to assign
		 * @param	b	the Y-angle to assign
		 * @param	c	the Z-angle to assign
		 */
		public Tuio3DObject (TuioTime ttime, long si, int sym, float xp, float yp, float zp, float a, float b, float c):base(ttime, si,xp,yp,zp) {
			symbol_id = sym;
			angleX = a;
			angleY = b;
			angleZ = c;
			rotation_speedX = 0.0f;
			rotation_speedY = 0.0f;
			rotation_speedZ = 0.0f;
			rotation_accel = 0.0f;
		}

		/**
		 * This constructor takes the provided Session ID, Symbol ID, X, Y and Z-coordinates
		 * and -angles, and assigns these values to the newly created Tuio3DObject.
		 *
		 * @param	si	the Session ID to assign
		 * @param	sym	the Symbol ID to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Z coordinate to assign
		 * @param	a	the X-angle to assign
		 * @param	b	the Y-angle to assign
		 * @param	c	the Z-angle to assign
		 */
		public Tuio3DObject (long si, int sym, float xp, float yp, float zp, float a, float b, float c):base(si,xp,yp,zp) {
			symbol_id = sym;
			angleX = a;
			angleY = b;
			angleZ = c;
			rotation_speedX = 0.0f;
			rotation_speedY = 0.0f;
			rotation_speedZ = 0.0f;
			rotation_accel = 0.0f;
		}

		/**
		 * This constructor takes the attributes of the provided Tuio3DObject
		 * and assigns these values to the newly created Tuio3DObject.
		 *
		 * @param	tobj	the Tuio3DObject to assign
		 */
		public Tuio3DObject (Tuio3DObject tobj):base(tobj) {
			symbol_id = tobj.getSymbolID();
			angleX = tobj.getAngleX();
			angleY = tobj.getAngleY();
			angleZ = tobj.getAngleZ();
			rotation_speedX = 0.0f;
			rotation_speedY = 0.0f;
			rotation_speedZ = 0.0f;
			rotation_accel = 0.0f;
		}

		/**
		 * Takes a TuioTime argument and assigns it along with the provided
		 * X, Y and Z coordinates, -angles, -velocities, motion acceleration,
		 * rotational speeds and rotation acceleration to the private Tuio3DObject attributes.
		 *
		 * @param	ttime	the TuioTime to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Y coordinate to assign
		 * @param	a	the X-angle coordinate to assign
		 * @param	b	the Y_angle coordinate to assign
		 * @param	c	the Z-angle coordinate to assign
		 * @param	xs	the X velocity to assign
		 * @param	ys	the Y velocity to assign
		 * @param	zs	the Z velocity to assign
		 * @param	xrs	the X-rotation velocity to assign
		 * @param	yrs	the Y-rotation velocity to assign
		 * @param	zrs	the Z-rotation velocity to assign
		 * @param	ma	the motion acceleration to assign
		 * @param	ra	the rotation acceleration to assign
		 */
		public void update (TuioTime ttime, float xp, float yp, float zp, float a, float b, float c, float xs, float ys, float zs, float xrs, float yrs, float zrs, float ma, float ra) {
			base.update(ttime, xp,yp,zp,xs,ys,zs,ma);
			angleX = a;
			angleY = b;
			angleZ = c;
			rotation_speedX = xrs;
			rotation_speedY = yrs;
			rotation_speedZ = zrs;
			rotation_accel = ra;
			if ((rotation_accel!=0) && (state!=TUIO_STOPPED)) state = TUIO_ROTATING;
		}

		/**
		 * Assigns the provided X, Y and Z coordinates, angles and velocities, motion acceleration
		 * rotation velocities and rotation acceleration to the private Tuio3DContainer attributes.
		 * The TuioTime time stamp remains unchanged.
		 *
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Z coordinate to assign
		 * @param	a	the X-angle coordinate to assign
		 * @param	b	the Y-angle coordinate to assign
		 * @param	c	the Z-angle coordinate to assign
		 * @param	xs	the X velocity to assign
		 * @param	ys	the Y velocity to assign
		 * @param	zs	the Z velocity to assign
		 * @param	xrs	the X-rotation velocity to assign
		 * @param	yrs	the Y-rotation velocity to assign
		 * @param	zrs	the Z-rotation velocity to assign
		 * @param	ma	the motion acceleration to assign
		 * @param	ra	the rotation acceleration to assign
		 */
		public void update (float xp, float yp, float zp, float a, float b, float c, float xs, float ys, float zs, float xrs, float yrs, float zrs, float ma, float ra) {
			base.update(xp,yp,zp,xs,ys,zs,ma);
			angleX = a;
			angleY = b;
			angleZ = c;
			rotation_speedX = xrs;
			rotation_speedY = yrs;
			rotation_speedZ = zrs;
			rotation_accel = ra;
			if ((rotation_accel!=0) && (state!=TUIO_STOPPED)) state = TUIO_ROTATING;
		}

		/**
		 * Takes a TuioTime argument and assigns it along with the provided
		 * X, Y and Z coordinates and angles to the private Tuio3DObject attributes.
		 * The speed and acceleration values are calculated accordingly.
		 *
		 * @param	ttime	the TuioTime to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Y coordinate to assign
		 * @param	a	the X-angle coordinate to assign
		 * @param	b	the Y-angle coordinate to assign
		 * @param	c	the Z-angle coordinate to assign
		 */
		public void update (TuioTime ttime, float xp, float yp, float zp, float a, float b, float c) {
			Tuio3DPoint lastPoint = path3D[path3D.Count-1];
			base.update(ttime, xp,yp,zp);

			TuioTime diffTime = currentTime - lastPoint.getTuioTime();
			float dt = diffTime.getTotalMilliseconds()/1000.0f;
			float last_angleX = angleX;
			float last_angleY = angleY;
			float last_angleZ = angleZ;
			float last_rotation_speedX = rotation_speedX;
			float last_rotation_speedY = rotation_speedY;
			float last_rotation_speedZ = rotation_speedZ;
			angleX = a;
			angleY = b;
			angleZ = c;

			float da = (angleX-last_angleX)/(2.0f*(float)Math.PI);
			if (da > 0.75f) da-=1.0f;
			else if (da < -0.75f) da+=1.0f;
			float db = (angleY-last_angleY)/(2.0f*(float)Math.PI);
			if (db > 0.75f) db-=1.0f;
			else if (db < -0.75f) db+=1.0f;
			float dc = (angleZ-last_angleZ)/(2.0f*(float)Math.PI);
			if (dc > 0.75f) dc-=1.0f;
			else if (dc < -0.75f) dc+=1.0f;			
			
			rotation_speedX = da/dt;
			rotation_speedY = db/dt;
			rotation_speedZ = dc/dt;
			rotation_accel = (rotation_speedX - last_rotation_speedX)/dt + (rotation_speedY - last_rotation_speedY)/dt + (rotation_speedZ - last_rotation_speedZ)/dt;
			if ((rotation_accel!=0) && (state!=TUIO_STOPPED)) state = TUIO_ROTATING;
		}

		/**
		 * Takes the attributes of the provided Tuio3DObject
		 * and assigns these values to this Tuio3DObject.
		 * The TuioTime time stamp of this Tuio3DContainer remains unchanged.
		 *
		 * @param	tobj	the Tuio3DContainer to assign
		 */
		public void update (Tuio3DObject tobj) {
			base.update(tobj);
			angleX = tobj.getAngleX();
			angleY = tobj.getAngleY();
			angleZ = tobj.getAngleZ();
			rotation_speedX = tobj.getRotationSpeedX();
			rotation_speedY = tobj.getRotationSpeedY();
			rotation_speedZ = tobj.getRotationSpeedZ();
			rotation_accel = tobj.getRotationAccel();
			if ((rotation_accel!=0) && (state!=TUIO_STOPPED)) state = TUIO_ROTATING;
		}

		/**
		 * This method is used to calculate the speed and acceleration values of a
		 * TuioObject with unchanged position and angle.
		 */
		public new void stop (TuioTime ttime) {
			update(ttime,this.xpos,this.ypos,this.zpos,this.angleX,this.angleY,this.angleZ);
		}

		/**
		 * Returns the symbol ID of this TuioObject.
		 * @return	the symbol ID of this TuioObject
		 */
		public int getSymbolID() {
			return symbol_id;
		}

		/**
		 * Returns the rotation angle of this TuioObject.
		 * @return	the rotation angle of this TuioObject
		 */
		public float getAngleX() {
			return angleX;
		}
		public float getAngleY() {
			return angleY;
		}
		public float getAngleZ() {
			return angleZ;
		}

		/**
		 * Returns the rotation angle in degrees of this TuioObject.
		 * @return	the rotation angle in degrees of this TuioObject
		 */
		public float getAngleDegreesX() {
			return angleX/(float)Math.PI*180.0f;
		}
		public float getAngleDegreesY() {
			return angleY/(float)Math.PI*180.0f;
		}
		public float getAngleDegreesZ() {
			return angleZ/(float)Math.PI*180.0f;
		}

		/**
		 * Returns the rotation speed of this TuioObject.
		 * @return	the rotation speed of this TuioObject
		 */
		public float getRotationSpeedX() {
			return rotation_speedX;
		}
		public float getRotationSpeedY() {
			return rotation_speedY;
		}
		public float getRotationSpeedZ() {
			return rotation_speedZ;
		}

		/**
		 * Returns the rotation acceleration of this TuioObject.
		 * @return	the rotation acceleration of this TuioObject
		 */
		public float getRotationAccel() {
			return rotation_accel;
		}

		/**
		 * Returns true of this TuioObject is moving.
		 * @return	true of this TuioObject is moving
		 */
		public new bool isMoving() {
			if ((state==TUIO_ACCELERATING) || (state==TUIO_DECELERATING) || (state==TUIO_ROTATING)) return true;
			else return false;
		}
	}
}
