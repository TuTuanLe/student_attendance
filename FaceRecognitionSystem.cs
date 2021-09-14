using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using MySqlConnector;

namespace KSLR_R_FaceRecognitionsSystem
{
    public partial class FaceRecognitionSystem : Form
    {
        List<lop> LsLop = new List<lop>();
        //Variables 
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);

        //HaarCascade Library
        HaarCascade faceDetected;

        //For Camera as WebCams 
        Capture camera;

        //Images List if Stored
        Image<Bgr, Byte> Frame;

        Image<Gray, byte> result;
        Image<Gray, byte> TrainedFace = null;
        Image<Gray, byte> grayFace = null;

        //List 
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();

        List<string> labels = new List<string>();
        List<string> users = new List<string>();

        int Count, NumLables, t;
        string name, names = null;



        // Speech 
        SpeechRecognitionEngine _recorgnizer = new SpeechRecognitionEngine();
        SpeechSynthesizer Sarah = new SpeechSynthesizer();
        SpeechRecognitionEngine startlistening = new SpeechRecognitionEngine();
        Random rnd = new Random();
        int RectTimeOut = 0;

        DateTime TimeNow = DateTime.Now;

        // Show data
        private DataTable data;
        DataGridViewImageColumn img = new DataGridViewImageColumn();
        public FaceRecognitionSystem()
        {
            InitializeComponent();
            faceDetected = new HaarCascade("haarcascade_frontalface_alt.xml");

            try
            {
                string Labelsinf = File.ReadAllText(Application.StartupPath + "/Faces/Faces.txt");
                string[] Labels = Labelsinf.Split(',');

                NumLables = Convert.ToInt16(Labels[0]);
                Count = NumLables;

                string FacesLoad;

                for (int i = 1; i < NumLables + 1; i++)
                {
                    FacesLoad = "face" + i + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/Faces/" + FacesLoad));
                    labels.Add(Labels[i]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Database Folder is empty..!, please Register Face");
            }



            _recorgnizer.SetInputToDefaultAudioDevice();
            _recorgnizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            _recorgnizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
            _recorgnizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(_recorgnizer_SpeechRecognized);
            _recorgnizer.RecognizeAsync(RecognizeMode.Multiple);

            startlistening.SetInputToDefaultAudioDevice();
            startlistening.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            startlistening.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(startlistening_SpeechRecognized);

            data = new DataTable();
            data.Columns.Add("MASV", typeof(string));
            data.Columns.Add("HỌ TÊN", typeof(string));
            data.Columns.Add("MÃ LỚP", typeof(string));
            data.Columns.Add("THỜI GIAN ĐIỂM DANH", typeof(string));
            data.Columns.Add("##", typeof(Image));
            dgvTable.DataSource = data;


            


            showData();


            using (MySqlConnection conn = Conect_mysql.GetDBConnection("localhost", 3306, "db_diemdanhkm", "root", ""))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from lop ", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LsLop.Add(new lop()
                        {
                            MaLop= reader["malop"].ToString(),
                            TenLop = reader["TenLop"].ToString()
                        });
                    }
                }
            }

            foreach (var item in LsLop)
            {
                cbbMaLop.Items.Add(item.TenLop);
            }


            panelTongQuan.Visible = true;
            
        }




        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int ranNum;
            string speech = e.Result.Text;
            if(speech == "hello")
            {
                Sarah.SpeakAsync("Hello UIT, i am here, May I Help you ");
            }
            if (speech == "how are you")
            {
                Sarah.SpeakAsync("I am fine ");
            }
            if (speech == "what time is it")
            {
                Sarah.SpeakAsync(DateTime.Now.ToString("h mm tt"));
                lbTime.Text = DateTime.Now.ToString("h mm tt");
            }
            if (speech == "Stop talking")
            {
                Sarah.SpeakAsyncCancelAll();
                ranNum = rnd.Next(1, 2);
                if(ranNum == 1)
                {
                    Sarah.SpeakAsync("Yes sir");
                }
                if(ranNum == 2)
                {
                    Sarah.SpeakAsync("i am sorry i will be quist");
                }
            } 
            if(speech == "Stop listening")
            {
                Sarah.SpeakAsync("if you need me just ask");
                _recorgnizer.RecognizeAsyncCancel();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
            }
            if(speech == "show")
            {
                string[] commands = File.ReadAllLines(@"DefaultCommands.txt");
                lstCommands.Items.Clear();
                lstCommands.SelectionMode = SelectionMode.None;
                lstCommands.Visible = true;
                foreach(string command in commands)
                {
                    lstCommands.Items.Add(command);
                }
            }
            if(speech == "hide")
            {
                lstCommands.Visible = false;
            }
            if (speech == "attendance")
            {
                if(lblName.Text!="")
                {
                    Sarah.SpeakAsync("atttendance start ");
                    btnDiemDanh_Click(sender, e);
                }
                else
                {
                    Sarah.SpeakAsync("atttendance fail ");
                }

                
            }    
            if(speech == "camera")
            {
                Sarah.SpeakAsync("camera start ");
                Start_Click(sender, e);
            }
            if (speech == "what your name")
            {
                Sarah.SpeakAsync("I'am your Assitant ");

            }
            if(speech =="who are you")
            {
                if (lblName.Text != "")
                    Sarah.SpeakAsync(lblName.Text);
                else
                    Sarah.SpeakAsync("Who are you , i don't Face Recognition ");
            }    

            if (speech == "where are you")
            {
                Sarah.SpeakAsync("KTX A");

            }
            if(speech == "save")
            {
                if (txName.Text == "")
                {
                    Sarah.SpeakAsync("please fill in your information before saveing");
                }
                else
                {
                    if(lblCountAllFaces.Text=="0")
                    {
                        Sarah.SpeakAsync("the face has not been detected");
                    }
                    else{
                        Sarah.SpeakAsync("save UIT");
                        Save_Click(sender, e);
                        data.Clear();
                        showData();
                    }
                }

               

            }
            if(speech == "close")
            {
                Sarah.SpeakAsync("Application close");
                this.Close(); 

            }

            if(speech == "restart")
            {
                Sarah.SpeakAsync("Application restart");
                Restart_Click(sender, e);
            }    
            


            lbAssitant.Text = speech;

        }

       

        private void _recorgnizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RectTimeOut = 0;
        }

        private void startlistening_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            if(speech == "wake up ")
            {
                startlistening.RecognizeAsyncCancel();
                Sarah.SpeakAsync("yes , i am here");
                _recorgnizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }
        private List<DiemDanh> LsDD = new List<DiemDanh>();
        private List<object> Lssv = new List<object>();

        private void insertNhanVien( sinhvien sv)
        {
            String sql = "insert into sinhvien (masv, hoten, malop) values ("
               + "'" + sv.Masv + "',"
               + "'" + sv.Hoten + "',"
               + "'" + sv.Malop + "'"
               + " )";
            string sql2 = "insert into diemdanh(masv, thang, nam, tinhtrang) values ("
               + "'" + sv.Masv + "',"
               + "" + DateTime.Now.Month + ","
               + "" + DateTime.Now.Year + ","
               + "'absent'" 
               + " )";
            

            using (MySqlConnection con = Conect_mysql.GetDBConnection("localhost", 3306, "db_diemdanhkm", "root", ""))
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                MySqlCommand cmd2 = new MySqlCommand(sql2, con);
                cmd2.ExecuteNonQuery();
                cmd2.Dispose();
                con.Close();
            }

            



        }

        private int KiemTraDiemDanh(string masv)
        {
            int countDD = 0;
            using (MySqlConnection conn = Conect_mysql.GetDBConnection("localhost", 3306, "db_diemdanhkm", "root", ""))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from sinhvien sv, diemdanh dd, ct_diemdanh ctdd where sv.masv = dd.masv and dd.madiemdanh = ctdd.madiemdanh and sv.masv =" + masv + " and ctdd.ngay="+DateTime.Now.Day+" ", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        countDD++;
                    }
                }

            }
            if(countDD == 0)
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

        private string CheckTimeDD(string masv)
        {
            string DateCheck = "-----------------";
            using (MySqlConnection conn = Conect_mysql.GetDBConnection("localhost", 3306, "db_diemdanhkm", "root", ""))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from sinhvien sv, diemdanh dd, ct_diemdanh ctdd where sv.masv = dd.masv and dd.madiemdanh = ctdd.madiemdanh and sv.masv =" + masv + " and ctdd.ngay=" + DateTime.Now.Day + "", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateCheck = reader["lido"].ToString();
                    }
                }

            }
            return DateCheck;
        }

        private void showData()
        {
            Lssv.Clear();
            data.Clear();
            using (MySqlConnection conn = Conect_mysql.GetDBConnection("localhost", 3306, "db_diemdanhkm", "root", ""))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from sinhvien sv, diemdanh dd  where sv.masv = dd.masv ", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        

                        Lssv.Add(new
                        {
                            Masv = reader["masv"].ToString(),
                            Hoten = reader["hoten"].ToString(),
                            MaLop = reader["malop"].ToString(),
                            ThoiGianDiemDanh= CheckTimeDD(reader["masv"].ToString()),                      
                        });
                    }
                }
            }

            Image image;
            
            for (int i = 0; i < Lssv.Count; i++)
            {
                if (KiemTraDiemDanh(Lssv[i].GetType().GetProperty("Masv").GetValue(Lssv[i], null).ToString()) == 0)
                {
                    image = Image.FromFile(@"1.png");
                }
                else
                {
                    image = Image.FromFile(@"heart_50px.png");
                }
                img.Image = image;

                data.Rows.Add(Lssv[i].GetType().GetProperty("Masv").GetValue(Lssv[i], null),
                              Lssv[i].GetType().GetProperty("Hoten").GetValue(Lssv[i],null),
                              Lssv[i].GetType().GetProperty("MaLop").GetValue(Lssv[i], null),
                              Lssv[i].GetType().GetProperty("ThoiGianDiemDanh").GetValue(Lssv[i], null).ToString(),
                              img.Image
                );
            }



            // delete nhân viên

            //string sql = "delete from account where manv='" + ac.Manv + "'";
            //using (MySqlConnection con = GetConnection())
            //{
            //    MySqlCommand cmd = new MySqlCommand(sql, con);
            //    con.Open();
            //    cmd.ExecuteNonQuery();
            //    cmd.Dispose();
            //    con.Close();
            //}

            dgvTable.DataSource = data;
        }

        public bool camstatus = false;
        private void Start_Click(object sender, EventArgs e)
        {
            camstatus = true;
            camera = new Capture(); 
            camera.QueryFrame();

            Application.Idle += new EventHandler(FrameProcedure);

            btnStart.Enabled = false;

            btnSave.Enabled = true;
 
            btnRestart.Enabled = true;
            txtMasv.Focus();

        }
        private void Open_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = Application.StartupPath + " / Faces/",
                UseShellExecute = true,
                Verb = "open"
            });

        }

        private void Restart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void FaceRecognitionSystem_Load(object sender, EventArgs e)
        {

        }

        private void btnAssistant_Click(object sender, EventArgs e)
        {
            btnDiemDanh_Click(sender, e);
        }

        private void TmrSpeaking_Tick(object sender, EventArgs e)
        {
            if(RectTimeOut == 10)
            {
                _recorgnizer.RecognizeAsyncCancel();
            }
            else if(RectTimeOut == 11)
            {
                TmrSpeaking.Stop();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
                RectTimeOut = 0;
            }
        }

        private void btnDiemDanh_Click(object sender, EventArgs e)
        {
            if(camstatus == true)
            {
                if (lblCountAllFaces.Text != "" && lblName.Text !="" )
                {
                    if (KiemTraDiemDanh(lblName.Text) != 0)
                    {
                        MessageBox.Show("Bạn Đã Điểm Danh rồi !!");
                    }
                    else
                    {

                        DiemDanhSinHVien(lblName.Text);
                        showData();
                        MessageBox.Show("điểm danh thành công!!");
                    }

                }
                else
                {
                    MessageBox.Show("chưa nhận diện được bạn là ai!");
                }
            }
            else
            {
                MessageBox.Show("hãy bật camera để thực hiện chức năng này!");
            }
            

           
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            

        }

        private void dgvTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            string masv = dgvTable.Rows[index].Cells[0].Value.ToString();
            FormChiTietDiemDanh form = new FormChiTietDiemDanh(masv);
            form.Show();
          
             showData();
        }

        private void dgvTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            MessageBox.Show(index.ToString());


        }

        private void btnTongQuan_Click(object sender, EventArgs e)
        {
            panelTongQuan.Visible = true;
            
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            panelTongQuan.Visible = false;
            
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void ExitPro_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (txtMasv.Text == "" || txtMasv.Text.Length < 2 || txtMasv.Text == string.Empty)
            {
                MessageBox.Show("Please enter name of person");
            }
            else
            {
                Count += 1;
                grayFace = camera.QueryGrayFrame().Resize(320, 240, INTER.CV_INTER_CUBIC);
                MCvAvgComp[][] DetectedFace = grayFace.DetectHaarCascade(faceDetected, 1.2, 10,
                    HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

                foreach (MCvAvgComp f in DetectedFace[0])
                {

                    TrainedFace = Frame.Copy(f.rect).Convert<Gray, Byte>();
                    break;
                }

                TrainedFace = result.Resize(100, 100, INTER.CV_INTER_CUBIC);

                trainingImages.Add(TrainedFace);
                IBOutput.Image = TrainedFace;

                labels.Add(txtMasv.Text);

                File.WriteAllText(Application.StartupPath + "/Faces/Faces.txt", trainingImages.ToArray().Length.ToString() + ",");

                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/Faces/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/Faces/Faces.txt", labels.ToArray()[i - 1] + ",");
                }

                MessageBox.Show("Face Stored.");
                txtMasv.Focus();
                insertNhanVien(new sinhvien {Masv =txtMasv.Text,Hoten =txName.Text,Malop=LsLop[cbbMaLop.SelectedIndex].MaLop });
            }
            showData();
         

           

        }

        private void FrameProcedure(object sender, EventArgs e)
        {
            users.Add("");
            lblCountAllFaces.Text = "0";

            Frame = camera.QueryFrame().Resize(320, 240, INTER.CV_INTER_CUBIC);
            grayFace = Frame.Convert<Gray, Byte>();

            MCvAvgComp[][] faceDetectedShow = grayFace.DetectHaarCascade(faceDetected, 1.2, 10,
                HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

            foreach (MCvAvgComp f in faceDetectedShow[0])
            {
                t += 1;

                result = Frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
                Frame.Draw(f.rect, new Bgr(Color.Blue), 3);

                if (trainingImages.ToArray().Length != 0)
                {
                    MCvTermCriteria termCriterias = new MCvTermCriteria(Count, 0.001);
                    EigenObjectRecognizer recognizer =
                        new EigenObjectRecognizer(trainingImages.ToArray(),
                        labels.ToArray(), 3000,
                        ref termCriterias);

                    name = recognizer.Recognize(result);
                    Frame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));

                }
                users[t - 1] = name;
                users.Add("");
                //Set the number of faces detected on the scene
                lblCountAllFaces.Text = faceDetectedShow[0].Length.ToString();
                users.Add("");

            }

            t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < faceDetectedShow[0].Length; nnn++)
            {
                names = names + users[nnn] ;

            }

            //Show the faces procesed and recognized
            cameraBox.Image = Frame;
            lblName.Text = names;
            names = "";
       
            users.Clear();
        }

    }

}
