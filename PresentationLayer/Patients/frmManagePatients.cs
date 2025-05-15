using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using DataAccessLayer;
using DomainLayer.BaseClasses;
using DomainLayer.Interfaces;
using DomainLayer.Models;
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

        private async Task LoadPatientsInfo()
        {
            var patients = await _patientService.GetAll();
            if (patients != null)
            {
                dgvTable.DataSource = patients;
                lbRecords.Text = patients.Count().ToString();
            }
        }

        private int GetPatientID()
        {
            int patientId = int.Parse(dgvTable.SelectedRows[0].Cells["Id"].Value.ToString());
            return patientId;
        }

        private async Task updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int patientId = GetPatientID();

            var patient = await _patientService.GetById(patientId);

            if (patient != null)
            {
                var form = new frmAddNewPatient(_patientService, patient);
                form.ShowDialog();
            }
            else
                clsUtilityLibrary.PrintErrorMessage("Patient is not found.");

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var patientID = GetPatientID();

            if (MessageBox.Show($"Do you want to delete this patient with id = {patientID}")
                == DialogResult.OK)
            {
                Expression<Func<DomainLayer.Models.Patient, bool>> add = (x) => x.Id == patientID;
               Result<Patient> result = _patientService.Delete(add);
                clsUtilityLibrary.PrintInfoMessage(result.Message);
            }
        }
    }
}
