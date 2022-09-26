using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WpfApp1.Model;

namespace Projekat2.Functionality
{
    public class Drawing
    {
        static double left = 100;
        static double top = 100;
        static double right = 0;
        static double bottom = 0;

        public static GeometryModel3D DrawCube(double x, double z, ref Model3DGroup group, SolidColorBrush color)
        {
            int y = 0;
            foreach (var item in group.Children)
            {
                if (item is GeometryModel3D)
                    if (item.Bounds.X == x && item.Bounds.Z == z)
                    {
                        while (item.Bounds.Y == y)
                        {
                            y++;
                        }
                    }
            }
            GeometryModel3D mGeometry;
            MeshGeometry3D mesh = new MeshGeometry3D();
            // Donja strana
            x = (int)x;
            z = (int)z;
            mesh.Positions.Add(new Point3D(x, y, z));
            mesh.Positions.Add(new Point3D(x + 1, y, z));
            mesh.Positions.Add(new Point3D(x, y, z + 1));
            mesh.Positions.Add(new Point3D(x + 1, y, z + 1));
            // Gornja strana
            mesh.Positions.Add(new Point3D(x, y + 1, z));
            mesh.Positions.Add(new Point3D(x + 1, y + 1, z));
            mesh.Positions.Add(new Point3D(x, y + 1, z + 1));
            mesh.Positions.Add(new Point3D(x + 1, y + 1, z + 1));

            // Donja strana
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(3);
            // Desna strana
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(7);
            // Zadnja strana
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(4);
            // Leva strana
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(4);
            // Prednja strana
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(6);
            // Gornja strana
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(6);

            mGeometry = new GeometryModel3D(mesh, new DiffuseMaterial(color));
            mGeometry.Transform = new Transform3DGroup();

            group.Children.Add(mGeometry);

            return mGeometry;

        }
        public static List<GeometryModel3D> DrawLine(List<Point> points, ref Model3DGroup group, SolidColorBrush color)
        {
            List<GeometryModel3D> lines = new List<GeometryModel3D>();
            int y = 0;
            GeometryModel3D mGeometry;

            for (int i = 0; i < points.Count - 1; i++)
            {
                MeshGeometry3D mesh = new MeshGeometry3D();
                int x1 = (int)points[i].X;
                int y1 = (int)points[i].Y;
                int x2 = (int)points[i + 1].X;
                int y2 = (int)points[i + 1].Y;
                var L = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

                var offsetPixels = 1;

                // This is the second line
                var x1p = x1 + offsetPixels * (y2 - y1) / L;
                var x2p = x2 + offsetPixels * (y2 - y1) / L;
                var y1p = y1 + offsetPixels * (x1 - x2) / L;
                var y2p = y2 + offsetPixels * (x1 - x2) / L;

                // Donja strana
                mesh.Positions.Add(new Point3D(x1, y, y1));
                mesh.Positions.Add(new Point3D(x1p, y, y1p));
                mesh.Positions.Add(new Point3D(x2, y, y2));
                mesh.Positions.Add(new Point3D(x2p, y, y2p));
                // Gornja strana
                mesh.Positions.Add(new Point3D(x1, y + 1, y1));
                mesh.Positions.Add(new Point3D(x1p, y + 1, y1p));
                mesh.Positions.Add(new Point3D(x2, y + 1, y2));
                mesh.Positions.Add(new Point3D(x2p, y + 1, y2p));

                // Donja strana
                mesh.TriangleIndices.Add(0);
                mesh.TriangleIndices.Add(1);
                mesh.TriangleIndices.Add(2);
                mesh.TriangleIndices.Add(2);
                mesh.TriangleIndices.Add(1);
                mesh.TriangleIndices.Add(3);
                // Desna strana
                mesh.TriangleIndices.Add(1);
                mesh.TriangleIndices.Add(5);
                mesh.TriangleIndices.Add(3);
                mesh.TriangleIndices.Add(3);
                mesh.TriangleIndices.Add(5);
                mesh.TriangleIndices.Add(7);
                // Zadnja strana
                mesh.TriangleIndices.Add(1);
                mesh.TriangleIndices.Add(0);
                mesh.TriangleIndices.Add(5);
                mesh.TriangleIndices.Add(5);
                mesh.TriangleIndices.Add(0);
                mesh.TriangleIndices.Add(4);
                // Leva strana
                mesh.TriangleIndices.Add(4);
                mesh.TriangleIndices.Add(0);
                mesh.TriangleIndices.Add(2);
                mesh.TriangleIndices.Add(2);
                mesh.TriangleIndices.Add(6);
                mesh.TriangleIndices.Add(4);
                // Prednja strana
                mesh.TriangleIndices.Add(3);
                mesh.TriangleIndices.Add(7);
                mesh.TriangleIndices.Add(2);
                mesh.TriangleIndices.Add(2);
                mesh.TriangleIndices.Add(7);
                mesh.TriangleIndices.Add(6);
                // Gornja strana
                mesh.TriangleIndices.Add(5);
                mesh.TriangleIndices.Add(4);
                mesh.TriangleIndices.Add(7);
                mesh.TriangleIndices.Add(7);
                mesh.TriangleIndices.Add(4);
                mesh.TriangleIndices.Add(6);

                mGeometry = new GeometryModel3D(mesh, new DiffuseMaterial(color));
                lines.Add(mGeometry);
                mGeometry.Transform = new Transform3DGroup();
                group.Children.Add(mGeometry);
            }

            return lines;
        }
        public static void getBoundingRect(List<SwitchEntity> switchEntities,List<NodeEntity> nodeEntities,List<SubstationEntity> substationEntities)
        {
            foreach (SwitchEntity item in switchEntities)
            {
                if (left > item.X)
                    left = item.X;
                if (top > item.Y)
                    top = item.Y;
                if (right < item.X)
                    right = item.X;
                if (bottom < item.Y)
                    bottom = item.Y;
            }
            foreach (NodeEntity item in nodeEntities)
            {
                if (left > item.X)
                    left = item.X;
                if (top > item.Y)
                    top = item.Y;
                if (right < item.X)
                    right = item.X;
                if (bottom < item.Y)
                    bottom = item.Y;
            }
            foreach (SubstationEntity item in substationEntities)
            {
                if (left > item.X)
                    left = item.X;
                if (top > item.Y)
                    top = item.Y;
                if (right < item.X)
                    right = item.X;
                if (bottom < item.Y)
                    bottom = item.Y;
            }
        }

