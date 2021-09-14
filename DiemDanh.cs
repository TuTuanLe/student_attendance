using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSLR_R_FaceRecognitionsSystem
{
    class DiemDanh
    {
        private int id;
        private string name;
        private string tinhTrang;
        private int soLanDiemDanh;
        private DateTime ngayDiemDanh;


        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string TinhTrang { get => tinhTrang; set => tinhTrang = value; }
        public int SoLanDiemDanh { get => soLanDiemDanh; set => soLanDiemDanh = value; }
        public DateTime NgayDiemDanh { get => ngayDiemDanh; set => ngayDiemDanh = value; }
    }
}
