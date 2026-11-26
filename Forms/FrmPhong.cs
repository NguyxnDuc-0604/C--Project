using System.Data;
using System.Drawing;
using QuanLyKhachSan.BUS;

namespace QuanLyKhachSan.Forms
{
    public class FrmPhong : Form
    {
        readonly PhongBUS bus=new(); readonly TextBox ma=new(),ten=new(),vitri=new(),tim=new(); readonly ComboBox loai=new(),trangthai=new(); readonly DataGridView grid=new();
        public FrmPhong(){Text="QUẢN LÝ PHÒNG";Size=new Size(1000,650);StartPosition=FormStartPosition.CenterScreen;Build();LoadCombos();LoadData();}
        void Build(){Controls.Add(new Label{Text="QUẢN LÝ PHÒNG",Font=new Font("Arial",19,FontStyle.Bold),ForeColor=Color.DarkBlue,Location=new Point(390,20),AutoSize=true});Field("Mã phòng:",ma,30,90);Field("Tên phòng:",ten,30,140);Field("Loại phòng:",loai,500,90);Field("Vị trí:",vitri,500,140);Field("Trạng thái:",trangthai,30,190);trangthai.Items.AddRange(new object[]{"Trống","Đang sử dụng","Bảo trì"});trangthai.SelectedIndex=0;
            Btn("Thêm",30,245,Add);Btn("Sửa",140,245,Edit);Btn("Xóa",250,245,Delete);Btn("Làm mới",360,245,Reset);Controls.Add(new Label{Text="Tìm kiếm:",Location=new Point(500,255),AutoSize=true});tim.SetBounds(580,250,200,28);Controls.Add(tim);Btn("Tìm",790,248,Search);Grid(grid,30,300,920,300);grid.CellClick+=(_,e)=>{if(e.RowIndex>=0){var r=grid.Rows[e.RowIndex];ma.Text=V(r,"MaPhong");ten.Text=V(r,"TenPhong");vitri.Text=V(r,"ViTri");loai.SelectedValue=Convert.ToInt32(r.Cells["MaLoai"].Value);trangthai.Text=V(r,"TrangThai");}};}
        void Field(string label,Control c,int x,int y){Controls.Add(new Label{Text=label,Location=new Point(x,y+4),Size=new Size(125,24)});c.SetBounds(x+130,y,300,28);if(c is ComboBox cb)cb.DropDownStyle=ComboBoxStyle.DropDownList;Controls.Add(c);}
        void Btn(string s,int x,int y,EventHandler h){var b=new Button{Text=s,Location=new Point(x,y),Size=new Size(100,34),BackColor=Color.LightSteelBlue};b.Click+=h;Controls.Add(b);} static void Grid(DataGridView g,int x,int y,int w,int h){g.SetBounds(x,y,w,h);g.ReadOnly=true;g.AllowUserToAddRows=false;g.SelectionMode=DataGridViewSelectionMode.FullRowSelect;g.MultiSelect=false;g.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;g.RowHeadersVisible=false;} static string V(DataGridViewRow r,string c)=>r.Cells[c].Value?.ToString()??"";
        void LoadCombos(){try{loai.DataSource=bus.GetLoaiPhong();loai.DisplayMember="TenLoai";loai.ValueMember="MaLoai";loai.SelectedIndex=-1;}catch(Exception ex){MessageBox.Show("Lỗi tải loại phòng: "+ex.Message);}} void LoadData(){try{grid.DataSource=bus.GetAll();}catch(Exception ex){MessageBox.Show("Lỗi tải phòng: "+ex.Message);}}
        bool Valid(){if(!int.TryParse(ma.Text,out _)||string.IsNullOrWhiteSpace(ten.Text)||loai.SelectedValue==null||loai.SelectedValue is DataRowView||string.IsNullOrWhiteSpace(trangthai.Text)){MessageBox.Show("Nhập mã, tên phòng và chọn loại phòng, trạng thái.");return false;}return true;}
        void Add(object? s,EventArgs e){if(Valid())Run(()=>bus.Them(int.Parse(ma.Text),Convert.ToInt32(loai.SelectedValue),ten.Text.Trim(),vitri.Text.Trim(),trangthai.Text),"Thêm phòng");} void Edit(object? s,EventArgs e){if(Valid())Run(()=>bus.Sua(int.Parse(ma.Text),Convert.ToInt32(loai.SelectedValue),ten.Text.Trim(),vitri.Text.Trim(),trangthai.Text),"Sửa phòng");}
        void Delete(object? s,EventArgs e){if(int.TryParse(ma.Text,out int id)&&MessageBox.Show("Xóa phòng này?","Xác nhận",MessageBoxButtons.YesNo)==DialogResult.Yes)Run(()=>bus.Xoa(id),"Xóa phòng");} void Search(object? s,EventArgs e){try{grid.DataSource=bus.TimKiem(tim.Text.Trim());}catch(Exception ex){MessageBox.Show(ex.Message);}}
        void Reset(object? s,EventArgs e){ma.Clear();ten.Clear();vitri.Clear();tim.Clear();loai.SelectedIndex=-1;trangthai.SelectedIndex=0;LoadData();} void Run(Action a,string op){try{a();LoadData();MessageBox.Show(op+" thành công.");Reset(null!,EventArgs.Empty);}catch(Exception ex){MessageBox.Show(op+" thất bại: "+ex.Message);}}
    }
}
