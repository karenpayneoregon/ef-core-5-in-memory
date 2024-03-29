﻿using System;
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
using EntityFrameworkCoreDbExtensions.Classes;
using NorthWindCoreLibrary.Data;
using NorthWindCoreLibrary.Models;
using Customers = NorthWindCoreLibrary.Models.Customers;

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


        private async void OnShown(object sender, EventArgs e)
        {

            _modelsBindingSource.DataSource = await GetModelNamesTask();

            ModelNamesListBox.DataSource = _modelsBindingSource;
            _modelsBindingSource.PositionChanged += ModelsBindingSourceOnPositionChanged;

            var index = ModelNamesListBox.FindString(nameof(Customers));

            if (index > -1)
            {
                ModelNamesListBox.SelectedIndex = index;
            }

            ModelPositionChanged();

        }

        /// <summary>
        /// - Using a Task keeps the form responsive.
        /// - Not always wise, learn from experience
        /// </summary>
        /// <returns></returns>
        public static async Task<List<string>> GetModelNamesTask()
        {
            return await Task.Run(async () =>
            {
                await using var context = new NorthwindContext();
                return context.GetModelNames().OrderBy(x => x).ToList() ;
            });

        }

        private void ModelsBindingSourceOnPositionChanged(object sender, EventArgs e)
        {
            ModelPositionChanged();
        }

        private void ModelPositionChanged()
        {
            if (!_modelsBindingSource.CurrentIsValid()) return;

            using var context = new NorthwindContext();
            ColumnNamesListBox.DataSource = context.ColumnNames(ModelNamesListBox.Text).OrderBy(column => column).ToList();
        }

    }
}
