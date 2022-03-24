using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpulTechTest.Models;
using SimpulTechTest.Response;

namespace SimpulTechTest.BLL.Interfaces
{
    public interface IAppointment : IBLL<Appointment>
    {
        Task<Response<List<DataAppointment>>> GetAppointmentList(string pet_name, DateTime? scheduleTime, int page_no, int page_size);
    }
}
