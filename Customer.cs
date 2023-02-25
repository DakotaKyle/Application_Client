using System;

public abstract class Customer
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
}
