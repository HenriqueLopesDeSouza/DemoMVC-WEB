﻿// <auto-generated />
using System;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevIO.Data.Migrations
{
    [DbContext(typeof(MeuDbContext))]
    [Migration("20230129234420_primeia")]
    partial class primeia
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DevIO.Business.Models.Autor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("Documento")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Autores");
                });

            modelBuilder.Entity("DevIO.Business.Models.Endereco", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AutorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Complemento")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AutorId")
                        .IsUnique();

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("DevIO.Business.Models.Livro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AutorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Imagem")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.ToTable("Livros");
                });

            modelBuilder.Entity("DevIO.Business.Models.Endereco", b =>
                {
                    b.HasOne("DevIO.Business.Models.Autor", "Autor")
                        .WithOne("Endereco")
                        .HasForeignKey("DevIO.Business.Models.Endereco", "AutorId")
                        .IsRequired();

                    b.Navigation("Autor");
                });

            modelBuilder.Entity("DevIO.Business.Models.Livro", b =>
                {
                    b.HasOne("DevIO.Business.Models.Autor", "Autor")
                        .WithMany("Livros")
                        .HasForeignKey("AutorId")
                        .IsRequired();

                    b.Navigation("Autor");
                });

            modelBuilder.Entity("DevIO.Business.Models.Autor", b =>
                {
                    b.Navigation("Endereco")
                        .IsRequired();

                    b.Navigation("Livros");
                });
#pragma warning restore 612, 618
        }
    }
}