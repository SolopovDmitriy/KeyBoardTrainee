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
using System.Windows.Threading;

namespace KeyBoardTrainee
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] _quests = {
           "Lo,rem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
           "Du,is aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur",
           "Ex,cepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum"
        };
        private int _indexQuest = -1; //идекс текущей задачи
        private int _indexCurrentLetter = 0; //индекс вводимого символа
        private string _userResult = "";
        private int _countFails = 0; //количество ошибок
        private int _countTotal = 0; //количество нажатых клавиш

        private DispatcherTimer _taskTimer; //таймер одного раунда
        private DateTime _startTime;
        private DateTime _endTime;
        private TimeSpan _elapsedTime; //_endTime - _startTime - прошедшее время

        // current speed
        private DateTime _timeOnKeyPress;
        private TimeSpan _timeBetweenKeyPress; // - прошедшее время


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _taskTimer = new DispatcherTimer();
            _taskTimer.Interval = new TimeSpan(100);
            _taskTimer.Tick += _taskTimer_Tick;
            Button_End.IsEnabled = false;
        }

        private void _taskTimer_Tick(object sender, EventArgs e)
        {
            //_endTime = DateTime.Now;
            //_elapsedTime = _endTime - _startTime;
            //double speed = 60000 * _indexCurrentLetter / _elapsedTime.TotalMilliseconds;// средняя скорость 
            double speed = 60000 / _timeBetweenKeyPress.TotalMilliseconds;//мгновенная скорость
            textBlockSpeed.Text = "Speed: " + Math.Round(speed, 2);
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            if (!_taskTimer.IsEnabled)
            {
                _startTime = DateTime.Now;
                Button_End.IsEnabled = true;
                Button_Start.IsEnabled = false;
                _taskTimer.Start();
                Random random = new Random();
                _indexQuest = random.Next(0, _quests.Length); // 0, 1, 2
                RTextBox_QuestText.Document.Blocks.Clear();
                // RTextBox_QuestText.Document.Blocks.Add(new Paragraph(new Run(_quests[_indexQuest])));
                RTextBox_QuestText.AppendText(_quests[_indexQuest]);

                RTextBox_UserText.Document.Blocks.Clear();
                _timeOnKeyPress = DateTime.Now;
                _timeBetweenKeyPress = new TimeSpan(0);
                _indexCurrentLetter = 0;
                _countFails = 0;
                textBlockFails.Text = "Fails: " + _countFails;
                textBlockSpeed.Text = "Speed: " ;
            }
        }




        private void Button_End_Click(object sender, RoutedEventArgs e)
        {
            if (_taskTimer.IsEnabled)
            {
                Button_Start.IsEnabled = true;
                Button_End.IsEnabled = false;

                _endTime = DateTime.Now;
                _elapsedTime = _endTime - _startTime;
                MessageBox.Show("elapsed time = " + _elapsedTime.TotalMilliseconds);
                _taskTimer.Stop();

            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_taskTimer.IsEnabled) return;            

            int keyKode = Convert.ToInt32(e.Key);

            string keySymbol = e.Key.ToString();

            keySymbol = keySymbol.ToLower();

            #region
            //if (Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.None)
            //{
            //    keySymbol = keySymbol.ToLower();
            //}
            //if (Keyboard.GetKeyStates(Key.LeftShift) == KeyStates.None)
            //{
            //    keySymbol = keySymbol.ToUpper();                
            //}
            //if (Keyboard.GetKeyStates(Key.RightShift) == KeyStates.Down)
            //{
            //    keySymbol = keySymbol.ToUpper();
            //}

            #endregion
                                    
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                keySymbol = keySymbol.ToUpper();
            }

            //             a                 z
            if (keyKode >= 44 && keyKode <= 69 || keyKode == 18 || keyKode == 142)
            {
                
                if (keyKode == 18)
                {
                    keySymbol = " ";                    
                }
                if (keyKode == 142)
                {
                    keySymbol = ",";
                }

                if (_quests[_indexQuest][_indexCurrentLetter].ToString().Equals(keySymbol))
                {
                    RTextBox_UserText.AppendText(keySymbol);
                    _indexCurrentLetter++;
                    _timeBetweenKeyPress = DateTime.Now - _timeOnKeyPress; // 2 sec
                    _timeOnKeyPress = DateTime.Now;
                }
                else
                {
                    _countFails++;
                    textBlockFails.Text = "Fails: " + _countFails;
                }
              

            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //double aspect = 2;
            //if (e.WidthChanged) this.Width = e.NewSize.Height * aspect;
            //else this.Height = e.NewSize.Width / aspect;
        }
    }
}
