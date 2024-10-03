using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLinQ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        
            private void btnLuuSP_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text) ||
                    string.IsNullOrWhiteSpace(txtSoLuong.Text) || string.IsNullOrWhiteSpace(txtDonGia.Text) ||
                    string.IsNullOrWhiteSpace(txtXuatXu.Text) || !int.TryParse(txtSoLuong.Text, out _) ||
                    !decimal.TryParse(txtDonGia.Text, out _))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ và hợp lệ thông tin sản phẩm.");
                    return;
                }

                using (var context = new QuanLySanPhamEntities())
                {
                    SanPham sp = new SanPham
                    {
                        MaSP = txtMaSP.Text,
                        TenSP = txtTenSP.Text,
                        SoLuong = int.Parse(txtSoLuong.Text),
                        DonGia = decimal.Parse(txtDonGia.Text),
                        XuatXu = txtXuatXu.Text,
                        NgayHetHan = dtpNgayHetHan.Value
                    };

                    context.SanPhams.Add(sp);
                    context.SaveChanges();
                    LoadData();
                }
            }


            private void LoadData()
        {
            using (var context = new QuanLySanPhamEntities())
            {
                var sanPhams = context.SanPhams.ToList();
                dgvSanPham.DataSource = sanPhams;
            }
        }
        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sản phẩm để xóa.");
                return;
            }

            using (var context = new QuanLySanPhamEntities())
            {
                string maSP = txtMaSP.Text;
                var sp = context.SanPhams.SingleOrDefault(s => s.MaSP == maSP);
                if (sp != null)
                {
                    context.SanPhams.Remove(sp);
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm với mã này.");
                }
            }
        }


        private void btnSPCaoNhat_Click(object sender, EventArgs e)
        {
            using (var context = new QuanLySanPhamEntities())
            {
                var sanPham = context.SanPhams.OrderByDescending(s => s.DonGia).FirstOrDefault();
                if (sanPham != null)
                {
                    List<SanPham> sanPhamList = new List<SanPham> { sanPham };
                    dgvTimKiemSP.DataSource = sanPhamList;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm.");
                }
            }
        }

        private void btnSPNhatBan_Click(object sender, EventArgs e)
        {
            using (var context = new QuanLySanPhamEntities())
            {
                var sanPham = context.SanPhams.Where(s => s.XuatXu == "Nhật Bản").ToList();
                if (sanPham.Count > 0)
                {
                    dgvTimKiemSP.DataSource = sanPham;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm từ Nhật Bản.");
                }
            }
        }
        private void btnKiemTraQuaHan_Click(object sender, EventArgs e)
        {
            using (var context = new QuanLySanPhamEntities())
            {
                var sanPhamQuaHan = context.SanPhams.Any(sp => sp.NgayHetHan < DateTime.Now);
                if (sanPhamQuaHan)
                {
                    MessageBox.Show("Có sản phẩm quá hạn trong kho!");
                }
                else
                {
                    MessageBox.Show("Không có sản phẩm quá hạn.");
                }
            }
        }


        private void btnSPQuaHan_Click(object sender, EventArgs e)
        {
            using (var context = new QuanLySanPhamEntities())
            {
                var sanPhamQuaHan = context.SanPhams.Where(sp => sp.NgayHetHan < DateTime.Now).ToList();
                if (sanPhamQuaHan.Count > 0)
                {
                    dgvTimKiemSP.DataSource = sanPhamQuaHan;
                }
                else
                {
                    MessageBox.Show("Không có sản phẩm quá hạn.");
                }
            }
        }
        private void btnSPDonGiaAB_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGiaMin.Text) || string.IsNullOrWhiteSpace(txtGiaMax.Text) ||
                !decimal.TryParse(txtGiaMin.Text, out _) || !decimal.TryParse(txtGiaMax.Text, out _))
            {
                MessageBox.Show("Vui lòng nhập khoảng giá hợp lệ.");
                return;
            }

            decimal giaMin = decimal.Parse(txtGiaMin.Text);
            decimal giaMax = decimal.Parse(txtGiaMax.Text);

            using (var context = new QuanLySanPhamEntities())
            {
                var sanPhamTheoGia = context.SanPhams.Where(sp => sp.DonGia >= giaMin && sp.DonGia <= giaMax).ToList();
                if (sanPhamTheoGia.Count > 0)
                {
                    dgvTimKiemSP.DataSource = sanPhamTheoGia;
                }
                else
                {
                    MessageBox.Show("Không có sản phẩm trong khoảng giá này.");
                }
            }
        }


        private void btnXoaTheoXuatXu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtXuatXuXoa.Text))
            {
                MessageBox.Show("Vui lòng nhập xuất xứ để xóa.");
                return;
            }

            string xuatXu = txtXuatXuXoa.Text;

            using (var context = new QuanLySanPhamEntities())
            {
                var sanPhams = context.SanPhams.Where(sp => sp.XuatXu == xuatXu).ToList();
                if (sanPhams.Count > 0)
                {
                    context.SanPhams.RemoveRange(sanPhams);
                    context.SaveChanges();
                    LoadData();
                    MessageBox.Show($"Đã xóa tất cả sản phẩm có xuất xứ từ {xuatXu}.");
                }
                else
                {
                    MessageBox.Show($"Không có sản phẩm nào có xuất xứ từ {xuatXu}.");
                }
            }
        }


        private void btnXoaToanBo_Click(object sender, EventArgs e)
        {

            using (var context = new QuanLySanPhamEntities())
            {
                var allSanPhams = context.SanPhams.ToList();
                if (allSanPhams.Count > 0)
                {
                    context.SanPhams.RemoveRange(allSanPhams);
                    context.SaveChanges();  
                    LoadData();  
                    MessageBox.Show("Đã xóa toàn bộ sản phẩm trong kho.");
                }
                else
                {
                    MessageBox.Show("Kho hiện không có sản phẩm.");
                }
            }
        }

        private void XoaQuaHan_Click(object sender, EventArgs e)
        {
            using (var context = new QuanLySanPhamEntities())
            {
                var sanPhamQuaHan = context.SanPhams.Where(sp => sp.NgayHetHan < DateTime.Now).ToList();
                if (sanPhamQuaHan.Count > 0)
                {
                    context.SanPhams.RemoveRange(sanPhamQuaHan);
                    context.SaveChanges();
                    LoadData();
                    MessageBox.Show("Đã xóa tất cả sản phẩm quá hạn trong kho.");
                }
                else
                {
                    MessageBox.Show("Không có sản phẩm quá hạn trong kho.");
                }
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }

}
