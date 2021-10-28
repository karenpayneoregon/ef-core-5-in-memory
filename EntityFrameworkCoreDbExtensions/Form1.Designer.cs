
namespace EntityFrameworkCoreDbExtensions
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
            this.ModelNamesListBox = new System.Windows.Forms.ListBox();
            this.ColumnNamesListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ModelNamesListBox
            // 
            this.ModelNamesListBox.FormattingEnabled = true;
            this.ModelNamesListBox.ItemHeight = 15;
            this.ModelNamesListBox.Location = new System.Drawing.Point(12, 25);
            this.ModelNamesListBox.Name = "ModelNamesListBox";
            this.ModelNamesListBox.Size = new System.Drawing.Size(204, 259);
            this.ModelNamesListBox.TabIndex = 0;
            // 
            // ColumnNamesListBox
            // 
            this.ColumnNamesListBox.FormattingEnabled = true;
            this.ColumnNamesListBox.ItemHeight = 15;
            this.ColumnNamesListBox.Location = new System.Drawing.Point(235, 25);
            this.ColumnNamesListBox.Name = "ColumnNamesListBox";
            this.ColumnNamesListBox.Size = new System.Drawing.Size(204, 259);
            this.ColumnNamesListBox.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 297);
            this.Controls.Add(this.ColumnNamesListBox);
            this.Controls.Add(this.ModelNamesListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EF Core DbContext extensions";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ModelNamesListBox;
        private System.Windows.Forms.ListBox ColumnNamesListBox;
    }
}

