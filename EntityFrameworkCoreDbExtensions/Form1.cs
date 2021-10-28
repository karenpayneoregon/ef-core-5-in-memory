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
using EntityCoreExtensions;
using NorthWindCoreLibrary.Data;
using NorthWindCoreLibrary.Models;

namespace EntityFrameworkCoreDbExtensions
{
    public partial class Form1 : Form
    {
        private readonly BindingSource _modelsBindingSource = new ();
        public Form1()
        {
            InitializeComponent();
            Shown += OnShown;
        }

        private void OnShown(object? sender, EventArgs e)
        {
            using var context = new NorthwindContext();

            _modelsBindingSource.DataSource = context.GetModelNames().OrderBy(x => x).ToList();
            ModelNamesListBox.DataSource = _modelsBindingSource;
            _modelsBindingSource.PositionChanged += ModelsBindingSourceOnPositionChanged;

            var index = ModelNamesListBox.FindString(nameof(Customers));

            if (index > -1)
            {
                ModelNamesListBox.SelectedIndex = index;
            }

            ModelPositionChanged();

        }

        private void ModelsBindingSourceOnPositionChanged(object sender, EventArgs e)
        {
            ModelPositionChanged();
        }

        private void ModelPositionChanged()
        {
            if (_modelsBindingSource.CurrentIsValid())
            {
                using var context = new NorthwindContext();
                ColumnNamesListBox.DataSource = context.ColumnNames(ModelNamesListBox.Text).OrderBy(column => column).ToList();
            }
        }
    }
}
