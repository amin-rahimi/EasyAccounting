using EasyAccounting.data;
using EasyAccounting.util;
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
using Telerik.WinControls.UI;

namespace EasyAccounting.form
{
    public partial class CustomerInfoForm : Form
    {
        private int customerId;
        private int selectedContractId;
        public CustomerInfoForm(int id)
        {
            InitializeComponent();
            customerId = id;
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

        private void CustomerInfoForm_Load(object sender, EventArgs e)
        {

            PersianDateFormatter pdf = new PersianDateFormatter();
            CustomerRepository repository = new CustomerRepository();
            Customer customer = repository.getCustomer(customerId);
            lName.Text = customer.FirstName + " " + customer.LastName;
            lPhoneNumber.Text = "شماره تماس:" + "   " + customer.PhoneNumber;
            lCreatedDate.Text = "تاریخ ایجاد:" + "   " + pdf.convert(customer.CreatedDate.Value);
            if (String.IsNullOrWhiteSpace(customer.Description))
            {
                lDescription.Text = "توضیحات:" + "   " + "-";
            }
            else
            {
                lDescription.Text = "توضیحات:" + "   " + customer.Description;
            }
            ContractRepository crepository = new ContractRepository();
            radGridView1.DataSource = crepository.getContractsByCustomerId(customerId).ToList();

            ((GridTableElement)radGridView1.TableElement).AlternatingRowColor = Color.FromArgb(215, 234, 124);
            radGridView1.TableElement.RowHeight = 25;
            ((GridTableElement)radGridView2.TableElement).AlternatingRowColor = Color.FromArgb(255, 205, 139);
            radGridView2.TableElement.RowHeight = 25;
            ((GridTableElement)radGridView3.TableElement).AlternatingRowColor = Color.FromArgb(240, 240, 240);
            radGridView3.TableElement.RowHeight = 25;


            if (radGridView1.SelectedRows.Count > 0)
            {
                //MessageBox.Show(radGridView1.SelectedRows[0].Cells[0].Value.ToString());
                PaymentRepository paymentRepository = new PaymentRepository();
                AppointmentRepository appointmentRepository = new AppointmentRepository();
                selectedContractId = Convert.ToInt32(radGridView1.SelectedRows[0].Cells[0].Value.ToString());
                radGridView2.DataSource = paymentRepository.getPaymentsByContractId(selectedContractId).ToList();
                radGridView3.DataSource = appointmentRepository.getAppointmentByContractId(selectedContractId).ToList();
            }

            if (radGridView1.SelectedRows.Count < 1)
            {
                bNewAppointment.Enabled = false;
                bNewPayment.Enabled = false;
            }


        }

        private void bEditInfo_Click(object sender, EventArgs e)
        {
            PersianDateFormatter pdf = new PersianDateFormatter();
            CustomerRepository repository = new CustomerRepository();
            Customer customer = repository.getCustomer(customerId);
            EditCustomerForm editCustomerForm = new EditCustomerForm(customer);
            var result = editCustomerForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                CustomerRepository cr = new CustomerRepository();
                customer = cr.getCustomer(customerId);
                lName.Text = customer.FirstName + " " + customer.LastName;
                lPhoneNumber.Text = "شماره تماس:" + "   " + customer.PhoneNumber;
                lCreatedDate.Text = "تاریخ ایجاد:" + "   " + pdf.convert(customer.CreatedDate.Value);
                if (String.IsNullOrWhiteSpace(customer.Description))
                {
                    lDescription.Text = "توضیحات:" + "   " + "-";
                }
                else
                {
                    lDescription.Text = "توضیحات:" + "   " + customer.Description;
                }

            }
        }

        private void bNewContract_Click(object sender, EventArgs e)
        {
            AddEditContractForm contractForm = new AddEditContractForm(true, -1, customerId);
            var result = contractForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                ContractRepository repository = new ContractRepository();
                radGridView1.DataSource = repository.getContractsByCustomerId(customerId).ToList();
            }

        }

