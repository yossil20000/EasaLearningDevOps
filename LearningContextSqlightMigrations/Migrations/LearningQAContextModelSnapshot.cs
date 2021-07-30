﻿// <auto-generated />
using System;
using LearningQA.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LearningContextSqlightMigrations.Migrations
{
    [DbContext(typeof(LearningQAContext))]
    partial class LearningQAContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0-preview.6.21352.1");

            modelBuilder.Entity("LearningQA.Shared.Entities.AnswareOption<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Answer<int>Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsTrue")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TenantId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Answer<int>Id");

                    b.ToTable("AnswareOptions");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Answer<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAnswered")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsMarked")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("QUestionSqlId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TenantId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Test<QUestionSql, int>Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("QUestionSqlId");

                    b.HasIndex("Test<QUestionSql, int>Id");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Category<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Person<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("IdNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PreferanceId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("IdNumber")
                        .IsUnique();

                    b.HasIndex("PreferanceId");

                    b.ToTable("Person");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Gilon",
                            Email = "Yos@gmail.com",
                            IdNumber = "135792468",
                            Name = "yosef Levy",
                            Password = "12345@12345",
                            Phone = "+97249984222"
                        },
                        new
                        {
                            Id = 2,
                            Address = "Gilon",
                            Email = "Yoni@gmail.com",
                            IdNumber = "246813579",
                            Name = "Yoni Levy",
                            Password = "12345@12345",
                            Phone = "+97249984220"
                        },
                        new
                        {
                            Id = 4,
                            Address = "Gilon",
                            Email = "Tal@gmail.com",
                            IdNumber = "1502626",
                            Name = "Tal Levy",
                            Password = "12345@12345",
                            Phone = "+97249984226"
                        });
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Preferance<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("HUE")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Theme")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Preferance<int>");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.QUestionSql", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AnswerExplain")
                        .HasColumnType("TEXT");

                    b.Property<int>("AnswerType")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Mark")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Question")
                        .HasColumnType("TEXT");

                    b.Property<string>("QuestionNumber")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TestItem<QUestionSql, int>Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TestItem<QUestionSql, int>Id");

                    b.ToTable("QUestionSqls");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.QuestionOption<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsTrue")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("QUestionSqlId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TenantId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("QUestionSqlId");

                    b.ToTable("QuestionOptions");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Supplement<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<int>("ContentType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OriginalContent")
                        .HasColumnType("TEXT");

                    b.Property<int>("OriginalcontentType")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("QUestionSqlId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RotateContent")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TenantId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("QUestionSqlId");

                    b.ToTable("Supplements");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Test<LearningQA.Shared.Entities.QUestionSql, int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateFinish")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Mark")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Person<int>Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TestItemId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Person<int>Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.TestItem<LearningQA.Shared.Entities.QUestionSql, int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.Property<string>("Chapter")
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExamRemarks")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .HasColumnType("TEXT");

                    b.Property<int>("TestQuestionMarking")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Category", "Chapter", "Subject", "Version")
                        .IsUnique();

                    b.ToTable("TestItems");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.AnswareOption<int>", b =>
                {
                    b.HasOne("LearningQA.Shared.Entities.Answer<int>", null)
                        .WithMany("SelectedAnswer")
                        .HasForeignKey("Answer<int>Id");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Answer<int>", b =>
                {
                    b.HasOne("LearningQA.Shared.Entities.QUestionSql", "QUestionSql")
                        .WithMany()
                        .HasForeignKey("QUestionSqlId");

                    b.HasOne("LearningQA.Shared.Entities.Test<LearningQA.Shared.Entities.QUestionSql, int>", null)
                        .WithMany("Answers")
                        .HasForeignKey("Test<QUestionSql, int>Id");

                    b.Navigation("QUestionSql");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Person<int>", b =>
                {
                    b.HasOne("LearningQA.Shared.Entities.Preferance<int>", "Preferance")
                        .WithMany()
                        .HasForeignKey("PreferanceId");

                    b.Navigation("Preferance");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.QUestionSql", b =>
                {
                    b.HasOne("LearningQA.Shared.Entities.TestItem<LearningQA.Shared.Entities.QUestionSql, int>", null)
                        .WithMany("Questions")
                        .HasForeignKey("TestItem<QUestionSql, int>Id");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.QuestionOption<int>", b =>
                {
                    b.HasOne("LearningQA.Shared.Entities.QUestionSql", null)
                        .WithMany("Options")
                        .HasForeignKey("QUestionSqlId");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Supplement<int>", b =>
                {
                    b.HasOne("LearningQA.Shared.Entities.QUestionSql", null)
                        .WithMany("Supplements")
                        .HasForeignKey("QUestionSqlId");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Test<LearningQA.Shared.Entities.QUestionSql, int>", b =>
                {
                    b.HasOne("LearningQA.Shared.Entities.Person<int>", null)
                        .WithMany("Tests")
                        .HasForeignKey("Person<int>Id");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Answer<int>", b =>
                {
                    b.Navigation("SelectedAnswer");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Person<int>", b =>
                {
                    b.Navigation("Tests");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.QUestionSql", b =>
                {
                    b.Navigation("Options");

                    b.Navigation("Supplements");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.Test<LearningQA.Shared.Entities.QUestionSql, int>", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("LearningQA.Shared.Entities.TestItem<LearningQA.Shared.Entities.QUestionSql, int>", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
