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
using System.Collections;
using System.Collections.Generic;

namespace TUIO {
	public abstract class Tuio3DContainer : Tuio3DPoint {
		protected long session_id;
		
		protected float x_speed;
		protected float y_speed;
		protected float z_speed;

		protected float motion_speed;		
		protected float motion_accel;
		
		protected List<Tuio3DPoint> path3D;
		protected int state;

		// Constants
		public const int TUIO_ADDED = 0;
		public const int TUIO_ACCELERATING = 1;
		public const int TUIO_DECELERATING = 2;
		public const int TUIO_STOPPED = 3;
		public const int TUIO_REMOVED = 4;		
		
		/**
		 * This constructor takes a TuioTime argument and assigns it along with the provided
		 * Session ID, X, Y and Z coordinates to the newly created Tuio3DContainer.
		 *
		 * @param	ttime	the TuioTime to assign
		 * @param	si	the Session ID to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Z coordinate to assign
		 */
		public Tuio3DContainer(TuioTime ttime, long si, float xp, float yp, float zp):base(ttime,xp,yp,zp) {
			session_id = si;
			x_speed = 0.0f;
			y_speed = 0.0f;
			z_speed = 0.0f;
			motion_speed = 0.0f;
			motion_accel = 0.0f;

			path3D = new List<Tuio3DPoint>();
			path3D.Add(new Tuio3DPoint(currentTime,xpos,ypos,zpos));
			state = TUIO_ADDED;
		}

		/**
		 * This constructor takes the provided Session ID, X, Y and Z coordinates
		 * and assigns these values to the newly created Tuio3DContainer.
		 *
		 * @param	si	the Session ID to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Z coordinate to assign
		 */
		public Tuio3DContainer (long si, float xp, float yp, float zp):base(xp,yp,zp) {
			session_id = si;
			x_speed = 0.0f;
			y_speed = 0.0f;
			z_speed = 0.0f;
			motion_speed = 0.0f;
			motion_accel = 0.0f;
			path3D = new List<Tuio3DPoint>();
			path3D.Add(new Tuio3DPoint(currentTime,xpos,ypos,zpos));
			state = TUIO_ADDED;
		}

		/**
		 * This constructor takes the attributes of the provided Tuio3DContainer
		 * and assigns these values to the newly created Tuio3DContainer.
		 *
		 * @param	tcon	the Tuio3DContainer to assign
		 */
		public Tuio3DContainer (Tuio3DContainer tcon):base(tcon) {
			session_id = tcon.getSessionID();
			x_speed = 0.0f;
			y_speed = 0.0f;
			z_speed = 0.0f;
			motion_speed = 0.0f;
			motion_accel = 0.0f;
			path3D = new List<Tuio3DPoint>();
			path3D.Add(new Tuio3DPoint(currentTime,xpos,ypos,zpos));
			state = TUIO_ADDED;
		}

		/**
		 * Takes a TuioTime argument and assigns it along with the provided
		 * X, Y and Z coordinates to the private Tuio3DContainer attributes.
		 * The speed and acceleration values are calculated accordingly.
		 *
		 * @param	ttime	the TuioTime to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Z coordinate to assign
		 */
		public new void update(TuioTime ttime, float xp, float yp, float zp) {
			Tuio3DPoint lastPoint = path3D[path3D.Count-1];
			base.update(ttime,xp,yp,zp);

			TuioTime diffTime = currentTime - lastPoint.getTuioTime();
			float dt = diffTime.getTotalMilliseconds()/1000.0f;
			float dx = this.xpos - lastPoint.getX();
			float dy = this.ypos - lastPoint.getY();
			float dz = this.zpos - lastPoint.getZ();
			float dist = (float)Math.Sqrt(dx*dx+dy*dy+dz*dz);
			float last_motion_speed = this.motion_speed;

			this.x_speed = dx/dt;
			this.y_speed = dy/dt;
			this.z_speed = dz/dt;
			this.motion_speed = dist/dt;
			this.motion_accel = (motion_speed - last_motion_speed)/dt;

			path3D.Add(new Tuio3DPoint(currentTime,xpos,ypos,zpos));
			if (motion_accel>0) state = TUIO_ACCELERATING;
			else if (motion_accel<0) state = TUIO_DECELERATING;
			else state = TUIO_STOPPED;
		}

		/**
		 * This method is used to calculate the speed and acceleration values of
		 * TuioContainers with unchanged positions.
		 */
		public void stop(TuioTime ttime) {
			update(ttime,this.xpos,this.ypos,this.zpos);
		}