        private void radGridView1_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            DialogResult result = MessageBox.Show("آیا از عملیات حذف مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                ContractRepository repository = new ContractRepository();
                //MessageBox.Show(radGridView1.SelectedRows[0].Cells[0].Value.ToString());
                repository.deleteContract(Convert.ToInt32(radGridView1.SelectedRows[0].Cells[0].Value.ToString()));
            }
        }

        private void bNewAppointment_Click(object sender, EventArgs e)
        {

            AddAppointment addAppointment = new AddAppointment(selectedContractId);
            var result = addAppointment.ShowDialog();
            if (result == DialogResult.OK)
            {
                ContractRepository repo = new ContractRepository();
                AppointmentRepository appointmentRepository = new AppointmentRepository();
                radGridView3.DataSource = appointmentRepository.getAppointmentByContractId(selectedContractId).ToList();
                radGridView1.DataSource = repo.getContractsByCustomerId(customerId).ToList();
            }
        }

        private void bNewPayment_Click(object sender, EventArgs e)
        {
            AddPayment addPayment = new AddPayment(selectedContractId);
            var result = addPayment.ShowDialog();
            if (result == DialogResult.OK)
            {
                PaymentRepository paymentRepository = new PaymentRepository();
                ContractRepository repository = new ContractRepository();
                radGridView1.DataSource = repository.getContractsByCustomerId(customerId).ToList();
                radGridView2.DataSource = paymentRepository.getPaymentsByContractId(selectedContractId).ToList();
            }
        }

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                int contractId = Convert.ToInt32(radGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                AddEditContractForm contractForm = new AddEditContractForm(false, contractId, customerId);
                var result = contractForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ContractRepository repository = new ContractRepository();
                    radGridView1.DataSource = repository.getContractsByCustomerId(customerId).ToList();
                }


            }
        }

        private void radGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(radGridView1.SelectedRows[0].Cells[0].Value.ToString());
            PaymentRepository paymentRepository = new PaymentRepository();
            AppointmentRepository appointmentRepository = new AppointmentRepository();
            selectedContractId = Convert.ToInt32(radGridView1.SelectedRows[0].Cells[0].Value.ToString());
            radGridView2.DataSource = paymentRepository.getPaymentsByContractId(selectedContractId).ToList();
            radGridView3.DataSource = appointmentRepository.getAppointmentByContractId(selectedContractId).ToList();
            bNewAppointment.Enabled = true;
            bNewPayment.Enabled = true;
        }

        private void radGridView2_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            //payment
            DialogResult result = MessageBox.Show("آیا از عملیات حذف مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                ContractRepository repository = new ContractRepository();
                PaymentRepository pRepo = new PaymentRepository();
                int paymentAmount = Convert.ToInt32(radGridView2.SelectedRows[0].Cells[1].Value.ToString());
                int paymentId = Convert.ToInt32(radGridView2.SelectedRows[0].Cells[0].Value.ToString());
                Contract contract = repository.getContract(selectedContractId);
                contract.Payment -= paymentAmount;
                pRepo.deletePayment(paymentId);
                repository.updateContract(contract);
                radGridView1.DataSource = repository.getContractsByCustomerId(customerId).ToList();
            }
        }

        private void radGridView3_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                int appointmentId = Convert.ToInt32(radGridView3.Rows[e.RowIndex].Cells[0].Value.ToString());
                EditAppointmentForm edaForm = new EditAppointmentForm(appointmentId);
                var result = edaForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    AppointmentRepository repo = new AppointmentRepository();
                    radGridView3.DataSource = repo.getAppointmentByContractId(selectedContractId).ToList();
                    ContractRepository repository = new ContractRepository();
                    radGridView1.DataSource = repository.getContractsByCustomerId(customerId).ToList();
                }
            }
        }

        private void radGridView3_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            DialogResult result = MessageBox.Show("آیا از عملیات حذف مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                int value = 0;
                DefaultSettingsRepository dRepo = new DefaultSettingsRepository();
                ContractRepository cRepo = new ContractRepository();
                AppointmentRepository rep = new AppointmentRepository();
                int appointmentId = Convert.ToInt32(radGridView3.SelectedRows[0].Cells[0].Value.ToString());
                Appointment appo = rep.getAppointment(appointmentId);
                string[] parts = appo.Description.Split('/');
                foreach (string part in parts)
                {
                    DefaultSetting ds = dRepo.GetSetting(part);
                    value += Convert.ToInt32(ds.Value);

                }
                Contract contract = cRepo.getContract(appo.ContractId.Value);
                contract.ContractPayment -= value;
                cRepo.updateContract(contract);

                rep.deleteAppointment(appointmentId);
                ContractRepository repository = new ContractRepository();
                radGridView1.DataSource = repository.getContractsByCustomerId(customerId).ToList();
            }
        }

        private void bPhotoManager_Click(object sender, EventArgs e)
        {
            new PhotoManagerForm(customerId).ShowDialog();
        }

    }
}
