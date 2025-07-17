namespace DomainLayer.Constants;

public class ClaimConstants
{
    // claim types
    public const string Permission = "Permission";

    // claims values
    // patients claims
    public const string ViewPatients = "view-patients";
    public const string AddPatient = "add-patient";
    public const string EditPatient = "edit-patient";
    public const string DeletePatient = "delete-patient";

    // doctors claims
    public const string ViewDoctors = "view-doctors";
    public const string AddDoctor = "add-doctor";
    public const string EditDoctor = "edit-doctor";
    public const string DeleteDoctor = "delete-doctor";

    // appointments claims
    public const string ViewAppointments = "view-appointments";
    public const string CreateAppointment = "create-appointment";
    public const string EditAppointment = "edit-appointment";
    public const string CancelAppointment = "cancel-appointment";
    public const string RescheduleAppointment = "reschedule-appointment";

    // medical records claims
    public const string ViewMedicalRecords = "view-medical-records";
    public const string CreateMedicalRecord = "create-medical-record";
    public const string EditMedicalRecord = "edit-medical-record";

    // prescriptions claims
    public const string ViewPrescriptions = "view-prescriptions";
    public const string CreatePrescription = "create-prescription";
    public const string EditPrescription = "edit-prescription";
    public const string DeletePrescription = "delete-prescription";

    // payments claims
    public const string ViewPayments = "view-payments";
    public const string ProcessPayment = "process-payment";
}