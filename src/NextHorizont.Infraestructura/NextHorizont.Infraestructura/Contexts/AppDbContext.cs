using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NextHorizont.Domain.Entities;
using NextHorizont.Domain.Enums;

namespace NextHorizont.Infraestructura.Contexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<CashShift> CashShifts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<LogicalPrinter> LogicalPrinters { get; set; }

    public virtual DbSet<Modifier> Modifiers { get; set; }

    public virtual DbSet<ModifierGroup> ModifierGroups { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderLine> OrderLines { get; set; }

    public virtual DbSet<OrderLineModifier> OrderLineModifiers { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Stay> Stays { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("areas_pkey");

            entity.ToTable("areas");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Areas)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("areas_tenant_id_fkey");
        });

        modelBuilder.Entity<CashShift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cash_shifts_pkey");

            entity.ToTable("cash_shifts");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ClosingBalance)
                .HasPrecision(10, 2)
                .HasColumnName("closing_balance");
            entity.Property(e => e.ClosingTime).HasColumnName("closing_time");
            entity.Property(e => e.DenominationsClosing)
                .HasColumnType("jsonb")
                .HasColumnName("denominations_closing");
            entity.Property(e => e.OpeningBalance)
                .HasPrecision(10, 2)
                .HasColumnName("opening_balance");
            entity.Property(e => e.OpeningTime)
                .HasDefaultValueSql("now()")
                .HasColumnName("opening_time");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasConversion<string>()
                .HasColumnName("status");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Tenant).WithMany(p => p.CashShifts)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cash_shifts_tenant_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.CashShifts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cash_shifts_user_id_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Categories)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("categories_tenant_id_fkey");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("guests_pkey");

            entity.ToTable("guests");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.IdentificationDocument)
                .HasMaxLength(50)
                .HasColumnName("identification_document");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Guests)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("guests_tenant_id_fkey");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("locations_pkey");

            entity.ToTable("locations");

            entity.HasIndex(e => new { e.TenantId, e.AreaId }, "idx_locations_tenant_area");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.Capacity)
                .HasDefaultValue(4)
                .HasColumnName("capacity");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.RoomCategory)
                .HasMaxLength(50)
                .HasColumnName("room_category");
            entity.Property(e => e.RoomRatePerNight)
                .HasPrecision(10, 2)
                .HasColumnName("room_rate_per_night");
            entity.Property(e => e.RoomStatus)
                .HasMaxLength(20)
                .HasConversion<string>()
                .HasColumnName("room_status");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasConversion<string>()
                .HasColumnName("type");

            entity.HasOne(d => d.Area).WithMany(p => p.Locations)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("locations_area_id_fkey");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Locations)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("locations_tenant_id_fkey");
        });

        modelBuilder.Entity<LogicalPrinter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("logical_printers_pkey");

            entity.ToTable("logical_printers");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .HasColumnName("ip_address");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Tenant).WithMany(p => p.LogicalPrinters)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("logical_printers_tenant_id_fkey");
        });

        modelBuilder.Entity<Modifier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("modifiers_pkey");

            entity.ToTable("modifiers");

            entity.HasIndex(e => e.ModifierGroupId, "idx_modifiers_group");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ExtraPrice)
                .HasPrecision(10, 2)
                .HasDefaultValue(0m)
                .HasColumnName("extra_price");
            entity.Property(e => e.ModifierGroupId).HasColumnName("modifier_group_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.ModifierGroup).WithMany(p => p.Modifiers)
                .HasForeignKey(d => d.ModifierGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("modifiers_modifier_group_id_fkey");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Modifiers)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("modifiers_tenant_id_fkey");
        });

        modelBuilder.Entity<ModifierGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("modifier_groups_pkey");

            entity.ToTable("modifier_groups");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.MaxSelections)
                .HasDefaultValue(1)
                .HasColumnName("max_selections");
            entity.Property(e => e.MinSelections)
                .HasDefaultValue(0)
                .HasColumnName("min_selections");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Tenant).WithMany(p => p.ModifierGroups)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("modifier_groups_tenant_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.HasIndex(e => new { e.TenantId, e.LocationId }, "idx_orders_tenant_location");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Origin)
                .HasMaxLength(30)
                .HasDefaultValueSql("'pos'::character varying")
                .HasConversion<string>()
                .HasColumnName("origin");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasConversion<string>()
                .HasColumnName("status");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.TipAmount)
                .HasPrecision(10, 2)
                .HasDefaultValue(0m)
                .HasColumnName("tip_amount");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasDefaultValue(0m)
                .HasColumnName("total");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Location).WithMany(p => p.Orders)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("orders_location_id_fkey");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_tenant_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("orders_user_id_fkey");
        });

        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_lines_pkey");

            entity.ToTable("order_lines");

            entity.HasIndex(e => e.OrderId, "idx_order_lines_order");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderLines)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_lines_order_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderLines)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_lines_product_id_fkey");

            entity.HasOne(d => d.Tenant).WithMany(p => p.OrderLines)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_lines_tenant_id_fkey");
        });

        modelBuilder.Entity<OrderLineModifier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_line_modifiers_pkey");

            entity.ToTable("order_line_modifiers");

            entity.HasIndex(e => e.OrderLineId, "idx_order_line_modifiers_line");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.ExtraPrice)
                .HasPrecision(10, 2)
                .HasDefaultValue(0m)
                .HasColumnName("extra_price");
            entity.Property(e => e.ModifierId).HasColumnName("modifier_id");
            entity.Property(e => e.OrderLineId).HasColumnName("order_line_id");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Modifier).WithMany(p => p.OrderLineModifiers)
                .HasForeignKey(d => d.ModifierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_line_modifiers_modifier_id_fkey");

            entity.HasOne(d => d.OrderLine).WithMany(p => p.OrderLineModifiers)
                .HasForeignKey(d => d.OrderLineId)
                .HasConstraintName("order_line_modifiers_order_line_id_fkey");

            entity.HasOne(d => d.Tenant).WithMany(p => p.OrderLineModifiers)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_line_modifiers_tenant_id_fkey");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.HasIndex(e => new { e.TenantId, e.PaymentMethodId }, "idx_payments_tenant_method");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CashShiftId).HasColumnName("cash_shift_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.ReferenceCode)
                .HasMaxLength(100)
                .HasColumnName("reference_code");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.CashShift).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CashShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payments_cash_shift_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payments_order_id_fkey");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payments_payment_method_id_fkey");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Payments)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payments_tenant_id_fkey");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_methods_pkey");

            entity.ToTable("payment_methods");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.RequiresReference)
                .HasDefaultValue(false)
                .HasColumnName("requires_reference");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Tenant).WithMany(p => p.PaymentMethods)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_methods_tenant_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.HasIndex(e => new { e.TenantId, e.CategoryId }, "idx_products_tenant_category");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Availability)
                .HasMaxLength(20)
                .HasDefaultValueSql("'all_day'::character varying")
                .HasColumnName("availability");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LogicalPrinterId).HasColumnName("logical_printer_id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_category_id_fkey");

            entity.HasOne(d => d.LogicalPrinter).WithMany(p => p.Products)
                .HasForeignKey(d => d.LogicalPrinterId)
                .HasConstraintName("products_logical_printer_id_fkey");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Products)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_tenant_id_fkey");

            entity.HasMany(d => d.ModifierGroups).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductModifierGroup",
                    r => r.HasOne<ModifierGroup>().WithMany()
                        .HasForeignKey("ModifierGroupId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("product_modifier_groups_modifier_group_id_fkey"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("product_modifier_groups_product_id_fkey"),
                    j =>
                    {
                        j.HasKey("ProductId", "ModifierGroupId").HasName("product_modifier_groups_pkey");
                        j.ToTable("product_modifier_groups");
                        j.IndexerProperty<Guid>("ProductId").HasColumnName("product_id");
                        j.IndexerProperty<Guid>("ModifierGroupId").HasColumnName("modifier_group_id");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PermissionsJson)
                .HasColumnType("jsonb")
                .HasColumnName("permissions_json");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Roles)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("roles_tenant_id_fkey");
        });

        modelBuilder.Entity<Stay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("stays_pkey");

            entity.ToTable("stays");

            entity.HasIndex(e => e.LocationId, "idx_stays_location");

            entity.HasIndex(e => new { e.TenantId, e.CheckInDate, e.CheckOutDate }, "idx_stays_tenant_dates");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CheckInDate).HasColumnName("check_in_date");
            entity.Property(e => e.CheckOutDate).HasColumnName("check_out_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.GuestId).HasColumnName("guest_id");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.MasterOrderId).HasColumnName("master_order_id");
            entity.Property(e => e.NightlyRate)
                .HasPrecision(10, 2)
                .HasColumnName("nightly_rate");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasConversion<string>()
                .HasColumnName("status");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Guest).WithMany(p => p.Stays)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stays_guest_id_fkey");

            entity.HasOne(d => d.Location).WithMany(p => p.Stays)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stays_location_id_fkey");

            entity.HasOne(d => d.MasterOrder).WithMany(p => p.Stays)
                .HasForeignKey(d => d.MasterOrderId)
                .HasConstraintName("stays_master_order_id_fkey");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Stays)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stays_tenant_id_fkey");
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tenants_pkey");

            entity.ToTable("tenants");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ActiveModules)
                .HasDefaultValueSql("'{}'::jsonb")
                .HasColumnType("jsonb")
                .HasColumnName("active_modules");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.OrgType)
                .HasMaxLength(30)
                .HasDefaultValueSql("'restaurant'::character varying")
                .HasColumnName("org_type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => new { e.TenantId, e.RoleId }, "idx_users_tenant_role");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_role_id_fkey");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Users)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_tenant_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
