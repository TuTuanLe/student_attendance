using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KSLR_R_FaceRecognitionsSystem
{
    public partial class FormChiTietDiemDanh : Form
    {
        private string Mass;
        List<int> LsNam = new List<int>();
        List<int> LsThang = new List<int>();
        private List<Button> lsbutton = new List<Button>();
        public FormChiTietDiemDanh(string masv)
        {
           
            InitializeComponent();
            Mass = masv;
            loadData(DateTime.Now.Month, DateTime.Now.Year, masv);
        }

        private void button38_Click(object sender, EventArgs e)
        {
            this.Close();
       
        }

        

        private void loadData(int month, int year, string masv)
        {
            LsNam.Clear();
            LsNam.Add(2021);
            LsNam.Add(2020);
            LsNam.Add(2019);
            LsNam.Add(2018);
            LsThang.Clear();
            LsThang.Add(1);
            LsThang.Add(2);
            LsThang.Add(3);
            LsThang.Add(4);
            LsThang.Add(5);
            LsThang.Add(6);
            LsThang.Add(7);
            LsThang.Add(8);
            LsThang.Add(9);
            LsThang.Add(10);
            LsThang.Add(11);
            LsThang.Add(12);
            loadbuton();
            btnt2.Text = ThuCuaNam(1,month, year);
            btnt3.Text = ThuCuaNam(2, month, year);
            btnt4.Text = ThuCuaNam(3, month, year);
            btnt5.Text = ThuCuaNam(4, month, year);
            btnt6.Text = ThuCuaNam(5, month, year);
            btnt7.Text = ThuCuaNam(6, month, year);
            btnt8.Text = ThuCuaNam(7, month, year);


            for (int i = fun(month, year); i < lsbutton.Count; i++)
            {
                lsbutton[i].Text = "";
            }

            using (MySqlConnection conn = Conect_mysql.GetDBConnection("localhost", 3306, "db_diemdanhkm", "root", ""))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from sinhvien sv, diemdanh dd , ct_diemdanh ctdd " +
                    "where sv.masv = dd.masv and ctdd.madiemdanh  = dd.madiemdanh and sv.masv ='" + masv+"' and thang = "+month+" and nam  = "+year+" ", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lsbutton[ int.Parse( reader["ngay"].ToString())-1].Image = Image.FromFile(@"heart_50px.png");
                        if (int.Parse(reader["ngay"].ToString())  == DateTime.Now.Day)
                        {
                            lbdiemdanh.Text = "Đã Điểm Danh";
                            lbdiemdanh.ForeColor = Color.Green;
                        }
                    }
                }
            }

            using (MySqlConnection conn = Conect_mysql.GetDBConnection("localhost", 3306, "db_diemdanhkm", "root", ""))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from sinhvien sv where  sv.masv ='" + masv + "'", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        txtMasv.Text = masv;
                        txtTensv.Text = reader["hoten"].ToString();
                        txtmalop.Text = reader["malop"].ToString();
                    }
                }
            }
        }

        private void loadbuton()
        {
            lsbutton.Add(button1);
            lsbutton.Add(button2);
            lsbutton.Add(button3);
            lsbutton.Add(button4);
            lsbutton.Add(button5);
            lsbutton.Add(button6);
            lsbutton.Add(button7);
            lsbutton.Add(button8);
            lsbutton.Add(button9);
            lsbutton.Add(button10);
            lsbutton.Add(button11);
            lsbutton.Add(button12);
            lsbutton.Add(button13);
            lsbutton.Add(button14);
            lsbutton.Add(button15);
            lsbutton.Add(button16);
            lsbutton.Add(button17);
            lsbutton.Add(button18);
            lsbutton.Add(button19);
            lsbutton.Add(button20);
            lsbutton.Add(button21);
            lsbutton.Add(button22);
            lsbutton.Add(button23);
            lsbutton.Add(button24);
            lsbutton.Add(button25);
            lsbutton.Add(button26);
            lsbutton.Add(button27);
            lsbutton.Add(button28);
            lsbutton.Add(button29);
            lsbutton.Add(button30);
            lsbutton.Add(button31);

            for (int i = 0; i < lsbutton.Count; i++)
            {   
                if(i <=  DateTime.Now.Day -1)
                {
                    lsbutton[i].Image = Image.FromFile(@"1.png");
                    lsbutton[i].Text = "                "+(i+1);
                }
            }
        }

        private string ThuCuaNam(int ngay, int thang, int nam)
        {
            string[] thu = { "Thứ 5", "thứ 6", "thứ 7", "Chủ Nhật", "thứ 2", "thứ 3", "thứ 4" };
            int s = 0;
            for (int i = 1970; i < nam; i++)
            {
                if (i % 4 == 0)
                    s += 366;
                else
                    s += 365;
            }
            s += TuDauNam(ngay, thang, nam);
            s = s - 1;
            return thu[s % 7];
        }

        private int TuDauNam(int ngay, int thang, int nam)
        {
            int s = 0;
            int i;
            for (i = 1; i < thang; i++)
            {
                switch (i)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        s = s + 31;
                        break;
                    case 2:
                        if (nam % 4 == 0)
                            s += 29;
                        else
                            s += 28;
                        break;
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        s += 30;
                        break;
                }
            }
            s += ngay;
            return s;
        }

        private bool isCheck(int nam)
        {
            if ((nam % 4 == 0 && nam % 100 != 0) || nam % 400 == 0)
                return true;
            return false;
        }
        private int fun(int thang, int nam)
        {
            switch (thang)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                case 2:
                    if (isCheck(nam))
                        return 29;
                    else
                        return 28;
             

            }
            return 0;
        }
        
        private void btnXem_Click(object sender, EventArgs e)
        {
            loadData(LsThang[cbbMonth.SelectedIndex], LsNam[cbbYear.SelectedIndex], Mass);
        }
        public int tempp = 0;
        public void btnDiemDanh_Click(object sender, EventArgs e)
        {
            if (KiemTraDiemDanh(txtMasv.Text) != 0)
            {
                MessageBox.Show("Bạn Đã Điểm Danh rồi !!");
            }
            else
            {
                DiemDanhSinHVien(txtMasv.Text);
                loadData(DateTime.Now.Month, DateTime.Now.Year, Mass);
                tempp = 1;
            }
        }
        private int KiemTraDiemDanh(string masv)
        {
            int countDD = 0;
            using (MySqlConnection conn = Conect_mysql.GetDBConnection("localhost", 3306, "db_diemdanhkm", "root", ""))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from sinhvien sv, diemdanh dd, ct_diemdanh ctdd where sv.masv = dd.masv and dd.madiemdanh = ctdd.madiemdanh and sv.masv =" + masv + " and ctdd.ngay=" + DateTime.Now.Day + " ", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        countDD++;
                    }
                }

            }
            if (countDD == 0)
                return 0; // chưa điểm danh
            return 1; // Đã điểm danh

        }
        private void DiemDanhSinHVien(string masv)
        {
            string madiemdanh = "";
            using (MySqlConnection conn = Conect_mysql.GetDBConnection("localhost", 3306, "db_diemdanhkm", "root", ""))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from sinhvien sv, diemdanh dd where sv.masv = dd.masv and sv.masv =" + masv + "", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        madiemdanh = reader["madiemdanh"].ToString();
                    }
                }

            }

            string sql = "insert into ct_diemdanh(madiemdanh, ngay, lido, sogiotre) values ("
               + "'" + madiemdanh + "',"
               + "" + DateTime.Now.Day + ","
               + "'" + DateTime.Now + "',"
               + "0"
               + ")";

            using (MySqlConnection con = Conect_mysql.GetDBConnection("localhost", 3306, "db_diemdanhkm", "root", ""))
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
    }
}
