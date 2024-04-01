using FrmMain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjGym
{
    public partial class FrmHomePage : Form
    {
        public tIdentity admin { get; set; }
        public tIdentity member { get; set; }

        public FrmHomePage()
        {
            InitializeComponent();
            splitContainer1.SplitterWidth = 3;
            //OpenLoginForm(); 

        }
        private void closeCurrentForm()
        {
            if (this.ActiveMdiChild != null)
            {
                this.ActiveMdiChild.Close();
            }
        }
        private void OpenLoginForm()
        {
            FrmLogin loginForm = new FrmLogin();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                FrmMain courseForm = new FrmMain();
                courseForm.MdiParent = this;
                courseForm.Show();
            }
            else
            {
                Application.Exit(); // 如果取消登入，關閉應用程式
            }
        } 

        private void 首頁ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            Frm_首頁 mm = new Frm_首頁();
            mm.TopLevel = false;
            mm.FormBorderStyle =FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add( mm );
            mm.Show();
        }

        private void 修改會員資料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeCurrentForm();

            lbl_Info.Visible = false;

            FrmEditMemberRegister editMemberRegister = new FrmEditMemberRegister();

            editMemberRegister.afterEdit += this.showinfo;

            editMemberRegister.StartPosition = FormStartPosition.CenterParent;
            editMemberRegister.MdiParent = this;
            editMemberRegister.Show();
        }

        private void showinfo(tIdentity m)
        {
            this.member = m;
            lbl_Info.Visible = true;
            gymEntities db = new gymEntities();
            tgender_Table g = db.tgender_Table.FirstOrDefault(x => x.gender_id == m.gender_id);
            this.lbl_Info.Text = $"會員名稱: {m.name}\r\n" +
                $"生日: {m.birthday.ToString("yyyy/MM/dd")}\r\n" +
                $"性別: {g.gender_text}\r\n" +
                $"電話: {m.phone}\r\n" +
                $"地址: {m.address}\r\n" +
                $"信箱: {m.e_mail}";
        }

        

        private void 常見問題ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            FrmFAQ fAQ = new FrmFAQ();
            fAQ.TopLevel = false;
            fAQ.FormBorderStyle = FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add(fAQ);
            fAQ.Show();
        }

        private void FrmHomePage_Load_1(object sender, EventArgs e)
        {
            this.Visible = false; 
            MainLog();

            Label lblWelcome = new Label();

            Label label3 = new Label();
            label3.Text = "歡迎 " + this.member.name + " 登入";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            label3.Font = new Font("微軟正黑體", 20, FontStyle.Bold);
            label3.ForeColor = Color.Blue;
            label3.AutoSize = true; // 啟用自動調整大小
            label3.MaximumSize = new Size(this.splitContainer1.Panel2.Width, 0);

            int x = (splitContainer1.Panel2.Width - label3.Width) / 2;
            int y = (splitContainer1.Panel2.Height - label3.Height) / 2;
            label3.Location = new Point(x, y);

            this.splitContainer1.Panel2.Controls.Add(label3);

            this.Text = "歡迎 ~" + this.member.name + " 登入";

        }

        private void MainLog()
        {
            lbl_Info.Text = string.Empty;
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.afterLogin += this.showinfo;
            frmLogin.ShowDialog();
            if (frmLogin.isOK != DialogResult.OK) return;
            this.Visible = true;
        }

        private void 新增管理者ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNewAdminRegister frm = new FrmNewAdminRegister();
            frm.savedata += FrmAdmin_register;
            //將[新增會員資料]表單顯示出來
            frm.ShowDialog();
            //如果[新增會員資料]表單的result屬性為DialogResult.No
            if (frm.result == DialogResult.No)
                //將程式控制權回傳(在此案例中，程式控制權會從[新增會員資料]表單回傳給[登入頁面]表單)
                return;
        }

        private void FrmAdmin_register(FrmNewAdminRegister frm)
        {
            //產生[gym資料庫實體]
            gymEntities db = new gymEntities();
            //產生[Identity]表單物件
            tIdentity admin = new tIdentity();
            //設定新會員資料(名稱、身分ID、性別ID、電話、地址、生日、電郵、密碼、照片檔案名稱)
            int admin_count = db.tIdentity.Count(x => x.role_id.Equals(3)) + 1;
            admin.role_id = 3;
            admin.name = $"Admin {admin_count}";
            admin.phone = frm.phone;
            admin.e_mail = "x";
            admin.password = frm.password;
            admin.photo = "x";
            admin.birthday = DateTime.Now;
            admin.address = "x";
            admin.gender_id = 3;
            //將新會員資料新增至[gym資料庫實體]
            db.tIdentity.Add(admin);
            //存回資料庫
            db.SaveChanges();
            MessageBox.Show("新增管理員完成");
        }
    }
}
