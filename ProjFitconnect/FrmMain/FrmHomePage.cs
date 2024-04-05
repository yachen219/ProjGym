using FrmMain;
using mid_Coonect;
using Revised_V1._1;
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
        public tIdentity identity { get; set; }
        Label lblWelcome;

        public FrmHomePage()
        {
            InitializeComponent();
            splitContainer1.SplitterWidth = 3;
            this.找商品ToolStripMenuItem.Visible = false;
            this.lbl_Info.Visible = false;
        }
        private void closeCurrentForm()
        {
            if (this.ActiveMdiChild != null)
            {
                this.ActiveMdiChild.Close();
            }
        }

        private void 首頁ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showmain();
        }

        private void 修改會員資料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeCurrentForm();
            this.splitContainer1.Panel2.Controls.Clear();
            FrmEditMemberRegister editMemberRegister = new FrmEditMemberRegister();
            editMemberRegister.identity = this.identity;
            editMemberRegister.afterEdit += this.showinfo;
            editMemberRegister.TopLevel = false;
            editMemberRegister.FormBorderStyle = FormBorderStyle.None;

            editMemberRegister.Visible = true;
            editMemberRegister.Dock = DockStyle.Fill;
            editMemberRegister.StartPosition = FormStartPosition.CenterParent;
            editMemberRegister.MdiParent = this;
            this.splitContainer1.Panel2.Controls.Add(editMemberRegister);
            editMemberRegister.Show();
        }

        private void showinfo(tIdentity m)
        {
            this.identity = m;

            if (m.role_id == 1)
                info_member(m);
            if (m.role_id == 2)
                info_coach(m);
            if (m.role_id == 3)
                info_admin(m);
        }

        private void info_member(tIdentity m)
        {
            this.會員中心ToolStripMenuItem.Visible = true;
            this.教練中心ToolStripMenuItem.Visible = false;
            this.管理者中心ToolStripMenuItem.Visible = false;
            lblWecomeshow(m);
            this.Text = "歡迎登入 ： " + m.name;
        }

        private void lblWecomeshow(tIdentity m)
        {
            Label lblWelcome = new Label();
            string s = "歡迎登入 ： " + m.name;
            if (m.role_id == 1)
                s += "\n您的身份是：會員";
            else if (m.role_id == 2)
                s += "\n您的身份是：教練";
            else
                s += "\n您的身份是：管理者";
            lblWelcome.Text = s;
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            lblWelcome.Font = new Font("微軟正黑體", 20, FontStyle.Bold);
            lblWelcome.ForeColor = Color.Blue;
            lblWelcome.AutoSize = true;
            lblWelcome.MaximumSize = new Size(this.splitContainer1.Panel2.Width, 0);

            int x = (splitContainer1.Panel2.Width - lblWelcome.Width) / 2;
            int y = (splitContainer1.Panel2.Height - lblWelcome.Height) / 2;
            lblWelcome.Location = new Point(x, y);

            this.splitContainer1.Panel2.Controls.Add(lblWelcome);
        }

        private void info_coach(tIdentity m)
        {
            this.會員中心ToolStripMenuItem.Visible = false;
            this.教練中心ToolStripMenuItem.Visible = true;
            this.管理者中心ToolStripMenuItem.Visible = false;

            lblWecomeshow(m);
            this.Text = "歡迎登入 ： " + m.name;
        }

        private void info_admin(tIdentity m)
        {
            this.會員中心ToolStripMenuItem.Visible = false;
            this.教練中心ToolStripMenuItem.Visible = false;
            this.管理者中心ToolStripMenuItem.Visible = true;

            lblWecomeshow(m);
            this.Text = "歡迎登入 ： " + m.name;
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
            showinfo(this.identity);
        }

        private void MainLog()
        {
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
            frm.ShowDialog();
            if (frm.result == DialogResult.No)
                return;
        }

        private void FrmAdmin_register(FrmNewAdminRegister frm)
        {
            gymEntities db = new gymEntities();
            tIdentity admin = new tIdentity();

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
            db.tIdentity.Add(admin);
            db.SaveChanges();

            MessageBox.Show("新增管理員完成");
        }

        private void 會員登出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logoutevent();
        }

        private void 帳號登出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logoutevent();
        }

        private void logoutevent()
        {
            DialogResult Logout = MessageBox.Show("確定要登出？", "", MessageBoxButtons.OKCancel);
            if (Logout != DialogResult.OK)
                return;
            this.identity = null;
            this.lblWelcome = null;
            showmain();
            MainLog();
        }

        private void showmain()
        {
            this.splitContainer1.Panel2.Controls.Clear();
            Frm_首頁 mm = new Frm_首頁();
            mm.TopLevel = false;
            mm.FormBorderStyle = FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add(mm);
            mm.Show();
        }

        private void 已預約課程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            FrmClassReserved f = new FrmClassReserved();
            f.MdiParent = this;
            f.identity = this.identity;
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add(f);
            f.Show();
        }

        private void 找教練ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            FrmFindCoach f = new FrmFindCoach();
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add(f);
            f.Show();
        }

        private void yOUTUBEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void 預約體驗ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new Frmfreetrial()).Show();
        }

        private void 新增課程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            FrmCoach_AddNewCourse f = new FrmCoach_AddNewCourse();
            f.MdiParent = this;
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add(f);
            f.Show();
        }

        private void 開課審核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            FrmAdmin_ClassUpdate f = new FrmAdmin_ClassUpdate();
            f.MdiParent = this;
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add(f);
            f.Show();
        }
        private void 訓練課程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            FrmMember_ClassList f = new FrmMember_ClassList();
            f.MdiParent = this;
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add(f);
            f.Show();
        }

        private void 預約場地ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            FrmOpenedClassCheck f = new FrmOpenedClassCheck();
            f.MdiParent = this;
            f.Identity = this.identity;
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add(f);
            f.Show();
        }
        private void Identity_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            lblWecomeshow(this.identity);
        }

        private void 教練審核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            FrmCoachVertify f = new FrmCoachVertify();
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add(f);
            f.Show();
        }

        private void 修改教練資料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            FrmEditCoachRegister f = new FrmEditCoachRegister();
            f.TopLevel = false;
            f.identity = this.identity;
            f.FormBorderStyle = FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add(f);
            f.Show();
        }

        private void 登出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logoutevent();
        }

        private void 會員資訊ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            FrmAdmin_Checkmember f = new FrmAdmin_Checkmember();
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            this.splitContainer1.Panel2.Controls.Add(f);
            f.Show();
        }

        private void 登出ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            logoutevent();
        }

        private void 登入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainLog();
            showinfo(this.identity);
        }
    }
}
