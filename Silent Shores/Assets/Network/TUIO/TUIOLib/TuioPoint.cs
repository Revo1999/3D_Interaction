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

namespace TUIO {
	/**
	 * The TuioPoint class on the one hand is a simple container and utility class to handle TUIO positions in general,
	 * on the other hand the TuioPoint is the base class for the TuioCursor and TuioObject classes.
	 *
	 * @author Martin Kaltenbrunner
	 * @version 1.4
	 */
	public class TuioPoint : Tuio3DPoint {
		/**
		 * The default constructor takes no arguments and sets
		 * its coordinate attributes to zero and its time stamp to the current session time.
		 */
		public TuioPoint ():base() {}

		/**
		 * This constructor takes two floating point coordinate arguments and sets
		 * its coordinate attributes to these values and its time stamp to the current session time.
		 *
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 */
		public TuioPoint (float xp, float yp):base(xp,yp,0f) {}

		/**
		 * This constructor takes a TuioPoint argument and sets its coordinate attributes
		 * to the coordinates of the provided TuioPoint and its time stamp to the current session time.
		 *
		 * @param	tpoint	the TuioPoint to assign
		 */
		public TuioPoint(TuioPoint tpoint):base(tpoint.getX(),tpoint.getY(),0f) {}

		/**
		 * This constructor takes a TuioTime object and two floating point coordinate arguments and sets
		 * its coordinate attributes to these values and its time stamp to the provided TUIO time object.
		 *
		 * @param	ttime	the TuioTime to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 */
		public TuioPoint(TuioTime ttime, float xp, float yp):base(ttime,xp,yp,0f) {}

		/**
		 * Takes a TuioPoint argument and updates its coordinate attributes
		 * to the coordinates of the provided TuioPoint and leaves its time stamp unchanged.
		 *
		 * @param	tpoint	the TuioPoint to assign
		 */
		public void update(TuioPoint tpoint) {
			update(tpoint.getX(),tpoint.getY());
		}

		/**
		 * Takes two floating point coordinate arguments and updates its coordinate attributes
		 * to the coordinates of the provided TuioPoint and leaves its time stamp unchanged.
		 *
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 */
		public void update(float xp, float yp) {
			base.update(xp,yp,0f);
		}

		/**
		 * Takes a TuioTime object and two floating point coordinate arguments and updates its coordinate attributes
		 * to the coordinates of the provided TuioPoint and its time stamp to the provided TUIO time object.
		 *
		 * @param	ttime	the TuioTime to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 */
		public void update(TuioTime ttime, float xp, float yp) {
			base.update(ttime,xp,yp,0f);
		}


		/**
		 * Returns the angle to the provided coordinates
		 *
		 * @param	xp	the X coordinate of the distant point
		 * @param	yp	the Y coordinate of the distant point
		 * @return	the angle to the provided coordinates
		 */
		public float getAngle(float xp, float yp) {
			return base.getAngle(xp,yp,0f);
		}

		/**
		 * Returns the angle to the provided TuioPoint
		 *
		 * @param	tpoint	the distant TuioPoint
		 * @return	the angle to the provided TuioPoint
		 */
		public float getAngle(TuioPoint tpoint) {
			return getAngle(tpoint.getX(),tpoint.getY());
		}

		/**
		 * Returns the angle in degrees to the provided coordinates
		 *
		 * @param	xp	the X coordinate of the distant point
		 * @param	yp	the Y coordinate of the distant point
		 * @return	the angle in degrees to the provided TuioPoint
		 */
		public float getAngleDegrees(float xp, float yp) {
			return (getAngle(xp,yp)/(float)Math.PI)*180.0f;
		}

		/**
		 * Returns the angle in degrees to the provided TuioPoint
		 *
		 * @param	tpoint	the distant TuioPoint
		 * @return	the angle in degrees to the provided TuioPoint
		 */
		public float getAngleDegrees(TuioPoint tpoint) {
			return (getAngle(tpoint)/(float)Math.PI)*180.0f;
		}

	}
}