using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataScriptWriter
{
    public partial class frmNoteForm : Form
    {
        public frmNoteForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Hide();
        }

        public string[] Lines { 
            get
            {
                return this.textBox1.Lines;
            }
        }
    }
}
