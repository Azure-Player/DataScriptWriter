using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using Ionic.Zip;

namespace DataScriptWriter
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        ScriptWriter _gen = null;
        string _OutputFolder = "";

        public frmMain()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName name = new AssemblyName(assembly.FullName);
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Global.AppName = String.Format("{0} - ver. {1}", fileVersionInfo.ProductName, fileVersionInfo.ProductVersion);
            this.Text = Global.AppName;

            string strTempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(strTempPath);

            _OutputFolder = strTempPath;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void Connect()
        {
            ConnectDbForm f = new ConnectDbForm("SQLPlayer", "Data Script Writer");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                _gen = new ScriptWriter(f.Db, _OutputFolder);
                _gen.OptionProcWrapUp = true;
                _gen.LoadListOfTables();
                gridControl1.DataSource = _gen.TableList;
                bsiDatabaseName.Caption = _gen.DbName;
            }
        }

        private void bbiExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Directory.Delete(_OutputFolder, true);
            }
            catch { }
            this.Close();
        }

        private void bbiConnect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Connect();
        }

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            int AllCount = _gen.TableList.Rows.Count;
            int SelectedCount = _gen.SelectedItemView.Count;
            bsiCount.Caption = String.Format("{0}/{1}", SelectedCount, AllCount);
        }

        private void bbiScript_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int cnt = 0;
            if (_gen == null || _gen.SelectedItemView == null)
            {
                barStaticItem1.Caption = "Incomplete connection to script";
            }
            else
            {
                foreach (DataRowView item in _gen.SelectedItemView)
                {
                    ScriptObject so = new ScriptObject(item);
                    barStaticItem1.Caption = "Scripting: " + so.FullQuoted;
                    this.Refresh();
                    _gen.GenerateForTable(so);
                    cnt++;
                }
                barStaticItem1.Caption = String.Format("Done. {0} tables were scripted.", cnt);

                string[] arrSqlFiles = Directory.GetFiles(_OutputFolder, "*.sql");
                string strZipPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DataScriptWriter");
                Directory.CreateDirectory(strZipPath);

                ZipFile zipFile = new ZipFile(_OutputFolder);
                zipFile.AddFiles(arrSqlFiles, "/");
                zipFile.Save(Path.Combine(strZipPath,  string.Format("Export_{0}" + ".zip",DateTime.Now.ToString("yyyyMMdd HHmmss"))));
                zipFile = null;
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
        }

        private void bbiCopyDb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }
    }
}
