using Microsoft.AspNetCore.Mvc;
using RestfulApi.Models;

namespace RestfulApi.Interfaces
{
    public interface ICustomerController
    {
        JsonResult GetAll();
        JsonResult GetByID(int id);
        JsonResult GetByDNI(string dni);
        JsonResult GetByName(string name);
        JsonResult GetByState(string state);
        JsonResult GetByCity(string city);
        JsonResult GetByEmail(string email);
        JsonResult GetByPhoneNumber(string phoneNumber);
        JsonResult GetByAddress(string address);
        JsonResult CreateEdit(Customer customer);
        JsonResult Delete(int id);
    }
}
