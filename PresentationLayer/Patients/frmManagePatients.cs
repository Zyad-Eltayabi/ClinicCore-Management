using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using DataAccessLayer;
using DomainLayer.Interfaces;
using Presentation_Tier.Users;

namespace Presentation_Tier.Component
{
    public partial class frmManagePatients : Form
    {
        private IPatientService _patientService;

        public frmManagePatients(IPatientService patientService)
        {
            InitializeComponent();
            _patientService = patientService;
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddPatient();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPatient();
        }

        private void AddPatient()
        {
            var addPatient = new frmAddNewPatient(_patientService);
            addPatient.ShowDialog();
        }

        private void frmManagePatients_Load(object sender, EventArgs e)
        {
            LoadPatientsInfo();
        }

        private void LoadPatientsInfo()
        {
            var patients = _patientService.GetAll();
            if (patients != null)
            {
                dgvTable.DataSource = patients;
                lbRecords.Text = patients.Count().ToString();
            }
        }
    }
}
