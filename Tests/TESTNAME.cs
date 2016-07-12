using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DoctorOffice.Objects
{
  public class DoctorOfficeTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=doctor_office_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      ****.DeleteAll();
    }
  }
}
