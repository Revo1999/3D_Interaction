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
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
*/

using System;

namespace TUIO {
	public class Tuio3DCursor:Tuio3DContainer {
		protected int cursor_id;

		/**
		 * This constructor takes a TuioTime argument and assigns it along with the provided
		 * Session ID, Cursor ID, X, Y and Z coordinates to the newly created Tuio3DCursor.
		 *
		 * @param	ttime	the TuioTime to assign
		 * @param	si	the Session ID to assign
		 * @param	ci	the Cursor ID to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Z coordinate to assign
		 */
		public Tuio3DCursor (TuioTime ttime, long si, int ci, float xp, float yp, float zp):base(ttime, si,xp,yp,zp) {
			cursor_id = ci;
		}

		/**
		 * This constructor takes the provided Session ID, Cursor ID, X, Y and Z coordinates
		 * and assigns these values to the newly created Tuio3DCursor.
		 *
		 * @param	si	the Session ID to assign
		 * @param	ci	the Cursor ID to assign
		 * @param	xp	the X coordinate to assign
		 * @param	yp	the Y coordinate to assign
		 * @param	zp	the Z coordinate to assign
		 */
		public Tuio3DCursor (long si, int ci, float xp, float yp, float zp):base(si,xp,yp,zp) {
			cursor_id = ci;
		}

		/**
		 * This constructor takes the attributes of the provided Tuio3DCursor
		 * and assigns these values to the newly created Tuio3DCursor.
		 *
		 * @param	tcur	the TuioCursor to assign
		 */
		public Tuio3DCursor (Tuio3DCursor tcur):base(tcur) {
			cursor_id = tcur.getCursorID();
		}

		/**
		 * Returns the Cursor ID of this TuioCursor.
		 * @return	the Cursor ID of this TuioCursor
		 */
		public int getCursorID() {
			return cursor_id;
		}
	}
}