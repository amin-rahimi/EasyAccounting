using EasyAccounting.form;
using EasyAccounting.util;
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
using Telerik.WinControls.UI;
using GsmComm.GsmCommunication;
using System.IO.Ports;
using System.Threading;
using GsmComm.PduConverter;



namespace EasyAccounting
{
    public partial class EasyAccounting : Form
    {
        private int dayCounter = 0;
        public EasyAccounting()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CustomersForm().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new CustomersForm().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void AddCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddCustomerForm().ShowDialog();
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

        private void EasyAccounting_Load(object sender, EventArgs e)
        {
            PersianDateFormatter pdf = new PersianDateFormatter();
            toolStripTextBox1.Text = pdf.getDateString(DateTime.Now);
            ((GridTableElement)radGridView1.TableElement).AlternatingRowColor = Color.FromArgb(215, 234, 124);
            radGridView1.TableElement.RowHeight = 50;
            OtherRepository otherRepository = new OtherRepository();
            radGridView1.DataSource = otherRepository.getCustomerJoinContract(DateTime.Now);
            timer1.Start();



            //GsmCommMain comm = new GsmCommMain("COM8", 9600, 150);
            //comm.Open();
            //byte dcs = (byte)DataCodingScheme.GeneralCoding.Alpha16Bit;
            //SmsSubmitPdu pdu = new SmsSubmitPdu("امین", "09398987855", dcs);
            //comm.SendMessage(pdu);



          
        }

        private void radGridView1_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            //TimeSpan eight = new TimeSpan(8,0,0);
            TimeSpan ten = new TimeSpan(10, 0, 0);
            TimeSpan twelve = new TimeSpan(12, 0, 0);
            TimeSpan fourteen = new TimeSpan(14, 0, 0);
            TimeSpan sixteen = new TimeSpan(16, 0, 0);
            TimeSpan eighteen = new TimeSpan(18, 0, 0);
            TimeSpan twenty = new TimeSpan(20, 0, 0);

            PersianDateFormatter pdf = new PersianDateFormatter();
            DateTime dt = DateTime.Now;
            TimeSpan ts = new TimeSpan(dt.TimeOfDay.Hours, dt.TimeOfDay.Minutes, 0);
            int gridDate = Convert.ToInt32(e.RowElement.RowInfo.Cells[5].Value.ToString());
            int currentDate = pdf.getDateInteger(dt);
            TimeSpan gridTimeSpan = (TimeSpan)e.RowElement.RowInfo.Cells[4].Value;
            if (e.RowElement.RowInfo.Index > -1)
            {
                if (TimeSpan.Compare(gridTimeSpan, ts) < 0 && gridDate <= currentDate || gridDate < currentDate)
                {

                    e.RowElement.BackColor = Color.FromArgb(245, 245, 245);
                }
                else
                {
                    if (TimeSpan.Compare(gridTimeSpan, ten) < 0)
                    {
                        //8 10
                        e.RowElement.BackColor = Color.FromArgb(255, 252, 220);
                    }
                    else if (TimeSpan.Compare(gridTimeSpan, twelve) < 0)
                    {
                        //10 12
                        e.RowElement.BackColor = Color.FromArgb(255, 247, 179);
                    }
                    else if (TimeSpan.Compare(gridTimeSpan, fourteen) < 0)
                    {
                        //12 14
                        e.RowElement.BackColor = Color.FromArgb(255, 232, 138);
                    }
                    else if (TimeSpan.Compare(gridTimeSpan, sixteen) < 0)
                    {
                        //14 16
                        e.RowElement.BackColor = Color.FromArgb(255, 193, 74);

                    }
                    else if (TimeSpan.Compare(gridTimeSpan, eighteen) < 0)
                    {
                        //16 18
                        e.RowElement.BackColor = Color.FromArgb(255, 189, 234);
                    }
                    else if (TimeSpan.Compare(gridTimeSpan, twenty) < 0)
                    {
                        //18 20
                        e.RowElement.BackColor = Color.FromArgb(227, 166, 255);
                    }
                    else
                    {
                        e.RowElement.BackColor = Color.FromArgb(196, 180, 255);
                    }
                }

            }
        }

        private void toolStripbBack_Click(object sender, EventArgs e)
        {
            PersianDateFormatter pdf = new PersianDateFormatter();
            DateTime dt = DateTime.Now;
            dayCounter--;
            dt = dt.AddDays(dayCounter);
            toolStripTextBox1.Text = pdf.getDateString(dt);
            OtherRepository otherRepository = new OtherRepository();
            radGridView1.DataSource = otherRepository.getCustomerJoinContract(dt);
        }

