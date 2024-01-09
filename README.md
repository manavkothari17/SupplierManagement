## JSON Data Processing and Database Operations

This repository implements functionality to read data from a JSON file, expose an endpoint to view those records, insert the JSON data into a database, and remove duplicate data to return unique records.

### Usage

1. **Read Data from JSON File:**

   The system reads data from a specified JSON file.

2. **View Records Endpoint:**

   An API endpoint is provided to view the records read from the JSON file.

   - **Endpoint:** `/api/Supplier/GetSuppliersHotelList`
   - **HTTP Method:** `GET`

3. **Insert JSON Data into Database:**

   The system inserts the data read from the JSON file into a database.

4. **Remove Duplicate Data:**

   Duplicate records are removed from the database to ensure only unique records are retained.
