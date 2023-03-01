using System;


public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int AddressId { get; set; }
    public string City { get; set; }
    public int CityId { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }
    public int CountryId { get; set; }
    public string Phone { get; set; }

    public Customer(String name, int customerId, String address, int addressId, String city, int cityId,
            String zipcode, String country, int countryId, String phone)
    {
        Name = name;
        CustomerId = customerId;
        Address = address;
        AddressId = addressId;
        City = city;
        CityId = cityId;
        Zip = zipcode;
        Country = country;
        CountryId = countryId;
        Phone = phone;
    }
}
