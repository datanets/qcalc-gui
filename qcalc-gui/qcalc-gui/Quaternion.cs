using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// A quaternion is of the form w + xi + yj + zk where i^2 = j^2 = k^2 = ijk = -1.
// The value w is called the real part, and <x,y,z> is called the imaginary vector 
// part of the quaternion.

namespace qcalc
{
    class Quaternion
    {
        private Dictionary<string, double> myQuat = new Dictionary<string, double>();
        private bool roundingForOutput = true;
        private static bool debugMode = false;
        private static bool rotationRoundingForOutput = true;
        
        public Quaternion(double w, double x, double y, double z)
        {
            // The empty constructor which will initialize the real and imaginary
            // vector parts to zero. A constructor which takes 4 doubles and uses
            // them to initialize w, x, y, and z.
            setW(w);
            setX(x);
            setY(y);
            setZ(z);
        }

        void setW(double coord)
        {
            this.myQuat["w"] = coord;
        }

        void setX(double coord)
        {
            this.myQuat["x"] = coord;
        }

        void setY(double coord)
        {
            this.myQuat["y"] = coord;
        }

        void setZ(double coord)
        {
            this.myQuat["z"] = coord;
        }

        double getW()
        {
            return this.myQuat["w"];
        }

        double getX()
        {
            return this.myQuat["x"];
        }

        double getY()
        {
            return this.myQuat["y"];
        }

        double getZ()
        {
            return this.myQuat["z"];
        }

        public Quaternion inverse()
        {
            double s = this.getW();
            Dictionary<string, double> v = new Dictionary<string, double>();
            Quaternion result = new Quaternion(0, 0, 0, 0);

            v.Add("x", this.getX());
            v.Add("y", this.getY());
            v.Add("z", this.getZ());

            double totalToDivideBy = Math.Pow(s, 2)
                + Math.Pow(v["x"], 2)
                + Math.Pow(v["y"], 2)
                + Math.Pow(v["z"], 2);

            result.setW(s / totalToDivideBy);
            result.setX(-v["x"] / totalToDivideBy);
            result.setY(-v["y"] / totalToDivideBy);
            result.setZ(-v["z"] / totalToDivideBy);

            // final round for output (if set)
            if (roundingForOutput)
            {
                result.setW(Math.Round(result.getW(), 4));
                result.setX(Math.Round(result.getX(), 4));
                result.setY(Math.Round(result.getY(), 4));
                result.setZ(Math.Round(result.getZ(), 4));
            }

            return result;
        }

        public Quaternion add(Quaternion other)
        {
            // (a + e)+i(b + f)+j(c + g)+k(d + h)
            Quaternion result = new Quaternion(0, 0, 0, 0);
            result.setW(this.getW() + other.getW());
            result.setX(this.getX() + other.getX());
            result.setY(this.getY() + other.getY());
            result.setZ(this.getZ() + other.getZ());
            return result;
        }

        public Quaternion subtract(Quaternion other)
        {
            // (a - e)+i(b - f)+j(c - g)+k(d - h)
            Quaternion result = new Quaternion(0, 0, 0, 0);
            result.setW(this.getW() - other.getW());
            result.setX(this.getX() - other.getX());
            result.setY(this.getY() - other.getY());
            result.setZ(this.getZ() - other.getZ());
            return result;
        }

