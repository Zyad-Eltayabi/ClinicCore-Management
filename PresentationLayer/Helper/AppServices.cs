using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Interfaces;
using Microsoft.Extensions.Logging;
using Presentation_Tier;

namespace PresentationLayer.Helper
{
    public class AppServices
    {
        public IPatientService PatientService { get; }
        public IUnitOfWork UnitOfWork { get; }

        public AppServices(
            IPatientService patientService,
            IUnitOfWork unitOfWork)
        {
            PatientService = patientService;
            UnitOfWork = unitOfWork;
        }
    }

}
