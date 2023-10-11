using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Telescope_Traking_Delivery.Models
{
    public partial class DB_TRACKER2Context : DbContext
    {
        public DB_TRACKER2Context()
        {
        }

        public DB_TRACKER2Context(DbContextOptions<DB_TRACKER2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditTracking> AuditTrackings { get; set; } = null!;
        public virtual DbSet<Cancelacion> Cancelacions { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<Negocio> Negocios { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderTracking> OrderTrackings { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<RolMenu> RolMenus { get; set; } = null!;
        public virtual DbSet<Transport> Transports { get; set; } = null!;
        public virtual DbSet<Transportist> Transportists { get; set; } = null!;
        public virtual DbSet<TypeOrder> TypeOrders { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=(local); DataBase=DB_TRACKER2;Integrated Security=true");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditTracking>(entity =>
            {
                entity.HasKey(e => e.IdAuditTracking)
                    .HasName("PK_ AuditTracking");

                entity.ToTable("AuditTracking");

                entity.Property(e => e.IdAuditTracking).HasColumnName("idAuditTracking");

                entity.Property(e => e.Action)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("action");

                entity.Property(e => e.DtChange)
                    .HasColumnType("datetime")
                    .HasColumnName("dtChange");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdRegister).HasColumnName("idRegister");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Observation)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("observation");

                entity.Property(e => e.TableName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("tableName");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.AuditTrackings)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("AuditTracking_fk0");
            });

            modelBuilder.Entity<Cancelacion>(entity =>
            {
                entity.HasKey(e => e.IdCancelation);

                entity.ToTable("Cancelacion");

                entity.Property(e => e.IdCancelation).HasColumnName("idCancelation");

                entity.Property(e => e.Caducidad)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Entrega)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Factura)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdClient).HasColumnName("idClient");

                entity.Property(e => e.IdOrders).HasColumnName("idOrders");

                entity.Property(e => e.Lote)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Motivo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OriginalResto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pedido)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Producto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sku)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("SKU");

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.Cancelacions)
                    .HasForeignKey(d => d.IdClient)
                    .HasConstraintName("Cancelacion_fk0");

                entity.HasOne(d => d.IdOrdersNavigation)
                    .WithMany(p => p.Cancelacions)
                    .HasForeignKey(d => d.IdOrders)
                    .HasConstraintName("Cancel_fk1");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient)
                    .HasName("PK_CLIENT");

                entity.ToTable("Client");

                entity.Property(e => e.IdClient).HasColumnName("idClient");

                entity.Property(e => e.AddressClient)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("addressClient");

                entity.Property(e => e.ClientName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("clientName");

                entity.Property(e => e.CodeClient)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("codeClient");

                entity.Property(e => e.ContactAliases)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("contactAliases");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rfc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("rfc");

                entity.Property(e => e.TypeClient)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("typeClient");

                entity.Property(e => e.TypeCredit)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("typeCredit");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.IdMenu);

                entity.ToTable("Menu");

                entity.Property(e => e.IdMenu).HasColumnName("idMenu");

                entity.Property(e => e.Controlador)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("controlador");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Icono)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("icono");

                entity.Property(e => e.IdMenuPadre).HasColumnName("idMenuPadre");

                entity.Property(e => e.PaginaAccion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("paginaAccion");

                entity.HasOne(d => d.IdMenuPadreNavigation)
                    .WithMany(p => p.InverseIdMenuPadreNavigation)
                    .HasForeignKey(d => d.IdMenuPadre)
                    .HasConstraintName("FK__Menu__idMenuPadr__5FB337D6");
            });

            modelBuilder.Entity<Negocio>(entity =>
            {
                entity.HasKey(e => e.IdNegocio);

                entity.ToTable("Negocio");

                entity.Property(e => e.IdNegocio)
                    .ValueGeneratedNever()
                    .HasColumnName("idNegocio");

                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("direccion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.NombreLogo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombreLogo");

                entity.Property(e => e.NumeroDocumento)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("numeroDocumento");

                entity.Property(e => e.PorcentajeImpuesto)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("porcentajeImpuesto");

                entity.Property(e => e.SimboloMoneda)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("simboloMoneda");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono");

                entity.Property(e => e.UrlLogo)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("urlLogo");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.IdOrders);

                entity.Property(e => e.IdOrders).HasColumnName("idOrders");

                entity.Property(e => e.AccidentInRoute)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Boxes).HasColumnName("boxes");

                entity.Property(e => e.Confirmation)
                    .HasColumnType("datetime")
                    .HasColumnName("confirmation");

                entity.Property(e => e.DeliveryStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("deliveryStatus");

                entity.Property(e => e.Destination)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("destination");

                entity.Property(e => e.DtAppointedForShipment)
                    .HasColumnType("datetime")
                    .HasColumnName("DT_AppointedForShipment");

                entity.Property(e => e.DtArrivalToUnload)
                    .HasColumnType("datetime")
                    .HasColumnName("DT_ArrivalToUnload");

                entity.Property(e => e.DtDeliveryAppointment)
                    .HasColumnType("datetime")
                    .HasColumnName("DT_DeliveryAppointment");

                entity.Property(e => e.DtLoadingEnd)
                    .HasColumnType("datetime")
                    .HasColumnName("DT_LoadingEnd");

                entity.Property(e => e.DtOfBoarding)
                    .HasColumnType("datetime")
                    .HasColumnName("DT_OfBoarding");

                entity.Property(e => e.DtOfChargingStart)
                    .HasColumnType("datetime")
                    .HasColumnName("DT_OfChargingStart");

                entity.Property(e => e.DtOfDepartureFromSite)
                    .HasColumnType("datetime")
                    .HasColumnName("DT_OfDepartureFromSite");

                entity.Property(e => e.DtOfFarrivalOfUnit)
                    .HasColumnType("datetime")
                    .HasColumnName("DT_OfFarrivalOfUnit");

                entity.Property(e => e.EndDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("endDateTime");

                entity.Property(e => e.ExtraRouteIndicator)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdClient).HasColumnName("idClient");

                entity.Property(e => e.IdTransport).HasColumnName("idTransport");

                entity.Property(e => e.IdTransportist).HasColumnName("idTransportist");

                entity.Property(e => e.IdTypeOrders).HasColumnName("idTypeOrders");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.IncidentInShipmentWh)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("IncidentInShipmentWH");

                entity.Property(e => e.IncidentOfArrivalCargoTr)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("IncidentOfArrivalCargoTR");

                entity.Property(e => e.IncidentOfArrivalClientTr)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("IncidentOfArrivalClientTR");

                entity.Property(e => e.Obsevations)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.OnTime)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("onTime");

                entity.Property(e => e.OrdersDelivery)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OtOfBoarding)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OT_OfBoarding");

                entity.Property(e => e.OtOfChargingStart)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OT_OfChargingStart");

                entity.Property(e => e.Pieces).HasColumnName("pieces");

                entity.Property(e => e.RoadTimeIndicators)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.ShippingObservations)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("shippingObservations");

                entity.Property(e => e.SiteLoading)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("siteLoading");

                entity.Property(e => e.VehicleControl)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("vehicleControl");

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdClient)
                    .HasConstraintName("Orders_fk0");

                entity.HasOne(d => d.IdTransportNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdTransport)
                    .HasConstraintName("Orders_fk2");

                entity.HasOne(d => d.IdTransportistNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdTransportist)
                    .HasConstraintName("Orders_fk3");

                entity.HasOne(d => d.IdTypeOrdersNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdTypeOrders)
                    .HasConstraintName("Orders_fk1");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Orders_fk4");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.IdOrderDetails)
                    .HasName("PK_ OrderDetails");

                entity.Property(e => e.IdOrderDetails).HasColumnName("idOrderDetails");

                entity.Property(e => e.DtDelivery)
                    .HasColumnType("datetime")
                    .HasColumnName("dtDelivery");

                entity.Property(e => e.DtPedido)
                    .HasColumnType("datetime")
                    .HasColumnName("dtPedido");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Fragile)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("fragile");

                entity.Property(e => e.IdClient).HasColumnName("idClient");

                entity.Property(e => e.IdOrders).HasColumnName("idOrders");

                entity.Property(e => e.Observation)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("observation");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.TotalCost)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("totalCost");

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.IdClient)
                    .HasConstraintName("OrderDetails_fk0");

                entity.HasOne(d => d.IdOrdersNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.IdOrders)
                    .HasConstraintName("OrderDetails_fk1");
            });

            modelBuilder.Entity<OrderTracking>(entity =>
            {
                entity.HasKey(e => e.IdOrderTracking)
                    .HasName("PK_ idOrderTracking");

                entity.ToTable("OrderTracking");

                entity.Property(e => e.IdOrderTracking).HasColumnName("idOrderTracking");

                entity.Property(e => e.DtUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("dtUpdate");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdOrders).HasColumnName("idOrders");

                entity.Property(e => e.Location)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("location");

                entity.Property(e => e.Observation)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("observation");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.HasOne(d => d.IdOrdersNavigation)
                    .WithMany(p => p.OrderTrackings)
                    .HasForeignKey(d => d.IdOrders)
                    .HasConstraintName("OrderTracking_fk0");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK_ROL");

                entity.ToTable("Rol");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<RolMenu>(entity =>
            {
                entity.HasKey(e => e.IdRolMenu);

                entity.ToTable("RolMenu");

                entity.Property(e => e.IdRolMenu).HasColumnName("idRolMenu");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdMenu).HasColumnName("idMenu");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.HasOne(d => d.IdMenuNavigation)
                    .WithMany(p => p.RolMenus)
                    .HasForeignKey(d => d.IdMenu)
                    .HasConstraintName("RolMenu_fk1");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.RolMenus)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("RolMenu_fk0");
            });

            modelBuilder.Entity<Transport>(entity =>
            {
                entity.HasKey(e => e.IdTransport)
                    .HasName("PK_TRANSPORT");

                entity.ToTable("Transport");

                entity.Property(e => e.IdTransport).HasColumnName("idTransport");

                entity.Property(e => e.Capacity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Details)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("details");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Plate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("plate");

                entity.Property(e => e.TransportName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("transportName");

                entity.Property(e => e.UnitType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("unitType");

                entity.Property(e => e.UrlImagen)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("urlImagen");
            });

            modelBuilder.Entity<Transportist>(entity =>
            {
                entity.HasKey(e => e.IdTransportist);

                entity.ToTable("Transportist");

                entity.Property(e => e.IdTransportist).HasColumnName("idTransportist");

                entity.Property(e => e.Age)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("age");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.License)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("license");

                entity.Property(e => e.NameTransportist)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nameTransportist");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.TypeLicense)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("typeLicense");

                entity.Property(e => e.UrlFoto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("urlFoto");
            });

            modelBuilder.Entity<TypeOrder>(entity =>
            {
                entity.HasKey(e => e.IdTypeOrders)
                    .HasName("PK_TYPEOrders");

                entity.ToTable("typeOrders");

                entity.Property(e => e.IdTypeOrders).HasColumnName("idTypeOrders");

                entity.Property(e => e.Clasification)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("clasification");

                entity.Property(e => e.DescriptionOrder)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descriptionOrder");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LocalForaneo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NormalOrFast)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Promotion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("promotion");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK_ Usuario");

                entity.ToTable("Usuario");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Clave)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("clave");

                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.EsActivo).HasColumnName("esActivo");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.NombreFoto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombreFoto");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("telefono");

                entity.Property(e => e.UrlFoto)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("urlFoto");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("Usuario_fk0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
