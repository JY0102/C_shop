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

namespace Chess
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button[,] btns = new Button[8, 8];

        #region 겜 시작
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }
        public void Start()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Button btn = new Button();

                    btn.Background = this.Background;
                    btn.Click += Btn_Click;

                    btns[x, y] = btn;

                    Grid.SetColumn(btn, x);
                    Grid.SetRow(btn, y);

                    chess.Children.Add(btn);

                    SetBorad(btns, x, y);
                    CreateObject(x, y);
                }
            }
            SetDanger();
            SetPieces();
        }
        private void CreateObject(int x, int y)
        {
            if (y == 7)
            {
                btns[x, y].Foreground = Brushes.Blue;
            }
            else
            {
                btns[x, y].Foreground = Brushes.Black;
            }

            if ((x == 0 || x == 7) && (y == 7 || y == 0))
            {
                btns[x, y].Content = "룩";
            }
            else if ((x == 1 || x == 6) && (y == 7 || y == 0))
            {
                btns[x, y].Content = "말";
            }
            else if ((x == 2 || x == 5) && (y == 7 || y == 0))
            {
                btns[x, y].Content = "비숍";
            }
            else if (x == 3 && (y == 7 || y == 0))
            {
                btns[x, y].Content = "퀸";
            }
            else if (x == 4 && (y == 7 || y == 0))
            {
                btns[x, y].Content = "킹";
            }
            else if (y == 1)
            {
                btns[x, y].Content = "폰";
                btns[x, y].Foreground = Brushes.Black;
            }
            else if (y == 6)
            {
                btns[x, y].Content = "폰";
                btns[x, y].Foreground = Brushes.Blue;
            }
        }
        private void SetDanger()
        {
            for (int y = 5; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Black_Danger[x, y] = true;
                }
            }

            for(int y= 0; y<3; y++)
            {
                for(int x=0; x<8; x++)
                {
                    White_Danger[x, y] = true;
                }
            }
        }
        private void SetBorad(Button[,] btns , int x, int y)
        {
            if ((x + y) % 2 == 0)
                btns[x,y].Background = Brushes.Beige;
            else
                btns[x,y].Background = Brushes.DarkSeaGreen;
        }
        private void SetPieces()
        {
            for(int y =0; y< 8; y++)
            {
                for(int x=0; x< 8; x++)
                {
                    if(btns[x,y].Content != null)
                    {
                        Enables[x, y] = true;
                    }
                }
            }
        }
        #endregion

        private bool[,] Enables = new bool[8, 8];

        // 블랙 , 화이트 왕이 가면 안되는 지역
        private bool[,] Black_Danger = new bool[8, 8];
        private bool[,] White_Danger = new bool[8, 8];

        // 왕 위치
        private int[] Black_King = new int[] { 4, 0 };
        private int[] White_King = new int[] { 4, 7 };

        private int Before_X = -1;
        private int Before_Y = -1;

        private Brush Before_Color = null;

        private List<int[]> Location = null;

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                e.Handled = true;
            }

            Button  btn = (Button)sender;


            int x = Grid.GetColumn(btn);
            int y = Grid.GetRow(btn);

            //첫 클릭
            if(Before_X == -1 )
            {
                if (btn.Content == null)
                    return;

                Before_X = x;
                Before_Y = y;
                Before_Color = btn.Foreground;

                Location = Piece.MovePiece(btn, x, y , Enables);

                foreach ( var item in Location)
                {
                    btns[item[0], item[1]].Background = Brushes.LightGreen;
                }
                btn.Background = Brushes.LightBlue;
            }
            // 나중 클릭
            else
            {
                if (Before_X == x && Before_Y == y)
                {
                    Reset(btn);
                }

                if (Location.FindIndex((temp) => temp[0] == Grid.GetColumn(btn) && temp[1] == Grid.GetRow(btn)) == -1)
                    return;


                #region 움직임 -> 이름 변경 + 이름 색 맞춤
                btn.Content = btns[Before_X, Before_Y].Content;
                btns[Before_X, Before_Y].Content = null;

                btn.Foreground = Before_Color;

                Enables[Before_X, Before_Y] = false;
                Enables[x,y] = true;
                #endregion

                #region 체크 확인
                if (Check_Checking(btn))
                    MessageBox.Show("체크");
                #endregion

                Reset(btn);
            }
        }
        private bool Check_Checking(Button btn)
        {
            var list = Piece.MovePiece(btn, Grid.GetColumn(btn), Grid.GetRow(btn) , Enables);

            foreach (var item in list)
            {
                if (btn.Foreground == Brushes.Black)
                {
                    if (White_King[0] == item[0] && White_King[1] == item[1])
                        return true;
                }
                else
                {
                    if (Black_King[0] == item[0] && Black_King[1] == item[1])
                        return true;
                }
            }

            return false;
        }
        private void Reset(Button btn)
        {
            if ((string)btn.Content == "킹")
            {
                if (btn.Foreground == Brushes.Black)
                    Black_King = new int[] { Grid.GetColumn(btn), Grid.GetRow(btn) };
                else
                    White_King = new int[] { Grid.GetColumn(btn), Grid.GetRow(btn) };
            }

            SetBorad(btns, Before_X , Before_Y);

            foreach (var item in Location)
            {
                SetBorad(btns, item[0], item[1]);
            }
            Location.Clear();

            Before_X = -1;
            Before_Y = -1;
            Before_Color = null;
        }
    }
}
