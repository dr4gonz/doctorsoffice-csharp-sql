using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DoctorOffice.Objects
{
  public class Patient
  {
    private int _id;
    private string _name;
    private int _doctor_id;
    private DateTime? _birthday;

    public Patient(string name, DateTime? birthday, int doctor_id, int id = 0)
    {
      _name = name;
      _birthday = birthday;
      _doctor_id = doctor_id;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public int GetDoctorId()
    {
      return _doctor_id;
    }
    public DateTime? GetBirthday()
    {
      return _birthday;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    public static List<Patient> GetAll()
    {
      List<Patient> allPatients = new List<Patient> {};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand  cmd = new SqlCommand("SELECT * FROM patients;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        string patientName = rdr.GetString(0);
        DateTime? patientBirthday = rdr.GetDateTime(1);
        int patientDoctorId = rdr.GetInt32(2);
        int patientId = rdr.GetInt32(3);
        Patient newPatient = new Patient(patientName, patientBirthday, patientDoctorId, patientId);
        allPatients.Add(newPatient)
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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO patients (name, birthday, doctor_id) OUTPUT INSERTED.id VALUES (@PatientName, @PatientBirthday, @DoctorId);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@PatientName";
      nameParameter.Value = this.GetName();

      SqlParameter birthdayParameter = new SqlParameter();
      birthdayParameter.ParameterName = "@PatientBirthday";
      birthdayParameter.Value = this.GetBirthday();

      SqlParameter doctorIdParameter = new SqlParameter();
      doctorIdParameter.ParameterName = "@DoctorId";
      doctorIdParameter.Value = this.GetDoctorId();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(birthdayParameter);
      cmd.Parameters.Add(doctorIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Patient Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patients WHERE id = @PatientId;", conn);
      sqlParameter patientId = new SqlParameter();
      patientIdParameter.ParameterName = "@PatientId";
      patientIdParameter.Value = id.ToString();
      cmd.Parameters.Add(patientIdParameter);
      rdr = cmd.ExecuteReader();

      string foundPatientName = null;
      DateTime? foundBirthday = null;
      int foundDoctorId = 0;
      int foundId = 0;

      while (rdr.Read())
      {
        foundPatientName = rdr.GetString(0);
        foundBirthday = rdr.GetDateTime(1);
        foundDoctorId = rdr.GetInt32(2);
        foundId = rdr.GetInt32(3);
      }
      Patient foundPatient = new Patient(foundPatientName, foundBirthday, foundDoctorId, foundId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundPatient;
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM patients;", conn);
      cmd.ExecuteNonQuery();
    }
    public override bool Equals(System.Object otherPatient)
    {
      if (!(otherPatient is Patient))
      {
        return false;
      }
      else
      {
        Patient newPatient = (Patient) otherPatient;
        bool nameEquality = (this.GetName() == newPatient.GetName());
        bool birthdayEquality = (this.GetBirthday() == newPatient.GetBirthday());
        bool doctorIdEquality = (this.GetDoctorId() == newPatient.GetDoctorId());
        bool idEquality = (this.GetId() == newPatient.GetId());
      }
    }
    
  }
}
