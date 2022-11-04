using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NorthWindCoreLibrary.Classes;
using NorthWindCoreLibrary.Models;
using StackWinFormsApp.Classes;

namespace StackWinFormsApp
{
    public partial class Form1 : Form
    {
        private SortableBindingList<DataContainer> _dataContainers;
        private BindingSource _source = new BindingSource();
        public Form1()
        {
            InitializeComponent();
            Shown += OnShown;
        }

        private void OnShown(object? sender, EventArgs e)
        {
            _dataContainers = new SortableBindingList<DataContainer>(StackoverflowOperations.ReadData());
            _source.DataSource = _dataContainers;
            dataGridView1.DataSource = _source;
            dataGridView1.Columns["OrderId"].Visible = false;
            dataGridView1.ExpandColumns();
            coreBindingNavigator1.BindingSource = _source;
        }
        private void SortButton_Click(object sender, EventArgs e)
        {
            _source.Sort = "CategoryName DESC";
        }
    }
}
