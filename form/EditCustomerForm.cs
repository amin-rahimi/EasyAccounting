using EasyAccounting.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyAccounting.form
{
    public partial class EditCustomerForm : Form
    {
        private Customer customer;
        CustomerRepository repository = new CustomerRepository();
        public EditCustomerForm(Customer customer)
        {
            InitializeComponent();
            this.customer = customer;
            bSave.DialogResult = DialogResult.OK;
            bCancel.DialogResult = DialogResult.Cancel;
            tName.Text = customer.FirstName;
            tLastName.Text = customer.LastName;
            tPhoneNumber.Text = customer.PhoneNumber;
            tDescription.Text = customer.Description;
            if (customer.Gender.Value)
            {
                rWoman.Checked = true;

            }
            else
            {
                rMan.Checked = true;
            }
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

 

        private void bSave_Click(object sender, EventArgs e)
        {
            Customer c = new Customer();
            c.Id = customer.Id;
            c.CreatedDate = customer.CreatedDate;
            c.FirstName = tName.Text;
            c.LastName = tLastName.Text;
            c.PhoneNumber = tPhoneNumber.Text;
            c.Description = tDescription.Text;
            if (rWoman.Checked == true)
            {
                c.Gender = true;
            }
            else
            {
                c.Gender = false;
            }

         
            repository.updateCustomer(c);
           
            
        }
    }
}
