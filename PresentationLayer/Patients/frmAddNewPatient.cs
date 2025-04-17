using BusinessLayer;
using DomainLayer.Enums;
using DomainLayer.Interfaces;
using DomainLayer.Models;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Presentation_Tier.Users
{
    public partial class frmAddNewPatient : Form
    {
        private GeneralEnum.SaveMode _enMode;

        private Patient _patient;

        private IPatientService _patientService { get; }

        public frmAddNewPatient(IPatientService patientService)
        {
            InitializeComponent();
            _patientService = patientService;
            _patient = new Patient();
            _enMode = GeneralEnum.SaveMode.Add;
        }

        public frmAddNewPatient(IPatientService patientService, Patient patient)
        {
            InitializeComponent();
            _patientService = patientService;
            _patient = patient;
            _enMode = GeneralEnum.SaveMode.Update;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }


        private void Save()
        {
            switch (_enMode)
            {
                case GeneralEnum.SaveMode.Add:
                    SavePatient();
                    _enMode = GeneralEnum.SaveMode.Update;
                    break;
                case GeneralEnum.SaveMode.Update:
                    SavePatient();
                    break;
                default:
                    break;
            }
        }

        private void SavePatient() // for save a new record or update 
        {
            _patient.FullName = txtFullName.Text;
            _patient.Address = txtAddress.Text;
            _patient.Email = txtEmail.Text;
            _patient.PhoneNumber = txtPhoneNumber.Text;
            _patient.DateOfBirth = dtDateOfBirth.Value;
            _patient.Gender = rbMale.Checked;

            bool saved = false;
            switch (_enMode)
            {
                case GeneralEnum.SaveMode.Add:
                    _enMode = GeneralEnum.SaveMode.Update;
                    saved = _patientService.Add(_patient);
                    break;
                case GeneralEnum.SaveMode.Update:
                    saved = _patientService.Update(_patient);
                    break;
                default:
                    break;
            }
            if (saved)
            {
                lbPatientID.Text = _patient.Id.ToString();
                this.Text = "Update Patient";
                clsUtilityLibrary.PrintInfoMessage("Data Saved Successfully");
            }
            else
            {
                clsUtilityLibrary.PrintErrorMessage("Sorry, Failed To Save");
            }
        }

        private void frmAddNewPatient_Load(object sender, EventArgs e)
        {
            CheckUpdateMode();
        }

        private void CheckUpdateMode()
        {
            if (_enMode == GeneralEnum.SaveMode.Update)
            {
                SetPatientInfo();
            }
        }

        private void SetPatientInfo()
        {
            if (_patient != null)
            {
                lbPatientID.Text = _patient.Id.ToString();
                txtFullName.Text = _patient.FullName;
                txtAddress.Text = _patient.Address;
                txtEmail.Text = _patient.Email;
                txtPhoneNumber.Text = _patient.PhoneNumber;
                dtDateOfBirth.Value = _patient.DateOfBirth;
                if (_patient.Gender)
                    rbMale.Checked = true;
                else
                    rbFemale.Checked = true;
            }
        }
    }
}
