﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kOS
{
    public class Vector : Structure
    {
        double x;
        double y;
        double z;

        public Vector(Vector3d init)
        {
            x = init.x;
            y = init.y;
            z = init.z;
        }

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y, float z)
        {
            this.x = (double)x;
            this.y = (double)y;
            this.z = (double)z;
        }

        public Direction ToDirection()
        {
            return new Direction(ToVector3D(), false);
        }

        public override object GetSuffix(string suffixName)
        {
            if (suffixName == "X") return x;
            if (suffixName == "Y") return y;
            if (suffixName == "Z") return z;
            if (suffixName == "MAG") return new Vector3d(x, y, z).magnitude;
            if (suffixName == "VEC") return new Vector(x, y, z);

            return base.GetSuffix(suffixName);
        }

        public override bool SetSuffix(string suffixName, object value)
        {
            double dblValue;
            if (value is double)
            {
                dblValue = (double)value;
            }
            else if (!double.TryParse(value.ToString(), out dblValue))
            {
                return false;
            }

            if (suffixName == "X") { x = dblValue; return true; }
            if (suffixName == "Y") { y = dblValue; return true; }
            if (suffixName == "Z") { z = dblValue; return true; }

            if (suffixName == "MAG")
            {
                double oldMag = new Vector3d(x, y, z).magnitude;

                if (oldMag == 0) return true; // Avoid division by zero

                x = x / oldMag * dblValue;
                y = y / oldMag * dblValue;
                z = z / oldMag * dblValue;

                return true;
            }

            return base.SetSuffix(suffixName, value);
        }

        public Vector3d ToVector3D()
        {
            return new Vector3d(x,y,z); 
        }

        public override string ToString()
        {
            return "V(" + x + ", " + y + ", " + z + ")";
        }

        public static implicit operator Vector3d(Vector d)
        {
            return d.ToVector3D();
        }

        public static explicit operator Direction(Vector d)
        {
            return new Direction(d.ToVector3D(), false);
        }

        public static Vector operator *(Vector a, Vector b) { return new Vector(a.x * b.x, a.y * b.y, a.z * b.z); }
        public static Vector operator *(Vector a, float b) { return new Vector(a.x * b, a.y * b, a.z * b); }
        public static Vector operator *(Vector a, double b) { return new Vector(a.x * b, a.y * b, a.z * b); }
        public static Vector operator +(Vector a, Vector b) { return new Vector(a.ToVector3D() + b.ToVector3D()); }
        public static Vector operator -(Vector a, Vector b) { return new Vector(a.ToVector3D() - b.ToVector3D()); }

        public override object TryOperation(string op, object other, bool reverseOrder)
        {
            if (op == "+")
            {
                if (other is Vector) return this + (Vector)other;
            }
            else if (op == "*")
            {
                if (other is Vector) return this * (Vector)other;
                if (other is double) return this * (double)other;
            }
            else if (op == "-")
            {
                if (!reverseOrder)
                {
                    if (other is Vector) return this - (Vector)other;
                }
                else
                {
                    if (other is Vector) return (Vector)other - this;
                }
            }

            return null;
        }
    }
}
