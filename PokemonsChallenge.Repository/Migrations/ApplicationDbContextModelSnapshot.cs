﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokemonsChallenge.Repository;

#nullable disable

namespace PokemonsChallenge.Repository.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("PokemonsChallenge.Domain.Entities.CapturedPokemon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CapturedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("PokemonId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrainerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PokemonId");

                    b.HasIndex("TrainerId");

                    b.ToTable("CapturedPokemons");
                });

            modelBuilder.Entity("PokemonsChallenge.Domain.Entities.Pokemon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Pokemons");
                });

            modelBuilder.Entity("PokemonsChallenge.Domain.Entities.Trainer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Trainers");
                });

            modelBuilder.Entity("PokemonsChallenge.Domain.Entities.CapturedPokemon", b =>
                {
                    b.HasOne("PokemonsChallenge.Domain.Entities.Pokemon", "Pokemon")
                        .WithMany()
                        .HasForeignKey("PokemonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokemonsChallenge.Domain.Entities.Trainer", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pokemon");

                    b.Navigation("Trainer");
                });
#pragma warning restore 612, 618
        }
    }
}
