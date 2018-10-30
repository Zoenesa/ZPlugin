using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTools
{
    public static class Extensions
    {
        public static object[] PointRow(this string ImportedPoint)
        {
            return ImportedPoint.Split(new[] { ',' });
        }

        public static object[] PointRow(this string ImportedPoint, string Delimiter)
        {
            return ImportedPoint.Split(Delimiter.ToCharArray());
        }

        public static double ParsePointFromCSV(this object Value, IFormatProvider Provider)
        {
            try
            {
                return Convert.ToDouble(Value, Provider);
            }
            catch (Exception)
            {
                throw new Exception("Invalid Value.");
            }
        }

        public static void ParsePointFromCSV(this object[] xyz, System.Globalization.NumberStyles numberStyles, IFormatProvider Provider, out double[] OutValue, out string refMsg)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                double[] _value = new double[xyz.Length];
                bool fx = double.TryParse(xyz[0].ToString(), numberStyles, Provider, out _value[0]);
                bool fy = double.TryParse(xyz[1].ToString(), numberStyles, Provider, out _value[1]);
                bool fz = double.TryParse(xyz[2].ToString(), numberStyles, Provider, out _value[2]);
                OutValue = _value;
                if (fx & fy & fz)
                {
                    sb.Append("Valid.");
                }
                else
                {
                    sb.Append("Invalid ");
                }
                if (!fx)
                {
                    sb.Append("X ");
                }
                if (!fy)
                {
                    sb.Append("Y ");
                }
                if (!fz)
                {
                    sb.Append("Z ");
                }
                refMsg = sb.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Invalid Value.");
            }
        }
    }
}
