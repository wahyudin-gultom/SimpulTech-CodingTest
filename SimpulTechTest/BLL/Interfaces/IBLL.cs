using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpulTechTest.Models;
using SimpulTechTest.Response;

namespace SimpulTechTest.BLL.Interfaces
{
    public interface IBLL<T>
    {
        SimpulTechContext Context { get; set; }
        Task<Response<T>> GetItem(string filter);
        Task<Response<bool?>> IsCanSave(T item, bool isupdated);
        Task<Response<List<T>>> GetItems(string filter, int page_no, int page_size);
        Task<Response<bool?>> IsCanDelete(T item);
    }
}
