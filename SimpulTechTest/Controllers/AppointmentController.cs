using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpulTechTest.BLL.Interfaces;
using SimpulTechTest.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpulTechTest.Models;

namespace SimpulTechTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {

        private readonly ILogger _log;
        private IAppointment services = null;
        private readonly SimpulTechContext context = null;
        public AppointmentController(ILoggerFactory loggerFactory, IAppointment _service, SimpulTechContext context)
        {
            this._log = loggerFactory.CreateLogger<AppointmentController>();
            this.services = _service;
            this.context = context;
        }

        [HttpGet]
        [Route("ListAppointment")]
        public async Task<IActionResult> ListAppointment(string pet_name, DateTime? schedule_date, int page_no, int page_size)
        {
            var result = new Response<List<Models.DataAppointment>>();
            try
            {
                page_no = page_no > 1 ? page_no : 1;
                page_size = page_size > 0 ? page_size : 10;

                services.Context = context;
                result = await services.GetAppointmentList(pet_name, schedule_date, page_no, page_size);
                _log.LogInformation(result.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ResponseType = ResponseType.Failed.ToString();
                result.Message = ex.ToString();

                _log.LogError(result.ToString());
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("GetDataAppointments")]
        public async Task<IActionResult> GetAppointment(string filter, int page_no, int page_size)
        {
            var result = new Response<List<Models.Appointment>>();
            try
            {
                page_no = page_no > 1 ? page_no : 1;
                page_size = page_size > 0 ? page_size : 10;
                services.Context = context;
                result = await services.GetItems(filter, page_no, page_size);
                _log.LogInformation(result.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ResponseType = ResponseType.Failed.ToString();
                result.Message = ex.ToString();

                _log.LogError(result.ToString());
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("GetAppointment")]
        public async Task<IActionResult> GetAppointment(string filter)
        {
            var result = new Response<Models.Appointment>();
            try
            {
                services.Context = context;
                result = await services.GetItem(filter);
                _log.LogInformation(result.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ResponseType = ResponseType.Failed.ToString();
                result.Message = ex.ToString();

                _log.LogError(result.ToString());
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("AddAppointment")]
        public async Task<IActionResult> Insert([FromBody] Models.Appointment request)
        {
            var result = new Response<bool?>();
            result.Result = null;
            try
            {
                services.Context = context;
                result = await services.IsCanSave(request, false);
                _log.LogInformation(result.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ResponseType = ResponseType.Failed.ToString();
                result.Message = ex.ToString();

                _log.LogError(result.ToString());
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("UpdateAppointment")]
        public async Task<IActionResult> Update([FromBody] Models.Appointment request)
        {
            var result = new Response<bool?>();
            result.Result = null;
            try
            {
                services.Context = context;
                result = await services.IsCanSave(request, true);
                _log.LogInformation(result.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ResponseType = ResponseType.Failed.ToString();
                result.Message = ex.ToString();

                _log.LogError(result.ToString());
                return BadRequest(result);
            }
        }

        [HttpDelete]
        [Route("DeleteAppointment")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = new Response<bool?>();
            result.Result = null;
            try
            {
                var item = new Models.Appointment() { AppointmentId = id };
                services.Context = context;
                result = await services.IsCanDelete(item);
                _log.LogInformation(result.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ResponseType = ResponseType.Failed.ToString();
                result.Message = ex.ToString();

                _log.LogError(result.ToString());
                return BadRequest(result);
            }
        }
    }
}
