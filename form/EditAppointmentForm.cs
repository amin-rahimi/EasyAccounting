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
using EasyAccounting.util;
using EasyAccounting.data;
using Telerik.WinControls.UI;

namespace EasyAccounting.form
{
    public partial class EditAppointmentForm : Form
    {
        int appointmentId;
        int valueChange = 0;
        int unchangedValue = 0;
        public EditAppointmentForm(int appointmentId)
        {
            InitializeComponent();
            this.appointmentId = appointmentId;
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

        private void EditAppointmentForm_Load(object sender, EventArgs e)
        {
            DefaultSettingsRepository dRepo = new DefaultSettingsRepository();
            radCheckedDropDownList1.DataSource = dRepo.GetSetingsByType("price").ToList();
            radCheckedDropDownList1.DisplayMember = "Name";
            radCheckedDropDownList1.ValueMember = "Value";


            PersianDateFormatter pdf = new PersianDateFormatter();
            tTime.Format = DateTimePickerFormat.Custom;
            tTime.CustomFormat = "HH:mm";
            tTime.ShowUpDown = true;
            bSave.DialogResult = DialogResult.OK;
            bCancel.DialogResult = DialogResult.Cancel;
            AppointmentRepository repo = new AppointmentRepository();
            Appointment appo = repo.getAppointment(appointmentId);
            DateTime dt = new DateTime(2000, 1, 1, 0, 0, 0).Add(appo.AppointmentTime.Value);
            tTime.Value = dt;
            tDate.Text = pdf.convert(appo.AppointmentDate.Value);
            string[] parts = appo.Description.Split('/');

            foreach (RadCheckedListDataItem item in radCheckedDropDownList1.Items)
            {
                if (parts.Contains(item.DisplayValue.ToString()))
                {
                    item.Checked = true;
                    unchangedValue += Convert.ToInt32(item.Value.ToString());
                }
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            PersianDateFormatter pdf = new PersianDateFormatter();
            AppointmentRepository appRepo = new AppointmentRepository();
            Appointment app = appRepo.getAppointment(appointmentId);
            //app.Description = tDescription.Text;
            app.AppointmentDate = pdf.convert(tDate.Text);
            TimeSpan ts = new TimeSpan(tTime.Value.TimeOfDay.Hours, tTime.Value.TimeOfDay.Minutes, 0);
            app.AppointmentTime = ts;
            string description = "";
            foreach (var item in radCheckedDropDownList1.CheckedItems)
            {
                description += item.DisplayValue.ToString() + "/";
            }
            description = description.Remove(description.Length - 1);
            app.Description = description;
            appRepo.updateAppointment(app);
            ContractRepository cRepo = new ContractRepository();
            Contract contract = cRepo.getContract(app.ContractId.Value);
            contract.ContractPayment += valueChange - unchangedValue;
            cRepo.updateContract(contract);
        }

        private void radCheckedDropDownList1_ItemCheckedChanged(object sender, RadCheckedListDataItemEventArgs e)
        {
            if (e.Item.Checked == false)
            {
                valueChange -= Convert.ToInt32(e.Item.Value);
            }
            else
            {
                valueChange += Convert.ToInt32(e.Item.Value);
            }
        }

    }
}
