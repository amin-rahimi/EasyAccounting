using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyAccounting.data;
using EasyAccounting.util;

namespace EasyAccounting.form
{
    public partial class GSMSetting : Form
    {
        public GSMSetting()
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

        private void GSMSetting_Load(object sender, EventArgs e)
        {

            bCancel.DialogResult = DialogResult.Cancel;
            bSave.DialogResult = DialogResult.OK;

            PersianDateFormatter pdf = new PersianDateFormatter();
            tTime.Format = DateTimePickerFormat.Custom;
            tTime.CustomFormat = "HH:mm";
            tTime.ShowUpDown = true;


            List<String> ports = SerialPort.GetPortNames().ToList();
            ports.Insert(0, " ");
            comboBox1.DataSource = ports;


            DefaultSettingsRepository repo = new DefaultSettingsRepository();
            DefaultSetting gsmPort = repo.GetSetting("gsm_port");
            DefaultSetting sendMessageTime = repo.GetSetting("send_message_time");
            DefaultSetting sendMessafeDaysBeforeAppointment = repo.GetSetting("days_before_send");
            DefaultSetting messageText = repo.GetSetting("message_text");

            if (gsmPort == null)
            {
                comboBox1.Text = " ";

            }
            else
            {
                comboBox1.Text = gsmPort.Value;
            }

            if (sendMessafeDaysBeforeAppointment == null)
            {
                tDays.Text = "1";
            }
            else
            {
                tDays.Text = sendMessafeDaysBeforeAppointment.Value;
            }

            if (sendMessageTime != null)
            {
                TimeSpan ts = TimeSpan.Parse(sendMessageTime.Value);
                DateTime dt = new DateTime(2000, 1, 1, 0, 0, 0).Add(ts);
                tTime.Value = dt;
            }

            if (messageText != null)
            {
                tMessageText.Text = messageText.Value;
            }



        }

        private void bFindPort_Click(object sender, EventArgs e)
        {
            List<String> ports = SerialPort.GetPortNames().ToList();


            foreach (String port in ports)
            {

                SerialPort sPort = new SerialPort(port);

                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    sPort.Open();
                    bool x = CheckExistingModemOnComPort(sPort);
                    sPort.Close();
                    if (x)
                    {
                        comboBox1.Text = port;
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            MessageBox.Show("مودم متصل نمی باشد");
        }


        private bool CheckExistingModemOnComPort(SerialPort serialPort)
        {
            if ((serialPort == null) || !serialPort.IsOpen)
                return false;

            // Commands for modem checking
            string[] modemCommands = new string[] { "AT",       // Check connected modem. After 'AT' command some modems autobaud their speed.
                                            "ATQ0" };   // Switch on confirmations
            serialPort.DtrEnable = true;    // Set Data Terminal Ready (DTR) signal 
            serialPort.RtsEnable = true;    // Set Request to Send (RTS) signal

            string answer = "";
            bool retOk = false;
            for (int rtsInd = 0; rtsInd < 2; rtsInd++)
            {
                foreach (string command in modemCommands)
                {
                    serialPort.Write(command + serialPort.NewLine);
                    retOk = false;
                    answer = "";
                    int timeout = (command == "AT") ? 10 : 20;

                    // Waiting for response 1-2 sec
                    for (int i = 0; i < timeout; i++)
                    {
                        Thread.Sleep(100);
                        answer += serialPort.ReadExisting();
                        if (answer.IndexOf("OK") >= 0)
                        {
                            retOk = true;
                            break;
                        }
                    }
                }
                // If got responses, we found a modem
                if (retOk)
                {
                    return true;
                }
                // Trying to execute the commands without RTS
                serialPort.RtsEnable = false;

            }
            return false;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            DefaultSettingsRepository repo = new DefaultSettingsRepository();
            DefaultSetting gsmPortTemp = repo.GetSetting("gsm_port");

            if (gsmPortTemp == null)
            {

                DefaultSetting gsmPort = new DefaultSetting();
                gsmPort.Name = "gsm_port";
                if (!String.IsNullOrEmpty(comboBox1.Text))
                {
                    gsmPort.Value = comboBox1.Text;
                    repo.AddSettings(gsmPort);
                }
                else
                {
                    gsmPort.Value = "NULL";
                    repo.AddSettings(gsmPort);
                }
                DefaultSetting sendMessageTime = new DefaultSetting();
                sendMessageTime.Name = "send_message_time";
                sendMessageTime.Value = tTime.Value.TimeOfDay.Hours.ToString() + ":" + tTime.Value.TimeOfDay.Minutes.ToString();
                repo.AddSettings(sendMessageTime);

                DefaultSetting sendMessafeDaysBeforeAppointment = new DefaultSetting();
                sendMessafeDaysBeforeAppointment.Name = "days_before_send";
                sendMessafeDaysBeforeAppointment.Value = tDays.Text;
                repo.AddSettings(sendMessafeDaysBeforeAppointment);

                DefaultSetting messageText = new DefaultSetting();
                messageText.Name = "message_text";
                messageText.Value = tMessageText.Text;
                repo.AddSettings(messageText);
            }
            else
            {
                DefaultSetting gsmPort = repo.GetSetting("gsm_port");
                DefaultSetting sendMessageTime = repo.GetSetting("send_message_time");
                DefaultSetting sendMessafeDaysBeforeAppointment = repo.GetSetting("days_before_send");
                DefaultSetting messageText = repo.GetSetting("message_text");

                gsmPort.Value = comboBox1.Text;
                sendMessageTime.Value = tTime.Text;
                sendMessafeDaysBeforeAppointment.Value = tDays.Text;
                messageText.Value = tMessageText.Text;

                repo.UpdateSetting(gsmPort);
                repo.UpdateSetting(sendMessageTime);
                repo.UpdateSetting(sendMessafeDaysBeforeAppointment);
                repo.UpdateSetting(messageText);
            }
        }


    }
}
