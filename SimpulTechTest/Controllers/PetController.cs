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
    public class PetController : ControllerBase
    {

        private readonly ILogger _log;
        private IPet services = null;
        private readonly SimpulTechContext context = null;
        public PetController(ILoggerFactory loggerFactory, IPet _service, SimpulTechContext context)
        {
            this._log = loggerFactory.CreateLogger<PetController>();
            this.services = _service;
            this.context = context;
        }

        [HttpGet]
        [Route("GetPets")]
        public async Task<IActionResult> GetPets(string filter, int page_no, int skip)
        {
            var result = new Response<List<Models.Pet>>();
            try
            {
                page_no = page_no > 1 ? page_no : 1;
                skip = skip > 0 ? skip : 10;
                services.Context = context;
                result = await services.GetItems(filter, page_no, skip);
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
        [Route("GetPet")]
        public async Task<IActionResult> GetPet(string filter)
        {
            var result = new Response<Models.Pet>();
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
        [Route("AddPet")]
        public async Task<IActionResult> Insert([FromBody] Models.Pet request)
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
        [Route("UpdatePet")]
        public async Task<IActionResult> Update([FromBody] Models.Pet request)
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
        [Route("DeletePet")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = new Response<bool?>();
            result.Result = null;
            try
            {
                var item = new Models.Pet() { PetId = id };
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
