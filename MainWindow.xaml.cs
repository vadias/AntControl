using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Runtime.CompilerServices;

namespace AntControl
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
   

    public partial class mainWindow : Window
    {
        public SerialPort Serial;
        bool connect;
        public int numPorts = 0;
        /* Буффер принятых данных */
        byte[] rx_data = new byte[256];
        /* Буффер переданных данных */
        byte[] tx_data = new byte[256];

        public mainWindow()
        {
            InitializeComponent();  
            Serial = new SerialPort();
            string[] ports = SerialPort.GetPortNames();

            // new ComboBox();
            /*
             foreach (string port in ports)
             {
                 COMComboBox.ItemsSource = ports;
                 numPorts++;
             }
             */
        }

        private void RotLeft_Click(object sender, RoutedEventArgs e)
        {
            Serial.Write("L");
        }
        private void RotRight_Click(object sender, RoutedEventArgs e)
        {
            Serial.Write("R");
        }
        private void AzimutSet_Click(object sender, RoutedEventArgs e)
        {      
            int azimut = int.Parse(AzimutEnter.Text);   // Преобразование строки в число

            if (azimut < 361 && azimut > -1)
            {
                Serial.Write("" + tx_data[0]);              // Отправка числа в порт
            }
            else {

                AzimutEnter.Text = "Error";
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e) 
        {
            ConnectWindow connectWindow = new ConnectWindow(Serial = new SerialPort());
            Serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);
            // connectWindow(Serial);
            connectWindow.Show();
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /*
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (connect == false)
            {
                if (!(COMComboBox.Text == null))
                {
                    Serial.PortName = COMComboBox.Text;
                    Serial.Handshake = System.IO.Ports.Handshake.None;
                    Serial.BaudRate = 9600;
                    Serial.Parity = Parity.None;
                    Serial.DataBits = 8;
                    Serial.StopBits = StopBits.One;
                    Serial.ReadTimeout = 200;
                    Serial.Open();
                    Serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);
                    connect = true;
                    ButConnect.Content = "Разсоед";
                }
            }
            else
            {
                Serial.Close();
                connect = false;
                ButConnect.Content = "Соединен";
            }

        }    
    */

    /*
        public static void Recieve_start(SerialPort Serial)
        {
            Serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);
        }
        */
       
        private void Recieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            rx_data[0] = (byte)Serial.ReadByte();
            rx_data[1] = (byte)Serial.ReadByte();
           // rx_data[2] = (byte)Serial.ReadByte();
           // rx_data[3] = (byte)Serial.ReadByte();
          //  rx_data[4] = (byte)Serial.ReadByte();
           // rx_data[5] = (byte)Serial.ReadByte();
            Serial.DiscardInBuffer();
            //Dispatcher.Invoke(DispatcherPriority.Background, new Delegate(WriteData), recieved_data);
            /*  Azimut.Dispatcher.Invoke(() =>
              {
                  WriteDataAsync(rx_data);
              });
            */
            Dispatcher.Invoke(() =>
            {
                WriteDataAsync(rx_data);
            });
               
        }

        /*Обработка принятого пакета */
        private async Task WriteDataAsync(byte[] rx)
        {
            int deg = ((rx[0] << 8) + rx[1] >> 4);
            Azimut.Text = "" + (deg / (4095 / 360));
            double angel = Math.PI * (deg / (4095/360));
            LineAzimut.X1 = 200+ 150*Math.Cos(angel/180);
            LineAzimut.Y1 = 330+ 150*Math.Sin(angel/180);
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            Serial.Write("E");
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Serial.Write("R");
        }
    }


}