        public static void Draw(List<SwitchEntity> switchEntities, List<NodeEntity> nodeEntities, List<SubstationEntity> substationEntities,
            Canvas canvas, ref Model3DGroup group)
        {
            getBoundingRect(switchEntities, nodeEntities, substationEntities);
            double scale = Math.Min(canvas.ActualWidth, canvas.ActualHeight);
            foreach (SubstationEntity sub in substationEntities)
            {
                int x = (int)((sub.X - left) / (right - left) * scale);
                int y = (int)((sub.Y - top) / (bottom - top) * scale);
                sub.MatX = x;
                sub.MatY = y;
                sub.Model = DrawCube(x, y, ref (group), Brushes.Purple);
            }

            foreach (SwitchEntity sw in switchEntities)
            {
                int x = (int)((sw.X - left) / (right - left) * scale);
                int y = (int)((sw.Y - top) / (bottom - top) * scale);
                sw.MatX = x;
                sw.MatY = y;
                sw.Model = DrawCube(x, y, ref (group), Brushes.Yellow);
            }

            foreach (NodeEntity node in nodeEntities)
            {
                int x = (int)((node.X - left) / (right - left) * scale);
                int y = (int)((node.Y - top) / (bottom - top) * scale);
                node.MatX = x;
                node.MatY = y;
                node.Model = DrawCube(x, y, ref (group), Brushes.Blue);
            }

        }

    }
}
