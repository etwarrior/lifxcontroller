using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LIFXController
{
    public partial class newEventForm : Form
    {
        public object ReturnEvent { get; set; }
        public newEventForm()
        {
            InitializeComponent();
        }

        private void newEventOkButton_Click(object sender, EventArgs e)
        {
            this.ReturnEvent = new LifxEvent();
        }
    }
}
