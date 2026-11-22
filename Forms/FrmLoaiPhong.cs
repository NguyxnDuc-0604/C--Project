using System.Data;
using System.Drawing;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.Forms
{
    public class FrmLoaiPhong : Form
    {
        readonly LoaiPhongDAL dal=new(); readonly TextBox ma=new(),ten=new(),gia=new(),nguoi=new(),mota=new(),soluong=new(),tim=new(); readonly DataGridView grid=new();
        public FrmLoaiPhong(){ Text="QUẢN LÝ LOẠI PHÒNG"; Size=new Size(1000,680); StartPosition=FormStartPosition.CenterScreen; Build(); LoadData(); }
        void Build(){ Controls.Add(new Label{Text="QUẢN LÝ LOẠI PHÒNG",Font=new Font("Arial",19,FontStyle.Bold),ForeColor=Color.DarkBlue,Location=new Point(330,20),AutoSize=true});
            Field("Mã loại:",ma,30,85); Field("Tên loại:",ten,30,130); Field("Giá phòng:",gia,30,175); Field("Số người tối đa:",nguoi,500,85); Field("Số lượng phòng:",soluong,500,130); Field("Mô tả:",mota,500,175);
            Btn("Thêm",30,225,Add); Btn("Sửa",140,225,Edit); Btn("Xóa",250,225,Delete); Btn("Làm mới",360,225,Reset);
            Controls.Add(new Label{Text="Tìm kiếm:",Location=new Point(500,235),AutoSize=true}); tim.SetBounds(580,230,200,28); Controls.Add(tim); Btn("Tìm",790,228,Search);
            Grid(grid,30,280,920,330); grid.CellClick+=(_,e)=>{if(e.RowIndex>=0){var r=grid.Rows[e.RowIndex]; ma.Text=V(r,"MaLoai");ten.Text=V(r,"TenLoai");gia.Text=V(r,"GiaPhong");nguoi.Text=V(r,"SoNguoiToiDa");mota.Text=V(r,"MoTa");soluong.Text=V(r,"SoLuongPhong");}}; }
        void Field(string label,TextBox box,int x,int y){Controls.Add(new Label{Text=label,Location=new Point(x,y+4),Size=new Size(130,24)});box.SetBounds(x+135,y,300,28);Controls.Add(box);}
        void Btn(string s,int x,int y,EventHandler h){var b=new Button{Text=s,Location=new Point(x,y),Size=new Size(100,34),BackColor=Color.LightSteelBlue};b.Click+=h;Controls.Add(b);}
        static void Grid(DataGridView g,int x,int y,int w,int h){g.SetBounds(x,y,w,h);g.ReadOnly=true;g.AllowUserToAddRows=false;g.SelectionMode=DataGridViewSelectionMode.FullRowSelect;g.MultiSelect=false;g.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;g.RowHeadersVisible=false;}
        static string V(DataGridViewRow r,string c)=>r.Cells[c].Value?.ToString()??"";
        void LoadData(){try{grid.DataSource=dal.GetAll();}catch(Exception ex){MessageBox.Show("Lỗi tải loại phòng: "+ex.Message);}}
        bool Valid(){if(!int.TryParse(ma.Text,out _)||string.IsNullOrWhiteSpace(ten.Text)||!decimal.TryParse(gia.Text,out var p)||p<0||!int.TryParse(nguoi.Text,out var n)||n<1||!int.TryParse(soluong.Text,out var s)||s<0){MessageBox.Show("Vui lòng nhập mã, tên và các số liệu hợp lệ.");return false;}return true;}
        void Add(object? s,EventArgs e){if(!Valid())return;Run(()=>dal.Them(int.Parse(ma.Text),ten.Text.Trim(),decimal.Parse(gia.Text),int.Parse(nguoi.Text),mota.Text.Trim(),int.Parse(soluong.Text)),"Thêm loại phòng");}
        void Edit(object? s,EventArgs e){if(!Valid())return;Run(()=>dal.Sua(int.Parse(ma.Text),ten.Text.Trim(),decimal.Parse(gia.Text),int.Parse(nguoi.Text),mota.Text.Trim(),int.Parse(soluong.Text)),"Sửa loại phòng");}
        void Delete(object? s,EventArgs e){if(int.TryParse(ma.Text,out int id)&&MessageBox.Show("Xóa loại phòng này?","Xác nhận",MessageBoxButtons.YesNo)==DialogResult.Yes)Run(()=>dal.Xoa(id),"Xóa loại phòng");}
        void Search(object? s,EventArgs e){try{grid.DataSource=dal.TimKiem(tim.Text.Trim());}catch(Exception ex){MessageBox.Show(ex.Message);}}
        void Reset(object? s,EventArgs e){ma.Clear();ten.Clear();gia.Clear();nguoi.Clear();mota.Clear();soluong.Clear();tim.Clear();LoadData();}
        void Run(Action action,string operation){try{action();LoadData();MessageBox.Show(operation+" thành công.");Reset(null!,EventArgs.Empty);}catch(Exception ex){MessageBox.Show(operation+" thất bại: "+ex.Message);}}
    }
}
