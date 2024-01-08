using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplierInfo.Models
{
    public class Hotel
    {
        public Guid UniqueIdentifier { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }
}