        private void toolStripbNext_Click(object sender, EventArgs e)
        {
            PersianDateFormatter pdf = new PersianDateFormatter();
            DateTime dt = DateTime.Now;
            dayCounter++;
            dt = dt.AddDays(dayCounter);
            toolStripTextBox1.Text = pdf.getDateString(dt);
            OtherRepository otherRepository = new OtherRepository();
            radGridView1.DataSource = otherRepository.getCustomerJoinContract(dt);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            PersianDateFormatter pdf = new PersianDateFormatter();
            int date = pdf.convert(toolStripTextBox1.Text);
            OtherRepository otherRepository = new OtherRepository();
            radGridView1.DataSource = otherRepository.getCustomerJoinContract(date);
        }

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                int customerId = Convert.ToInt32(radGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                //MessageBox.Show(radGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                new CustomerInfoForm(customerId).ShowDialog();

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            PersianDateFormatter pdf = new PersianDateFormatter();
            int date = pdf.convert(toolStripTextBox1.Text);
            OtherRepository otherRepository = new OtherRepository();
            radGridView1.DataSource = otherRepository.getCustomerJoinContract(date);
           
            
            /////////////////////////////////////////////////
            DefaultSettingsRepository dRepo = new DefaultSettingsRepository();
            DefaultSetting gsmPort = dRepo.GetSetting("gsm_port");
            DefaultSetting lastSend = dRepo.GetSetting("last_send");

            if (lastSend == null)
            {
                DefaultSetting ls = new DefaultSetting();
                ls.Value = "13000101";
                ls.Name = "last_send";
                dRepo.AddSettings(ls);
            }
            else
            {
                int lastSendInt = Convert.ToInt32(lastSend.Value);
                int todayInt = pdf.getDateInteger(DateTime.Now);
                int x = 0;
                if (todayInt <= lastSendInt)
                {
                    return;
                }
            }



            if (gsmPort != null)
            {

                if (gsmPort.Value != "NULL")
                {
                    try
                    {
                        GsmCommMain comm = new GsmCommMain(gsmPort.Value, 9600, 150);
                        comm.Open();
                        byte dcs = (byte)DataCodingScheme.GeneralCoding.Alpha16Bit;

                        DefaultSetting sendTime = dRepo.GetSetting("send_message_time");
                        DefaultSetting days = dRepo.GetSetting("days_before_send");
                        DefaultSetting text = dRepo.GetSetting("message_text");
                        int daysInteger = Convert.ToInt32(days.Value);

                        TimeSpan ts = TimeSpan.Parse(sendTime.Value);
                        TimeSpan nts = DateTime.Now.TimeOfDay;
                        TimeSpan fivemin = TimeSpan.FromMinutes(5);

                        if (nts.CompareTo(ts) >= 0)
                        {
                            if (nts.Subtract(ts).CompareTo(fivemin) > 0)
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (ts.Subtract(nts).CompareTo(fivemin) > 0)
                            {
                                return;
                            }
                        }

                        DateTime dt = DateTime.Now;
                        dt = dt.AddDays(daysInteger);
                        string dateString = pdf.getDateString(dt);

                        object customers = otherRepository.getCustomerJoinContract(dt);

                        IEnumerable<object> collection = (IEnumerable<object>)customers;
                        foreach (object item in collection)
                        {
                            var nameOfProperty = "PhoneNumber";
                            var propertyInfo = item.GetType().GetProperty(nameOfProperty);
                            var phoneNumber = propertyInfo.GetValue(item, null);
                            

                            var nameOfProperty2 = "Time";
                            var propertyInfo2 = item.GetType().GetProperty(nameOfProperty2);
                            var time = propertyInfo2.GetValue(item, null);

                            string timeString = time.ToString();
                            timeString = timeString.Remove(timeString.Length-3);

                            string smsText = text.Value + Environment.NewLine + dateString + Environment.NewLine + timeString;

                            SmsSubmitPdu pdu = new SmsSubmitPdu(smsText, phoneNumber.ToString(), dcs);
                            comm.SendMessage(pdu);
                        }

                        DefaultSettingsRepository dRepo2 = new DefaultSettingsRepository();
                        DefaultSetting lsls =  dRepo2.GetSetting("last_send");
                        lsls.Value = pdf.getDateInteger(DateTime.Now).ToString();
                        dRepo2.UpdateSetting(lsls);
                          
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
       

        }

        private void bChartView_Click(object sender, EventArgs e)
        {
            new ChartViewForm().ShowDialog();
        }

        

        private void bSms_Click(object sender, EventArgs e)
        {
            new GSMSetting().ShowDialog();
        }

        private void bSetting_Click(object sender, EventArgs e)
        {
            new DefaultSettingsForm().ShowDialog();
        }

        private void bPayments_Click(object sender, EventArgs e)
        {
            new CustomerPaymentsForm().ShowDialog();
        }






    }
}
