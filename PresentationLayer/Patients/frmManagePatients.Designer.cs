namespace Presentation_Tier.Component
{
    partial class frmManagePatients
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManagePatients));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            dgvTable = new Guna.UI2.WinForms.Guna2DataGridView();
            guna2ContextMenuStrip1 = new Guna.UI2.WinForms.Guna2ContextMenuStrip();
            showDetailsToolStripMenuItem1 = new ToolStripMenuItem();
            addToolStripMenuItem = new ToolStripMenuItem();
            updateToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            label2 = new Label();
            lbRecords = new Label();
            btnAddNew = new PictureBox();
            pictureBox1 = new PictureBox();
            txtFilter = new Guna.UI2.WinForms.Guna2TextBox();
            cbFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvTable).BeginInit();
            guna2ContextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnAddNew).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 30F, FontStyle.Bold);
            label1.ForeColor = Color.Teal;
            label1.Location = new Point(609, 31);
            label1.Name = "label1";
            label1.Size = new Size(214, 58);
            label1.TabIndex = 0;
            label1.Text = "Patients";
            // 
            // dgvTable
            // 
            dgvTable.AllowUserToAddRows = false;
            dgvTable.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvTable.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Tahoma", 8F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvTable.ColumnHeadersHeight = 25;
            dgvTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvTable.ContextMenuStrip = guna2ContextMenuStrip1;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvTable.DefaultCellStyle = dataGridViewCellStyle3;
            dgvTable.GridColor = Color.FromArgb(231, 229, 255);
            dgvTable.Location = new Point(47, 414);
            dgvTable.Margin = new Padding(3, 4, 3, 4);
            dgvTable.Name = "dgvTable";
            dgvTable.ReadOnly = true;
            dgvTable.RowHeadersVisible = false;
            dgvTable.RowHeadersWidth = 51;
            dgvTable.RowTemplate.Height = 26;
            dgvTable.Size = new Size(1341, 416);
            dgvTable.TabIndex = 2;
            dgvTable.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvTable.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvTable.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvTable.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvTable.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvTable.ThemeStyle.BackColor = Color.White;
            dgvTable.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvTable.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvTable.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvTable.ThemeStyle.HeaderStyle.Font = new Font("Tahoma", 8F);
            dgvTable.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvTable.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvTable.ThemeStyle.HeaderStyle.Height = 25;
            dgvTable.ThemeStyle.ReadOnly = true;
            dgvTable.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvTable.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTable.ThemeStyle.RowsStyle.Font = new Font("Tahoma", 8F);
            dgvTable.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvTable.ThemeStyle.RowsStyle.Height = 26;
            dgvTable.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvTable.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // guna2ContextMenuStrip1
            // 
            guna2ContextMenuStrip1.ImageScalingSize = new Size(20, 20);
            guna2ContextMenuStrip1.Items.AddRange(new ToolStripItem[] { showDetailsToolStripMenuItem1, addToolStripMenuItem, updateToolStripMenuItem, deleteToolStripMenuItem });
            guna2ContextMenuStrip1.Name = "guna2ContextMenuStrip1";
            guna2ContextMenuStrip1.RenderStyle.ArrowColor = Color.FromArgb(151, 143, 255);
            guna2ContextMenuStrip1.RenderStyle.BorderColor = Color.Gainsboro;
            guna2ContextMenuStrip1.RenderStyle.ColorTable = null;
            guna2ContextMenuStrip1.RenderStyle.RoundedEdges = true;
            guna2ContextMenuStrip1.RenderStyle.SelectionArrowColor = Color.White;
            guna2ContextMenuStrip1.RenderStyle.SelectionBackColor = Color.FromArgb(100, 88, 255);
            guna2ContextMenuStrip1.RenderStyle.SelectionForeColor = Color.White;
            guna2ContextMenuStrip1.RenderStyle.SeparatorColor = Color.Gainsboro;
            guna2ContextMenuStrip1.RenderStyle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            guna2ContextMenuStrip1.Size = new Size(215, 136);
            // 
            // showDetailsToolStripMenuItem1
            // 
            showDetailsToolStripMenuItem1.Image = (Image)resources.GetObject("showDetailsToolStripMenuItem1.Image");
            showDetailsToolStripMenuItem1.Name = "showDetailsToolStripMenuItem1";
            showDetailsToolStripMenuItem1.Size = new Size(214, 26);
            showDetailsToolStripMenuItem1.Text = "Show Details";
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Image = (Image)resources.GetObject("addToolStripMenuItem.Image");
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new Size(214, 26);
            addToolStripMenuItem.Text = "Add";
            addToolStripMenuItem.Click += addToolStripMenuItem_Click;
            // 
            // updateToolStripMenuItem
            // 
            updateToolStripMenuItem.Image = (Image)resources.GetObject("updateToolStripMenuItem.Image");
            updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            updateToolStripMenuItem.Size = new Size(214, 26);
            updateToolStripMenuItem.Text = "Update";
            updateToolStripMenuItem.Click += updateToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Image = (Image)resources.GetObject("deleteToolStripMenuItem.Image");
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(214, 26);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            label2.ForeColor = Color.Teal;
            label2.Location = new Point(42, 846);
            label2.Name = "label2";
            label2.Size = new Size(111, 25);
            label2.TabIndex = 4;
            label2.Text = "Records  -";
            // 
            // lbRecords
            // 
            lbRecords.AutoSize = true;
            lbRecords.Font = new Font("Microsoft Sans Serif", 10F);
            lbRecords.ForeColor = Color.Black;
            lbRecords.Location = new Point(147, 846);
            lbRecords.Name = "lbRecords";
            lbRecords.Size = new Size(18, 20);
            lbRecords.TabIndex = 4;
            lbRecords.Text = "?";
            // 
            // btnAddNew
            // 
            btnAddNew.Image = PresentationLayer.Properties.Resources.add_button;
            btnAddNew.Location = new Point(1319, 321);
            btnAddNew.Margin = new Padding(3, 4, 3, 4);
            btnAddNew.Name = "btnAddNew";
            btnAddNew.Size = new Size(69, 69);
            btnAddNew.SizeMode = PictureBoxSizeMode.Zoom;
            btnAddNew.TabIndex = 3;
            btnAddNew.TabStop = false;
            btnAddNew.Click += btnAddNew_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(631, 125);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(170, 114);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // txtFilter
            // 
            txtFilter.Cursor = Cursors.IBeam;
            txtFilter.CustomizableEdges = customizableEdges1;
            txtFilter.DefaultText = "";
            txtFilter.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtFilter.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtFilter.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtFilter.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtFilter.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtFilter.Font = new Font("Segoe UI", 9F);
            txtFilter.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtFilter.Location = new Point(379, 345);
            txtFilter.Margin = new Padding(3, 5, 3, 5);
            txtFilter.Name = "txtFilter";
            txtFilter.PlaceholderText = "";
            txtFilter.SelectedText = "";
            txtFilter.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtFilter.Size = new Size(262, 45);
            txtFilter.TabIndex = 16;
            // 
            // cbFilter
            // 
            cbFilter.BackColor = Color.Transparent;
            cbFilter.CustomizableEdges = customizableEdges3;
            cbFilter.DrawMode = DrawMode.OwnerDrawFixed;
            cbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilter.FocusedColor = Color.FromArgb(94, 148, 255);
            cbFilter.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbFilter.Font = new Font("Segoe UI", 10F);
            cbFilter.ForeColor = Color.FromArgb(68, 88, 112);
            cbFilter.ItemHeight = 30;
            cbFilter.Location = new Point(168, 345);
            cbFilter.Margin = new Padding(3, 4, 3, 4);
            cbFilter.Name = "cbFilter";
            cbFilter.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cbFilter.Size = new Size(203, 36);
            cbFilter.TabIndex = 15;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            label3.ForeColor = Color.Teal;
            label3.Location = new Point(45, 348);
            label3.Name = "label3";
            label3.Size = new Size(110, 29);
            label3.TabIndex = 14;
            label3.Text = "Filter By";
            // 
            // frmManagePatients
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1433, 900);
            Controls.Add(txtFilter);
            Controls.Add(cbFilter);
            Controls.Add(label3);
            Controls.Add(lbRecords);
            Controls.Add(label2);
            Controls.Add(btnAddNew);
            Controls.Add(dgvTable);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmManagePatients";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frm";
            TopMost = true;
            Load += frmManagePatients_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTable).EndInit();
            guna2ContextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)btnAddNew).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvTable;
        private System.Windows.Forms.PictureBox btnAddNew;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbRecords;
        private Guna.UI2.WinForms.Guna2ContextMenuStrip guna2ContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showDetailsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private Guna.UI2.WinForms.Guna2TextBox txtFilter;
        private Guna.UI2.WinForms.Guna2ComboBox cbFilter;
        private System.Windows.Forms.Label label3;
    }
}