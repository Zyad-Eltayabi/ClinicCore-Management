namespace Presentation_Tier.Users
{
    partial class frmAddNewPatient
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddNewPatient));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pictureBox1 = new PictureBox();
            groupBox1 = new GroupBox();
            rbFemale = new Guna.UI2.WinForms.Guna2RadioButton();
            rbMale = new Guna.UI2.WinForms.Guna2RadioButton();
            label7 = new Label();
            dtDateOfBirth = new Guna.UI2.WinForms.Guna2DateTimePicker();
            label5 = new Label();
            lbPatientID = new Label();
            label2 = new Label();
            txtFullName = new Guna.UI2.WinForms.Guna2TextBox();
            label1 = new Label();
            txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            label4 = new Label();
            txtPhoneNumber = new Guna.UI2.WinForms.Guna2TextBox();
            label6 = new Label();
            txtAddress = new Guna.UI2.WinForms.Guna2TextBox();
            label9 = new Label();
            btnSave = new Guna.UI2.WinForms.Guna2Button();
            errorProvider1 = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(577, 36);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(127, 142);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rbFemale);
            groupBox1.Controls.Add(rbMale);
            groupBox1.Location = new Point(854, 289);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(362, 76);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            // 
            // rbFemale
            // 
            rbFemale.AutoSize = true;
            rbFemale.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
            rbFemale.CheckedState.BorderThickness = 0;
            rbFemale.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
            rbFemale.CheckedState.InnerColor = Color.White;
            rbFemale.CheckedState.InnerOffset = -4;
            rbFemale.Location = new Point(241, 29);
            rbFemale.Margin = new Padding(3, 4, 3, 4);
            rbFemale.Name = "rbFemale";
            rbFemale.Size = new Size(78, 24);
            rbFemale.TabIndex = 1;
            rbFemale.Text = "Female";
            rbFemale.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            rbFemale.UncheckedState.BorderThickness = 2;
            rbFemale.UncheckedState.FillColor = Color.Transparent;
            rbFemale.UncheckedState.InnerColor = Color.Transparent;
            // 
            // rbMale
            // 
            rbMale.AutoSize = true;
            rbMale.Checked = true;
            rbMale.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
            rbMale.CheckedState.BorderThickness = 0;
            rbMale.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
            rbMale.CheckedState.InnerColor = Color.White;
            rbMale.CheckedState.InnerOffset = -4;
            rbMale.Location = new Point(25, 29);
            rbMale.Margin = new Padding(3, 4, 3, 4);
            rbMale.Name = "rbMale";
            rbMale.Size = new Size(63, 24);
            rbMale.TabIndex = 0;
            rbMale.TabStop = true;
            rbMale.Text = "Male";
            rbMale.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            rbMale.UncheckedState.BorderThickness = 2;
            rbMale.UncheckedState.FillColor = Color.Transparent;
            rbMale.UncheckedState.InnerColor = Color.Transparent;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(697, 304);
            label7.Name = "label7";
            label7.Size = new Size(83, 25);
            label7.TabIndex = 20;
            label7.Text = "Gender";
            // 
            // dtDateOfBirth
            // 
            dtDateOfBirth.Checked = true;
            dtDateOfBirth.CustomizableEdges = customizableEdges1;
            dtDateOfBirth.Font = new Font("Segoe UI", 9F);
            dtDateOfBirth.Format = DateTimePickerFormat.Long;
            dtDateOfBirth.Location = new Point(205, 419);
            dtDateOfBirth.Margin = new Padding(3, 4, 3, 4);
            dtDateOfBirth.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtDateOfBirth.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtDateOfBirth.Name = "dtDateOfBirth";
            dtDateOfBirth.ShadowDecoration.CustomizableEdges = customizableEdges2;
            dtDateOfBirth.Size = new Size(362, 56);
            dtDateOfBirth.TabIndex = 19;
            dtDateOfBirth.Value = new DateTime(2024, 10, 18, 3, 42, 42, 51);
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(30, 424);
            label5.Name = "label5";
            label5.Size = new Size(136, 25);
            label5.TabIndex = 18;
            label5.Text = "Date Of Birth";
            // 
            // lbPatientID
            // 
            lbPatientID.AutoSize = true;
            lbPatientID.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbPatientID.Location = new Point(198, 228);
            lbPatientID.Name = "lbPatientID";
            lbPatientID.Size = new Size(36, 25);
            lbPatientID.TabIndex = 17;
            lbPatientID.Text = "??";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(30, 228);
            label2.Name = "label2";
            label2.Size = new Size(100, 25);
            label2.TabIndex = 16;
            label2.Text = "PatientID";
            // 
            // txtFullName
            // 
            txtFullName.Cursor = Cursors.IBeam;
            txtFullName.CustomizableEdges = customizableEdges3;
            txtFullName.DefaultText = "";
            txtFullName.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtFullName.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtFullName.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtFullName.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtFullName.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtFullName.Font = new Font("Segoe UI", 9F);
            txtFullName.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtFullName.Location = new Point(205, 304);
            txtFullName.Margin = new Padding(3, 5, 3, 5);
            txtFullName.Name = "txtFullName";
            txtFullName.PlaceholderText = "";
            txtFullName.SelectedText = "";
            txtFullName.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtFullName.Size = new Size(362, 60);
            txtFullName.TabIndex = 15;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(30, 315);
            label1.Name = "label1";
            label1.Size = new Size(109, 25);
            label1.TabIndex = 14;
            label1.Text = "Full Name";
            // 
            // txtEmail
            // 
            txtEmail.Cursor = Cursors.IBeam;
            txtEmail.CustomizableEdges = customizableEdges5;
            txtEmail.DefaultText = "";
            txtEmail.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtEmail.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtEmail.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Location = new Point(205, 522);
            txtEmail.Margin = new Padding(3, 5, 3, 5);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "";
            txtEmail.SelectedText = "";
            txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtEmail.Size = new Size(362, 60);
            txtEmail.TabIndex = 27;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(30, 534);
            label4.Name = "label4";
            label4.Size = new Size(65, 25);
            label4.TabIndex = 26;
            label4.Text = "Email";
            // 
            // txtPhoneNumber
            // 
            txtPhoneNumber.Cursor = Cursors.IBeam;
            txtPhoneNumber.CustomizableEdges = customizableEdges7;
            txtPhoneNumber.DefaultText = "";
            txtPhoneNumber.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPhoneNumber.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPhoneNumber.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPhoneNumber.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPhoneNumber.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPhoneNumber.Font = new Font("Segoe UI", 9F);
            txtPhoneNumber.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPhoneNumber.Location = new Point(854, 396);
            txtPhoneNumber.Margin = new Padding(3, 5, 3, 5);
            txtPhoneNumber.Name = "txtPhoneNumber";
            txtPhoneNumber.PlaceholderText = "";
            txtPhoneNumber.SelectedText = "";
            txtPhoneNumber.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtPhoneNumber.Size = new Size(362, 60);
            txtPhoneNumber.TabIndex = 29;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(697, 408);
            label6.Name = "label6";
            label6.Size = new Size(124, 25);
            label6.TabIndex = 28;
            label6.Text = "Tel Number";
            // 
            // txtAddress
            // 
            txtAddress.Cursor = Cursors.IBeam;
            txtAddress.CustomizableEdges = customizableEdges9;
            txtAddress.DefaultText = "";
            txtAddress.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtAddress.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtAddress.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtAddress.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtAddress.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtAddress.Font = new Font("Segoe UI", 9F);
            txtAddress.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtAddress.Location = new Point(854, 499);
            txtAddress.Margin = new Padding(3, 5, 3, 5);
            txtAddress.Name = "txtAddress";
            txtAddress.PlaceholderText = "";
            txtAddress.SelectedText = "";
            txtAddress.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtAddress.Size = new Size(362, 129);
            txtAddress.TabIndex = 31;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(697, 510);
            label9.Name = "label9";
            label9.Size = new Size(92, 25);
            label9.TabIndex = 30;
            label9.Text = "Address";
            // 
            // btnSave
            // 
            btnSave.BorderRadius = 10;
            btnSave.CustomizableEdges = customizableEdges11;
            btnSave.DisabledState.BorderColor = Color.DarkGray;
            btnSave.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSave.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSave.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSave.Font = new Font("Segoe UI", 19F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(1010, 672);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnSave.Size = new Size(206, 61);
            btnSave.TabIndex = 32;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // frmAddNewPatient
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1281, 764);
            Controls.Add(btnSave);
            Controls.Add(txtAddress);
            Controls.Add(label9);
            Controls.Add(txtPhoneNumber);
            Controls.Add(label6);
            Controls.Add(txtEmail);
            Controls.Add(label4);
            Controls.Add(groupBox1);
            Controls.Add(label7);
            Controls.Add(dtDateOfBirth);
            Controls.Add(label5);
            Controls.Add(lbPatientID);
            Controls.Add(label2);
            Controls.Add(txtFullName);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmAddNewPatient";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Add New Patient";
            Load += frmAddNewPatient_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Guna.UI2.WinForms.Guna2RadioButton rbFemale;
        private Guna.UI2.WinForms.Guna2RadioButton rbMale;
        private System.Windows.Forms.Label label7;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtDateOfBirth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbPatientID;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TextBox txtFullName;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2TextBox txtPhoneNumber;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2TextBox txtAddress;
        private System.Windows.Forms.Label label9;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}