using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using System.Windows.Shapes;

namespace AntControl
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>

    public partial class ConnectWindow : Window
    {
        SerialPort Serial;
        bool connect;
        private int numPorts;

        public string[] baud = { "4800", "9600", "19200", "38400", "57600", "115 200"};
        public int baud_num;

        public ConnectWindow(SerialPort serial)
        {
            InitializeComponent();
            Serial = serial;
            string[] ports = SerialPort.GetPortNames();
            int numBauds = 0;

            foreach (string port in ports)
            {
                COMComboBox.ItemsSource = ports;
                numPorts++;
            }

            foreach (string i in baud)
            {
                BaudComboBox.Items.Add(baud[numBauds]);
                numBauds++;
            }
        }

        
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void BaudComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            BaudComboBox.Text = baud[BaudComboBox.SelectedIndex];           // Вывод скрости порта

        }
        // Установка соединения через клик
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (connect == false)
            {
                if (!(COMComboBox.Text == null))
                {
                    Serial.PortName = COMComboBox.Text;
                    Serial.Handshake = System.IO.Ports.Handshake.None;
                    Serial.BaudRate = int.Parse(baud[BaudComboBox.SelectedIndex]);  // Установка скорости порта через преобразование типов
                    //Serial.BaudRate = 9600;
                    Serial.Parity = Parity.None;
                    Serial.DataBits = 8;
                    Serial.StopBits = StopBits.One;
                    Serial.ReadTimeout = 200;
                    Serial.Open();
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

            //mainWindow.Recieve_start(Serial);

        }
       
    }
}
