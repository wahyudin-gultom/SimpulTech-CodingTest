using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpulTechTest.BLL.Interfaces;
using SimpulTechTest.Models;
using SimpulTechTest.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpulTechTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly ILogger _log;
        private IOwner services = null;
        private readonly SimpulTechContext context = null;
        public OwnerController(ILoggerFactory loggerFactory, IOwner _service, SimpulTechContext context)
        {
            this._log = loggerFactory.CreateLogger<OwnerController>();
            this.services = _service;
            this.context = context;
        }

        [HttpGet]
        [Route("GetOwners")]
        public async Task<IActionResult> GetOwners(string filter, int page_no, int page_size)
        {
            var result = new Response<List<Models.Owner>>();
            try
            {
                page_no = page_no < 1 ? 1 : page_no;
                page_size = page_size < 1 ? 10 : page_size;
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
        [Route("GetOwner")]
        public async Task<IActionResult> GetOwner(int id)
        {
            var result = new Response<Models.Owner>();
            try
            {
                services.Context = context;
                result = await services.GetItem(id.ToString());
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
        [Route("AddOwner")]
        public async Task<IActionResult> Insert([FromBody] Models.Owner request)
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
        [Route("UpdateOwner")]
        public async Task<IActionResult> Update([FromBody] Models.Owner request)
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
        [Route("DeleteOwner")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = new Response<bool?>();
            result.Result = null;
            try
            {
                var item = new Models.Owner() { OwnerId = id };
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
