using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZTools.ZoomTools
{
    public class Zooms
    {
        private readonly Editor editor;

        public Zooms(Editor Editor)
        {
            this.editor = Editor;
        }

        public void Zoom(Point3d pMin, Point3d pMax, Point3d pCenter, double dFactor, int steps)
        {
            Point3d maxPoint;
            double x;
            double y;
            Point2d point2d;
            Document document = this.editor.Document;
            Database database = document.Database;
            int num = Convert.ToInt32(Application.GetSystemVariable("CVPORT"));
            if (database.TileMode)
            {
                maxPoint = new Point3d();
                if (pMin.Equals(maxPoint))
                {
                    maxPoint = new Point3d();
                    if (pMax.Equals(maxPoint))
                    {
                        pMin = database.Extmin;
                        pMax = database.Extmax;
                    }
                }
            }
            else if (num != 1)
            {
                maxPoint = new Point3d();
                if (pMin.Equals(maxPoint))
                {
                    maxPoint = new Point3d();
                    if (pMax.Equals(maxPoint))
                    {
                        pMin = database.Extmin;
                        pMax = database.Extmax;
                    }
                }
            }
            else
            {
                maxPoint = new Point3d();
                if (pMin.Equals(maxPoint))
                {
                    maxPoint = new Point3d();
                    if (pMax.Equals(maxPoint))
                    {
                        pMin = database.Pextmin;
                        pMax = database.Pextmax;
                    }
                }
            }
            using (DocumentLock documentLock = document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    using (ViewTableRecord currentView = document.Editor.GetCurrentView())
                    {
                        Matrix3d world = Matrix3d.PlaneToWorld(currentView.ViewDirection);
                        world = Matrix3d.Displacement(currentView.Target - Point3d.Origin) * world;
                        world = Matrix3d.Rotation(-currentView.ViewTwist, currentView.ViewDirection, currentView.Target) * world;
                        if (pCenter.DistanceTo(Point3d.Origin) != 0)
                        {
                            pMin = new Point3d(pCenter.X - currentView.Width / 2, pCenter.Y - currentView.Height / 2, 0);
                            pMax = new Point3d(currentView.Width / 2 + pCenter.X, currentView.Height / 2 + pCenter.Y, 0);
                        }
                        Extents3d extents3d = new Extents3d(pMin, pMax);
                        double width = currentView.Width / currentView.Height;
                        world = world.Inverse();
                        extents3d.TransformBy(world);
                        if (pCenter.DistanceTo(Point3d.Origin) == 0)
                        {
                            maxPoint = extents3d.MaxPoint;
                            double x1 = maxPoint.X;
                            maxPoint = extents3d.MinPoint;
                            x = x1 - maxPoint.X;
                            maxPoint = extents3d.MaxPoint;
                            double y1 = maxPoint.Y;
                            maxPoint = extents3d.MinPoint;
                            y = y1 - maxPoint.Y;
                            maxPoint = extents3d.MaxPoint;
                            double num1 = maxPoint.X;
                            maxPoint = extents3d.MinPoint;
                            double x2 = (num1 + maxPoint.X) * 0.5;
                            maxPoint = extents3d.MaxPoint;
                            double y2 = maxPoint.Y;
                            maxPoint = extents3d.MinPoint;
                            point2d = new Point2d(x2, (y2 + maxPoint.Y) * 0.5);
                        }
                        else
                        {
                            x = currentView.Width;
                            y = currentView.Height;
                            if (dFactor == 0)
                            {
                                pCenter = pCenter.TransformBy(world);
                            }
                            point2d = new Point2d(pCenter.X, pCenter.Y);
                        }
                        if (x > y * width)
                        {
                            y = x / width;
                        }
                        if (dFactor != 0)
                        {
                            y *= dFactor;
                            x *= dFactor;
                        }
                        if (steps >= 2)
                        {
                            double width1 = (x - currentView.Width) / (double)steps;
                            double height = (y - currentView.Height) / (double)steps;
                            Point2d centerPoint = currentView.CenterPoint;
                            centerPoint = point2d.Subtract(centerPoint.GetAsVector());
                            Point2d point2d1 = centerPoint.DivideBy((double)steps);
                            for (int i = 0; i < steps; i++)
                            {
                                ViewTableRecord viewTableRecord = currentView;
                                viewTableRecord.Width = viewTableRecord.Width + width1;
                                ViewTableRecord height1 = currentView;
                                height1.Height = height1.Height + height;
                                centerPoint = currentView.CenterPoint;
                                currentView.CenterPoint = centerPoint.Add(point2d1.GetAsVector());
                                this.editor.SetCurrentView(currentView);
                                Thread.Sleep(400 / steps);
                            }
                        }
                        else
                        {
                            currentView.CenterPoint = point2d;
                            currentView.Height = y;
                            currentView.Width = x;
                            this.editor.SetCurrentView(currentView);
                        }
                    }
                    transaction.Commit();
                }
            }
        }

        public void ZoomObjects(List<ObjectId> objectIds)
        {
            this.ZoomObjects(objectIds, 1);
        }

        public void ZoomObjects(List<ObjectId> objectIds, int steps)
        {
            Point3d minPoint;
            if (objectIds.Count > 0)
            {
                double x = double.MaxValue;
                double y = double.MaxValue;
                double z = double.MaxValue;
                double num = double.MinValue;
                double y1 = double.MinValue;
                double z1 = double.MinValue;
                using (DocumentLock documentLock = this.editor.Document.LockDocument())
                {
                    using (Transaction transaction = this.editor.Document.TransactionManager.StartTransaction())
                    {
                        foreach (ObjectId objectId in objectIds)
                        {
                            DBObject obj = transaction.GetObject(objectId, OpenMode.ForRead, true, true);
                            if (obj.IsErased || !(obj is Entity))
                            {
                                continue;
                            }
                            Extents3d geometricExtents = ((Entity)obj).GeometricExtents;
                            if (x > geometricExtents.MinPoint.X)
                            {
                                minPoint = geometricExtents.MinPoint;
                                x = minPoint.X;
                            }
                            if (y > geometricExtents.MinPoint.Y)
                            {
                                minPoint = geometricExtents.MinPoint;
                                y = minPoint.Y;
                            }
                            if (z > geometricExtents.MinPoint.Z)
                            {
                                minPoint = geometricExtents.MinPoint;
                                z = minPoint.Z;
                            }
                            if (num < geometricExtents.MaxPoint.X)
                            {
                                minPoint = geometricExtents.MaxPoint;
                                num = minPoint.X;
                            }
                            if (y1 < geometricExtents.MaxPoint.Y)
                            {
                                minPoint = geometricExtents.MaxPoint;
                                y1 = minPoint.Y;
                            }
                            if (z1 >= geometricExtents.MaxPoint.Z)
                            {
                                continue;
                            }
                            minPoint = geometricExtents.MaxPoint;
                            z1 = minPoint.Z;
                        }
                        transaction.Abort();
                    }
                }
                this.Zoom(new Point3d(x, y, z), new Point3d(num, y1, z1), Point3d.Origin, 1.1, steps);
            }
        }

    }
}
