﻿using Core;
using DataLayer.Entities;
using DataLayer.Entities.Selling;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Context;

public class AppDbContext : IdentityDbContext<User>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) 
		: base(options) { }

	public DbSet<Warehouse> Warehouses { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<WarehouseItem> WarehousesItems { get; set; }
	public DbSet<Customer> Customers { get; set; }
	public DbSet<Loan> Loans { get; set; }
	public DbSet<LoanPayment> LoanPayments { get; set; }
	public DbSet<Receipt> Receipts { get; set; }
	public DbSet<Transaction> Transactions { get; set; }
	public DbSet<RefreshToken> RefreshTokens { get; set; }
}