        public Quaternion multiply(Quaternion other)
        {
            Quaternion result = new Quaternion(0, 0, 0, 0);

            // FOIL
            Pair myW = new Pair(this.getW(), 0);
            Pair myX = new Pair(this.getX(), 1);
            Pair myY = new Pair(this.getY(), 2);
            Pair myZ = new Pair(this.getZ(), 3);

            Pair otherW = new Pair(other.getW(), 0);
            Pair otherX = new Pair(other.getX(), 1);
            Pair otherY = new Pair(other.getY(), 2);
            Pair otherZ = new Pair(other.getZ(), 3);

            Dictionary<string, double> expression = new Dictionary<string, double>();

            // type, value
            expression.Add("w", 0);
            expression.Add("i", 0);
            expression.Add("j", 0);
            expression.Add("k", 0);
            expression.Add("ij", 0);
            expression.Add("jk", 0);
            expression.Add("ki", 0);
            expression.Add("ji", 0);
            expression.Add("kj", 0);
            expression.Add("ik", 0);
            expression.Add("i^2", 0);
            expression.Add("j^2", 0);
            expression.Add("k^2", 0);

            // first group
            expression["w"] += myW.getX() * otherW.getX();
            expression["i"] += myW.getX() * otherX.getX();
            expression["j"] += myW.getX() * otherY.getX();
            expression["k"] += myW.getX() * otherZ.getX();

            // second group
            expression["i"] += myX.getX() * otherW.getX();
            expression["i^2"] += myX.getX() * otherX.getX();
            expression["ij"] += myX.getX() * otherY.getX();
            expression["ik"] += myX.getX() * otherZ.getX();

            // third group
            expression["j"] += myY.getX() * otherW.getX();
            expression["ji"] += myY.getX() * otherX.getX();
            expression["j^2"] += myY.getX() * otherY.getX();
            expression["jk"] += myY.getX() * otherZ.getX();

            // fourth group
            expression["k"] += myZ.getX() * otherW.getX();
            expression["ki"] += myZ.getX() * otherX.getX();
            expression["kj"] += myZ.getX() * otherY.getX();
            expression["k^2"] += myZ.getX() * otherZ.getX();

            // convert ij and ji
            expression["k"] += expression["ij"];
            expression["k"] += -expression["ji"];

            // convert jk and kj
            expression["i"] += expression["jk"];
            expression["i"] += -expression["kj"];

            // convert ki and ik
            expression["j"] += expression["ki"];
            expression["j"] += -expression["ik"];

            // then convert any squared i|j|k to -1 and store result in "w" index
            double iSquaredResult = expression["i^2"] * -1;
            double jSquaredResult = expression["j^2"] * -1;
            double kSquaredResult = expression["k^2"] * -1;
            expression["w"] += iSquaredResult;
            expression["w"] += jSquaredResult;
            expression["w"] += kSquaredResult;

            result.setW(expression["w"]);
            result.setX(expression["i"]);
            result.setY(expression["j"]);
            result.setZ(expression["k"]);

            if (debugMode)
            {
                Console.WriteLine("\n\nexpression results:");
                foreach (var key in expression.Keys)
                {
                    Console.WriteLine("{0} - {1}", key, expression[key]);
                }
            }

            return result;
        }

        public Quaternion divide(Quaternion other)
        {
            // multiply q by inverse of other
            Quaternion result = new Quaternion(0, 0, 0, 0);
            Quaternion inverseOther = other.inverse();

            result = this.multiply(inverseOther);

            // final round for output (if set)
            if (roundingForOutput)
            {
                result.setW(Math.Round(result.getW(), 4));
                result.setX(Math.Round(result.getX(), 4));
                result.setY(Math.Round(result.getY(), 4));
                result.setZ(Math.Round(result.getZ(), 4));
            }

            return result;
        }

        public double magnitude()
        {
            // length = sqrt(w^2 + x^2 + y^2 + z^2)
            double result = 0.0f;

            result = Math.Sqrt(Math.Pow(this.getW(), 2) + Math.Pow(this.getX(), 2) + Math.Pow(this.getY(), 2) + Math.Pow(this.getZ(), 2));

            // final round for output (if set)
            if (roundingForOutput)
                result = Math.Round(result, 4);

            return result;
        }

        public static Quaternion rotatePointByAngleAboutAxisVector(Quaternion point, double angle, Quaternion axis)
        {
            Quaternion result = new Quaternion(0, 0, 0, 0);

            // axis (r)
            // axis must be a unit vector
            axis.setX(axis.getX() / axis.magnitude());
            axis.setY(axis.getY() / axis.magnitude());
            axis.setZ(axis.getZ() / axis.magnitude());

            // angle (a)
            // dot product (dp) = cos(a)
            double dp = Math.Cos(angle);

            if (debugMode)
                Console.WriteLine("dot product: " + dp);

            // s = sqrt(2*(1+dp))
            double s = Math.Sqrt(2 * (1 + dp));

            if (debugMode)
                Console.WriteLine("s: " + s);

            // q
            Quaternion q = new Quaternion(0, 0, 0, 0);
            q.setW(s / 2);
            q.setX(axis.getX() / s);
            q.setY(axis.getY() / s);
            q.setZ(axis.getZ() / s);

            if (debugMode)
                Console.WriteLine("q: " + q);

            // point (p)
            // p'= qpq^-1
            result = q.multiply(point);
            result = result.multiply(q.inverse());

            // final round for output (if set)
            if (rotationRoundingForOutput)
            {
                result.setW(Math.Round(result.getW(), 2));
                result.setX(Math.Round(result.getX(), 2));
                result.setY(Math.Round(result.getY(), 2));
                result.setZ(Math.Round(result.getZ(), 2));
            }

            return result;
        }

        public override string ToString()
        {
            // final round for output (if set)
            if (roundingForOutput)
            {
                this.setW(Math.Round(this.getW(), 4));
                this.setX(Math.Round(this.getX(), 4));
                this.setY(Math.Round(this.getY(), 4));
                this.setZ(Math.Round(this.getZ(), 4));
            }

            return string.Format("({0},{1},{2},{3})", this.getW(), this.getX(), this.getY(), this.getZ());
        }
    }
}