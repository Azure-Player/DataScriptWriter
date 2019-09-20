namespace DataScriptWriter
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.isSelected = new DevExpress.XtraGrid.Columns.GridColumn();
            this.id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcSchema = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcTable = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMethod = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gcRows = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.barStaticItem4 = new DevExpress.XtraBars.BarStaticItem();
            this.bsiCount = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.bsiDatabaseName = new DevExpress.XtraBars.BarStaticItem();
            this.progress = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemProgressBar1 = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.bbiConnect = new DevExpress.XtraBars.BarButtonItem();
            this.bbiExit = new DevExpress.XtraBars.BarButtonItem();
            this.bbiScript = new DevExpress.XtraBars.BarButtonItem();
            this.bbiSettings = new DevExpress.XtraBars.BarButtonItem();
            this.bbiCopyDb = new DevExpress.XtraBars.BarButtonItem();
            this.rpHome = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Location = new System.Drawing.Point(0, 119);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1});
            this.gridControl1.Size = new System.Drawing.Size(928, 474);
            this.gridControl1.TabIndex = 18;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.isSelected,
            this.id,
            this.gcSchema,
            this.gcTable,
            this.gcMethod,
            this.gcRows});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways;
            this.gridView1.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.gridView1_RowUpdated);
            // 
            // isSelected
            // 
            this.isSelected.Caption = "S";
            this.isSelected.FieldName = "isSelected";
            this.isSelected.Name = "isSelected";
            this.isSelected.Visible = true;
            this.isSelected.VisibleIndex = 0;
            this.isSelected.Width = 68;
            // 
            // id
            // 
            this.id.Caption = "id";
            this.id.FieldName = "id";
            this.id.Name = "id";
            this.id.OptionsColumn.AllowEdit = false;
            // 
            // gcSchema
            // 
            this.gcSchema.Caption = "Schema Name";
            this.gcSchema.FieldName = "schema";
            this.gcSchema.Name = "gcSchema";
            this.gcSchema.OptionsColumn.AllowEdit = false;
            this.gcSchema.Visible = true;
            this.gcSchema.VisibleIndex = 1;
            this.gcSchema.Width = 274;
            // 
            // gcTable
            // 
            this.gcTable.Caption = "Table Name";
            this.gcTable.FieldName = "table";
            this.gcTable.Name = "gcTable";
            this.gcTable.OptionsColumn.AllowEdit = false;
            this.gcTable.Visible = true;
            this.gcTable.VisibleIndex = 2;
            this.gcTable.Width = 274;
            // 
            // gcMethod
            // 
            this.gcMethod.Caption = "Method";
            this.gcMethod.ColumnEdit = this.repositoryItemComboBox1;
            this.gcMethod.FieldName = "method";
            this.gcMethod.Name = "gcMethod";
            this.gcMethod.Visible = true;
            this.gcMethod.VisibleIndex = 3;
            this.gcMethod.Width = 277;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "INSERT",
            "MERGE",
            "MERGE_without_DELETE",
            "MERGE_NEW_ONLY"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // gcRows
            // 
            this.gcRows.Caption = "# rows";
            this.gcRows.DisplayFormat.FormatString = "# ### ### ##0";
            this.gcRows.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gcRows.FieldName = "rowcount";
            this.gcRows.Name = "gcRows";
            this.gcRows.OptionsColumn.AllowEdit = false;
            this.gcRows.Visible = true;
            this.gcRows.VisibleIndex = 4;
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.ItemLinks.Add(this.barStaticItem4);
            this.ribbonStatusBar1.ItemLinks.Add(this.bsiCount);
            this.ribbonStatusBar1.ItemLinks.Add(this.barStaticItem1);
            this.ribbonStatusBar1.ItemLinks.Add(this.bsiDatabaseName);
            this.ribbonStatusBar1.ItemLinks.Add(this.progress);
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 594);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(928, 27);
            // 
            // barStaticItem4
            // 
            this.barStaticItem4.Caption = "Selected/All:";
            this.barStaticItem4.Glyph = ((System.Drawing.Image)(resources.GetObject("barStaticItem4.Glyph")));
            this.barStaticItem4.Id = 8;
            this.barStaticItem4.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barStaticItem4.LargeGlyph")));
            this.barStaticItem4.Name = "barStaticItem4";
            this.barStaticItem4.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // bsiCount
            // 
            this.bsiCount.Caption = "0/0";
            this.bsiCount.Id = 9;
            this.bsiCount.Name = "bsiCount";
            this.bsiCount.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "Wait";
            this.barStaticItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barStaticItem1.Glyph")));
            this.barStaticItem1.Id = 1;
            this.barStaticItem1.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barStaticItem1.LargeGlyph")));
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // bsiDatabaseName
            // 
            this.bsiDatabaseName.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.bsiDatabaseName.Caption = "(database)";
            this.bsiDatabaseName.Glyph = ((System.Drawing.Image)(resources.GetObject("bsiDatabaseName.Glyph")));
            this.bsiDatabaseName.Id = 2;
            this.bsiDatabaseName.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("bsiDatabaseName.LargeGlyph")));
            this.bsiDatabaseName.Name = "bsiDatabaseName";
            this.bsiDatabaseName.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // progress
            // 
            this.progress.Caption = "Progress:";
            this.progress.Edit = this.repositoryItemProgressBar1;
            this.progress.Id = 3;
            this.progress.Name = "progress";
            this.progress.Width = 100;
            // 
            // repositoryItemProgressBar1
            // 
            this.repositoryItemProgressBar1.Name = "repositoryItemProgressBar1";
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.AllowMdiChildButtons = false;
            this.ribbonControl1.ApplicationButtonText = "ABC";
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.bbiConnect,
            this.bbiExit,
            this.barStaticItem4,
            this.bsiCount,
            this.bbiScript,
            this.barStaticItem1,
            this.bsiDatabaseName,
            this.bbiSettings,
            this.bbiCopyDb,
            this.progress});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 4;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rpHome});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemProgressBar1});
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(928, 121);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
            // 
            // bbiConnect
            // 
            this.bbiConnect.Caption = "Connect";
            this.bbiConnect.Id = 5;
            this.bbiConnect.LargeGlyph = global::DataScriptWriter.Properties.Resources.if_connect_established_1721;
            this.bbiConnect.Name = "bbiConnect";
            this.bbiConnect.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.bbiConnect.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiConnect_ItemClick);
            // 
            // bbiExit
            // 
            this.bbiExit.Caption = "Close app";
            this.bbiExit.Glyph = global::DataScriptWriter.Properties.Resources.if_exit_6035;
            this.bbiExit.Id = 6;
            this.bbiExit.Name = "bbiExit";
            this.bbiExit.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.bbiExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiExit_ItemClick);
            // 
            // bbiScript
            // 
            this.bbiScript.Caption = "Generate Script";
            this.bbiScript.Id = 10;
            this.bbiScript.LargeGlyph = global::DataScriptWriter.Properties.Resources.if_script_lightning_36406;
            this.bbiScript.Name = "bbiScript";
            this.bbiScript.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiScript_ItemClick);
            // 
            // bbiSettings
            // 
            this.bbiSettings.Caption = "Settings";
            this.bbiSettings.Glyph = ((System.Drawing.Image)(resources.GetObject("bbiSettings.Glyph")));
            this.bbiSettings.Id = 1;
            this.bbiSettings.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("bbiSettings.LargeGlyph")));
            this.bbiSettings.Name = "bbiSettings";
            this.bbiSettings.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // bbiCopyDb
            // 
            this.bbiCopyDb.Caption = "Copy database";
            this.bbiCopyDb.Glyph = ((System.Drawing.Image)(resources.GetObject("bbiCopyDb.Glyph")));
            this.bbiCopyDb.Id = 2;
            this.bbiCopyDb.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("bbiCopyDb.LargeGlyph")));
            this.bbiCopyDb.Name = "bbiCopyDb";
            this.bbiCopyDb.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.bbiCopyDb.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiCopyDb_ItemClick);
            // 
            // rpHome
            // 
            this.rpHome.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.rpHome.Name = "rpHome";
            this.rpHome.Text = "Home";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiConnect);
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiScript);
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiSettings);
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiCopyDb);
            this.ribbonPageGroup1.ItemLinks.Add(this.bbiExit);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Operation";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Id = -1;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Id = -1;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = "Selected";
            this.barStaticItem3.Id = -1;
            this.barStaticItem3.Name = "barStaticItem3";
            this.barStaticItem3.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.bbiConnect);
            this.ribbonPageGroup2.ItemLinks.Add(this.bbiScript);
            this.ribbonPageGroup2.ItemLinks.Add(this.bbiExit);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Operation";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 621);
            this.Controls.Add(this.ribbonControl1);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.gridControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(724, 400);
            this.Name = "frmMain";
            this.Text = "SQLPlayer Data Script Writer";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gcSchema;
        private DevExpress.XtraGrid.Columns.GridColumn gcTable;
        private DevExpress.XtraGrid.Columns.GridColumn gcMethod;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraGrid.Columns.GridColumn isSelected;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage rpHome;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem bbiConnect;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarStaticItem barStaticItem3;
        private DevExpress.XtraBars.BarButtonItem bbiExit;
        private DevExpress.XtraBars.BarStaticItem barStaticItem4;
        private DevExpress.XtraBars.BarStaticItem bsiCount;
        private DevExpress.XtraBars.BarButtonItem bbiScript;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraGrid.Columns.GridColumn gcRows;
        private DevExpress.XtraGrid.Columns.GridColumn id;
        private DevExpress.XtraBars.BarStaticItem bsiDatabaseName;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem bbiSettings;
        private DevExpress.XtraBars.BarButtonItem bbiCopyDb;
        private DevExpress.XtraBars.BarEditItem progress;
        private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar repositoryItemProgressBar1;
    }
}

