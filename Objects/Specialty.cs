using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DoctorOffice
{
  public class Specialty
  {
    private int _id;
    private string _name;

    public Specialty(string name, int id=0)
    {
      _name = name;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM specialties;", conn);
      cmd.ExecuteNonQuery();
    }

    public override bool Equals(System.Object otherSpecialty)
    {
      if (!(otherSpecialty is Specialty)) return false;
      else
      {
        Specialty newSpecialty = (Specialty) otherSpecialty;
        bool idEquality = (GetId() == newSpecialty.GetId());
        bool nameEquality = (GetName() == newSpecialty.GetName());
        return (idEquality && nameEquality);
      }
    }

    public static Specialty Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("SELECT * FROM specialties WHERE id = @SpecialtyId;", conn);
      SqlParameter specialtyIdParameter = new SqlParameter();
      specialtyIdParameter.ParameterName = "@SpecialtyId";
      specialtyIdParameter.Value = id.ToString();
      cmd.Parameters.Add(specialtyIdParameter);
      rdr = cmd.ExecuteReader();

      int foundId = 0;
      string foundName = null;

      while(rdr.Read())
      {
        foundName = rdr.GetString(0);
        foundId = rdr.GetInt32(1);
      }
      Specialty foundSpecialty = new Specialty(foundName, foundId);

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return foundSpecialty;
    }

    public static List<Specialty> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;
      List<Specialty> AllSpecialties = new List<Specialty>{};

      SqlCommand cmd = new SqlCommand("SELECT * FROM specialties ORDER BY name;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        string specialtyName = rdr.GetString(0);
        int specialtyId = rdr.GetInt32(1);
        Specialty newSpecialty = new Specialty(specialtyName, specialtyId);
        AllSpecialties.Add(newSpecialty);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return AllSpecialties;
    }

    public List<Doctor> GetAllDoctors()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand("SELECT * FROM doctors WHERE specialty_id = @SpecialtyId ORDER BY name;", conn);
      SqlParameter specialtyIdParameter = new SqlParameter();
      specialtyIdParameter.ParameterName = "@SpecialtyId";
      specialtyIdParameter.Value = GetId();
      cmd.Parameters.Add(specialtyIdParameter);
      rdr = cmd.ExecuteReader();

      List<Doctor> AllDoctors = new List<Doctor>{};
      while(rdr.Read())
      {
        string doctorName = rdr.GetString(0);
        int specialtyId = rdr.GetInt32(1);
        int doctorId = rdr.GetInt32(2);
        Doctor newDoctor = new Doctor(doctorName, specialtyId, doctorId);
        AllDoctors.Add(newDoctor);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return AllDoctors;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlDataReader rdr = null;

      SqlCommand cmd = new SqlCommand ("INSERT INTO specialties (name) OUTPUT INSERTED.id VALUES(@SpecialtyName);", conn);
      SqlParameter specialtyNameParameter = new SqlParameter();
      specialtyNameParameter.ParameterName = "@SpecialtyName";
      specialtyNameParameter.Value = GetName();
      cmd.Parameters.Add(specialtyNameParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        _id = rdr.GetInt32(0);
      }

      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
    }
  }
}
