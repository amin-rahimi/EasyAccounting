using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyAccounting.data;
using EasyAccounting.util;
using Telerik.WinControls.UI;
using Telerik.Charting;
using System.Runtime.InteropServices;
using System.Globalization;

namespace EasyAccounting.form
{
    public partial class ChartViewForm : Form
    {
        public ChartViewForm()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            const int wmNcHitTest = 0x84;
            const int htLeft = 10;
            const int htRight = 11;
            const int htTop = 12;
            const int htTopLeft = 13;
            const int htTopRight = 14;
            const int htBottom = 15;
            const int htBottomLeft = 16;
            const int htBottomRight = 17;

            if (m.Msg == wmNcHitTest)
            {
                int x = (int)(m.LParam.ToInt64() & 0xFFFF);
                int y = (int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16);
                Point pt = PointToClient(new Point(x, y));
                Size clientSize = ClientSize;
                ///allow resize on the lower right corner
                if (pt.X >= clientSize.Width - 16 && pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomLeft : htBottomRight);
                    return;
                }
                ///allow resize on the lower left corner
                if (pt.X <= 16 && pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htBottomRight : htBottomLeft);
                    return;
                }
                ///allow resize on the upper right corner
                if (pt.X <= 16 && pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopRight : htTopLeft);
                    return;
                }
                ///allow resize on the upper left corner
                if (pt.X >= clientSize.Width - 16 && pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(IsMirrored ? htTopLeft : htTopRight);
                    return;
                }
                ///allow resize on the top border
                if (pt.Y <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htTop);
                    return;
                }
                ///allow resize on the bottom border
                if (pt.Y >= clientSize.Height - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htBottom);
                    return;
                }
                ///allow resize on the left border
                if (pt.X <= 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htLeft);
                    return;
                }
                ///allow resize on the right border
                if (pt.X >= clientSize.Width - 16 && clientSize.Height >= 16)
                {
                    m.Result = (IntPtr)(htRight);
                    return;
                }
            }

            switch (m.Msg)
            {
                case WM_NCPAINT:                        // box shadow
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;                     // variables for box shadow
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS                           // struct for box shadow
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }


        private void ChartViewForm_Load(object sender, EventArgs e)
        {

            
            PersianDateFormatter pdf = new PersianDateFormatter();
            cDropDownMonth.DataSource = pdf.getPerisanMonthsNames();
            cDropDownMonth.DisplayMember = "monthString";
            cDropDownMonth.ValueMember = "monthInteger";
            PersianCalendar pc = new PersianCalendar();
            numericUpDown1.Value = pc.GetYear(DateTime.Now);
            cDropDownMonth.Enabled = false;




            //OtherRepository repo = new OtherRepository();
            //List<DayReport> list = repo.getGroupByDay(1394,2);
            //CartesianSeries series = new LineSeries();
            //CategoricalAxis horizontalAxis = new CategoricalAxis();
            //horizontalAxis.PlotMode = AxisPlotMode.OnTicksPadded;
            //LinearAxis verticalAxis = new LinearAxis();
            //verticalAxis.AxisType = AxisType.Second;
            ////verticalAxis.ShowLabels = false;
            //series.PointSize = new SizeF(1, 1); series.HorizontalAxis = horizontalAxis; 
            //series.VerticalAxis = verticalAxis;
            //series.BorderWidth = 3;
            //series.BorderColor = Color.SkyBlue;
            //series.CategoryMember = "Day";
            //series.BorderCornerRadius = 5;
            //series.ValueMember = "Income";
            //series.DataSource = list;
            //series.ShowLabels = true;
            //this.radChartView1.Series.Add(series); 
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                cDropDownMonth.Enabled = true;
            }
            else
            {
                cDropDownMonth.Enabled = false;
            }
        }

        private void bShowChart_Click(object sender, EventArgs e)
        {
            if (cDropDownMonth.Enabled == true)
            {
                radChartView1.Series.Clear();
                radChartView1.Axes.Clear();
                int monthInteger = Convert.ToInt32(cDropDownMonth.SelectedValue.ToString());
                int year = Convert.ToInt32(numericUpDown1.Value);
                OtherRepository repo = new OtherRepository();
                List<DayReport> list = repo.getGroupByDay(year, monthInteger);
                CartesianSeries series = new LineSeries();
                CategoricalAxis horizontalAxis = new CategoricalAxis();
                horizontalAxis.PlotMode = AxisPlotMode.OnTicksPadded;
                LinearAxis verticalAxis = new LinearAxis();
                verticalAxis.AxisType = AxisType.Second;
                series.PointSize = new SizeF(5, 5); 
                series.HorizontalAxis = horizontalAxis;
                series.VerticalAxis = verticalAxis;
                series.BorderWidth = 3;
                series.BorderColor = Color.SkyBlue;
                series.CategoryMember = "Day";
                series.ValueMember = "Income";
                series.DataSource = list;
                series.ShowLabels = true;
                this.radChartView1.Series.Add(series);
            }
            else if(comboBox1.SelectedIndex == 1)
            {
                radChartView1.Series.Clear();
                radChartView1.Axes.Clear();
                int monthInteger = Convert.ToInt32(cDropDownMonth.SelectedValue.ToString());
                int year = Convert.ToInt32(numericUpDown1.Value);
                OtherRepository repo = new OtherRepository();
                List<MonthReport> list = repo.getGroupByMonth(year);
                CartesianSeries series = new LineSeries();
                CategoricalAxis horizontalAxis = new CategoricalAxis();
                horizontalAxis.PlotMode = AxisPlotMode.OnTicksPadded;
                LinearAxis verticalAxis = new LinearAxis();
                verticalAxis.AxisType = AxisType.Second;
                series.PointSize = new SizeF(5, 5); 
                series.HorizontalAxis = horizontalAxis;
                series.VerticalAxis = verticalAxis;
                series.BorderWidth = 3;
                series.BorderColor = Color.SkyBlue;
                series.CategoryMember = "Month";
                series.ValueMember = "Income";
                series.DataSource = list;
                series.ShowLabels = true;
                this.radChartView1.Series.Add(series);
            }
            else
            {
                radChartView1.Series.Clear();
                radChartView1.Axes.Clear();
                int monthInteger = Convert.ToInt32(cDropDownMonth.SelectedValue.ToString());
                int year = Convert.ToInt32(numericUpDown1.Value);
                OtherRepository repo = new OtherRepository();
                List<YearReport> list = repo.getGroupByYear();
                CartesianSeries series = new LineSeries();
                CategoricalAxis horizontalAxis = new CategoricalAxis();
                horizontalAxis.PlotMode = AxisPlotMode.OnTicksPadded;
                LinearAxis verticalAxis = new LinearAxis();
                verticalAxis.AxisType = AxisType.Second;
                series.PointSize = new SizeF(5, 5);
                series.HorizontalAxis = horizontalAxis;
                series.VerticalAxis = verticalAxis;
                series.BorderWidth = 3;
                series.BorderColor = Color.SkyBlue;
                series.CategoryMember = "Year";
                series.ValueMember = "Income";
                series.DataSource = list;
                series.ShowLabels = true;
                this.radChartView1.Series.Add(series);
            }
        }
    }
}
