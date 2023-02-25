using System;

public class Customer
{
	public int CustomerId { get; set; }
	public string Name { get; set; }
	public string Address { get; set; }
	public string City { get; set; }
	public string Zip { get; set; }
	public string Country { get; set; }
	public string Phone { get; set; }

	public Customer(int customerId, String name, String address, String city,
		String zipcode,String country, String phone)
	{
		CustomerId = customerId;
		Name = name;
		Address = address;
		City = city;
		Zip = zipcode;
		Country = country;
		Phone = phone;
	}
}
