﻿// <auto-generated />
using GenericWebservice.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace GeneratedWebService.Migrations
{
    [DbContext(typeof(EventStoreContext))]
    partial class EventStoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Domain.DomainEventBase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<Guid>("EntityId");

                    b.HasKey("Id");

                    b.ToTable("EventHistory");

                    b.HasDiscriminator<string>("Discriminator").HasValue("DomainEventBase");
                });

            modelBuilder.Entity("Domain.Posts.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Age");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Posts.CreatePostEvent", b =>
                {
                    b.HasBaseType("Domain.DomainEventBase");


                    b.ToTable("CreatePostEvent");

                    b.HasDiscriminator().HasValue("CreatePostEvent");
                });

            modelBuilder.Entity("Domain.Users.CreateUserEvent", b =>
                {
                    b.HasBaseType("Domain.DomainEventBase");


                    b.ToTable("CreateUserEvent");

                    b.HasDiscriminator().HasValue("CreateUserEvent");
                });

            modelBuilder.Entity("Domain.Users.UserUpdateAgeEvent", b =>
                {
                    b.HasBaseType("Domain.DomainEventBase");

                    b.Property<int>("Age");

                    b.ToTable("UserUpdateAgeEvent");

                    b.HasDiscriminator().HasValue("UserUpdateAgeEvent");
                });

            modelBuilder.Entity("Domain.Users.UserUpdateNameEvent", b =>
                {
                    b.HasBaseType("Domain.DomainEventBase");

                    b.Property<string>("Name");

                    b.ToTable("UserUpdateNameEvent");

                    b.HasDiscriminator().HasValue("UserUpdateNameEvent");
                });
#pragma warning restore 612, 618
        }
    }
}