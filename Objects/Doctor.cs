using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DoctorOffice
{
  public class Doctor
  {
    private string _name;
    private int _specialtyId;
    private int _id;

    public Doctor(string name, int specialtyId, int id = 0)
    {
      _name = name;
      _specialtyId = specialtyId;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }
    public int GetSpecialtyId()
    {
      return _specialtyId;
    }
    public int GetId()
    {
      return _id;
    }

    public static List<Doctor> GetAll()
    {
      List<Doctor> allDoctors = new List<Doctor>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM doctors;", conn);
      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        string doctorName = rdr.GetString(0);
        int doctorSpecialty = rdr.GetInt32(1);
        int doctorId = rdr.GetInt32(2);
        Doctor newDoctor = new Doctor(doctorName, doctorSpecialty, doctorId);
        allDoctors.Add(newDoctor);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allDoctors;
    }
    public List<Patient> GetAllPatients()
    {
      List<Patient> allPatients = new List<Patient>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patients WHERE doctor_id = @DoctorId;", conn);
      SqlParameter doctorIdParameter = new SqlParameter();
      doctorIdParameter.ParameterName = "@DoctorId";
      doctorIdParameter.Value = _id;
      cmd.Parameters.Add(doctorIdParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        string patientName = rdr.GetString(0);
        DateTime? patientBirthday = rdr.GetDateTime(1);
        int doctorId = rdr.GetInt32(2);
        int patientId = rdr.GetInt32(3);
        Patient newPatient = new Patient(patientName, patientBirthday, doctorId, patientId);
        allPatients.Add(newPatient);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allPatients;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM doctors;", conn);
      cmd.ExecuteNonQuery();
    }

    public override bool Equals(System.Object otherDoctor)
    {
      if(!(otherDoctor is Doctor))
      {
        return false;
      }
      else
      {
        Doctor newDoctor = (Doctor) otherDoctor;
        bool nameEquality = (this.GetName() == newDoctor.GetName());
        bool specialtyIdEquality = (this.GetSpecialtyId() == newDoctor.GetSpecialtyId());
        bool idEquality = (this.GetId() == newDoctor.GetId());
        return (nameEquality && specialtyIdEquality && idEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO doctors (name, specialty_id) OUTPUT INSERTED.id VALUES (@DoctorName, @SpecialtyId);", conn);

      SqlParameter doctorNameParameter = new SqlParameter();
      doctorNameParameter.ParameterName = "@DoctorName";
      doctorNameParameter.Value = this.GetName();

      SqlParameter specialtyIdParameter = new SqlParameter();
      specialtyIdParameter.ParameterName = "@SpecialtyId";
      specialtyIdParameter.Value = this.GetSpecialtyId();

      cmd.Parameters.Add(doctorNameParameter);
      cmd.Parameters.Add(specialtyIdParameter);
      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }

    public static Doctor Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM doctors WHERE id = @DoctorId;", conn);
      SqlParameter doctorIdParameter = new SqlParameter();
      doctorIdParameter.ParameterName = "@DoctorId";
      doctorIdParameter.Value = id.ToString();
      cmd.Parameters.Add(doctorIdParameter);
      rdr = cmd.ExecuteReader();

      int foundDoctorId = 0;
      string foundDoctorName = null;
      int foundDoctorSpecialtyId = 0;

      while(rdr.Read())
      {
        foundDoctorName = rdr.GetString(0);
        foundDoctorSpecialtyId = rdr.GetInt32(1);
        foundDoctorId = rdr.GetInt32(2);
      }
      Doctor foundDoctor = new Doctor(foundDoctorName, foundDoctorSpecialtyId, foundDoctorId);

      if(rdr !=null) rdr.Close();
      if(conn !=null) conn.Close();

      return foundDoctor;
    }

  }
}
