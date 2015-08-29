using EasyAccounting.data;
using EasyAccounting.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyAccounting.form
{
    public partial class AddCustomerForm : Form
    {

        public AddCustomerForm()
        {
            InitializeComponent();
            bSave.DialogResult = DialogResult.OK;
            bCancel.DialogResult = DialogResult.Cancel;
        }



        private void AddCustomerForm_Load(object sender, EventArgs e)
        {
            //     openFileDialog1.Filter = "JPG|*.jpg;*.jpeg|BMP|*.bmp|GIF|*.gif|PNG|*.png|TIFF|*.tif;*.tiff|"
            //+ "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
            //     openFileDialog1.Title = "لطفا تصویر مشتری را انتخاب کنید";
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            CustomerRepository repository = new CustomerRepository();
            if (String.IsNullOrWhiteSpace(tName.Text) || String.IsNullOrWhiteSpace(tLastName.Text) || String.IsNullOrWhiteSpace(tPhoneNumber.Text))
            {
                MessageBox.Show("اطلاعات کامل وارد نشده است");
                return;
            }
            PersianDateFormatter pdf = new PersianDateFormatter();
            customer.CreatedDate = pdf.getDateInteger(DateTime.Now);
            customer.FirstName = tName.Text;
            customer.LastName = tLastName.Text;
            customer.PhoneNumber = tPhoneNumber.Text;
            if (rWoman.Checked)
            {
                customer.Gender = true;
            }
            else
            {
                customer.Gender = false;
            }

            if (!String.IsNullOrWhiteSpace(tDescription.Text))
            {
                customer.Description = tDescription.Text;
            }

            repository.addCustomer(customer);
            //MessageBox.Show("با موفقیت اضافه شد");


        }

        protected override void WndProc(ref Message m)
        {
            

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
    }
}
