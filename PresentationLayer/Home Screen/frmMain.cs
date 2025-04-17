using Presentation_Tier.Component;
using PresentationLayer.Helper;
namespace Presentation_Tier
{
    public partial class frmMain : Form
    {
        private readonly AppServices _services;

        public frmMain(AppServices services)
        {
            InitializeComponent();
            _services = services;
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddNewFormToMainPanel(Form form)
        {
            pnMainPanel.Controls.Clear();

            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;  // Remove borders to make it look integrated

            pnMainPanel.Controls.Add(form);
            form.Show();

        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            AddNewFormToMainPanel(new frmManagePatients(_services.PatientService)); 
        }
    }
}
