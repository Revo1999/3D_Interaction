/*
	TUIO C# Library - part of the reacTIVision project
	http://reactivision.sourceforge.net/

	Copyright (c) 2005-2009 Martin Kaltenbrunner <mkalten@iua.upf.edu>

    TUIO C# library extensions for 3D 
    Copyright (c) 2013 Janus B. Kristensen, CAVI, Aarhus University

    TUIO C# library extensions for 2DBlb 
    Copyright (c) 2013 Rolf Bagge, CAVI, Aarhus University

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
using System.Collections.Generic;

namespace TUIO
{

	/**
	 * The TuioObject class encapsulates /tuio/2Dobj TUIO objects.
	 *
	 * @author Martin Kaltenbrunner
	 * @version 1.4
	 */
 	public class TuioBlob:TuioContainer {

	/**
	 * The rotation angle value.
	 */
 	protected float angle;
	/**
	 * The rotation speed value.
	 */
 	protected float rotation_speed;
	/**
	 * The rotation acceleration value.
	 */
 	protected float rotation_accel;
	/**
	 * Defines the ROTATING state.
	 */
 	public static readonly int TUIO_ROTATING = 5;

	protected float width;

	protected float height;

	protected float area;

	/**
	 * This constructor takes a TuioTime argument and assigns it along with the provided
 	 * Session ID, Symbol ID, X and Y coordinate and angle to the newly created TuioObject.
	 *
	 * @param	ttime	the TuioTime to assign
	 * @param	si	the Session ID to assign
	 * @param	sym	the Symbol ID to assign
	 * @param	xp	the X coordinate to assign
	 * @param	yp	the Y coordinate to assign
	 * @param	a	the angle to assign
	 */
	public TuioBlob (TuioTime ttime, long si, float xp, float yp, float a, float w, float h, float f):base(ttime, si,xp,yp) {
		angle = a;
		width = w;
		height = h;
		area = f;
		rotation_speed = 0.0f;
		rotation_accel = 0.0f;
	}

	/**
	 * This constructor takes the provided Session ID, Symbol ID, X and Y coordinate
 	 * and angle, and assigs these values to the newly created TuioObject.
	 *
	 * @param	si	the Session ID to assign
	 * @param	sym	the Symbol ID to assign
	 * @param	xp	the X coordinate to assign
	 * @param	yp	the Y coordinate to assign
	 * @param	a	the angle to assign
	 */
	public TuioBlob (long si, float xp, float yp, float a, float w, float h, float f):base(si,xp,yp) {
		angle = a;
		width = w;
		height = h;
		area = f;
		rotation_speed = 0.0f;
		rotation_accel = 0.0f;
	}

	/**
	 * This constructor takes the atttibutes of the provided TuioObject
 	 * and assigs these values to the newly created TuioObject.
	 *
	 * @param	tobj	the TuioObject to assign
	 */
	public TuioBlob (TuioBlob tblb):base(tblb) {
		angle = tblb.getAngle();
		width = tblb.getWidth ();
		height = tblb.getHeight ();
		area = tblb.getArea ();
		rotation_speed = 0.0f;
		rotation_accel = 0.0f;
	}

	/**
	 * Takes a TuioTime argument and assigns it along with the provided
 	 * X and Y coordinate, angle, X and Y velocity, motion acceleration,
	 * rotation speed and rotation acceleration to the private TuioObject attributes.
	 *
	 * @param	ttime	the TuioTime to assign
	 * @param	xp	the X coordinate to assign
	 * @param	yp	the Y coordinate to assign
	 * @param	a	the angle coordinate to assign
	 * @param	xs	the X velocity to assign
	 * @param	ys	the Y velocity to assign
	 * @param	rs	the rotation velocity to assign
	 * @param	ma	the motion acceleration to assign
	 * @param	ra	the rotation acceleration to assign
	 */
	public void update (TuioTime ttime, float xp, float yp, float a, float w, float h, float f, float xs, float ys, float rs, float ma, float ra) {
		base.update(ttime, xp,yp,xs,ys,ma);
		angle = a;
		width = w;
		height = h;
		area = f;
		rotation_speed = rs;
		rotation_accel = ra;
		if ((rotation_accel!=0) && (state!=TUIO_STOPPED)) state = TUIO_ROTATING;
	}

	/**
	 * Assigns the provided X and Y coordinate, angle, X and Y velocity, motion acceleration
	 * rotation velocity and rotation acceleration to the private TuioContainer attributes.
	 * The TuioTime time stamp remains unchanged.
	 *
	 * @param	xp	the X coordinate to assign
	 * @param	yp	the Y coordinate to assign
	 * @param	a	the angle coordinate to assign
	 * @param	xs	the X velocity to assign
	 * @param	ys	the Y velocity to assign
	 * @param	rs	the rotation velocity to assign
	 * @param	ma	the motion acceleration to assign
	 * @param	ra	the rotation acceleration to assign
	 */
	public void update (float xp, float yp, float a, float w, float h, float f, float xs, float ys, float rs, float ma, float ra) {
		base.update(xp,yp,xs,ys,ma);
		angle = a;
		width = w;
		height = h;
		area = f;
		rotation_speed = rs;
		rotation_accel = ra;
		if ((rotation_accel!=0) && (state!=TUIO_STOPPED)) state = TUIO_ROTATING;
	}

	/**
	 * Takes a TuioTime argument and assigns it along with the provided
 	 * X and Y coordinate and angle to the private TuioObject attributes.
	 * The speed and accleration values are calculated accordingly.
	 *
	 * @param	ttime	the TuioTime to assign
	 * @param	xp	the X coordinate to assign
	 * @param	yp	the Y coordinate to assign
	 * @param	a	the angle coordinate to assign
	 */
	public void update (TuioTime ttime, float xp, float yp, float a, float w, float h, float f) { // Note: hides Tuio3DPoint.update(time,x,y,z)
		TuioPoint lastPoint = path[path.Count-1];
		base.update(ttime, xp,yp);

		TuioTime diffTime = currentTime - lastPoint.getTuioTime();
		float dt = diffTime.getTotalMilliseconds()/1000.0f;
		float last_angle = angle;
		float last_rotation_speed = rotation_speed;
		angle = a;
		width = w;
		height = h;
		area = f;

		float da = (angle-last_angle)/(2.0f*(float)Math.PI);
		if (da > 0.75f) da-=1.0f;
		else if (da < -0.75f) da+=1.0f;

		rotation_speed = da/dt;
		rotation_accel = (rotation_speed - last_rotation_speed)/dt;
		if ((rotation_accel!=0) && (state!=TUIO_STOPPED)) state = TUIO_ROTATING;
	}

	/**
	 * Takes the atttibutes of the provided TuioObject
 	 * and assigs these values to this TuioObject.
	 * The TuioTime time stamp of this TuioContainer remains unchanged.
	 *
	 * @param	tobj	the TuioContainer to assign
	 */
	public void update (TuioBlob tblb) {
		base.update(tblb);
		angle = tblb.getAngle();
		rotation_speed = tblb.getRotationSpeed();
		rotation_accel = tblb.getRotationAccel();
		width = tblb.getWidth();
		height = tblb.getHeight();
		area = tblb.getArea();
		if ((rotation_accel!=0) && (state!=TUIO_STOPPED)) state = TUIO_ROTATING;
	}

	/**
	 * This method is used to calculate the speed and acceleration values of a
	 * TuioObject with unchanged position and angle.
	 */
	public new void stop (TuioTime ttime) {
		update(ttime,this.xpos,this.ypos, this.angle, this.width, this.height, this.area);
	}

	/**
	 * Returns the rotation angle of this TuioObject.
	 * @return	the rotation angle of this TuioObject
	 */
	public float getAngle() {
		return angle;
	}

	/**
	 * Returns the rotation angle in degrees of this TuioObject.
	 * @return	the rotation angle in degrees of this TuioObject
	 */
	public float getAngleDegrees() {
		return angle/(float)Math.PI*180.0f;
	}

	/**
	 * Returns the rotation speed of this TuioObject.
	 * @return	the rotation speed of this TuioObject
	 */
	public float getRotationSpeed() {
		return rotation_speed;
	}

	/**
	 * Returns the rotation acceleration of this TuioObject.
	 * @return	the rotation acceleration of this TuioObject
	 */
	public float getRotationAccel() {
		return rotation_accel;
	}

	public float getWidth() {
		return width;
	}

	public float getHeight() {
		return height;
	}

	public float getArea() {
		return area;
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