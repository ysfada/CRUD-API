using Application.Customers.Model;
using Application.Orders.Model;
using Application.Products.Model;
using AutoMapper;
using Domain.Entities;

namespace Application.Common
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<Customer, CustomerDto>().ReverseMap();
			CreateMap<Customer, CreateCustomerDto>().ReverseMap();
			CreateMap<Product, ProductDto>().ReverseMap();
			CreateMap<Product, CreateProductDto>().ReverseMap();
			CreateMap<Order, OrderDto>().ReverseMap();
			CreateMap<Order, CreateOrderDto>().ReverseMap();
		}
	}
}