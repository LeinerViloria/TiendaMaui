﻿// <auto-generated />
using System;
using Backend.Access;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(TiendaContext))]
    partial class TiendaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Backend.Entities.Client", b =>
                {
                    b.Property<int>("Rowid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Rowid"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.HasKey("Rowid");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Backend.Entities.Product", b =>
                {
                    b.Property<int>("Rowid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Rowid"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.Property<double>("Precio")
                        .HasColumnType("double precision");

                    b.Property<string>("RutaImagen")
                        .HasColumnType("text");

                    b.Property<int>("Stock")
                        .HasColumnType("integer");

                    b.HasKey("Rowid");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Backend.Entities.Purchase", b =>
                {
                    b.Property<int>("Rowid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Rowid"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RowidCliente")
                        .HasColumnType("integer");

                    b.Property<int>("RowidProducto")
                        .HasColumnType("integer");

                    b.HasKey("Rowid");

                    b.HasIndex("RowidCliente");

                    b.HasIndex("RowidProducto");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("Backend.Entities.Purchase", b =>
                {
                    b.HasOne("Backend.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("RowidCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("RowidProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Product");
                });
#pragma warning restore 612, 618
        }
    }
}
