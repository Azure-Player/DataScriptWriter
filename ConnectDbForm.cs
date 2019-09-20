using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Security.Cryptography;
//using CAMOsoft.Common;
using System.Xml;

public partial class ConnectDbForm : DevExpress.XtraEditors.XtraForm
{

    private CAMOsoft.DbUtils.MsSqlSession _db = null;
    private bool _dblistLoaded = false;

    public ConnectDbForm(string AppName, string AreaName)
    {
        InitializeComponent();
        lblAreaName.Text = AppName + " " + AreaName;
    }



    private void ReloadDatabaseList()
    {
        if (_dblistLoaded) return;
        if (ConnectToDb("master"))
        {
            DataTable dblist = _db.SelectTable("SELECT [name] FROM sys.databases ORDER BY [name]", "databaseList");
            cmbDbName.Properties.Items.Clear();
            foreach (DataRow dr in dblist.Rows)
            {
                cmbDbName.Properties.Items.Add(dr[0].ToString());
            }
            _dblistLoaded = true;
        }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        this.Cursor = Cursors.WaitCursor;
        this.Enabled = false;
        ConnectToDb(cmbDbName.Text);
        this.Enabled = true;
        this.Cursor = Cursors.Default;
        if (_db.IsLogged)
        {
            DialogResult = DialogResult.OK;
        }
        else
        {
            MessageBox.Show(_db.LoginException.Message, "Connect to Server", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private bool ConnectToDb(string dbname)
    {
        if (cbeAuth.SelectedIndex == 0)
        {
            _db = new CAMOsoft.DbUtils.MsSqlSession(true);
            _db.LoginWindowsAuth(txtServer.Text, cmbDbName.Text);
        }
        else
        {
            _db = new CAMOsoft.DbUtils.MsSqlSession(true);
            _db.LoginSQLAuth(txtServer.Text, txtUser.Text, txtPass.Text, cmbDbName.Text);
        }
        return _db.IsLogged;
    }

    private void cbeAuth_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool WinAuth = cbeAuth.SelectedIndex == 0;
        txtUser.Enabled = !WinAuth;
        txtPass.Enabled = !WinAuth;
        lblUser.Enabled = !WinAuth;
        lblPass.Enabled = !WinAuth;
    }

    private void ConnectDbForm_Load(object sender, EventArgs e)
    {
        cbeAuth.SelectedIndex = 0;
    }

    public string AreaName
    {
        get { return lblAreaName.Text; }
    }

    public CAMOsoft.DbUtils.MsSqlSession Db
    {
        get
        {
            return _db;
        }

    }

    private void txtServer_EditValueChanged(object sender, EventArgs e)
    {
        _dblistLoaded = false;
    }

    private void cmbDbName_Enter(object sender, EventArgs e)
    {
        ReloadDatabaseList();
    }
}
