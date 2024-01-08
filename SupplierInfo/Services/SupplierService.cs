using SupplierInfo.Models;

namespace SupplierInfo.Services
{
    public class SupplierService
    {
        /// <summary>
        /// combine hotel based on name and address and return unique hotels
        /// </summary>
        /// <param name="combinedHotels"></param>
        /// <returns></returns>
        public List<Hotel> FilterHotelByNameAndAddress(List<Hotel> combinedHotels)
        {
            var filteredHotels = new List<Hotel>();
            // combinedHotels contains list of all hotels
            foreach (var hotel in combinedHotels)
            {
                var existingHotel = filteredHotels.FirstOrDefault(h =>
                    CheckIfDuplicateHotelExists(h.Name, hotel.Name) &&
                    h.Address.City == hotel.Address.City &&
                    h.Address.Country == hotel.Address.Country);

                if (existingHotel == null)
                {
                    filteredHotels.Add(hotel);
                }
            }
            return filteredHotels;
        }

        /// <summary>
        /// check if hotel already exists or not 
        /// </summary>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <returns></returns>
        public bool CheckIfDuplicateHotelExists(string name1, string name2)
        {
            var words1 = name1.Split(' ').OrderBy(w => w);
            var words2 = name2.Split(' ').OrderBy(w => w);
            return words1.SequenceEqual(words2);
        }
    }
}
