using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyAccounting.form
{
    public partial class PhotoManagerForm : Form
    {
        private int customerId;
        private String mainUserPhotoPath;
        public PhotoManagerForm(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            mainUserPhotoPath = Application.StartupPath + "\\images\\" + customerId.ToString();
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

        private void PhotoManagerForm_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(mainUserPhotoPath))
            {
                Directory.CreateDirectory(mainUserPhotoPath);
                Directory.CreateDirectory(mainUserPhotoPath + "\\before");
                Directory.CreateDirectory(mainUserPhotoPath + "\\after");
            }
            FileInfo[] fiBeforeImages = getImagesFromFolder(mainUserPhotoPath + "\\before");
            FileInfo[] fiAfterImages = getImagesFromFolder(mainUserPhotoPath + "\\after");

            foreach (FileInfo fInfo in fiBeforeImages)
            {
                PictureBox pBox = new PictureBox();
                pBox.ImageLocation = fInfo.FullName;
                pBox.Height = 100;
                pBox.Width = 100;
                pBox.Margin = new Padding(2);
                pBox.SizeMode = PictureBoxSizeMode.StretchImage;
                //pBox.Click += new EventHandler(this.pictureBoxClick);
                pBox.DoubleClick += new EventHandler(this.pictureBoxClick);
                flowLayoutPanel1.Controls.Add(pBox);
            }

            foreach (FileInfo fInfo in fiAfterImages)
            {
                PictureBox pBox = new PictureBox();
                pBox.ImageLocation = fInfo.FullName;
                pBox.Height = 100;
                pBox.Width = 100;
                pBox.Margin = new Padding(2);
                pBox.SizeMode = PictureBoxSizeMode.StretchImage;
                //pBox.Click += new EventHandler(this.pictureBoxClick);
                pBox.DoubleClick += new EventHandler(this.pictureBoxClick);
                flowLayoutPanel2.Controls.Add(pBox);
            }

  
            
        }

        private void bOpenDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(mainUserPhotoPath + "\\before");
        }

        private FileInfo[] getImagesFromFolder(String path)
        {
            DirectoryInfo dInfo = new DirectoryInfo(path);
            FileInfo[] fInfo = dInfo.GetFiles("*.jpg")
                .Union(dInfo.GetFiles("*.jpeg"))
                .Union(dInfo.GetFiles("*.bmp"))
                .Union(dInfo.GetFiles("*.tiff")).ToArray();

            return fInfo;
        }

        void pictureBoxClick(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            Process.Start(pb.ImageLocation);
        }

        private void bAferOpen_Click(object sender, EventArgs e)
        {
            Process.Start(mainUserPhotoPath + "\\after");
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            FileInfo[] fiBeforeImages = getImagesFromFolder(mainUserPhotoPath + "\\before");
            FileInfo[] fiAfterImages = getImagesFromFolder(mainUserPhotoPath + "\\after");

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            foreach (FileInfo fInfo in fiBeforeImages)
            {
                PictureBox pBox = new PictureBox();
                pBox.ImageLocation = fInfo.FullName;
                pBox.Height = 100;
                pBox.Width = 100;
                pBox.Margin = new Padding(2);
                pBox.SizeMode = PictureBoxSizeMode.StretchImage;
                //pBox.Click += new EventHandler(this.pictureBoxClick);
                pBox.DoubleClick += new EventHandler(this.pictureBoxClick);
                flowLayoutPanel1.Controls.Add(pBox);
            }

            foreach (FileInfo fInfo in fiAfterImages)
            {
                PictureBox pBox = new PictureBox();
                pBox.ImageLocation = fInfo.FullName;
                pBox.Height = 100;
                pBox.Width = 100;
                pBox.Margin = new Padding(2);
                pBox.SizeMode = PictureBoxSizeMode.StretchImage;
                //pBox.Click += new EventHandler(this.pictureBoxClick);
                pBox.DoubleClick += new EventHandler(this.pictureBoxClick);
                flowLayoutPanel2.Controls.Add(pBox);
            }
        }
    }
}
