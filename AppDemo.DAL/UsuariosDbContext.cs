using AppDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppDemo.DAL
{
    public class UsuariosDbContext : DbContext
    {
        public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options)
            : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioCSV> UsuariosCsv { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entidad => 
            {
                entidad.ToTable("usuarios");

                entidad.HasKey(e => e.IdUsuario);

                entidad.Property(e => e.IdUsuario)
                .HasColumnName("id_usuario")
                .ValueGeneratedOnAdd();

                entidad.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(65)");

                entidad.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasColumnType("varchar(10)");

                entidad.Property(e => e.Correo)
                    .HasColumnName("correo_electronico")
                    .HasColumnType("varchar(255)");

                entidad.Property(e => e.PasswordHash)
                    .HasColumnName("password_hash")
                    .HasColumnType("text");

                entidad.Property(e => e.PasswordSalt)
                    .HasColumnName("password_salt")
                    .HasColumnType("text");

            });

            modelBuilder.Entity<UsuarioCSV>(entidad =>
            {
                entidad.ToTable("usuarios_csv");

                entidad.HasKey(e => e.IdUsuario);

                entidad.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario").ValueGeneratedNever();

                entidad.Property(e => e.HorasTrabajadas)
                    .HasColumnName("horas_trabajadas");

                entidad.Property(e => e.SueldoPorHora)
                    .HasColumnName("sueldo_por_hora")
                    .HasColumnType("money");

                entidad.Property(e => e.FechaProximoPago)
                    .HasColumnName("fecha_prox_pago");
            });

        }
    }
}
