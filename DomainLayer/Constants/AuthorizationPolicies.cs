namespace DomainLayer.Constants;

public class AuthorizationPolicies
{
    // 🔹 Patient-related Policies
    public const string CanViewPatients = "CanViewPatients";
    public const string CanAddPatient = "CanAddPatient";
    public const string CanEditPatient = "CanEditPatient";
    public const string CanDeletePatient = "CanDeletePatient";

    // 🔹 Doctor-related Policies
    public const string CanViewDoctors = "CanViewDoctors";
    public const string CanAddDoctor = "CanAddDoctor";
    public const string CanEditDoctor = "CanEditDoctor";
    public const string CanDeleteDoctor = "CanDeleteDoctor";

    // 🔹 Appointment-related Policies
    public const string CanViewAppointments = "CanViewAppointments";
    public const string CanCreateAppointment = "CanCreateAppointment";
    public const string CanEditAppointment = "CanEditAppointment";
    public const string CanCancelAppointment = "CanCancelAppointment";
    public const string CanRescheduleAppointment = "CanRescheduleAppointment";
    public const string CanCompleteAppointment = "CanCompleteAppointment";

    // 🔹 MedicalRecord-related Policies
    public const string CanViewMedicalRecords = "CanViewMedicalRecords";
    public const string CanCreateMedicalRecord = "CanCreateMedicalRecord";
    public const string CanEditMedicalRecord = "CanEditMedicalRecord";
    public const string CanDeleteMedicalRecord = "CanDeleteMedicalRecord";

    // 🔹 Prescription-related Policies
    public const string CanViewPrescriptions = "CanViewPrescriptions";
    public const string CanCreatePrescription = "CanCreatePrescription";
    public const string CanEditPrescription = "CanEditPrescription";
    public const string CanDeletePrescription = "CanDeletePrescription";

    // 🔹 Payment-related Policies
    public const string CanViewPayments = "CanViewPayments";
    public const string CanProcessPayment = "CanProcessPayment";
}