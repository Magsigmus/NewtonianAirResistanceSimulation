using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataFiles;
using KGySoft.CoreLibraries;

namespace pH_calculator
{
    public partial class GraphDrawingForm : Form
    {
        private GraphDrawer.GraphDrawer graphDrawer1;
        private TextBox xZoomTextbox;
        private TextBox yZoomTextbox;
        private TextBox cwTextbox;
        private TextBox mTextbox;
        private TextBox ATextbox;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox rhoTextbox;
        private TextBox gTextbox;
        private Button DrawButton;
        private Label label8;
        private TextBox alphaTextbox;
        private Label label9;
        private TextBox v0Textbox;
        private CheckBox textfileCheckbox;
        private TextBox y0TextBox;
        private Label label10;
        private IContainer components;

        public GraphDrawingForm()
        {
            InitializeComponent();
            yZoomTextbox.Text = graphDrawer1.yZoom.ToString();
            xZoomTextbox.Text = graphDrawer1.xZoom.ToString();
        }

        private void zoomTextbox_TextChanged(object sender, EventArgs e)
        {
            if (yZoomTextbox.Text.Length == 0 || xZoomTextbox.Text.Length == 0) { return; }
            if (Convert.ToDouble(xZoomTextbox.Text) == 0 || Convert.ToDouble(yZoomTextbox.Text) == 0) { return; }

            graphDrawer1.xZoom = Convert.ToDecimal(xZoomTextbox.Text);
            graphDrawer1.yZoom = Convert.ToDecimal(yZoomTextbox.Text);
            graphDrawer1.Refresh();
        }

