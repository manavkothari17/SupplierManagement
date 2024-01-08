using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SupplierInfo.DatabaseContext;
using SupplierInfo.Models;
using SupplierInfo.Services;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SupplierInfo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly DatabaseContextInfo _context;
        string path = @"C:/Users/Admin/Desktop/Json.json";

        public SupplierController(DatabaseContextInfo context)
        {
            _context = context;
        }
        /// <summary>
        /// Get Suppliers Hotel List from JSON
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSuppliersHotelList")]
        public Response GetSuppliersHotelList()
        {
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    return new Response { StatusCode = (int)HttpStatusCode.NotFound, Message = "File Not Found", ResponseBody = null };
                }

                string json = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(json))
                {
                    return new Response { StatusCode = (int)HttpStatusCode.NoContent, Message = "No Content Found", ResponseBody = null };
                }

                var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(json);

                if (suppliers == null || !suppliers.Any())
                {
                    return new Response { StatusCode = (int)HttpStatusCode.NoContent, Message = "No Content Found", ResponseBody = null };
                }

                var result = suppliers
                    .SelectMany(supplier => supplier.Hotels)
                    .Select(hotel => new Hotel
                    {
                        Id = hotel.Id,
                        Name = hotel.Name,
                        Address = hotel.Address
                    }).ToList();

                return new Response { StatusCode = (int)HttpStatusCode.OK, Message = "Success", ResponseBody = result };
            }
            catch (Exception ex)
            {
                return new Response { StatusCode = (int)HttpStatusCode.InternalServerError, Message = ex.Message };
            }

        }

        /// <summary>
        /// Get Consolidated Hotels List based on NAME and ADDRESS
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetConsolidatedHotelList")]
        public Response GetConsolidatedHotelList()
        {
            try
            {
                var data = _context.Hotels.Include(hotel => hotel.Address).ToList();
                if (data != null && data.Any())
                {
                    SupplierService supplierService = new SupplierService();
                    var combinedHotels = supplierService.FilterHotelByNameAndAddress(data);
                    return new Response { StatusCode = (int)HttpStatusCode.OK, Message = "Success", ResponseBody = combinedHotels };
                }
                return new Response { StatusCode = (int)HttpStatusCode.NoContent, Message = "No Content Found", ResponseBody = null };
            }
            catch (Exception ex)
            {
                return new Response { StatusCode = (int)HttpStatusCode.InternalServerError, Message = ex.Message, ResponseBody = null };
            }

        }

        /// <summary>
        /// Save Hotel Data with Address in database
        /// </summary>
        /// <returns></returns>
        [HttpPost("PostHotelDataFromJson")]
        public Response PostHotelsFromSupliersList()
        {
            try
            {
                if (!System.IO.File.Exists(path))
                {
                    return new Response { StatusCode = (int)HttpStatusCode.NotFound, Message = "File Not Found", ResponseBody = null };
                }

                string json = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(json))
                    return new Response { StatusCode = (int)HttpStatusCode.NoContent, Message = "No Content Found", ResponseBody = null };

                var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(json);

                if (suppliers == null || !suppliers.Any())
                {
                    return new Response { StatusCode = (int)HttpStatusCode.NoContent, Message = "No Content Found", ResponseBody = null };
                }

                var hotelsToAdd = suppliers
                  .SelectMany(supplier => supplier.Hotels)
                  .Select(hotel => new Hotel
                  {
                      UniqueIdentifier = Guid.NewGuid(),
                      Name = hotel.Name,
                      Address = hotel.Address
                  }).ToList();

                _context.Hotels.AddRange(hotelsToAdd);
                _context.SaveChanges();

                return new Response { StatusCode = (int)HttpStatusCode.Created, Message = "Records Added Successfully" };

            }
            catch (Exception ex)
            {
                return new Response { StatusCode = (int)HttpStatusCode.InternalServerError, Message = ex.Message, ResponseBody = null };
            }
        }
    }
}
