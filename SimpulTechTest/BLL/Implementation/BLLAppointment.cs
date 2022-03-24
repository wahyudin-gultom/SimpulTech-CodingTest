using SimpulTechTest.Response;
using SimpulTechTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpulTechTest.BLL.Interfaces;

namespace SimpulCodingTest.BLL.Implementation
{
    public class BLLAppointment : IAppointment
    {

        public SimpulTechContext Context { get; set; }
        public async Task<Response<Appointment>> GetItem(string filter)
        {
            var response = new Response<Appointment>();
            var item = new Appointment();
            try
            {
                var list = await Context.Appointments.ToListAsync();
                item = list.FirstOrDefault(m => m.AppointmentId == Int32.Parse(filter));

                response.IsSuccess = true;
                response.ResponseType = item != null ? ResponseType.Success.ToString() : ResponseType.Failed.ToString();
                response.Result = item;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseType = ResponseType.Failed.ToString();
                response.Message = ex.ToString();
            }


            return response;
        }

        public async Task<Response<List<Appointment>>> GetItems(string filter, int page_no, int page_size)
        {
            var response = new Response<List<Appointment>>();
            try
            {
                var items = new List<Appointment>();
                
                items = await Context.Appointments.ToListAsync();

                items = items.Skip((page_no - 1) * page_size).Take(page_size).
                        OrderBy(m => m.AppointmentId).ToList();

                response.IsSuccess = true;
                response.ResponseType = ResponseType.Success.ToString();
                response.Message = "success";
                response.Result = items;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseType = ResponseType.Failed.ToString();
                response.Message = ex.ToString();
            }

            return response;
        }

        public async Task<Response<bool?>> IsCanDelete(Appointment item)
        {
            var response = new Response<bool?>();
            response.Result = null;
            try
            {
                
                    var detil = Context.Appointments.Where(m => m.PetId == item.PetId).SingleOrDefault();

                    if (detil == null)
                    {
                        response.IsSuccess = false;
                        response.ResponseType = ResponseType.Failed.ToString();
                        response.Message = "Data not existed";

                        return response;
                    }
                    Context.Remove(detil);
                    response.IsSuccess = await Context.SaveChangesAsync() > 0;
                    response.ResponseType = ResponseType.Success.ToString();
                    response.Message = "Deleted successfully";
                
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseType = ResponseType.Failed.ToString();
                response.Message = ex.ToString();
            }

            return response;
        }

        public async Task<Response<bool?>> IsCanSave(Appointment item, bool isupdated)
        {
            var response = new Response<bool?>();
            response.Result = null;
            try
            {
                    var detil = await Context.Appointments.Where(m => m.AppointmentId == item.AppointmentId &&
                    m.DateTime == item.DateTime && m.PetId == m.PetId).SingleOrDefaultAsync();
                    if (!isupdated)
                    {
                        if (detil != null)
                        {
                            response.IsSuccess = false;
                            response.ResponseType = ResponseType.Failed.ToString();
                            response.Message = "Data already exist";

                            return response;
                        }
                        Context.Appointments.Add(item);
                    }
                    else
                    {
                        detil.DateTime = item.DateTime;
                        detil.Notes = item.Notes;
                        detil.PetId = item.PetId;
                    }

                    response.IsSuccess = Context.SaveChanges() > 0;
                    response.ResponseType = response.IsSuccess ? ResponseType.Success.ToString() : ResponseType.Failed.ToString();
                    response.Message = response.IsSuccess && isupdated ? "Updated successfully" :
                          response.IsSuccess && !isupdated ? "Inserted successfully" : "failed";
                
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseType = ResponseType.Failed.ToString();
                response.Message = ex.ToString();
            }

            return response;
        }

        public async Task<Response<List<DataAppointment>>> GetAppointmentList(string pet_name, DateTime? scheduleTime, int page_no, int page_size)
        {
            var response = new Response<List<DataAppointment>>();
            response.IsSuccess = true;
            response.ResponseType = "success";

            try
            {
               
                    var appointmentList = (from app in Context.Appointments
                                           join p in Context.Pets
                                           on app.PetId equals p.PetId
                                           join o in Context.Owners
                                           on p.OwnerId equals o.OwnerId

                                           select new DataAppointment
                                           {
                                               AppointmentId = app.AppointmentId,
                                               DateTime = app.DateTime.Value,
                                               Notes = app.Notes,
                                               PetId = app.PetId,
                                               PetName = p.PetName,
                                               OwnerId = p.OwnerId,
                                               Telephone = o.Telephone,
                                               OwnerName = o.FirstName + " " + o.LastName
                                           });

                    if (!string.IsNullOrEmpty(pet_name))
                    {
                        appointmentList = appointmentList.Where(m => m.PetName.Contains(pet_name));
                    }

                    if (scheduleTime.HasValue)
                    {
                        appointmentList = appointmentList.Where(m => m.DateTime == scheduleTime.Value);
                    }

                    response.Result = await appointmentList.Select(m => new DataAppointment()
                    {
                        DateTime = m.DateTime,
                        PetName = m.PetName,
                        OwnerName = m.OwnerName,
                        Telephone = m.Telephone,
                        Notes = m.Notes
                    }).Skip((page_no - 1) * page_size).Take(page_size).OrderBy(m => m.DateTime).ToListAsync();
                
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseType = "Failed";
                response.Message = ex.ToString();
            }

            return response;
        }
    }
}

