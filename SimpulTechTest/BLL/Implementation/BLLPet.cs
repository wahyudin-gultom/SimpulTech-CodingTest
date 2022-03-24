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
    public class BLLPet : IPet
    {

        public SimpulTechContext Context { get; set; }
        public async Task<Response<Pet>> GetItem(string filter)
        {
            var response = new Response<Pet>();
            var item = new Pet();
            try
            {
                
                    item = await Context.Pets.FindAsync(filter).AsTask();
                

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

        public async Task<Response<List<Pet>>> GetItems(string filter, int page_no, int page_size)
        {
            var response = new Response<List<Pet>>();
            try
            {
                var items = new List<Pet>();
                
                    items = await Context.Pets.ToListAsync();
                    if (string.IsNullOrEmpty(filter))
                    {
                        items = (from m in items.AsEnumerable()
                                 where m.PetName.Contains(filter)

                                 select new Pet()
                                 {
                                     PetId = m.PetId,
                                     PetName = m.PetName,
                                     PetAge = m.PetAge,
                                     PetType = m.PetAge,
                                     OwnerId = m.OwnerId
                                 }).ToList();
                    }

                    items = items.Skip((page_no - 1) * page_size).Take(page_size).
                        OrderBy(m => m.PetId).ToList();

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

        public async Task<Response<bool?>> IsCanDelete(Pet item)
        {
            var response = new Response<bool?>();
            response.Result = null;
            try
            {
                
                    var detil = Context.Pets.Where(m => m.PetId == item.PetId).SingleOrDefault();

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

        public async Task<Response<bool?>> IsCanSave(Pet item, bool isupdated)
        {
            var response = new Response<bool?>();
            response.Result = null;
            try
            {
                    var detil = await Context.Pets.Where(m => m.PetName == item.PetName && m.OwnerId == item.OwnerId).SingleOrDefaultAsync();
                    if (!isupdated)
                    {
                        if (detil != null)
                        {
                            response.IsSuccess = false;
                            response.ResponseType = ResponseType.Failed.ToString();
                            response.Message = "Data already exist";

                            return response;
                        }
                        Context.Pets.Add(item);

                    }
                    else
                    {
                        detil.OwnerId = item.OwnerId;
                        detil.PetName = item.PetName;
                        detil.PetAge = item.PetAge;
                        detil.PetType = item.PetType;
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
    }
}

