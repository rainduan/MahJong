
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace majiang
{
    
    public partial class majiang : Form
    {
        class AutoSizeFormClass
        {
            //(1).声明结构,只记录窗体和其控件的初始位置和大小。
            public struct controlRect
            {
                public int Left;
                public int Top;
                public int Width;
                public int Height;
            }
            //(2).声明 1个对象
            //注意这里不能使用控件列表记录 List nCtrl;，因为控件的关联性，记录的始终是当前的大小。
            public List<controlRect> oldCtrl;
            //int ctrl_first = 0;
            //(3). 创建两个函数
            //(3.1)记录窗体和其控件的初始位置和大小,
            public void controllInitializeSize(Form mForm)
            {
                // if (ctrl_first == 0)
                {
                    //  ctrl_first = 1;
                    oldCtrl = new List<controlRect>();
                    controlRect cR;
                    cR.Left = mForm.Left; cR.Top = mForm.Top; cR.Width = mForm.Width; cR.Height = mForm.Height;
                    oldCtrl.Add(cR);
                    foreach (Control c in mForm.Controls)
                    {
                        controlRect objCtrl;
                        objCtrl.Left = c.Left; objCtrl.Top = c.Top; objCtrl.Width = c.Width; objCtrl.Height = c.Height;
                        oldCtrl.Add(objCtrl);
                    }
                }
                // this.WindowState = (System.Windows.Forms.FormWindowState)(2);//记录完控件的初始位置和大小后，再最大化
                //0 - Normalize , 1 - Minimize,2- Maximize
            }
            //(3.2)控件自适应大小,
            public void controlAutoSize(Form mForm)
            {
                //int wLeft0 = oldCtrl[0].Left; ;//窗体最初的位置
                //int wTop0 = oldCtrl[0].Top;
                ////int wLeft1 = this.Left;//窗体当前的位置
                //int wTop1 = this.Top;
                float wScale = (float)mForm.Width / (float)oldCtrl[0].Width;//新旧窗体之间的比例，与最早的旧窗体
                float hScale = (float)mForm.Height / (float)oldCtrl[0].Height;//.Height;
                int ctrLeft0, ctrTop0, ctrWidth0, ctrHeight0;
                int ctrlNo = 1;//第1个是窗体自身的 Left,Top,Width,Height，所以窗体控件从ctrlNo=1开始
                foreach (Control c in mForm.Controls)
                {
                    ctrLeft0 = oldCtrl[ctrlNo].Left;
                    ctrTop0 = oldCtrl[ctrlNo].Top;
                    ctrWidth0 = oldCtrl[ctrlNo].Width;
                    ctrHeight0 = oldCtrl[ctrlNo].Height;
                    //c.Left = (int)((ctrLeft0 - wLeft0) * wScale) + wLeft1;//新旧控件之间的线性比例
                    //c.Top = (int)((ctrTop0 - wTop0) * h) + wTop1;
                    c.Left = (int)((ctrLeft0) * wScale);//新旧控件之间的线性比例。控件位置只相对于窗体，所以不能加 + wLeft1
                    c.Top = (int)((ctrTop0) * hScale);//
                    c.Width = (int)(ctrWidth0 * wScale);//只与最初的大小相关，所以不能与现在的宽度相乘 (int)(c.Width * w);
                    c.Height = (int)(ctrHeight0 * hScale);//
                    ctrlNo += 1;
                }
            }

        }
        public struct Acart { public string type; public int num; public string name; public int id; public Image picture;}
        int int_whos_turn;
        Acart last_cart;
        List<Acart> list_all = new List<Acart>(),list_com1=new List<Acart>(), list_com2=new List<Acart>(), list_com3=new List<Acart>(), list_player=new List<Acart>();
        List<Acart> list_player_peng = new List<Acart>(), list_player_gang = new List<Acart>();
        List<Acart> list_com1_peng = new List<Acart>(), list_com1_gang = new List<Acart>();
        List<Acart> list_com2_peng = new List<Acart>(), list_com2_gang = new List<Acart>();
        List<Acart> list_com3_peng = new List<Acart>(), list_com3_gang = new List<Acart>();
        Button[] btnArray = new Button[26];
        string que_player, que_com1,que_com2,que_com3;
        
        /*
        List<string> last_cart = new List<string>() { "", "" };
        List<int> list_wan = new List<int>(), list_tong = new List<int>(), list_tiao = new List<int>();
        List<int> list_player_wan = new List<int>(), list_player_tong = new List<int>(), list_player_tiao = new List<int>();
        List<int> list_com1_wan = new List<int>(), list_com1_tong = new List<int>(), list_com1_tiao = new List<int>();
        List<int> list_com2_wan = new List<int>(), list_com2_tong = new List<int>(), list_com2_tiao = new List<int>();
        List<int> list_com3_wan = new List<int>(), list_com3_tong = new List<int>(), list_com3_tiao = new List<int>();
        List<string> list_com1 = new List<string>(), list_com2 = new List<string>(), list_com3 = new List<string>(), list_all = new List<string>(), list_player = new List<string>();
        List<string> list_peng_com1 = new List<string>(), list_gang_com1 = new List<string>(), list_set_com1 = new List<string>();
        List<string> list_peng_com2 = new List<string>(), list_gang_com2 = new List<string>(), list_set_com2 = new List<string>();
        List<string> list_peng_com3 = new List<string>(), list_gang_com3 = new List<string>(), list_set_com3 = new List<string>();
        List<string> list_peng_player = new List<string>(), list_gang_player = new List<string>(), list_set_player = new List<string>(), list_doble_gang_player = new List<string>();
        */
        Random rand_num = new Random();
        //int int_test=0;
        AutoSizeFormClass asc = new AutoSizeFormClass();
        public majiang()
        {

            InitializeComponent();

        }
        //2. 为窗体添加Load事件，并在其方法Form1_Load中，调用类的初始化方法，记录窗体和其控件的初始位置和大小
        private void Form1_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
        }
        //3.为窗体添加SizeChanged事件，并在其方法Form1_SizeChanged中，调用类的自适应方法，完成自适应
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }
        private void btn_start_Click(object sender, EventArgs e)
        {
            lab_win.Text="";
            btnhu.Enabled = true;
            btnpass.Enabled = true;
            btnpeng.Enabled = true;
            btngang.Enabled = true;
            btn1.Enabled = true;
            btn2.Enabled = true;
            btn3.Enabled = true;
            btn4.Enabled = true;
            btn5.Enabled = true;
            btn6.Enabled = true;
            btn7.Enabled = true;
            btn8.Enabled = true;
            btn9.Enabled = true;
            btn10.Enabled = true;
            btn11.Enabled = true;
            btn12.Enabled = true;
            btn13.Enabled = true;
            btnnew.Enabled = true;
            btn_com1.Enabled = true;
            btn_com2.Enabled = true;
            btn_com3.Enabled = true;

            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;
            btn5.Enabled = false;
            btn6.Enabled = false;
            btn7.Enabled = false;
            btn8.Enabled = false;
            btn9.Enabled = false;
            btn10.Enabled = false;
            btn11.Enabled = false;
            btn12.Enabled = false;
            btn13.Enabled = false;
            btnnew.Enabled = false;

            que_player = "";
            que_com1 = "";
            que_com2 = "";
            que_com3 = "";
            btnArray[0] = btn1;
            btnArray[1] = btn2;
            btnArray[2] = btn3;
            btnArray[3] = btn4;
            btnArray[4] = btn5;
            btnArray[5] = btn6;
            btnArray[6] = btn7;
            btnArray[7] = btn8;
            btnArray[8] = btn9;
            btnArray[9] = btn10;
            btnArray[10] = btn11;
            btnArray[11] = btn12;
            btnArray[12] = btn13;
            btnArray[13] = btnnew;
            
            lab_playercart.Text = "player:";
            lab_com1cart.Text = "com1:";
            lab_com2cart.Text = "com2:";
            lab_com3cart.Text = "com3:";
            lab_com1cartdisplay.Text = "";
            lab_com2cartdisplay.Text = "";
            lab_com3cartdisplay.Text = "";
            labpeng_gangcom1.Text = "";
            labpeng_gangcom3.Text = "";
            labpeng_gangcom2.Text = "";
            labpeng_gang.Text = "player:";
            int_whos_turn = 0;
            list_com1_gang.Clear();
            list_com1_peng.Clear();
            list_com2_gang.Clear();
            list_com2_peng.Clear();
            list_com3_gang.Clear();
            list_com3_peng.Clear();
            list_player_gang.Clear();
            list_player_peng.Clear();
            list_player.Clear();
            list_com1.Clear();
            list_com2.Clear();
            list_com3.Clear();
            list_all.Clear();
            btn_com1.Text = "";
            btn_com2.Text = "";
            btn_com3.Text = "";
            
            /*
            list_wan.Clear();
            list_tong.Clear();
            list_tiao.Clear();
            list_player.Clear();
            list_com1.Clear();
            list_com2.Clear();
            list_com3.Clear();
            list_com1_wan.Clear();
            list_com2_wan.Clear();
            list_com3_wan.Clear();
            list_player_wan.Clear();
            list_com1_tong.Clear();
            list_com2_tong.Clear();
            list_com3_tong.Clear();
            list_player_tong.Clear();
            list_com1_tiao.Clear();
            list_com2_tiao.Clear();
            list_com3_tiao.Clear();
            list_player_tiao.Clear();
            */ 
            btnpeng.Visible = false;
            btnpass.Visible = false;
            btngang.Visible = false;
            createcart();
            /*list_all.RemoveAt(0);
            int_test += 1;
            lab_test.Text = Convert.ToString(int_test);*/
            give_cart(list_player);
            give_cart(list_com1);
            give_cart(list_com2);
            give_cart(list_com3);
            display_com(lab_com1cartdisplay, list_com1);
            display_com(lab_com2cartdisplay, list_com2);
            display_com(lab_com3cartdisplay, list_com3);
            playerdisplay();
            playernewcart(list_player);
            picplayer1.BackgroundImage = list_all[1].picture;
            picplayer2.BackgroundImage = list_all[1].picture;
            picplayer3.BackgroundImage = list_all[1].picture;
            /*    
            check_start(list_player);
            check_start(list_com1);
            check_start(list_com2);
            check_start(list_com3);
            int_temp = rand_num.Next(0, list_all.Count);
            list_player.Add(list_all.ElementAt(int_temp));
            list_all.RemoveAt(int_temp);
            btnnew.Text = list_player.Last();
            
            //lab_test.Text=Convert.ToString(list_all.Count());
            //lab_test.Text += list_all.ElementAt(1);
            foreach (string element in list_all)
            {
                lab_test.Text += element;
            }*/
        }
        /*void createcart(string ele)
        {
            for(int j=1;j<=9;++j)
            {
                list_all.Add(ele+j);
                list_all.Add(ele + j);
                list_all.Add(ele+j);
                list_all.Add(ele + j);
            }
        }*/
        void createcart()
        {
            int int_temp_id=0;
            for (int j = 1; j <= 9; ++j)
            {
                for (int i = 0; i < 4; i++)
                {
                    list_all.Add(new Acart() { type = "万", num = j, name = j + "万" ,id=int_temp_id, picture=Image.FromFile(j+"万.png")});
                    int_temp_id++;
                    
                }
            }
            for (int j = 1; j <= 9; ++j)
            {
                for (int i = 0; i < 4; i++)
                {
                    list_all.Add(new Acart() { type = "筒", num = j, name = j + "筒", id = int_temp_id,picture = Image.FromFile(j + "筒.png") });
                    int_temp_id++;
                }
            }
            for (int j = 1; j <= 9; ++j)
            {
                for (int i = 0; i < 4; i++)
                {
                    list_all.Add(new Acart() { type = "条", num = j, name = j + "条" ,id=int_temp_id,picture = Image.FromFile(j + "条.png")});
                    int_temp_id++;
                }
            }
			for (int i = 0; i < list_all.Count; i++)
			{
				if (i % 12 == 0)
				{
					restcart.Text += "\n";
				}
				restcart.Text += list_all[i].name + " ";
			}
			
		}
        void give_cart(List<Acart>temp_Cart)
        {
            int int_temp;
            for (int i = 0; i < 13; i++)
            {
                int_temp = rand_num.Next(0, list_all.Count());
                temp_Cart.Add(list_all[int_temp]);
                list_all.RemoveAt(int_temp);
            }
			restcart.Text = "剩下的牌：";
			for (int i = 0; i < list_all.Count; i++)
			{
				if (i % 12 == 0)
				{
					restcart.Text += "\n";
				}
				restcart.Text += list_all[i].name + " ";
			}
            restcart.Text += "\ncom1的牌：";
            for (int i = 0; i < list_com1.Count; i++)
            {
                if (i % 12 == 0)
                {
                    restcart.Text += "\n";
                }
                restcart.Text += list_com1[i].name + " ";
            }
            restcart.Text += "\ncom2的牌：";
            for (int i = 0; i < list_com2.Count; i++)
            {
                if (i % 12 == 0)
                {
                    restcart.Text += "\n";
                }
                restcart.Text += list_com2[i].name + " ";
            }
            restcart.Text += "\ncom3的牌：";
            for (int i = 0; i < list_com3.Count; i++)
            {
                if (i % 12 == 0)
                {
                    restcart.Text += "\n";
                }
                restcart.Text += list_com3[i].name + " ";
            }
        }

        

        void playerdisplay()
        {
            
            list_player.Sort((left, right) =>
            {
                if (left.id > right.id)
                    return 1;
                else if (left.id == right.id)
                    return 0;
                else
                    return -1;
            });
            //list_player.Sort(q => q.id);
            for (int i = 0; i < 13; i++)
            {
                btnArray[i].Text = "";
                btnArray[i].Image = Image.FromFile("0.png");
            } 
            
                for (int i = 0; i < 14; i++)
                {
                    if (i < list_player.Count)
                    {
                        btnArray[i].Image = list_player.ElementAt(i).picture;
                        btnArray[i].Text = list_player.ElementAt(i).name[0] + "\n" + list_player.ElementAt(i).name[1];
                        btnArray[i].Enabled = true;
                    }
                    else
                        btnArray[i].Enabled = false;
                }

                
            
           /* btn1.Text = list_player.ElementAt(0).name;
            btn2.Text = list_player.ElementAt(1).name;
            btn3.Text = list_player.ElementAt(2).name;
            btn4.Text = list_player.ElementAt(3).name;
            btn5.Text = list_player.ElementAt(4).name;
            btn6.Text = list_player.ElementAt(5).name;
            btn7.Text = list_player.ElementAt(6).name;
            btn8.Text = list_player.ElementAt(7).name;
            btn9.Text = list_player.ElementAt(8).name;
            btn10.Text = list_player.ElementAt(9).name;
            btn11.Text = list_player.ElementAt(10).name;
            btn12.Text = list_player.ElementAt(11).name;
            btn13.Text = list_player.ElementAt(12).name;
            */
        }

        void playernewcart(List<Acart> player)
        {
            if (list_all.Count() != 0)
            {
                lab_cartleft.Text = Convert.ToString(list_all.Count()-1);
                int int_temp;
                btn_com1.Visible = false;
                btn_com2.Visible = false;
                btn_com3.Visible = false;
                int_temp = rand_num.Next(0, list_all.Count - 1);
                player.Add(list_all.ElementAt(int_temp));
                list_all.RemoveAt(int_temp);
                btnnew.Image = player.Last().picture;
                btnnew.Text = player.Last().name[0] + "\n" + player.Last().name[1];
                btnnew.Enabled = true;
                foreach (var p in list_player_peng)
                {
                    if (player.Last().name == p.name)
                    {
                        btnpass.Visible = true;
                        btngang.Visible = true;
                    }
                }
                for (int i = 0; i < player.Count(); i++)
                {
                    if (player.FindAll(r => r.name.Equals(player[i].name)).Count() == 4)
                    {
                        btngang.Enabled = true;
                        btngang.Visible = true;
                        btnpass.Visible = true;
                        btnpass.Enabled = true;
                    }
                }
            }
            else
                lab_cartleft.Text = "没牌了";
			restcart.Text = "剩下的牌：";
			for (int i = 0; i < list_all.Count; i++)
			{
				if (i % 12 == 0)
				{
					restcart.Text += "\n";
				}
				restcart.Text += list_all[i].name + " ";
			}
            restcart.Text += "\ncom1的牌：";
            for (int i = 0; i < list_com1.Count; i++)
            {
                if (i % 12 == 0)
                {
                    restcart.Text += "\n";
                }
                restcart.Text += list_com1[i].name + " ";
            }
            restcart.Text += "\ncom2的牌：";
            for (int i = 0; i < list_com2.Count; i++)
            {
                if (i % 12 == 0)
                {
                    restcart.Text += "\n";
                }
                restcart.Text += list_com2[i].name + " ";
            }
            restcart.Text += "\ncom3的牌：";
            for (int i = 0; i < list_com3.Count; i++)
            {
                if (i % 12 == 0)
                {
                    restcart.Text += "\n";
                }
                restcart.Text += list_com3[i].name + " ";
            }
        }
            
           
            
        
        void comnewcart(Button btn_temp, List<Acart> com,Label lab_temp,Label lab,Label peng_gang,List<Acart>list_peng,List<Acart>list_gang)
        {
            int int_temp;
            btnpass.Visible = true;
            //playerdisplay();
            if (list_all.Count != 0)
            {
                lab_cartleft.Text = Convert.ToString(list_all.Count()-1);
                int_temp = rand_num.Next(0, list_all.Count - 1);
                com.Add(list_all.ElementAt(int_temp));
                list_all.Remove(list_all.ElementAt(int_temp));
                int n = 1;
                com.Sort((left, right) =>
                {
                    if (left.id > right.id)
                        return 1;
                    else if (left.id == right.id)
                        return 0;
                    else
                        return -1;
                });
                restcart.Text = "剩下的牌：";
                for (int i = 0; i < list_all.Count; i++)
                {
                    if (i % 12 == 0)
                    {
                        restcart.Text += "\n";
                    }
                    restcart.Text += list_all[i].name + " ";
                }
                restcart.Text += "\ncom1的牌：";
                for (int i = 0; i < list_com1.Count; i++)
                {
                    if (i % 12 == 0)
                    {
                        restcart.Text += "\n";
                    }
                    restcart.Text += list_com1[i].name + " ";
                }
                restcart.Text += "\ncom2的牌：";
                for (int i = 0; i < list_com2.Count; i++)
                {
                    if (i % 12 == 0)
                    {
                        restcart.Text += "\n";
                    }
                    restcart.Text += list_com2[i].name + " ";
                }
                restcart.Text += "\ncom3的牌：";
                for (int i = 0; i < list_com3.Count; i++)
                {
                    if (i % 12 == 0)
                    {
                        restcart.Text += "\n";
                    }
                    restcart.Text += list_com3[i].name + " ";
                }
                if (int_whos_turn == 1)
                {
                    check_com_hu(com, int_whos_turn, lab, list_gang, list_peng, que_com1);
                    com_cart(com, btn_com1, lab_temp, lab, peng_gang, list_peng, list_gang);
                }
                else if (int_whos_turn == 2)
                {
                    check_com_hu(com, int_whos_turn, lab, list_gang, list_peng, que_com2);
                    com_cart(com, btn_com2, lab_temp, lab, peng_gang, list_peng, list_gang);
                }
                else if (int_whos_turn == 3)
                {
                    check_com_hu(com, int_whos_turn, lab, list_gang, list_peng, que_com3);
                    com_cart(com, btn_com3, lab_temp, lab, peng_gang, list_peng, list_gang);
                }
                

                if (int_whos_turn == 1)
                {
                    check_com_hu(list_com2, 2, lab_com2cartdisplay, list_com2_gang, list_com2_peng, que_com2);
                    check_com_hu(list_com3, 3, lab_com3cartdisplay, list_com3_gang, list_com3_peng, que_com3);
                }
                else if (int_whos_turn == 2)
                {
                    check_com_hu(list_com1, 1, lab_com1cartdisplay, list_com1_gang, list_com1_peng, que_com1);
                    check_com_hu(list_com3, 3, lab_com3cartdisplay, list_com3_gang, list_com3_peng, que_com3);
                }
                else if (int_whos_turn == 3)
                {
                    check_com_hu(list_com2, 2, lab_com2cartdisplay, list_com2_gang, list_com2_peng, que_com1);
                    check_com_hu(list_com3, 3, lab_com3cartdisplay, list_com3_gang, list_com3_peng, que_com2);
                }
                
            }
            else
                lab_cartleft.Text = "没牌了";
        }

        void com_cart(List<Acart> com, Button btn_temp, Label lab_temp, Label lab, Label peng_gang, List<Acart> list_peng, List<Acart> list_gang)
        {
            bool ok = false;
            List<Acart> temp_list = new List<Acart>();
            Acart new_cart;
            int w, to, ti, x;
            string que = "";
            if (int_whos_turn == 1)
            {
                que = que_com1;
            }
            else if (int_whos_turn == 2)
            {
                que = que_com2;
            }
            else if (int_whos_turn == 3)
            {
                que = que_com3;
            }
            //display_com(lab, com);
            for (int i = 0; i < com.Count(); i++)
            {
                if (com.FindAll(r => r.type == com.ElementAt(i).type && r.name == com.ElementAt(i).name).Count == 4)
                {
                    list_gang.Add(com.ElementAt(i));
                    for (int j = 0; j < 4; j++)
                    {
                        peng_gang.Text += com.ElementAt(i).name;

                    }
                    com.RemoveAll(r => r.name == com.ElementAt(i).name);
                    peng_gang.Text += "   ";
                    comnewcart(btn_temp, com, lab_temp, lab, peng_gang, list_peng, list_gang);
                    ok = true;
                }
                else if (list_peng.Any(r => r.name == com.ElementAt(i).name))
                {
                    list_peng.Remove(com.ElementAt(i));
                    list_gang.Add(com.ElementAt(i));
                    peng_gang.Text += com.ElementAt(i).name;
                    com.Remove(com.Find(r => r.name == com.ElementAt(i).name));
                    peng_gang.Text += "   ";
                    comnewcart(btn_temp, com, lab_temp, lab, peng_gang, list_peng, list_gang);
                    ok = true;
                }
            }
            if (ok==false)
            {
                if (que == "")
                {
                    w = com.FindAll(r => r.type == "万").Count();
                    to = com.FindAll(r => r.type == "筒").Count();
                    ti = com.FindAll(r => r.type == "条").Count();
                    if (w <= to && w <= ti)
                        que = "万";
                    else if (to <= w && to <= ti)
                        que = "筒";
                    else if (ti <= w && ti <= to)
                        que = "条";
                    if (int_whos_turn == 1)
                    {
                        que_com1 = que;
                    }
                    else if (int_whos_turn == 2)
                    {
                        que_com2 = que;
                    }
                    else if (int_whos_turn == 3)
                    {
                        que_com3 = que;
                    }
                }

                if (com.Any(r => r.type == que))
                {
                    last_cart = com.Find(r => r.type == que);
                }
                else
                {
                    for (int i = 0; i < com.Count(); i++)
                    {
                        if (com.FindAll(r => r.type == com.ElementAt(i).type && r.name == com.ElementAt(i).name).Count < 2)
                        {

                        }
                        else
                        {
                            temp_list.AddRange(com.FindAll(r => r.type == com.ElementAt(i).type && r.num == com.ElementAt(i).num));
                            x = com.FindAll(r => r.type == com.ElementAt(i).type && r.num == com.ElementAt(i).num).Count;
                            for (int j = 0; j < x; j++)
                                com.Remove(com.Find(r => r.type == com.ElementAt(i).type && r.num == com.ElementAt(i).num));
                            i--;
                        }
                    }
                    for (int i = 0; i < com.Count(); i++)
                    {
                        if (com.Any(r => r.type == com.ElementAt(i).type && r.num == com.ElementAt(i).num + 1))
                        {
                            if (com.Any(r => r.type == com.ElementAt(i).type && r.num == com.ElementAt(i).num + 2))
                            {
                                new_cart = com.ElementAt(i);
                                temp_list.Add(new_cart);
                                temp_list.Add(com.Find(r => r.type == new_cart.type && r.num == new_cart.num + 1));
                                temp_list.Add(com.Find(r => r.type == new_cart.type && r.num == new_cart.num + 2));
                                com.Remove(com.Find(r => r.type == new_cart.type && r.num == new_cart.num + 2));
                                com.Remove(com.Find(r => r.type == new_cart.type && r.num == new_cart.num + 1));

                                com.Remove(new_cart);
                                i--;
                            }

                        }

                    }
                    if (com.Any())
                    {
                        last_cart = com.First();
                        com.Remove(com.First());
                        com.AddRange(temp_list);
                    }
                    else
                        com.AddRange(temp_list);
                }



                btn_temp.Image = last_cart.picture;
                btn_temp.Text = last_cart.name[0] + "\n" + last_cart.name[1];
                com.Remove(last_cart);
                display_com(lab, com);
                lab_temp.Text += last_cart.name + "   ";
                btn_com1.Visible = false;
                btn_com2.Visible = false;
                btn_com3.Visible = false;
                btn_temp.Visible = true;
                /*
                check_com_hu(list_com1, 1, lab_com1cartdisplay, list_com1_gang, list_com1_peng, que_com1);
                check_com_hu(list_com2, 2, lab_com2cartdisplay, list_com2_gang, list_com2_peng, que_com2);
                check_com_hu(list_com3, 3, lab_com3cartdisplay, list_com3_gang, list_com3_peng, que_com3);
                 */
                check_cart(list_player, last_cart);
                com_check_cart();
            }
        }

        void com_check_cart()
        {
            if (int_whos_turn != 1)
            {
                if (list_com1.FindAll(r => r.name.Equals(last_cart.name)).Count() == 3)
                {
                    list_com1_gang.Add(last_cart);
                    labpeng_gangcom1.Text += last_cart.name;
                    for (int i = 0; i < 3; i++)
                    {
                        labpeng_gangcom1.Text += last_cart.name;
                        list_com1.Remove(list_com1.Find(r => r.name == last_cart.name));
                    }
                    labpeng_gangcom1.Text += "   ";
                    int_whos_turn = 1;
                    comnewcart(btn_com1, list_com1, lab_com1cart, lab_com1cartdisplay, labpeng_gangcom1, list_com1_peng, list_com1_gang);
                }
                else if (list_com1.FindAll(r => r.name.Equals(last_cart.name)).Count() == 2)
                {
                    list_com1_peng.Add(last_cart);
                    labpeng_gangcom1.Text += last_cart.name;
                    for (int i = 0; i < 2; i++)
                    {
                        labpeng_gangcom1.Text += last_cart.name;
                        list_com1.Remove(list_com1.Find(r => r.name == last_cart.name));
                    }
                    labpeng_gangcom1.Text += "   ";
                    int_whos_turn = 1;
                    com_cart(list_com1, btn_com1, lab_com1cart, lab_com1cartdisplay, labpeng_gangcom1, list_com1_peng, list_com1_gang);
                }
            }
            if (int_whos_turn != 2)
            {
                if (list_com2.FindAll(r => r.name.Equals(last_cart.name)).Count() == 3)
                {
                    list_com2_gang.Add(last_cart);
                    labpeng_gangcom2.Text += last_cart.name;
                    for (int i = 0; i < 3; i++)
                    {
                        labpeng_gangcom2.Text += last_cart.name;
                        list_com2.Remove(list_com2.Find(r => r.name == last_cart.name));
                    }
                    labpeng_gangcom2.Text += "   ";
                    int_whos_turn = 2;
                    comnewcart(btn_com2, list_com2, lab_com2cart, lab_com2cartdisplay, labpeng_gangcom2, list_com2_peng, list_com2_gang);
                }
                else if (list_com2.FindAll(r => r.name.Equals(last_cart.name)).Count() == 2)
                {
                    list_com2_peng.Add(last_cart);
                    labpeng_gangcom2.Text += last_cart.name;
                    for (int i = 0; i < 2; i++)
                    {
                        labpeng_gangcom2.Text += last_cart.name;
                        list_com2.Remove(list_com2.Find(r => r.name == last_cart.name));
                    }
                    labpeng_gangcom2.Text += "   ";
                    int_whos_turn = 2;
                    com_cart(list_com2, btn_com2, lab_com2cart, lab_com2cartdisplay, labpeng_gangcom2, list_com2_peng, list_com2_gang);
                }
            }
            if (int_whos_turn != 3)
            {
                if (list_com3.FindAll(r => r.name.Equals(last_cart.name)).Count() == 3)
                {
                    list_com3_gang.Add(last_cart);
                    labpeng_gangcom3.Text += last_cart.name;
                    for (int i = 0; i < 3; i++)
                    {
                        labpeng_gangcom3.Text += last_cart.name;
                        list_com3.Remove(list_com3.Find(r => r.name == last_cart.name));
                    }
                    labpeng_gangcom3.Text += "   ";
                    int_whos_turn = 3;
                    comnewcart(btn_com3, list_com3, lab_com3cart, lab_com3cartdisplay, labpeng_gangcom3, list_com3_peng, list_com3_gang);
                }
                else if (list_com3.FindAll(r => r.name.Equals(last_cart.name)).Count() == 2)
                {
                    list_com3_peng.Add(last_cart);
                    labpeng_gangcom3.Text += last_cart.name;
                    for (int i = 0; i < 2; i++)
                    {
                        labpeng_gangcom3.Text += last_cart.name;
                        list_com3.Remove(list_com3.Find(r => r.name == last_cart.name));
                    }
                    labpeng_gangcom3.Text += "   ";
                    int_whos_turn = 3;
                    com_cart(list_com3, btn_com3, lab_com3cart, lab_com3cartdisplay, labpeng_gangcom3, list_com3_peng, list_com3_gang);
                }
               
            }
        }
        void check_cart(List<Acart> list_temp, Acart temp_cart)
        {

            if (list_temp.FindAll(r => r.name.Equals(temp_cart.name)).Count() == 3)
            {
                btngang.Enabled = true;
                btngang.Visible = true;
                btnpass.Visible = true;
                btnpass.Enabled = true;
                btnpeng.Visible = true;
            }
            if (list_temp.FindAll(r => r.name.Equals(temp_cart.name)).Count() == 2)
            {
                btnpass.Visible = true;
                btnpeng.Visible = true;
            }
        }
        
        void display_com(Label lab, List<Acart>com)//测试改动
        {
            int n=1;
            com.Sort((left, right) =>
            {
                if (left.id > right.id)
                    return 1;
                else if (left.id == right.id)
                    return 0;
                else
                    return -1;
            });
            //list_player.Sort(q => q.id);
            
            lab.Text = "";
            /*
            foreach (Acart q in com)
            {
                lab.Text += /*n + ". " +* q.name; //+"\n" + "\n";
                n++;
            }
        */
            lab.Text += "\n";
             
        }
        void cartbtn_click(Button btn_temp)
        {
            btnpeng.Visible = false;
            btngang.Visible = false;

            lab_playercart.Text += btn_temp.Text.Substring(0, 1) + btn_temp.Text.Substring(2, 1) + " ";
            last_cart = list_player.Find(delegate(Acart p) { return p.name == btn_temp.Text.Substring(0, 1) + btn_temp.Text.Substring(2, 1); });
            if (que_player == "")
                que_player = last_cart.type;
            list_player.Remove(last_cart);
            btnnew.Text = "";
            btnnew.Image = Image.FromFile("0.png");
            playerdisplay();
            btnpass.Visible = true;
            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;
            btn5.Enabled = false;
            btn6.Enabled = false;
            btn7.Enabled = false;
            btn8.Enabled = false;
            btn9.Enabled = false;
            btn10.Enabled = false;
            btn11.Enabled = false;
            btn12.Enabled = false;
            btn13.Enabled = false;
            btnnew.Enabled = false;
            check_com_hu(list_com1, 1, lab_com1cartdisplay, list_com1_gang, list_com1_peng, que_com1);
            check_com_hu(list_com2, 2, lab_com2cartdisplay, list_com2_gang, list_com2_peng, que_com2);
            check_com_hu(list_com3, 3, lab_com3cartdisplay, list_com3_gang, list_com3_peng, que_com3);
            com_check_cart();
            if (int_whos_turn == 0)
            {
                
                if (lab_win.Text == "")
                {
                    int_whos_turn += 1;
                    comnewcart(btn_com1, list_com1, lab_com1cart, lab_com1cartdisplay, labpeng_gangcom1, list_com1_peng, list_com1_gang);
                }
            }
        }
        private void btnnew_Click(object sender, EventArgs e)
        {
            btnpeng.Visible = false;
            btngang.Visible = false;

            lab_playercart.Text += btnnew.Text.Substring(0, 1) + btnnew.Text.Substring(2, 1) + " ";
            if (que_player == "")
                que_player = list_player.Last().type;
            last_cart = list_player.Last();
            list_player.Remove(list_player.Last());
            btnnew.Image = Image.FromFile("0.png");
            btnnew.Text = "";
            playerdisplay();
            btnpass.Visible = true;
            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;
            btn5.Enabled = false;
            btn6.Enabled = false;
            btn7.Enabled = false;
            btn8.Enabled = false;
            btn9.Enabled = false;
            btn10.Enabled = false;
            btn11.Enabled = false;
            btn12.Enabled = false;
            btn13.Enabled = false;
            btnnew.Enabled = false;
            check_com_hu(list_com1, 1, lab_com1cartdisplay, list_com1_gang, list_com1_peng,que_com1);
            check_com_hu(list_com2, 2, lab_com2cartdisplay, list_com2_gang, list_com2_peng,que_com2);
            check_com_hu(list_com3, 3, lab_com3cartdisplay, list_com3_gang, list_com3_peng,que_com3);
            com_check_cart();
            if (int_whos_turn == 0)
            {
                int_whos_turn += 1;
                comnewcart(btn_com1, list_com1, lab_com1cart, lab_com1cartdisplay, labpeng_gangcom1, list_com1_peng, list_com1_gang);
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            
            cartbtn_click(btn1);
            
        }

        private void btn2_Click(object sender, EventArgs e)
        {

            cartbtn_click(btn2);
        }

        private void btn3_Click(object sender, EventArgs e)
        {

            cartbtn_click(btn3);

        }

        private void btn4_Click(object sender, EventArgs e)
        {

            cartbtn_click(btn4);
        }

        private void btn5_Click(object sender, EventArgs e)
        {

            cartbtn_click(btn5);
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            cartbtn_click(btn6);
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            cartbtn_click(btn7);
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            cartbtn_click(btn8);
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            cartbtn_click(btn9);
        }

        private void btn10_Click(object sender, EventArgs e)
        {
            cartbtn_click(btn10);
        }

        private void btn11_Click(object sender, EventArgs e)
        {
            cartbtn_click(btn11);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btn12_Click(object sender, EventArgs e)
        {
            cartbtn_click(btn12);
        }

        private void btn13_Click(object sender, EventArgs e)
        {
            cartbtn_click(btn13);
        }

        private void btnpeng_Click(object sender, EventArgs e)
        {
            list_player_peng.Add(last_cart);
            list_player.RemoveAll(r => r.name.Equals(last_cart.name));
            int_whos_turn=0;
            for (int i = 0; i < 3;i++ )
                labpeng_gang.Text += last_cart.name+" ";
            btngang.Visible = false;
            btnpeng.Visible = false;
            btnpass.Visible = false;
            playerdisplay();
            
        }

        private void btngang_Click(object sender, EventArgs e)
        {
            if (int_whos_turn != 0)
            {
                list_player.RemoveAll(r => r.name == last_cart.name);
                list_player_gang.Add(last_cart);
                for (int j = 0; j < 4; j++)
                    labpeng_gang.Text +=last_cart.name + " ";
                
            }
            else if (list_player.FindAll(r=>r.name==list_player.Last().name).Count() == 4)
            {
                list_player_gang.Add(list_player.Last());
                list_player.RemoveAll(r => r.name.Equals(btnnew.Text.Substring(0, 1) + btnnew.Text.Substring(2, 1)));
                for (int j = 0; j < 4; j++)
                    labpeng_gang.Text += list_player.Last().name + " ";
            }
            else if(list_player_peng.FindAll(r=>r.name==list_player.Last().name).Count()>0)
                
            {
                list_player_peng.RemoveAll(r => r.name.Equals(btnnew.Text.Substring(0, 1) + btnnew.Text.Substring(2, 1)));
                list_player_gang.Add(list_player.Last());
                
                    labpeng_gang.Text += list_player.Last().name + " ";
            }
            int_whos_turn=0;
            btngang.Visible = false;
            btnpeng.Visible = false;
            btnpass.Visible = false;
            playerdisplay();
            playernewcart(list_player);
        }

        private void btnpass_Click(object sender, EventArgs e)
        {
            btnpeng.Visible = false;
            btngang.Visible = false;
            if(int_whos_turn!=0&&int_whos_turn!=3)
            {
                int_whos_turn++;
                if(int_whos_turn==2)
                {
                    comnewcart(btn_com2, list_com2, lab_com2cart,lab_com2cartdisplay,labpeng_gangcom2,list_com2_peng,list_com2_gang);
                }
                else if(int_whos_turn==3)
                {
                    comnewcart(btn_com3, list_com3, lab_com3cart,lab_com3cartdisplay,labpeng_gangcom3,list_com3_peng,list_com3_gang);
                }

            }
            else if(int_whos_turn==3)
            {
                int_whos_turn = 0;
                btnpass.Visible = false;
                playerdisplay();
                playernewcart(list_player);
            }
            
        }

        private void btnhu_Click(object sender, EventArgs e)
        {
            check_hu(list_player);
        }
        double group3(List<Acart> list_temp, double num)
        {
            List<Acart> list_temp2 = new List<Acart>();
            list_temp2.AddRange(list_temp);
            if (list_temp2.Any())
            {
                if (list_temp2.FindAll(r => r.name == list_temp2.First().name).Count < 3)
                {
                    if (list_temp2.Any(r => r.num == list_temp2.First().num + 1))
                    {
                        if (list_temp2.Any(r => r.num == list_temp2.First().num + 2))
                        {
                            list_temp2.Remove(list_temp2.Find(r => r.num == list_temp2.First().num + 1));
                            list_temp2.Remove(list_temp2.Find(r => r.num == list_temp2.First().num + 2));
                            list_temp2.Remove(list_temp2.First());
                            num++;
                            return group3(list_temp2, num);
                        }
                        else
                        {
                            num = -99;
                            return num;
                        }
                    }
                    else
                    {
                        num = -99;
                        return num;
                    }
                }
                else if (list_temp2.FindAll(r => r.name == list_temp2.First().name).Count >= 3)
                {
                    list_temp2.Remove(list_temp2.Find(r=>r.num==list_temp2.First().num));
                    list_temp2.Remove(list_temp2.Find(r => r.num == list_temp2.First().num));
                    list_temp2.Remove(list_temp2.Find(r => r.num == list_temp2.First().num));
                    num++;
                    return group3(list_temp2, num);
                }
            }
            return num;


        }
        double group(List<Acart>list_temp)
         {
            double num = 0;
            int numof2 = 0;
            bool find=false;
            List<Acart> list_temp2 = new List<Acart>();
            list_temp2.AddRange( list_temp);
            if (list_temp2.Count%3==2)
            {

                for (int i = 0; i < list_temp2.Count - 2; i++)
                {
                    if (list_temp2[i].name == list_temp2[i + 1].name && list_temp2[i].name == list_temp2[i + 2].name)
                    {
                        i++;
                    }
                    else if (list_temp2[i].name == list_temp2[i + 1].name && list_temp2[i].name != list_temp2[i + 2].name)
                    {
                        numof2++;
                    }
                    
                }
                if (list_temp2.Count > 2)
                {
                    if (list_temp2.Last().name == list_temp2.ElementAt(list_temp2.Count() - 2).name && list_temp2.Last().name != list_temp2.ElementAt(list_temp2.Count() - 3).name)
                    {
                        numof2++;
                    }
                }
                if (numof2 == 1)
                {
                    for (int i = 0; i < list_temp2.Count - 2; i++)
                    {
                        if (list_temp2[i].name == list_temp2[i + 1].name && list_temp2[i].name == list_temp2[i + 2].name)
                        {
                            i++;
                        }
                        else if (list_temp2[i].name == list_temp2[i + 1].name && list_temp2[i].name != list_temp2[i + 2].name)
                        {
                            list_temp2.Remove(list_temp2[i + 1]);
                            list_temp2.Remove(list_temp2[i]);
                            find = true;
                            num += 0.5;
                            i--;
                        }
                        
                    }

                    if (!find)
                    {
                        if (list_temp2.Last().name == list_temp2[list_temp2.Count() - 1].name)
                        {
                            list_temp2.Remove(list_temp2.Last());
                            list_temp2.Remove(list_temp2.Last());
                            find = true;
                            num += 0.5;
                        }
                    }
                    
                    return group3(list_temp2, num);
                }
                return group_too_many_two(list_temp2,num);
            }
            return group3(list_temp2, num);
        }
        double group_too_many_two(List<Acart> list_temp,double num)
        {
            double double_temp;
            List<Acart> list_temp2 = new List<Acart>();
            list_temp2.AddRange(list_temp);
            for (int i = 0; i < list_temp.Count - 2; i++)
            {
                if (list_temp2[i].name == list_temp2[i + 1].name && list_temp2[i].name == list_temp2[i + 2].name)
                {
                    i++;
                }
                else if (list_temp[i].name == list_temp[i + 1].name && list_temp[i].name != list_temp[i + 2].name)
                {
                    list_temp2.Remove(list_temp2[i + 1]);
                    list_temp2.Remove(list_temp2[i]);
                    double_temp = group3(list_temp2, num);
                    if(double_temp!=-99)
                    {
                        num += 0.5;
                        
                        return double_temp;
                    }
                    list_temp2.RemoveAll(r=>r.num>0);
                    list_temp2.AddRange(list_temp);
                }
            }
            if (list_temp2.Last().name == list_temp2.ElementAt(list_temp2.Count() - 1).name && list_temp2.Last().name == list_temp2.ElementAt(list_temp2.Count() - 2).name)
            {
                list_temp2.RemoveAt(list_temp2.Count()-1);
                list_temp2.RemoveAt(list_temp2.Count()-1);
                if (group3(list_temp2, num) != -99)
                {
                    num += 0.5;

                    return group3(list_temp2, num);
                }
            }
            return num;//-99;
        }
        void check_hu(List<Acart> player)
        {
            if (int_whos_turn != 0)
                player.Add(last_cart);
            double int_temp = 0;
            List<Acart> temp_acart = player, wan = new List<Acart>(), tong = new List<Acart>(), tiao = new List<Acart>();
            int_temp += list_player_gang.Count() + list_player_peng.Count();
            playerdisplay();
            if (player.Count() == 14)
                btnnew.Text = player.Last().name[0] + "\n" + player.Last().name[1];
            foreach (Acart q in temp_acart)
            {
                if (q.type == "万")
                {
                    wan.Add(q);
                }
                else if (q.type == "筒")
                {
                    tong.Add(q);
                }
                else if (q.type == "条")
                {
                    tiao.Add(q);
                }
            }
            if (que_player == "万")
            {
                if (!wan.Any())
                {
                    double w = group(wan), to = group(tong), ti = group(tiao);
                    int_temp += w + to + ti;
                    lab_playercart.Text += "万" + w + "筒" + to + "条" + ti;

                    if (int_temp == 4.5)
                    {
                        lab_win.Text = "你赢了" + int_temp;
                        //测试改动
                        lab_playercart.Text += "\n";
                        foreach (Acart q in list_all)
                        {
                            lab_playercart.Text += q.name + " ";
                        }
                    }
                    else
                    {
                        lab_win.Text = "诈胡" + int_temp;
                    }
                }
                else
                {
                    lab_win.Text = "花猪";
                }
            }
            else if (que_player == "筒")
            {
                if (!tong.Any())
                {
                    double w = group(wan), to = group(tong), ti = group(tiao);
                    int_temp += w + to + ti;
                    lab_playercart.Text += "万" + w + "筒" + to + "条" + ti;

                    if (int_temp == 4.5)
                    {
                        lab_win.Text = "你赢了" + int_temp;
                        //测试改动
                        lab_playercart.Text += "\n";
                        foreach (Acart q in list_all)
                        {
                            lab_playercart.Text += q.name + " ";
                        }
                    }
                    else
                    {
                        lab_win.Text = "诈胡" + int_temp;
                    }
                }
                else
                {
                    lab_win.Text = "花猪";
                }
            }
            else if (que_player == "条")
            {
                if (!tiao.Any())
                {
                    double w = group(wan), to = group(tong), ti = group(tiao);
                    int_temp += w + to + ti;
                    lab_playercart.Text += "万" + w + "筒" + to + "条" + ti;

                    if (int_temp == 4.5)
                    {
                        lab_win.Text = "你赢了" + int_temp;
                        //测试改动
                        lab_playercart.Text += "\n";
                        foreach (Acart q in list_all)
                        {
                            lab_playercart.Text += q.name + " ";
                        }
                    }
                    else
                    {
                        lab_win.Text = "诈胡" + int_temp;
                    }
                }
                else
                {
                    lab_win.Text = "花猪";
                }
            }
           
            btnhu.Enabled = false;
            btnpass.Enabled = false;
            btnpeng.Enabled = false;
            btngang.Enabled = false;
            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;
            btn5.Enabled = false;
            btn6.Enabled = false;
            btn7.Enabled = false;
            btn8.Enabled = false;
            btn9.Enabled = false;
            btn10.Enabled = false;
            btn11.Enabled = false;
            btn12.Enabled = false;
            btn13.Enabled = false;
            btnnew.Enabled = false;
            btn_com1.Enabled = false;
            btn_com2.Enabled = false;
            btn_com3.Enabled = false;
            
        }
        void check_com_hu(List<Acart> com, int int_temp_who, Label lab, List<Acart> list_gang, List<Acart> list_peng,string que)
        {
            int n=0;
            double int_temp = 0;
            List<Acart> temp_acart = new List<Acart>(), wan = new List<Acart>(), tong = new List<Acart>(), tiao = new List<Acart>();
            temp_acart.AddRange(com);
            if (int_whos_turn != int_temp_who)
                temp_acart.Add(last_cart);
            int_temp += list_gang.Count() + list_peng.Count();
            foreach (Acart q in temp_acart)
            {
                if (q.type == "万")
                {
                    wan.Add(q);
                }
                else if (q.type == "筒")
                {
                    tong.Add(q);
                }
                else if (q.type == "条")
                {
                    tiao.Add(q);
                }
            }
            if (que == "万")
            {
                if (!wan.Any())
                {
                    double w = group(wan), to = group(tong), ti = group(tiao);
                    int_temp += w + to + ti;

                    if (int_temp == 4.5)
                    {
                        lab_win.Text = "电脑赢了";
                        //测试改动
                        lab_playercart.Text += "\n";
                        foreach (Acart q in list_all)
                        {
                            lab_playercart.Text += q.name + " ";
                        }
                        btnhu.Enabled = false;
                        btnpass.Enabled = false;
                        btnpeng.Enabled = false;
                        btngang.Enabled = false;
                        btn1.Enabled = false;
                        btn2.Enabled = false;
                        btn3.Enabled = false;
                        btn4.Enabled = false;
                        btn5.Enabled = false;
                        btn6.Enabled = false;
                        btn7.Enabled = false;
                        btn8.Enabled = false;
                        btn9.Enabled = false;
                        btn10.Enabled = false;
                        btn11.Enabled = false;
                        btn12.Enabled = false;
                        btn13.Enabled = false;
                        btnnew.Enabled = false;
                        btn_com1.Enabled = false;
                        btn_com2.Enabled = false;
                        btn_com3.Enabled = false;
                        if (com == list_com1)
                        {
                            lab_com1cart.Text += "winner";
                            foreach (Acart q in com)
                            {
                                lab.Text += n + ". " + q.name + "\n" + "\n";
                                n++;
                            }
                        }
                        else if (com == list_com2)
                        {
                            lab_com2cart.Text += "winner";
                            foreach (Acart q in com)
                            {
                                lab.Text += n + ". " + q.name + "\n" + "\n";
                                n++;
                            }
                        }
                        else if (com == list_com3)
                        {
                            lab_com3cart.Text += "winner";
                            foreach (Acart q in com)
                            {
                                lab.Text += n + ". " + q.name + "\n" + "\n";
                                n++;
                            }
                        }
                    }

                }

            }
            else if (que == "筒")
            {
                if (!tong.Any())
                {
                    double w = group(wan), to = group(tong), ti = group(tiao);
                    int_temp += w + to + ti;

                    if (int_temp == 4.5)
                    {
                        lab_win.Text = "电脑赢了";
                        //测试改动
                        lab_playercart.Text += "\n";
                        foreach (Acart q in list_all)
                        {
                            lab_playercart.Text += q.name + " ";
                        }
                        btnhu.Enabled = false;
                        btnpass.Enabled = false;
                        btnpeng.Enabled = false;
                        btngang.Enabled = false;
                        btn1.Enabled = false;
                        btn2.Enabled = false;
                        btn3.Enabled = false;
                        btn4.Enabled = false;
                        btn5.Enabled = false;
                        btn6.Enabled = false;
                        btn7.Enabled = false;
                        btn8.Enabled = false;
                        btn9.Enabled = false;
                        btn10.Enabled = false;
                        btn11.Enabled = false;
                        btn12.Enabled = false;
                        btn13.Enabled = false;
                        btnnew.Enabled = false;
                        btn_com1.Enabled = false;
                        btn_com2.Enabled = false;
                        btn_com3.Enabled = false;
                        if (com == list_com1)
                        {
                            lab_com1cart.Text += "winner";
                            foreach (Acart q in com)
                            {
                                lab.Text += n + ". " + q.name + "\n" + "\n";
                                n++;
                            }
                        }
                        else if (com == list_com2)
                        {
                            lab_com2cart.Text += "winner";
                            foreach (Acart q in com)
                            {
                                lab.Text += n + ". " + q.name + "\n" + "\n";
                                n++;
                            }
                        }
                        else if (com == list_com3)
                        {
                            lab_com3cart.Text += "winner";
                            foreach (Acart q in com)
                            {
                                lab.Text += n + ". " + q.name + "\n" + "\n";
                                n++;
                            }
                        }
                    }

                }
            }
            else if (que == "条")
            {
                if (!tiao.Any())
                {
                    double w = group(wan), to = group(tong), ti = group(tiao);
                    int_temp += w + to + ti;

                    if (int_temp == 4.5)
                    {
                        lab_win.Text = "电脑赢了";
                        //测试改动
                        lab_playercart.Text += "\n";
                        foreach (Acart q in list_all)
                        {
                            lab_playercart.Text += q.name + " ";
                        }
                        
                        btnhu.Enabled = false;
                        btnpass.Enabled = false;
                        btnpeng.Enabled = false;
                        btngang.Enabled = false;
                        btn1.Enabled = false;
                        btn2.Enabled = false;
                        btn3.Enabled = false;
                        btn4.Enabled = false;
                        btn5.Enabled = false;
                        btn6.Enabled = false;
                        btn7.Enabled = false;
                        btn8.Enabled = false;
                        btn9.Enabled = false;
                        btn10.Enabled = false;
                        btn11.Enabled = false;
                        btn12.Enabled = false;
                        btn13.Enabled = false;
                        btnnew.Enabled = false;
                        btn_com1.Enabled = false;
                        btn_com2.Enabled = false;
                        btn_com3.Enabled = false;
                        if (com == list_com1)
                        {
                            lab_com1cart.Text += "winner";
                            foreach (Acart q in com)
                            {
                                lab.Text += n + ". " + q.name + "\n" + "\n";
                                n++;
                            }
                        }
                        else if (com == list_com2)
                        {
                            lab_com2cart.Text += "winner";
                            foreach (Acart q in com)
                            {
                                lab.Text += n + ". " + q.name + "\n" + "\n";
                                n++;
                            }
                        }
                        else if (com == list_com3)
                        {
                            lab_com3cart.Text += "winner";
                            foreach (Acart q in com)
                            {
                                lab.Text += n + ". " + q.name + "\n" + "\n";
                                n++;
                            }

                        }
                    }
                }
            }

            //lab.Text += "\n" + int_temp;
        }


    }
}
