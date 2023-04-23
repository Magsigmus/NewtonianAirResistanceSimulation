using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Net.Http.Headers;

namespace GraphDrawer
{
    public partial class GraphDrawer : Panel
    {

        public Func<int, PointF> function { get; set; }
        public DataTable points;

        public int pixelsPrUpdate = 2;
        private Pen myPen = new Pen(Color.Black);
        public decimal xZoom { get; set; }
        public decimal yZoom { get; set; }
        public int xOffset { get; set; }
        public int yOffset { get; set; }
        public GraphDrawer()
        {
            InitializeComponent();
        }

        public GraphDrawer(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public void DrawGraph(object sender, PaintEventArgs e)
        {
            //Sig: Draws the axes in the coordinate system
            Point xAxisStart = new Point(xOffset, 0);
            Point xAxisEnd = new Point(xOffset, Height);
            Point yAxisStart = new Point(0, Height - yOffset);
            Point yAxisEnd = new Point(Width, Height - yOffset);

            e.Graphics.DrawLine(myPen, xAxisStart, xAxisEnd);
            e.Graphics.DrawLine(myPen, yAxisStart, yAxisEnd);

            //Sig: Checks if a polynomial has been defined
            if (function == null && points == null) { return; }
            if(function == null) { DrawGraphFromData(e.Graphics); }
            if(points == null) { DrawGraphFromFunction(e.Graphics); }
        }

        private void DrawGraphFromFunction(Graphics e)
        {
            //Sig: Draws the graph
            Point point = new Point(), lastPoint = ToParentSpace(function(0)); // Sig: Gets the first value
            int t = 1;
            while (!IsInsideLocalBounds(point))
            {
                point = ToParentSpace(function(t));
                try
                {
                    e.DrawLine(myPen, point, lastPoint); // Sig: draws the line
                }
                catch (OverflowException)
                {
                    MessageBox.Show("ERROR: OVERFLOW IN GRAPHDRAWER");
                }
                t += 1;
                lastPoint = point;
            }
        }

        private void DrawGraphFromData(Graphics e)
        {
            //Sig: Draws the graph

            decimal localX = Convert.ToDecimal(points.Rows[0]["x"]), 
                localY = Convert.ToDecimal(points.Rows[0]["y"]);
            Point point = new Point(xOffset,yOffset), lastPoint = ToParentSpace(localX, localY); // Sig: Gets the first value
            for(int i = 0; i < points.Rows.Count; i++)
            {
                if (!IsInsideLocalBounds(point)) { continue; }

                localX = Convert.ToDecimal(points.Rows[i]["x"]);
                localY = Convert.ToDecimal(points.Rows[i]["y"]); 
                point = ToParentSpace(localX, localY);
                try
                {
                    e.DrawLine(myPen, point, lastPoint); // Sig: draws the line
                }
                catch (OverflowException)
                {
                    MessageBox.Show("ERROR: OVERFLOW IN GRAPHDRAWER");
                }
                lastPoint = point;
            }
        }

        Point ToParentSpace(decimal x, decimal y)
        {
            return new Point((int)(x * xZoom + xOffset), (int)(Height - y * yZoom - yOffset));
        }

        Point ToParentSpace(PointF p)
        {
            return ToParentSpace((decimal)p.X, (decimal)p.Y);
        }

        bool IsInsideLocalBounds(Point p)
        {
            return 0 < p.X && 0 < p.Y && Width > p.X && Height > p.Y;
        }
    }
}
