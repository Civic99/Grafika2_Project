using Projekat2.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Model;

namespace Projekat2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool mousePan = false;
        private bool mouseRotate = false;
        private Point lastPosition;
        XMLHelper xmlLoader;
        int maksX = 0;
        int maksY = 0;
        List<PowerEntity> entities;
        private GeometryModel3D hitgeo;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            camera.Position = new Point3D(
                camera.Position.X,
                camera.Position.Y - e.Delta / 30D, 
                camera.Position.Z);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            mousePan = true;
            Point pos = Mouse.GetPosition(viewport);
            lastPosition = new Point(
                    pos.X - viewport.ActualWidth / 2,
                    viewport.ActualHeight / 2 - pos.Y);
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mousePan = false;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePan)
            {
                Point pos = Mouse.GetPosition(viewport);
                Point actualPos = new Point(
                        pos.X - viewport.ActualWidth / 2,
                        viewport.ActualHeight / 2 - pos.Y);
                double dx = actualPos.X - lastPosition.X;
                double dy = actualPos.Y - lastPosition.Y;

                Vector3D vecX = Vector3D.CrossProduct(camera.UpDirection, camera.LookDirection);
                camera.Position += vecX * dx / 5D; 
                Vector3D vecZ = camera.UpDirection;
                camera.Position += vecZ * (-dy) / 5D;
                lastPosition = actualPos;
            }
            if (mouseRotate)
            {

                Point pos = Mouse.GetPosition(viewport);
                Point actualPos = new Point(
                        pos.X - viewport.ActualWidth / 2,
                        viewport.ActualHeight / 2 - pos.Y);
                double dx = actualPos.X - lastPosition.X;

                Vector3D vec = camera.LookDirection;
                Matrix3D matrix3D = new Matrix3D();
                matrix3D.RotateAt(new Quaternion(vec, dx), new Point3D(maksX / 2, 0, maksY / 2));

                camera.Position *= matrix3D;
                camera.UpDirection *= matrix3D;

                lastPosition = actualPos;
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                mouseRotate = true;
                Point pos = Mouse.GetPosition(viewport);
                lastPosition = new Point(
                        pos.X - viewport.ActualWidth / 2,
                        viewport.ActualHeight / 2 - pos.Y);
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Released)
            {
                mouseRotate = false;
            }
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            entities = new List<PowerEntity>();
            long[,] grid = new long[501, 501];

            xmlLoader = new XMLHelper();
            xmlLoader.LoadData(ref (group));
            Projekat2.Functionality.Drawing.Draw(xmlLoader.switchEntities, xmlLoader.nodeEntities, xmlLoader.substationEntities, canvas, ref (group));
            foreach (SwitchEntity item in xmlLoader.switchEntities)
            {
                if (maksX < item.MatX)
                    maksX = item.MatX;
                if (maksY < item.MatY)
                    maksY = item.MatY;
            }
            foreach (NodeEntity item in xmlLoader.nodeEntities)
            {
                if (maksX < item.MatX)
                    maksX = item.MatX;
                if (maksY < item.MatY)
                    maksY = item.MatY;
            }
            foreach (SubstationEntity item in xmlLoader.substationEntities)
            {
                if (maksX < item.MatX)
                    maksX = item.MatX;
                if (maksY < item.MatY)
                    maksY = item.MatY;
            }
            camera.Position = new Point3D(
                maksX / 2,
                camera.Position.Y,
                maksY / 2);

            entities = (from x in xmlLoader.substationEntities select (PowerEntity)x).ToList();

            // Now add the second list to the end of the last one
            entities.AddRange((from x in xmlLoader.switchEntities select (PowerEntity)x).ToList());
            entities.AddRange((from x in xmlLoader.nodeEntities select (PowerEntity)x).ToList());
            string s = "";
            foreach (LineEntity item in xmlLoader.lineEntities)
            {
                if (!entities.Any(c => c.Id == item.FirstEnd))
                {
                    continue;
                }
                else
                {
                    if (!entities.Any(c => c.Id == item.SecondEnd)) 
                    {
                        continue;
                    }
                    else
                    {
                        switch (item.ConductorMaterial)
                        {
                            case "Steel":
                                item.LinesModel = Functionality.Drawing.DrawLine(item.Points, ref (group), Brushes.DarkGray);
                                break;
                            case "Copper":
                                item.LinesModel = Functionality.Drawing.DrawLine(item.Points, ref (group), Brushes.OrangeRed);
                                break;
                            case "Acsr":
                                item.LinesModel = Functionality.Drawing.DrawLine(item.Points, ref (group), Brushes.LightSlateGray);
                                break;
                        }
                    }
                }
            }
            Console.WriteLine(s);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            foreach (SwitchEntity item in xmlLoader.switchEntities)
            {
                if (item.Status == "Open")
                {
                    item.Model.Material = new DiffuseMaterial(Brushes.Green);
                }
                else
                {
                    item.Model.Material = new DiffuseMaterial(Brushes.Red);
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            foreach (SwitchEntity item in xmlLoader.switchEntities)
            {
                item.Model.Material = new DiffuseMaterial(Brushes.Yellow);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            foreach (LineEntity item in xmlLoader.lineEntities)
            {
                if (item.LinesModel != null)
                {
                    switch (item.ConductorMaterial)
                    {
                        case "Steel":
                            foreach (GeometryModel3D model in item.LinesModel)
                            {
                                model.Material = new DiffuseMaterial(Brushes.DarkGray);
                            }
                            break;
                        case "Copper":
                            foreach (GeometryModel3D model in item.LinesModel)
                            {
                                model.Material = new DiffuseMaterial(Brushes.OrangeRed);
                            }
                            break;
                        case "Acsr":
                            foreach (GeometryModel3D model in item.LinesModel)
                            {
                                model.Material = new DiffuseMaterial(Brushes.LightSlateGray);
                            }
                            break;
                    }

                }
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            foreach (LineEntity item in xmlLoader.lineEntities)
            {
                if (item.LinesModel != null)
                {
                    if (item.R < 1)
                    {
                        foreach (GeometryModel3D model in item.LinesModel)
                        {
                            model.Material = new DiffuseMaterial(Brushes.Red);
                        }
                    }
                    else if (item.R >= 1 && item.R <= 2)
                    {
                        foreach (GeometryModel3D model in item.LinesModel)
                        {
                            model.Material = new DiffuseMaterial(Brushes.Orange);
                        }
                    }
                    else
                    {
                        foreach (GeometryModel3D model in item.LinesModel)
                        {
                            model.Material = new DiffuseMaterial(Brushes.Yellow);
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            foreach (SwitchEntity item in xmlLoader.switchEntities)
            {
                if (item.Status == "Open")
                {
                    foreach (LineEntity line in xmlLoader.lineEntities)
                    {
                        if (line.FirstEnd == item.Id && line.LinesModel != null)
                        {
                            foreach (GeometryModel3D model in line.LinesModel)
                            {
                                model.Material = new DiffuseMaterial(Brushes.Transparent);
                            }
                            foreach (PowerEntity ent in entities)
                            {
                                if (ent.Id == line.SecondEnd)
                                {
                                    ent.Model.Material = new DiffuseMaterial(Brushes.Transparent);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            foreach (SubstationEntity sub in xmlLoader.substationEntities)
            {
                sub.Model.Material = new DiffuseMaterial(Brushes.Purple);
            }

            foreach (SwitchEntity sw in xmlLoader.switchEntities)
            {
                sw.Model.Material = new DiffuseMaterial(Brushes.Yellow);
            }

            foreach (NodeEntity node in xmlLoader.nodeEntities)
            {

                node.Model.Material = new DiffuseMaterial(Brushes.Blue);
            }
            foreach (LineEntity item in xmlLoader.lineEntities)
            {
                if (item.LinesModel != null)
                {
                    switch (item.ConductorMaterial)
                    {
                        case "Steel":
                            foreach (GeometryModel3D model in item.LinesModel)
                            {
                                model.Material = new DiffuseMaterial(Brushes.DarkGray);
                            }
                            break;
                        case "Copper":
                            foreach (GeometryModel3D model in item.LinesModel)
                            {
                                model.Material = new DiffuseMaterial(Brushes.OrangeRed);
                            }
                            break;
                        case "Acsr":
                            foreach (GeometryModel3D model in item.LinesModel)
                            {
                                model.Material = new DiffuseMaterial(Brushes.LightSlateGray);
                            }
                            break;
                    }

                }
            }
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (SubstationEntity sub in xmlLoader.substationEntities)
            {
                sub.Model.Material = new DiffuseMaterial(Brushes.Transparent);
            }
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            foreach (SubstationEntity sub in xmlLoader.substationEntities)
            {
                sub.Model.Material = new DiffuseMaterial(Brushes.Purple);
            }
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            foreach (NodeEntity node in xmlLoader.nodeEntities)
            {

                node.Model.Material = new DiffuseMaterial(Brushes.Transparent);
            }
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            foreach (NodeEntity node in xmlLoader.nodeEntities)
            {

                node.Model.Material = new DiffuseMaterial(Brushes.Blue);
            }
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            foreach (SwitchEntity sw in xmlLoader.switchEntities)
            {
                sw.Model.Material = new DiffuseMaterial(Brushes.Transparent);
            }
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            foreach (SwitchEntity sw in xmlLoader.switchEntities)
            {
                sw.Model.Material = new DiffuseMaterial(Brushes.Yellow);
            }
        }

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point mouseposition = e.GetPosition(canvas);
            Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);

            PointHitTestParameters pointparams =
                     new PointHitTestParameters(mouseposition);
            RayHitTestParameters rayparams =
                     new RayHitTestParameters(testpoint3D, testdirection);

            //test for a result in the Viewport3D     
            hitgeo = null;
            VisualTreeHelper.HitTest(canvas, null, HTResult, pointparams);
        }

        private HitTestResultBehavior HTResult(System.Windows.Media.HitTestResult rawresult)
        {

            RayHitTestResult rayResult = rawresult as RayHitTestResult;

            if (rayResult != null)
            {
                bool entitet = false;
                bool ista = false;
                for (int i = 0; i < entities.Count; i++)
                {
                    if ((GeometryModel3D)entities[i].Model == rayResult.ModelHit)
                    {
                        hitgeo = (GeometryModel3D)rayResult.ModelHit;
                        entitet = true;
                        ista = true;
                        long Id = entities[i].Id; //dobijamo id pogodjenog entiteta, id je u recniku vrednost
                        ToolTip tt = new ToolTip();
                        tt.StaysOpen = false;
                        if (entities[i] is SubstationEntity)
                        {
                            tt.Content = "Substation\n" + "ID: " + Id + "\nName: " + entities[i].Name;
                            tt.IsOpen = true;
                        }
                        if (entities[i] is SwitchEntity)
                        {
                            tt.Content = "Switch\n" + "ID: " + Id + "\nName: " + entities[i].Name;
                            tt.IsOpen = true;
                        }
                        if (entities[i]is NodeEntity)
                        {
                            tt.Content = "Node\n" + "ID: " + Id + "\nName: " + entities[i].Name;
                            tt.IsOpen = true;
                        }
                    }
                }
                if (!entitet)
                {
                    for(int i =0; i < xmlLoader.lineEntities.Count; i++)
                    {
                        if (xmlLoader.lineEntities[i].LinesModel != null)
                        {
                            foreach (GeometryModel3D item in xmlLoader.lineEntities[i].LinesModel)
                            {
                                if (item == rayResult.ModelHit)
                                {
                                    ista = true;
                                    hitgeo = (GeometryModel3D)rayResult.ModelHit;
                                    long Id = xmlLoader.lineEntities[i].Id; //dobijamo id pogodjenog entiteta, id je u recniku vrednost
                                    ToolTip tool = new ToolTip();
                                    tool.StaysOpen = false;
                                    tool.Content = "Line\n" + "ID: " + Id + "\nName: " + xmlLoader.lineEntities[i].Name
                                    + "\nFirstEnd: " + xmlLoader.lineEntities[i].FirstEnd + "\nSecondEnd:" + xmlLoader.lineEntities[i].SecondEnd;
                                    tool.IsOpen = true;
                                    for (int j = 0; j < entities.Count; j++)
                                    {
                                        if (entities[j].Id == xmlLoader.lineEntities[i].FirstEnd)
                                        {
                                            entities[j].Model.Material = new DiffuseMaterial(Brushes.LimeGreen);
                                        }
                                        if (entities[j].Id == xmlLoader.lineEntities[i].SecondEnd)
                                        {
                                            entities[j].Model.Material = new DiffuseMaterial(Brushes.LimeGreen);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if(!ista)
                {
                    foreach (SubstationEntity sub in xmlLoader.substationEntities)
                    {
                        sub.Model.Material = new DiffuseMaterial(Brushes.Purple);
                    }

                    foreach (SwitchEntity sw in xmlLoader.switchEntities)
                    {
                        sw.Model.Material = new DiffuseMaterial(Brushes.Yellow);
                    }

                    foreach (NodeEntity node in xmlLoader.nodeEntities)
                    {

                        node.Model.Material = new DiffuseMaterial(Brushes.Blue);
                    }
                }
            }

            return HitTestResultBehavior.Stop;
        }
    }
}
