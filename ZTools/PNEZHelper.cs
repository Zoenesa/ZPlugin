using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTools
{
    public class PNEZHelper
    {
        /// <summary>
        /// Point Number / Point Name
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// Nothing : Y
        /// </summary>
        public double Northing { get; set; }

        /// <summary>
        /// Easting : X
        /// </summary>
        public double Easting { get; set; }

        /// <summary>
        /// Elevation : Z
        /// </summary>
        public double Elevasi { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        public readonly Point3d POINT3D;

        public PNEZHelper(string point, double Northing, double Easting, double Elevasi, string Kode)
        {
            this.Point = point;
            this.Northing = Northing;
            this.Easting = Easting;
            this.Elevasi = Elevasi;
            this.Code = Kode;
            POINT3D = setPoin();
        }

        protected Point3d setPoin()
        {
            double[] xyz = new double[]
            {
                this.Easting,
                this.Northing,
                this.Elevasi
            };
            return new Point3d(xyz);
        }
    }
}
