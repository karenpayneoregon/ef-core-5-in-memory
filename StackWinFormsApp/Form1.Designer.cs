namespace StackWinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SortButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.coreBindingNavigator1 = new StackWinFormsApp.Classes.CoreBindingNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.coreBindingNavigator1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new System.Drawing.Size(800, 363);
            this.dataGridView1.TabIndex = 0;
            // 
            // SortButton
            // 
            this.SortButton.Location = new System.Drawing.Point(12, 19);
            this.SortButton.Name = "SortButton";
            this.SortButton.Size = new System.Drawing.Size(94, 29);
            this.SortButton.TabIndex = 1;
            this.SortButton.Text = "Sort";
            this.SortButton.UseVisualStyleBackColor = true;
            this.SortButton.Click += new System.EventHandler(this.SortButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SortButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 390);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 60);
            this.panel1.TabIndex = 2;
            // 
            // coreBindingNavigator1
            // 
            this.coreBindingNavigator1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.coreBindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.coreBindingNavigator1.Name = "coreBindingNavigator1";
            this.coreBindingNavigator1.Size = new System.Drawing.Size(800, 27);
            this.coreBindingNavigator1.TabIndex = 3;
            this.coreBindingNavigator1.Text = "coreBindingNavigator1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.coreBindingNavigator1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code sample";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.coreBindingNavigator1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button SortButton;
        private System.Windows.Forms.Panel panel1;
        private Classes.CoreBindingNavigator coreBindingNavigator1;
    }
}
