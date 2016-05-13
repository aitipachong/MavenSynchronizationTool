using MavenSynchronizationTool.BLL;
using MavenSynchronizationTool.Core;
using MavenSynchronizationTool.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MavenSynchronizationTool
{
    public partial class FrmMain : Form
    {
        private CentralRepository SelectedCentralRepository { get; set; } = null;
        private bool IsLoadedCentralRepositoryIndexs { get; set; } = false;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            InitFrmControls();
            LoadingTreeNodes();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        /// <summary>
        /// 初始化窗体控件
        /// </summary>
        private void InitFrmControls()
        {
            int frmWidth = this.Width;
            //splitContainerControl1初始化位置
            this.splitContainerControl1.SplitterPosition = Convert.ToInt32(frmWidth / 4);
            //初始化toolStripStatusLabel1长度及内容
            this.toolStripStatusLabel1.Width = Convert.ToInt32(frmWidth / 4 * 3);
            this.toolStripStatusLabel1.Text = "系统初始化完毕！";
            //隐藏toolStripProgressBar1
            this.toolStripProgressBar1.Visible = false;

            扫描仓库索引ToolStripMenuItem.Enabled = false;
            toolStripButton2.Enabled = false;


            //初始化TreeView
            this.treeView1.Nodes.Clear();
            //初始化ListView，并且设置列头
            this.listView1.Columns.Clear();
            this.listView1.Items.Clear();
            this.listView1.CheckBoxes = true;
            this.listView1.GridLines = true;
            this.listView1.MultiSelect = true;
            this.listView1.FullRowSelect = true;
            this.listView1.View = View.Details;

            ColumnHeader checkBoxCH = new ColumnHeader();
            checkBoxCH.Text = "";
            checkBoxCH.Width = 60;
            checkBoxCH.TextAlign = HorizontalAlignment.Center;
            this.listView1.Columns.Add(checkBoxCH);
            ColumnHeader indexNameCH = new ColumnHeader();
            indexNameCH.Text = "索引";
            indexNameCH.Width = 300;
            indexNameCH.TextAlign = HorizontalAlignment.Left;
            this.listView1.Columns.Add(indexNameCH);
            ColumnHeader synchStateCH = new ColumnHeader();
            synchStateCH.Text = "同步状态";
            synchStateCH.Width = 150;
            synchStateCH.TextAlign = HorizontalAlignment.Center;
            this.listView1.Columns.Add(synchStateCH);
        }

        #region 委托

        #endregion

        #region 装载TreeView

        /// <summary>
        /// 加载“CentralRepository”表内容到TreeView控件，形成Maven中央仓库树
        ///     Maven中央仓库【该节点不变】
        ///          |_______repo1 [https://repo1.maven.org/maven2]
        ///          |_______repo2 [https://repo2.maven.org/maven2]
        /// </summary>
        private void LoadingTreeNodes()
        {
            //初始化TreeView
            this.treeView1.Nodes.Clear();
            TreeNode repositoryNode = null;
            //增加root
            TreeNode root = new TreeNode();
            root.Text = "Maven中央仓库";
            root.ImageIndex = 0;
            root.Tag = null;

            //增加Maven中央库节点
            if(Program.CentralRepositoryList != null && Program.CentralRepositoryList.Count > 0)
            {
                for(int loopi = 0;loopi < Program.CentralRepositoryList.Count;loopi++)
                {
                    repositoryNode = new TreeNode();
                    repositoryNode.ImageIndex = 1;
                    repositoryNode.Text = Program.CentralRepositoryList[loopi].CentralName + " [" 
                        + Program.CentralRepositoryList[loopi].CentralUrl.Trim() + "]";
                    repositoryNode.ForeColor = Color.Red;
                    repositoryNode.Tag = Program.CentralRepositoryList[loopi];

                    root.Nodes.Add(repositoryNode);
                }
            }

            this.treeView1.Nodes.Add(root);
            this.treeView1.ExpandAll();
            this.treeView1.SelectedNode = root;
        }

        private void InsertViewNode(CentralRepository centralRepository)
        {
            TreeNode repositoryNode = null;
            if (this.treeView1.Nodes == null || this.treeView1.Nodes.Count == 0) return;
            TreeNode root = this.treeView1.Nodes[0];
            this.treeView1.BeginUpdate();
            repositoryNode = new TreeNode();
            repositoryNode.ImageIndex = 1;
            repositoryNode.Text = centralRepository.CentralName + " [" + centralRepository.CentralUrl.Trim() + "]";
            repositoryNode.ForeColor = Color.Red;
            repositoryNode.Tag = centralRepository;
            root.Nodes.Add(repositoryNode);
            this.treeView1.EndUpdate();
            this.treeView1.ExpandAll();
            this.treeView1.SelectedNode = root;
        }
        #endregion

        #region 装载ListView

        private void LoadingListView(List<CentralRepositoryIndex> centralRepositoryIndexList)
        {
            if (centralRepositoryIndexList == null || centralRepositoryIndexList.Count == 0) return;
            this.listView1.Items.Clear();

            this.listView1.BeginUpdate();
            for(int loopi = 0;loopi < centralRepositoryIndexList.Count;loopi++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.UseItemStyleForSubItems = false;
                lvi.Checked = false;
                lvi.ImageIndex = 2;
                lvi.Text = "";
                lvi.Tag = centralRepositoryIndexList[loopi];
                lvi.SubItems.Add(centralRepositoryIndexList[loopi].IndexName.Trim());
                ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
                switch (centralRepositoryIndexList[loopi].SynchState)
                {
                    case 0:
                        lvsi.Text = "未同步";
                        lvsi.ForeColor = Color.Black;
                        lvi.SubItems.Add(lvsi);
                        break;
                    case 1:
                        lvsi.Text = "同步进行中";
                        lvsi.ForeColor = Color.Blue;
                        lvi.SubItems.Add(lvsi);
                        break;
                    case 2:
                        lvsi.Text = "同步中断";
                        lvsi.ForeColor = Color.Red;
                        lvi.SubItems.Add(lvsi);
                        break;
                    default:
                        lvsi.Text = "同步完成";
                        lvsi.ForeColor = Color.Gray;
                        lvi.SubItems.Add(lvsi);
                        break;
                }

                this.listView1.Items.Add(lvi);
            }

            this.IsLoadedCentralRepositoryIndexs = true;
            this.listView1.EndUpdate();
        }

        #endregion

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.IsLoadedCentralRepositoryIndexs = false;
            if (e.Node.Tag == null)
            {
                this.toolStripStatusLabel1.Text = "";
                this.SelectedCentralRepository = null;
                this.listView1.Items.Clear();
                扫描仓库索引ToolStripMenuItem.Enabled = false;
                toolStripButton2.Enabled = false;
                return;
            }

            e.Node.SelectedImageIndex = 1;
            this.SelectedCentralRepository = e.Node.Tag as CentralRepository;
            this.toolStripStatusLabel1.Text = "TreeView selected：“" + this.SelectedCentralRepository.CentralName + " [" +
                this.SelectedCentralRepository.CentralUrl.Trim() + "]”";
            //装载中央库索引
            LoadingListView(this.SelectedCentralRepository.IndexList);
            if(IsLoadedCentralRepositoryIndexs)
            {
                扫描仓库索引ToolStripMenuItem.Enabled = false;
                toolStripButton2.Enabled = false;
            }
            else
            {
                扫描仓库索引ToolStripMenuItem.Enabled = true;
                toolStripButton2.Enabled = true;
            }
        }

        #region Maven中央库设置菜单&工具栏按钮

        private void maven中央库设置SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAndSetMavenCentralRepositorySetting();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ShowAndSetMavenCentralRepositorySetting();
        }

        private void ShowAndSetMavenCentralRepositorySetting()
        {
            FrmMavenCentralRepositorySetting frmRepositorySetting = new FrmMavenCentralRepositorySetting();
            if(frmRepositorySetting.ShowDialog(this) == DialogResult.OK && frmRepositorySetting.Repository != null)
            {
                //Maven中央库内容存储入SQLite库，且返回主键
                SQLiteFactory factory = new SQLiteFactory();
                try
                {
                    if (!factory.InsertCentralRespository(frmRepositorySetting.Repository))
                    {
                        LogHelper.WriteLog("Maven中央库配置入SQLite库失败!");
                        return;
                    }
                    Program.CentralRepositoryList.Add(frmRepositorySetting.Repository);

                    //新增Maven中央库树
                    InsertViewNode(frmRepositorySetting.Repository);
                }
                catch(Exception ex)
                {
                    LogHelper.WriteLog(ex.Message, ex);
                    this.toolStripStatusLabel1.Text = string.Format("Maven中央库配置异常,异常信息：{0}", ex.Message);
                }
            }
        }

        #endregion

        #region 扫描仓库索引菜单&工具栏按钮
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ScanRepositoryIndexs();
        }

        private void 扫描仓库索引ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScanRepositoryIndexs();
        }

        private void ScanRepositoryIndexs()
        {
            AnalysisMavenCentralHtmlProcess process = new AnalysisMavenCentralHtmlProcess();
            try
            {
                //扫描
                process.ScanCentralRepositoryIndexs(this.SelectedCentralRepository);
                //入库

                //刷新ListView

            }
            catch(Exception ex)
            {
                LogHelper.WriteLog("扫描索引异常", ex);
                this.toolStripStatusLabel1.Text = "扫描索引异常!!!";
            }
        }

        #endregion
    }
}