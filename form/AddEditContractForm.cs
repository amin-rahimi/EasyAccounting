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

namespace EasyAccounting.form
{
    public partial class AddEditContractForm : Form
    {
        private Boolean isNewContract;
        private int contractId;
        private int customerId;
        public AddEditContractForm(Boolean isNew, int contractId, int customerId)
        {

            InitializeComponent();
            this.isNewContract = isNew;
            this.contractId = contractId;
            this.customerId = customerId;
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

        private void AddEditContractForm_Load(object sender, EventArgs e)
        {
            
            bSave.DialogResult = DialogResult.OK;
            bCancel.DialogResult = DialogResult.Cancel;
            PersianDateFormatter pdf = new PersianDateFormatter();
            tNextAppointmentTime.Format = DateTimePickerFormat.Custom;
            tNextAppointmentTime.CustomFormat = "HH:mm";
            tNextAppointmentTime.ShowUpDown = true;
            if (isNewContract)
            {
                tPayment.Text = "0";
                tStartDate.Text = pdf.getDateString(DateTime.Now);
                tNextAppointmentDate.Text = pdf.getDateString(DateTime.Now.AddMonths(1));

            }
            else
            {
                ContractRepository repository = new ContractRepository();
                Contract contract = repository.getContract(contractId);
                tPayment.Text = contract.ContractPayment.Value.ToString();

                tNextAppointmentDate.Text = pdf.convert(contract.NextAppointmentDate.Value);
                tStartDate.Text = pdf.convert(contract.ContractStartDate.Value);

                if (contract.NextAppointmentTime.Value != null)
                {
                    DateTime dt = new DateTime(2000, 1, 1, 0, 0, 0).Add(contract.NextAppointmentTime.Value);
                  
                    tNextAppointmentTime.Value = dt;
                }
                if (!String.IsNullOrWhiteSpace(contract.Description))
                {
                    tDescription.Text = contract.Description;
                }
                if (contract.IsAppointmentFinished.Value == true)
                {
                    cIsFinished.Checked = true;
                }
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (isNewContract)
            {
                PersianDateFormatter pdf = new PersianDateFormatter();
                ContractRepository repository = new ContractRepository();
                Contract contract = new Contract();
                contract.ContractPayment = Convert.ToInt32(tPayment.Text);
                contract.ContractStartDate = pdf.convert(tStartDate.Text);
                contract.CustomerId = this.customerId;
                contract.Description = tDescription.Text;
                contract.IsAppointmentFinished = cIsFinished.Checked;
                if (cIsFinished.Checked)
                {
                    contract.ContractEndDate = pdf.getDateInteger(DateTime.Now);
                }
                if (!String.IsNullOrWhiteSpace(tNextAppointmentDate.Text))
                {
                    contract.NextAppointmentDate = pdf.convert(tNextAppointmentDate.Text);
                }
                if (!String.IsNullOrWhiteSpace(tNextAppointmentTime.Text))
                {
                    TimeSpan ts = new TimeSpan(tNextAppointmentTime.Value.TimeOfDay.Hours, tNextAppointmentTime.Value.TimeOfDay.Minutes, 0);
                    contract.NextAppointmentTime = ts;
                }
                contract.Payment = 0;
                repository.addContract(contract);

            }
            else
            {
                PersianDateFormatter pdf = new PersianDateFormatter();
                ContractRepository repository = new ContractRepository();
                Contract contract = repository.getContract(contractId);
                contract.Id = contractId;
                contract.CustomerId = customerId;
                contract.ContractPayment = Convert.ToInt32(tPayment.Text);
                contract.ContractStartDate = pdf.convert(tStartDate.Text);
                contract.Description = tDescription.Text;
                contract.IsAppointmentFinished = cIsFinished.Checked;
                if (cIsFinished.Checked)
                {
                    contract.ContractEndDate = pdf.getDateInteger(DateTime.Now);
                }
                if (!String.IsNullOrWhiteSpace(tNextAppointmentDate.Text))
                {
                    contract.NextAppointmentDate = pdf.convert(tNextAppointmentDate.Text);
                }
                if (!String.IsNullOrWhiteSpace(tNextAppointmentTime.Text))
                {
                    TimeSpan ts = new TimeSpan(tNextAppointmentTime.Value.TimeOfDay.Hours, tNextAppointmentTime.Value.TimeOfDay.Minutes, 0);
                    contract.NextAppointmentTime = ts;
                }
                repository.updateContract(contract);
            }
        }
    }
}
