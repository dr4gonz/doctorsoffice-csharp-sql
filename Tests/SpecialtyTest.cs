using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DoctorOffice
{
  public class SpecialtyTest : IDisposable
  {
    public SpecialtyTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=doctor_office_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Specialty.DeleteAll();
    }

    [Fact]
    public void Specialty_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Specialty.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Specialty_ReturnsTrueIfObjectsSame()
    {
      //Arrange, Act
      Specialty firstSpecialty = new Specialty("Radiology");
      Specialty secondSpecialty = new Specialty("Radiology");
      //Assert
      Assert.Equal(firstSpecialty, secondSpecialty);
    }
    [Fact]
    public void Specialty_SavesToDatabase()
    {
      //Arrange
      Specialty testSpecialty = new Specialty("Radiology");
      //Act
      testSpecialty.Save();
      List<Specialty> result = Specialty.GetAll();
      List<Specialty> expectedResult = new List<Specialty> {testSpecialty};
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Specialty_AssignsIdWhenSaved()
    {
      //Arrange
      Specialty testSpecialty = new Specialty("Radiology");
      //Act
      testSpecialty.Save();
      Specialty savedSpecialty = Specialty.GetAll()[0];
      int expectedResult = testSpecialty.GetId();
      int result = savedSpecialty.GetId();
      //Assert
      Assert.Equal(expectedResult, result);
    }
    [Fact]
    public void Specialty_Find()
    {
      //Arrange
      Specialty testSpecialty = new Specialty("Radiology");
      //Act
      testSpecialty.Save();
      Specialty foundSpecialty = Specialty.Find(testSpecialty.GetId());
      //Assert
      Assert.Equal(testSpecialty, foundSpecialty);
    }
  }
}