        decimal vx, vy;
        decimal tick = 0.0001m;
        public DataTable data;
        private void DrawButton_Click(object sender, EventArgs e)
        {
            double alpha = Convert.ToDouble(alphaTextbox.Text);
            decimal v0 = (decimal)Convert.ToDouble(v0Textbox.Text);
            vx = (decimal)Math.Cos((alpha / 360) * 2 * Math.PI) * v0;
            vy = (decimal)Math.Sin((alpha / 360) * 2 * Math.PI) * v0;

            data = new DataTable();
            data.Columns.Add("Tid (s)");
            data.Columns.Add("vx (m/s)");
            data.Columns.Add("vy (m/s)");
            data.Columns.Add("x (m)");
            data.Columns.Add("y (m)");

            decimal cw = Convert.ToDecimal(cwTextbox.Text),
                    A = Convert.ToDecimal(ATextbox.Text),
                    rho = Convert.ToDecimal(rhoTextbox.Text),
                    g = Convert.ToDecimal(gTextbox.Text),
                    m = Convert.ToDecimal(mTextbox.Text),
                    y = Convert.ToDecimal(y0TextBox.Text);

            decimal x = 0, mu = (cw*rho*A)/(2*m);

            for(decimal t = 0; y > -1; t += tick)
            {
                if(t % 0.01m == 0)
                {
                    data.Rows.Add(t, vx, vy, x, y);
                }

                x += vx * tick;
                y += vy * tick;

                decimal v = DecimalExtensions.Pow(vx * vx + vy * vy, 1 / 2);

                vx -= tick * mu * vx * v;
                vy -= tick * (mu * vy * v + g);
            }

            if (textfileCheckbox.Checked)
            {
                TextFileDataTableHandler.CreateTextFile("KasteParabel.txt");
                TextFileDataTableHandler.WriteDataTableToTextfile("KasteParabel.txt", data);
            }

            data.Columns["x (m)"].ColumnName = "x";
            data.Columns["y (m)"].ColumnName = "y";

            graphDrawer1.points = data;
            graphDrawer1.Refresh();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.xZoomTextbox = new System.Windows.Forms.TextBox();
            this.yZoomTextbox = new System.Windows.Forms.TextBox();
            this.cwTextbox = new System.Windows.Forms.TextBox();
            this.mTextbox = new System.Windows.Forms.TextBox();
            this.ATextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rhoTextbox = new System.Windows.Forms.TextBox();
            this.gTextbox = new System.Windows.Forms.TextBox();
            this.DrawButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.alphaTextbox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.v0Textbox = new System.Windows.Forms.TextBox();
            this.graphDrawer1 = new GraphDrawer.GraphDrawer(this.components);
            this.textfileCheckbox = new System.Windows.Forms.CheckBox();
            this.y0TextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // xZoomTextbox
            // 
            this.xZoomTextbox.Location = new System.Drawing.Point(496, 35);
            this.xZoomTextbox.Name = "xZoomTextbox";
            this.xZoomTextbox.Size = new System.Drawing.Size(90, 20);
            this.xZoomTextbox.TabIndex = 1;
            this.xZoomTextbox.Text = "1";
            this.xZoomTextbox.TextChanged += new System.EventHandler(this.zoomTextbox_TextChanged);
            // 
            // yZoomTextbox
            // 
            this.yZoomTextbox.Location = new System.Drawing.Point(592, 35);
            this.yZoomTextbox.Name = "yZoomTextbox";
            this.yZoomTextbox.Size = new System.Drawing.Size(89, 20);
            this.yZoomTextbox.TabIndex = 2;
            this.yZoomTextbox.Text = "1";
            this.yZoomTextbox.TextChanged += new System.EventHandler(this.zoomTextbox_TextChanged);
            // 
            // cwTextbox
            // 
            this.cwTextbox.Location = new System.Drawing.Point(517, 74);
            this.cwTextbox.Name = "cwTextbox";
            this.cwTextbox.Size = new System.Drawing.Size(100, 20);
            this.cwTextbox.TabIndex = 3;
            // 
            // mTextbox
            // 
            this.mTextbox.Location = new System.Drawing.Point(517, 100);
            this.mTextbox.Name = "mTextbox";
            this.mTextbox.Size = new System.Drawing.Size(100, 20);
            this.mTextbox.TabIndex = 4;
            // 
            // ATextbox
            // 
            this.ATextbox.Location = new System.Drawing.Point(517, 126);
            this.ATextbox.Name = "ATextbox";
            this.ATextbox.Size = new System.Drawing.Size(100, 20);
            this.ATextbox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(496, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Zoom:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(496, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Konstanter:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(496, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "cᵥᵥ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(500, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "m";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(501, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "A";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(502, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "ρ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(501, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "g";
            // 
            // rhoTextbox
            // 
            this.rhoTextbox.Location = new System.Drawing.Point(517, 178);
            this.rhoTextbox.Name = "rhoTextbox";
            this.rhoTextbox.Size = new System.Drawing.Size(100, 20);
            this.rhoTextbox.TabIndex = 12;
            // 
            // gTextbox
            // 
            this.gTextbox.Location = new System.Drawing.Point(517, 152);
            this.gTextbox.Name = "gTextbox";
            this.gTextbox.Size = new System.Drawing.Size(100, 20);
            this.gTextbox.TabIndex = 11;
            // 
            // DrawButton
            // 
            this.DrawButton.Location = new System.Drawing.Point(496, 279);
            this.DrawButton.Name = "DrawButton";
            this.DrawButton.Size = new System.Drawing.Size(136, 23);
            this.DrawButton.TabIndex = 15;
            this.DrawButton.Text = "Tegn Kasteparabel!";
            this.DrawButton.UseVisualStyleBackColor = true;
            this.DrawButton.Click += new System.EventHandler(this.DrawButton_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(502, 207);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "α";
            // 
            // alphaTextbox
            // 
            this.alphaTextbox.Location = new System.Drawing.Point(517, 204);
            this.alphaTextbox.Name = "alphaTextbox";
            this.alphaTextbox.Size = new System.Drawing.Size(100, 20);
            this.alphaTextbox.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(498, 233);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "v₀";
            // 
            // v0Textbox
            // 
            this.v0Textbox.Location = new System.Drawing.Point(517, 230);
            this.v0Textbox.Name = "v0Textbox";
            this.v0Textbox.Size = new System.Drawing.Size(100, 20);
            this.v0Textbox.TabIndex = 18;
            // 
            // graphDrawer1
            // 
            this.graphDrawer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.graphDrawer1.function = null;
            this.graphDrawer1.Location = new System.Drawing.Point(12, 12);
            this.graphDrawer1.Name = "graphDrawer1";
            this.graphDrawer1.Size = new System.Drawing.Size(478, 309);
            this.graphDrawer1.TabIndex = 0;
            this.graphDrawer1.xOffset = 10;
            this.graphDrawer1.xZoom = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.graphDrawer1.yOffset = 10;
            this.graphDrawer1.yZoom = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // textfileCheckbox
            // 
            this.textfileCheckbox.AutoSize = true;
            this.textfileCheckbox.Location = new System.Drawing.Point(496, 308);
            this.textfileCheckbox.Name = "textfileCheckbox";
            this.textfileCheckbox.Size = new System.Drawing.Size(85, 17);
            this.textfileCheckbox.TabIndex = 20;
            this.textfileCheckbox.Text = "Output textfil";
            this.textfileCheckbox.UseVisualStyleBackColor = true;
            // 
            // y0TextBox
            // 
            this.y0TextBox.Location = new System.Drawing.Point(517, 253);
            this.y0TextBox.Name = "y0TextBox";
            this.y0TextBox.Size = new System.Drawing.Size(100, 20);
            this.y0TextBox.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(498, 256);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(16, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "y₀";
            // 
            // GraphDrawingForm
            // 
            this.ClientSize = new System.Drawing.Size(759, 333);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.y0TextBox);
            this.Controls.Add(this.textfileCheckbox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.v0Textbox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.alphaTextbox);
            this.Controls.Add(this.DrawButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.rhoTextbox);
            this.Controls.Add(this.gTextbox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ATextbox);
            this.Controls.Add(this.mTextbox);
            this.Controls.Add(this.cwTextbox);
            this.Controls.Add(this.yZoomTextbox);
            this.Controls.Add(this.xZoomTextbox);
            this.Controls.Add(this.graphDrawer1);
            this.Name = "GraphDrawingForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