		/**
		 * Takes a TuioTime argument and assigns it along with the provided
		 * X, Y and Z coordinates, X, Y and Z velocities and acceleration
		 * to the private Tuio3DContainer attributes.
		 *
		 * @param	ttime	the TuioTime to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Y coordinate to assign
		 * @param	xs	the X velocity to assign
		 * @param	ys	the Y velocity to assign
		 * @param	zs	the Z velocity to assign
		 * @param	ma	the acceleration to assign
		 */
		public void update(TuioTime ttime, float xp,float yp,float zp,float xs,float ys,float zs,float ma) {
			base.update(ttime,xp,yp,zp);
			x_speed = xs;
			y_speed = ys;
			z_speed = zs;
			motion_speed = (float)Math.Sqrt(x_speed*x_speed+y_speed*y_speed+z_speed*z_speed);
			motion_accel = ma;
			path3D.Add(new Tuio3DPoint(currentTime,xpos,ypos,zpos));
			if (motion_accel>0) state = TUIO_ACCELERATING;
			else if (motion_accel<0) state = TUIO_DECELERATING;
			else state = TUIO_STOPPED;
		}

		/**
		 * Assigns the provided X, Y and Z coordinates, X, Y and Z velocities and acceleration
		 * to the private Tuio3DContainer attributes. The TuioTime time stamp remains unchanged.
		 *
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Z coordinate to assign
		 * @param	xs	the X velocity to assign
		 * @param	ys	the Y velocity to assign
		 * @param	zs	the Z velocity to assign
		 * @param	ma	the acceleration to assign
		 */
		public void update (float xp,float yp,float zp,float xs,float ys,float zs,float ma) {
			base.update(xp,yp,zp);
			x_speed = xs;
			y_speed = ys;
			z_speed = zs;
			motion_speed = (float)Math.Sqrt(x_speed*x_speed+y_speed*y_speed+z_speed*z_speed);
			motion_accel = ma;
			path3D.Add(new Tuio3DPoint(currentTime,xpos,ypos,zpos));
			if (motion_accel>0) state = TUIO_ACCELERATING;
			else if (motion_accel<0) state = TUIO_DECELERATING;
			else state = TUIO_STOPPED;
		}

		/**
		 * Takes the attributes of the provided Tuio3DContainer
		 * and assigns these values to this Tuio3DContainer.
		 * The TuioTime time stamp of this Tuio3DContainer remains unchanged.
		 *
		 * @param	tcon	the Tuio3DContainer to assign
		 */
		public void update (Tuio3DContainer tcon) {
			base.update(tcon.getX(),tcon.getY(),tcon.getZ());
			x_speed = tcon.getXSpeed();
			y_speed = tcon.getYSpeed();
			z_speed = tcon.getZSpeed();
			motion_speed = (float)Math.Sqrt(x_speed*x_speed+y_speed*y_speed+z_speed*z_speed);
			motion_accel = tcon.getMotionAccel();
			path3D.Add(new Tuio3DPoint(currentTime,xpos,ypos,zpos));
			if (motion_accel>0) state = TUIO_ACCELERATING;
			else if (motion_accel<0) state = TUIO_DECELERATING;
			else state = TUIO_STOPPED;
		}

		/**
		 * Returns the Z velocity of this TuioContainer.
		 * @return	the Z velocity of this TuioContainer
		 */
		public float getXSpeed() {
			return x_speed;
		}
		public float getYSpeed() {
			return y_speed;
		}
		public float getZSpeed() {
			return z_speed;
		}

		/**
		 * Returns the position of this TuioContainer.
		 * @return	the position of this TuioContainer
		 */
		public Tuio3DPoint getPosition() {
			return new Tuio3DPoint(xpos,ypos,zpos);
		}

		/**
		 * Returns the path of this Tuio3DContainer.
		 * @return	the path of this Tuio3DContainer
		 */
		public List<Tuio3DPoint> getPath() {
			return path3D;
		}

		/**
		 * Returns the motion speed of this Tuio3DContainer.
		 * @return	the motion speed of this Tuio3DContainer
		 */
		public float getMotionSpeed() {
			return motion_speed;
		}

		/**
		 * Returns the motion acceleration of this Tuio3DContainer.
		 * @return	the motion acceleration of this Tuio3DContainer
		 */
		public float getMotionAccel() {
			return motion_accel;
		}

		/**
		 * Returns the TUIO state of this TuioContainer.
		 * @return	the TUIO state of this TuioContainer
		 */
		public int getTuioState() {
			return state;
		}

		/**
		 * Returns true of this TuioContainer is moving.
		 * @return	true of this TuioContainer is moving
		 */
		public bool isMoving() {
			if ((state==TUIO_ACCELERATING) || (state==TUIO_DECELERATING)) return true;
			else return false;
		}
		
		/**
		 * Returns the Session ID of this Tuio3DContainer.
		 * @return	the Session ID of this Tuio3DContainer
		 */
		public long getSessionID() {
			return session_id;
		}		
		
		/**
		 * Assigns the REMOVE state to this Tuio3DContainer and sets
		 * its TuioTime time stamp to the provided TuioTime argument.
		 *
		 * @param	ttime	the TuioTime to assign
		 */
		public void remove(TuioTime ttime) {
				currentTime = ttime;
				state = TUIO_REMOVED;
		}		
	}
}
