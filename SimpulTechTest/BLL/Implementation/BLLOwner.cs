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
    public class BLLOwner : IOwner
    {

        public SimpulTechContext Context { get; set; }

        public BLLOwner()
        {
            Context = new SimpulTechContext();
        }

        public async Task<Response<Owner>> GetItem(string filter)
        {
            var response = new Response<Owner>();
            var owner = new Owner();
            try
            {
                    var list = await Context.Owners.ToListAsync();
                    owner = list.FirstOrDefault(m => m.OwnerId == Int32.Parse(filter));

                response.IsSuccess = true;
                response.ResponseType = owner != null ? ResponseType.Success.ToString() : ResponseType.Failed.ToString();
                response.Result = owner;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseType = ResponseType.Failed.ToString();
                response.Message = ex.ToString();
            }


            return response;
        }

        public async Task<Response<bool?>> IsCanSave(Owner item, bool isupdated)
        {
            var response = new Response<bool?>();
            try
            {
                    var owner_detil = Context.Owners.Where(m => m.FirstName == item.FirstName && m.LastName == item.LastName).SingleOrDefault();
                    if (!isupdated)
                    {
                        if (owner_detil != null)
                        {
                            response.IsSuccess = false;
                            response.ResponseType = ResponseType.Failed.ToString();
                            response.Message = "Data already exist";

                            return response;
                        }
                        await Context.Owners.AddAsync(item);
                    }
                    else
                    {
                        owner_detil.FirstName = item.FirstName;
                        owner_detil.LastName = item.LastName;
                        owner_detil.Email = item.Email;
                        owner_detil.Telephone = item.Telephone;
                    }

                    response.IsSuccess = await Context.SaveChangesAsync() > 0;
                    response.ResponseType = response.IsSuccess ? ResponseType.Success.ToString() : ResponseType.Failed.ToString();
                    response.Message = response.IsSuccess && isupdated ? "Updated successfully" :
                          response.IsSuccess && !isupdated ? "Inserted successfully" : "failed";
                    response.Result = null;
                
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseType = ResponseType.Failed.ToString();
                response.Message = ex.ToString();
            }

            return response;
        }

        public async Task<Response<List<Owner>>> GetItems(string filter, int page_no, int page_size)
        {
            var response = new Response<List<Owner>>();
            try
            {
                filter = filter ?? string.Empty;
                var owners = new List<Owner>();
                
                    owners = await Context.Owners.ToListAsync();
                    if (!string.IsNullOrEmpty(filter))
                    {
                        owners = (from m in owners
                                  where m.FirstName.ToLower().Contains(filter.ToLower()) ||
                                        m.LastName.ToLower().Contains(filter.ToLower())

                                  select new Owner()
                                  {
                                      FirstName = m.FirstName,
                                      LastName = m.LastName,
                                      Email = m.Email,
                                      OwnerId = m.OwnerId,
                                      Telephone = m.Telephone
                                  }).ToList();
                    }

                    owners = owners.Skip((page_no - 1) * page_size).Take(page_size).
                        OrderBy(m => m.OwnerId).ToList();

                    response.IsSuccess = true;
                    response.ResponseType = ResponseType.Success.ToString();
                    response.Message = "success";
                    response.Result = owners;
                
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseType = ResponseType.Failed.ToString();
                response.Message = ex.ToString();
            }

            return response;
        }

        public async Task<Response<bool?>> IsCanDelete(Owner item)
        {
            var response = new Response<bool?>();
            try
            {
                
                    var owner_detil = Context.Owners.Where(m => m.OwnerId == item.OwnerId).SingleOrDefault();

                    if (owner_detil == null)
                    {
                        response.IsSuccess = false;
                        response.ResponseType = ResponseType.Failed.ToString();
                        response.Message = "Data not existed";

                        return response;
                    }
                    Context.Remove(owner_detil);
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
    }
}

