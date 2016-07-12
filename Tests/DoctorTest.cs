using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DoctorOffice
{
  public class DoctorTest : IDisposable
  {
    public DoctorTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=doctor_office_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Doctor.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Doctor.GetAll().Count;
      //Assert
      Assert.Equal(0,result);
    }

    [Fact]
    public void Test_Equal_TrueIfDoctorsAreSame()
    {
      //Arrange, Act
      Doctor firstDoctor = new Doctor("Matt Reyes", 1);
      Doctor secondDoctor = new Doctor("Matt Reyes", 1);
      //Assert
      Assert.Equal(firstDoctor, secondDoctor);
    }

    [Fact]
    public void Test_SavesToDatabase()
    {
      //Arrange
      Doctor testDoctor = new Doctor("Matt Reyes", 1);

      //Act
      testDoctor.Save();
      List<Doctor> allDoctors = Doctor.GetAll();
      List<Doctor> expectedResult = new List<Doctor>{testDoctor};

      //Assert
      Assert.Equal(expectedResult, allDoctors);
    }

    [Fact]
    public void Test_DoctorsAssignedId()
    {
      //Arrange
      Doctor testDoctor = new Doctor("Matt Reyes", 1);
      //Act
      testDoctor.Save();
      Doctor savedDoctor = Doctor.GetAll()[0];
      int expectedResult = testDoctor.GetId();
      int testId = savedDoctor.GetId();

      //Assert
      Assert.Equal(expectedResult, testId);
    }
    
    [Fact]
    public void Test_FindsDoctorInDatabase()
    {
      //Arrange
      Doctor testDoctor = new Doctor("Matt Reyes", 1);
      //Act
      testDoctor.Save();
      Doctor foundDoctor = Doctor.Find(testDoctor.GetId());
      //Assert
      Assert.Equal(testDoctor, foundDoctor);
    }


  }
}
