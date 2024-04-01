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

        private void closeCurrentForm()
        {
            if (this.ActiveMdiChild != null)
            {
                this.ActiveMdiChild.Close();
            }
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
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.ShowDialog();
            frmLogin.afterLogin += this.showinfo;
            if (frmLogin.isOK != DialogResult.OK) return;
            this.Visible = true;
            lbl_Info.Text = string.Empty;
        }
    }
}
