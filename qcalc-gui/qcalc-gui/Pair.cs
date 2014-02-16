using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qcalc
{
    class Pair
    {
        private double x;
        private int y;

        public Pair(double x, int y)
        {
            this.setX(x);
            this.setY(y);
        }

        public double getX()
        {
            return this.x;
        }

        public int getY()
        {
            return this.y;
        }

        public void setX(double x)
        {
            this.x = x;
        }

        public void setY(int y)
        {
            this.y = y;
        }

        public override string ToString()
        {
            String variableTypeToInt = "";

            if (this.getY() == 1)
                variableTypeToInt = "i";
            if (this.getY() == 2)
                variableTypeToInt = "j";
            if (this.getY() == 3)
                variableTypeToInt = "k";

            return string.Format("{0}{1}", this.getX(), variableTypeToInt);
        }
    }
}