using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DoctorOffice
{
  public class PatientTest : IDisposable
  {
    public PatientTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=doctor_office_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Patient.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Patient.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_TrueIfPatientsAreSame()
    {
      //Arrange, Act
      DateTime? birthday = new DateTime(1986, 5, 22);
      Patient firstPatient = new Patient("Carl Egbert", birthday, 1);
      Patient secondPatient = new Patient("Carl Egbert", birthday, 1);
      //Assert
      Assert.Equal(firstPatient, secondPatient);
    }

    [Fact]
    public void Test_SavesToDatabase()
    {
      //Arrange
      DateTime? birthday = new DateTime(1986, 5, 22);
      Patient testPatient = new Patient("Carl Egbert", birthday, 1);
      //Act
      testPatient.Save();
      List<Patient> result = Patient.GetAll();
      List<Patient> expectedResult = new List<Patient>{testPatient};
      //Assert
      Assert.Equal(expectedResult, result);
    }
    [Fact]
    public void Test_PatientAssignedId()
    {
      //Arrange
      DateTime? birthday = new DateTime(1986, 5, 22);
      Patient testPatient = new Patient("Carl Egbert", birthday, 1);
      //Act
      testPatient.Save();
      Patient savedPatient = Patient.GetAll()[0];
      int result = savedPatient.GetId();
      int expectedResult = testPatient.GetId();
      //Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Test_FindsPatientInDatabase()
    {
      //Arrange
      DateTime? birthday = new DateTime(1986, 5, 22);
      Patient testPatient = new Patient("Carl Egbert", birthday, 1);
      testPatient.Save();
      //Act
      Patient foundPatient = Patient.Find(testPatient.GetId());
      //Assert
      Assert.Equal(testPatient, foundPatient);
    }



  }
}
